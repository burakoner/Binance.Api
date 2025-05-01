namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientCoin : WebSocketApiClient, IBinanceFuturesSocketClientCoin
{
    // Internal
    internal ILogger Logger { get => _logger; }
    internal TimeSyncState TimeSyncState { get; } = new("Binance Coin-M Futures WS");
    internal CallResult<T> Deserializer<T>(string data, JsonSerializer? serializer = null, int? requestId = null) => Deserialize<T>(data, serializer, requestId);
    internal CallResult<T> Deserializer<T>(JToken obj, JsonSerializer? serializer = null, int? requestId = null) => Deserialize<T>(obj, serializer, requestId);

    protected Task<CallResult<DateTime>> GetServerTimestampAsync() => GetTimeAsync();
    protected TimeSyncInfo GetTimeSyncInfo() => new(Logger, SocketOptions.AutoTimestamp, SocketOptions.TimestampRecalculationInterval, TimeSyncState);
    protected TimeSpan GetTimeOffset() => TimeSyncState.TimeOffset;

    // Parent
    internal BinanceSocketApiClient __ { get; }
    internal BinanceFuturesSocketClient _ { get; }

    // Internal
    internal BinanceSocketApiClientOptions SocketOptions => _.SocketOptions;

    internal BinanceFuturesSocketClientCoin(BinanceFuturesSocketClient root) : base(root.Logger, root.SocketOptions)
    {
        _ = root;
        __ = root._;

        RateLimitPerConnectionPerSecond = 4;
        SetDataInterpreter((data) => string.Empty, null);
    }

    #region Overrided Methods
    protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
        => new BinanceAuthentication(credentials);

    protected override bool HandleQueryResponse<T>(WebSocketConnection connection, object request, JToken data, out CallResult<T>? callResult)
    {
        callResult = null;

        if (data.Type != JTokenType.Object)
            return false;

        if (data["id"] == null) return false;
        var id = data["id"]!.Value<int>();

        if (data["status"] == null) return false;
        var status = data["status"]!.Value<int>();

        if (request is BinanceSocketQuery query)
        {
            if (query.Id != id) return false;

            if (status != 200)
            {
                var errorCode = data["error"]?["code"]?.Value<int>() ?? status;
                var errorMessage = data["error"]?["msg"]?.Value<string>() ?? "Undefined Error";
                if (status == 418 || status == 429)
                {
                    // Rate limit error 
                    return new CallResult<T>(new BinanceRateLimitError(errorCode, errorMessage, null)
                    {
                        // RetryAfter = data["error"]?["data"].Data.Error.Data!.RetryAfter
                    }, SocketOptions.RawResponse ? data.ToString() : null);
                }

                callResult = new CallResult<T>(new ServerError(errorCode, errorMessage), SocketOptions.RawResponse ? data.ToString() : null);
                return true;
            }

            var error = data["error"];
            if (error != null && error["code"] != null && error["msg"] != null)
            {
                callResult = new CallResult<T>(new ServerError(error["code"]!.Value<int>(), error["msg"]!.ToString()));
                return true;
            }

            var desResult = Deserialize<T>(data);
            if (!desResult)
            {
                Logger.Log(LogLevel.Warning, $"Failed to deserialize data: {desResult.Error}. Data: {data}");
                return false;
            }

            callResult = new CallResult<T>(desResult.Data, SocketOptions.RawResponse ? data.ToString() : null);
            return true;
        }

        throw new NotImplementedException();
    }

    protected override bool HandleSubscriptionResponse(WebSocketConnection connection, WebSocketSubscription subscription, object request, JToken message, out CallResult<object> callResult)
    {
        callResult = null;
        if (message.Type != JTokenType.Object)
            return false;

        var id = message["id"];
        if (id == null)
            return false;

        var bRequest = (BinanceSocketRequest)request;
        if ((int)id != bRequest.Id)
            return false;

        var result = message["result"];
        if (result != null && result.Type == JTokenType.Null)
        {
            Logger.Log(LogLevel.Trace, $"Socket {connection.Id} Subscription completed");
            callResult = new CallResult<object>(new object());
            return true;
        }

        var error = message["error"];
        if (error == null)
        {
            callResult = new CallResult<object>(new ServerError("Unknown error: " + message));
            return true;
        }

        callResult = new CallResult<object>(new ServerError(error["code"]!.Value<int>(), error["msg"]!.ToString()));
        return true;
    }

    protected override bool MessageMatchesHandler(WebSocketConnection connection, JToken message, object request)
    {
        if (message.Type != JTokenType.Object)
            return false;

        var bRequest = (BinanceSocketRequest)request;
        var stream = message["stream"];
        if (stream == null)
            return false;

        return bRequest.Params.Contains(stream.ToString());
    }

    protected override bool MessageMatchesHandler(WebSocketConnection connection, JToken message, string identifier)
    {
        return true;
    }

    protected override async Task<CallResult<bool>> AuthenticateAsync(WebSocketConnection connection)
    {
        await Task.CompletedTask;
        return new CallResult<bool>(true);
        throw new NotImplementedException();
    }

    protected override async Task<bool> UnsubscribeAsync(WebSocketConnection connection, WebSocketSubscription subscription)
    {
        var topics = ((BinanceSocketRequest)subscription.Request!).Params;
        var unsub = new BinanceSocketRequest { Method = "UNSUBSCRIBE", Params = topics, Id = NextId() };
        var result = false;

        if (!connection.Connected)
            return true;

        await connection.SendAndWaitAsync(unsub, ClientOptions.ResponseTimeout, data =>
        {
            if (data.Type != JTokenType.Object)
                return false;

            var id = data["id"];
            if (id == null)
                return false;

            if ((int)id != unsub.Id)
                return false;

            var result = data["result"];
            if (result?.Type == JTokenType.Null)
            {
                result = true;
                return true;
            }

            return true;
        }).ConfigureAwait(false);
        return result;
    }
    #endregion

    internal Task<CallResult<WebSocketUpdateSubscription>> SubscribeAsync<T>(IEnumerable<string> topics, bool authenticated, Action<WebSocketDataEvent<T>> onData, CancellationToken ct)
    {
        var request = new BinanceSocketRequest
        {
            Method = "SUBSCRIBE",
            Params = [.. topics],
            Id = NextId()
        };

        return SubscribeAsync(BinanceAddress.Default.SpotSocketApiStreamAddress.AppendPath("stream"), request, "", authenticated, onData, ct);
    }

    internal async Task<CallResult<bool>> SyncTimeAsync()
    {
        var timeSyncParams = GetTimeSyncInfo();
        if (await timeSyncParams.TimeSyncState.Semaphore.WaitAsync(0).ConfigureAwait(false))
        {
            if (!timeSyncParams.SyncTime || (DateTime.UtcNow - timeSyncParams.TimeSyncState.LastSyncTime < timeSyncParams.RecalculationInterval))
            {
                timeSyncParams.TimeSyncState.Semaphore.Release();
                return new CallResult<bool>(true);
            }

            var sw = Stopwatch.StartNew();
            var localTime = DateTime.UtcNow;
            var result = await GetTimeAsync().ConfigureAwait(false);
            sw.Stop();
            if (!result)
            {
                timeSyncParams.TimeSyncState.Semaphore.Release();
                return result.As(false);
            }

            // Calculate time offset between local and server
            var offset = result.Data - (localTime.AddMilliseconds(sw.ElapsedMilliseconds / 2));
            timeSyncParams.UpdateTimeOffset(offset);
            timeSyncParams.TimeSyncState.Semaphore.Release();
        }

        return new CallResult<bool>(true);
    }

    internal async Task<CallResult<T>> RequestAsync<T>(string url, string method, Dictionary<string, object> parameters, bool authenticated = false, bool sign = false, int weight = 1, CancellationToken ct = default)
    {
        if (authenticated)
        {
            if (AuthenticationProvider == null)
                throw new InvalidOperationException("No credentials provided for authenticated endpoint");

            var syncTask = SyncTimeAsync();
            var timeSyncInfo = GetTimeSyncInfo();
            if (timeSyncInfo.TimeSyncState.LastSyncTime == default)
            {
                // Initially with first request we'll need to wait for the time syncing, if it's not the first request we can just continue
                var syncTimeResult = await syncTask.ConfigureAwait(false);
                if (!syncTimeResult)
                {
                    //_logger.Log(LogLevel.Debug, $"[{requestId}] Failed to sync time, aborting request: " + syncTimeResult.Error);
                    //return syncTimeResult.As<IRequest>(default);
                }
            }

            var authProvider = (BinanceAuthentication)AuthenticationProvider;
            var timestamp = DateTime.UtcNow.Add(GetTimeOffset()).ConvertToMilliseconds();
            if (sign) parameters = authProvider.AuthenticateSocketParameters(parameters, timestamp);
            else parameters.Add("apiKey", authProvider.Credentials.Key.GetString());
        }

        var request = new BinanceSocketQuery
        {
            Method = method,
            Params = parameters,
            Id = ExchangeHelpers.NextId()
        };

        var address = url.StartsWith("wss://") ? url : BinanceAddress.Default.CoinFuturesSocketApiQueryAddress.AppendPath(url);
        var result = await base.QueryAsync<BinanceResponse<T>>(address, request, sign).ConfigureAwait(false);
        if (!result.Success)
        {
            if (result.Error is BinanceRateLimitError rle)
            {
                /*
                if (rle.RetryAfter != null && RateLimiter != null && ClientOptions.RateLimiterEnabled)
                {
                    _logger.LogWarning("Ratelimit error from server, pausing requests until {Until}", rle.RetryAfter.Value);
                    await RateLimiter.SetRetryAfterGuardAsync(rle.RetryAfter.Value).ConfigureAwait(false);
                }
                */
            }

            else return result.AsError<T>(result.Error!);
        }

        return result.As(result.Data.Result);
    }

    public async Task UnsubscribeAsync(WebSocketUpdateSubscription subscription, bool force = false, CancellationToken ct = default)
    {
        // Soft Unsubscribe
        var wsc = subscription.GetConnection();
        var wss = subscription.GetSubscription();
        await this.UnsubscribeAsync(wsc, wss).ConfigureAwait(false);

        // Force Unsubscribe
        if (force)
        {
            await base.UnsubscribeAsync(subscription).ConfigureAwait(false);
        }
    }

    public Task UnsubscribeAsync(int subscriptionId, CancellationToken ct = default)
    {
        return base.UnsubscribeAsync(subscriptionId);
    }

    public Task UnsubscribeAllAsync(CancellationToken ct = default)
    {
        return base.UnsubscribeAllAsync();
    }
}