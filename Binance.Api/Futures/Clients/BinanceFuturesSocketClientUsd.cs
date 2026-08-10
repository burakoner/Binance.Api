namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientUsd : WebSocketApiClient, IBinanceFuturesSocketClientUsd
{
    private const string SessionRevokedHandler = "usd-futures-session-revoked";
    private ApiCredentialsType? apiCredentialsType;
    private bool hasApiKey;
    private bool hasApiSecret;
    private readonly System.Collections.Concurrent.ConcurrentDictionary<int, BinanceFuturesUsdWebSocketSession> sessions = new();
    private readonly BinanceWebSocketSessionTransitionGate sessionTransitions = new();

    // Internal
    internal ILogger Logger { get => _logger; }
    internal TimeSyncState TimeSyncState { get; } = new("Binance USDⓈ-M Futures WS");
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

    internal BinanceFuturesSocketClientUsd(BinanceFuturesSocketClient root) : base(root.Logger, root.SocketOptions)
    {
        _ = root;
        __ = root._;
        apiCredentialsType = root.SocketOptions.ApiCredentials?.Type;
        hasApiKey = HasApiKey(root.SocketOptions.ApiCredentials);
        hasApiSecret = HasApiSecret(root.SocketOptions.ApiCredentials);

        RateLimitPerConnectionPerSecond = 4;
        SetDataInterpreter((data) => string.Empty, null);
        AddGenericHandler(SessionRevokedHandler, HandleSessionRevocation);
    }

    internal new void SetApiCredentials(ApiCredentials credentials)
    {
        apiCredentialsType = credentials.Type;
        hasApiKey = HasApiKey(credentials);
        hasApiSecret = HasApiSecret(credentials);
        base.SetApiCredentials(credentials);
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
                    var retryAfter = BinanceServerRateLimitGuard.ParseWebSocketRetryAfter(data);
                    __.ServerRateLimitGuard.Extend(retryAfter);
                    callResult = new CallResult<T>(new BinanceRateLimitError(errorCode, errorMessage, data["error"]?["data"])
                    {
                        RetryAfter = retryAfter
                    }, SocketOptions.RawResponse ? data.ToString() : null);
                    return true;
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
        return identifier == SessionRevokedHandler && GetSessionRevocationPayload(message) != null;
    }

    protected override Task<CallResult<bool>> AuthenticateAsync(WebSocketConnection connection)
        => sessionTransitions.ExecuteAsync(
            () => AuthenticateSessionAsync(connection),
            CancellationToken.None);

    private async Task<CallResult<bool>> AuthenticateSessionAsync(WebSocketConnection connection)
    {
        if (!sessions.TryGetValue(connection.Id, out var session))
            return new CallResult<bool>(new InvalidOperationError("No USDⓈ-M WebSocket API session is registered for this connection."));

        session.MarkAuthenticationPending();

        var guardResult = await __.ServerRateLimitGuard.WaitAsync(CancellationToken.None).ConfigureAwait(false);
        if (!guardResult)
            return guardResult;

        var syncResult = await SyncTimeAsync().ConfigureAwait(false);
        if (!syncResult)
            return syncResult;

        CallResult<BinanceFuturesUsdWebSocketSessionStatus> result;
        try
        {
            result = await SendSessionLogonAsync(connection, session.ReceiveWindow).ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is ArgumentException
            || exception is InvalidOperationException
            || exception is NotSupportedException)
        {
            return new CallResult<bool>(new InvalidOperationError(exception.Message));
        }

        if (!result)
            return result.As(false);

        session.LastStatus = result.Data;
        return result.As(true);
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

    internal Task<CallResult<WebSocketUpdateSubscription>> SubscribeMarketAsync<T>(IEnumerable<string> topics, bool authenticated, Action<WebSocketDataEvent<T>> onData, CancellationToken ct)
        => SubscribeAsync(MarketStreamAddress, topics, authenticated, onData, ct);

    internal Task<CallResult<WebSocketUpdateSubscription>> SubscribePublicAsync<T>(IEnumerable<string> topics, bool authenticated, Action<WebSocketDataEvent<T>> onData, CancellationToken ct)
        => SubscribeAsync(PublicStreamAddress, topics, authenticated, onData, ct);

    internal Task<CallResult<WebSocketUpdateSubscription>> SubscribePrivateAsync<T>(IEnumerable<string> topics, bool authenticated, Action<WebSocketDataEvent<T>> onData, CancellationToken ct)
        => SubscribeAsync(PrivateStreamAddress, topics, authenticated, onData, ct);

    internal static string MarketStreamAddress
        => BinanceAddress.Default.UsdFuturesSocketApiStreamAddress.AppendPath("market/stream");

    internal static string PublicStreamAddress
        => BinanceAddress.Default.UsdFuturesSocketApiStreamAddress.AppendPath("public/stream");

    internal static string PrivateStreamAddress
        => BinanceAddress.Default.UsdFuturesSocketApiStreamAddress.AppendPath("private/stream");

    private Task<CallResult<WebSocketUpdateSubscription>> SubscribeAsync<T>(string address, IEnumerable<string> topics, bool authenticated, Action<WebSocketDataEvent<T>> onData, CancellationToken ct)
    {
        var request = new BinanceSocketRequest
        {
            Method = "SUBSCRIBE",
            Params = [.. topics],
            Id = NextId()
        };

        return SubscribeAsync(address, request, "", authenticated, onData, ct);
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
        var guardResult = await __.ServerRateLimitGuard.WaitAsync(ct).ConfigureAwait(false);
        if (!guardResult)
            return new CallResult<T>(guardResult.Error!);

        if (authenticated)
        {
            if (!hasApiKey)
                throw new InvalidOperationException("No credentials provided for authenticated endpoint");
            if (sign && !hasApiSecret)
                throw new InvalidOperationException("No API secret provided for signed endpoint");

            var authProvider = (BinanceAuthentication)AuthenticationProvider;
            if (sign)
            {
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

                var timestamp = DateTime.UtcNow.Add(GetTimeOffset()).ConvertToMilliseconds();
                parameters = authProvider.AuthenticateSocketParameters(parameters, timestamp);
            }
            else parameters.Add("apiKey", authProvider.Credentials.Key.GetString());
        }

        var request = new BinanceSocketQuery
        {
            Method = method,
            Params = parameters,
            Id = ExchangeHelpers.NextId()
        };

        var address = url.StartsWith("wss://") ? url : BinanceAddress.Default.UsdFuturesSocketApiQueryAddress.AppendPath(url);
        // Individually signed requests do not authenticate the underlying connection. Only
        // session.logon creates connection-bound authentication that must be restored.
        var result = await base.QueryAsync<BinanceResultWithRateLimits<T>>(address, request, false).ConfigureAwait(false);
        if (!result.Success)
            return result.AsError<T>(result.Error!);

        return result.As(result.Data.Result);
    }

    private static bool HasApiKey(ApiCredentials? credentials)
        => credentials != null && !string.IsNullOrWhiteSpace(credentials.Key.GetString());

    private static bool HasApiSecret(ApiCredentials? credentials)
        => credentials != null && !string.IsNullOrWhiteSpace(credentials.Secret.GetString());

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
