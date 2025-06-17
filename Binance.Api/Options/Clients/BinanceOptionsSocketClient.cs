namespace Binance.Api.Options;

internal partial class BinanceOptionsSocketClient : WebSocketApiClient, IBinanceOptionsSocketClient
{
    // Internal
    internal ILogger Logger { get => _logger; }
    internal TimeSyncState TimeSyncState { get; } = new("Binance Options WS");
    internal CallResult<T> Deserializer<T>(string data, JsonSerializer? serializer = null, int? requestId = null) => Deserialize<T>(data, serializer, requestId);
    internal CallResult<T> Deserializer<T>(JToken obj, JsonSerializer? serializer = null, int? requestId = null) => Deserialize<T>(obj, serializer, requestId);

    protected async Task<CallResult<DateTime>> GetServerTimestampAsync()
    {
        var result = await _.RestApiClient.Options.GetTimeAsync();
        if (!result.Success) return result.AsError<DateTime>(result.Error!);
        return result.As(result.Data);
    }
    protected TimeSyncInfo GetTimeSyncInfo() => new(Logger, SocketOptions.AutoTimestamp, SocketOptions.TimestampRecalculationInterval, TimeSyncState);
    protected TimeSpan GetTimeOffset() => TimeSyncState.TimeOffset;

    // Parent
    internal BinanceSocketApiClient _ { get; }

    // Internal
    internal BinanceSocketApiClientOptions SocketOptions => _.ApiOptions;
    internal DateTime? LastExchangeInfoUpdate { get; private set; }
    internal BinanceOptionsExchangeInfo? ExchangeInfo { get; private set; }

    internal BinanceOptionsSocketClient(BinanceSocketApiClient root) : base(root.Logger, root.ApiOptions)
    {
        _ = root;

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

    protected override bool HandleSubscriptionResponse(WebSocketConnection connection, WebSocketSubscription subscription, object request, JToken message, out CallResult<object>? callResult)
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

        return SubscribeAsync(BinanceAddress.Default.OptionsSocketApiStreamAddress.AppendPath("stream"), request, "", authenticated, onData, ct);
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
            var result = await GetServerTimestampAsync().ConfigureAwait(false);
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