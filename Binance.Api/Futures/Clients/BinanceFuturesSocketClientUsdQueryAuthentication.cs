namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientUsd
{
    internal const string SessionPath = "ws-fapi/v1";
    internal const string SessionLogonMethod = "session.logon";
    internal const string SessionStatusMethod = "session.status";
    internal const string SessionLogoutMethod = "session.logout";
    internal const int SessionRequestWeight = 2;

    public async Task<CallResult<BinanceFuturesUsdWebSocketSession>> LogonAsync(
        long? receiveWindow = null,
        CancellationToken ct = default)
    {
        var normalizedReceiveWindow = ValidateSessionCredentials(receiveWindow);
        return await sessionTransitions.ExecuteAsync(
            () => LogonSessionAsync(normalizedReceiveWindow, ct),
            ct).ConfigureAwait(false);
    }

    private async Task<CallResult<BinanceFuturesUsdWebSocketSession>> LogonSessionAsync(
        long? receiveWindow,
        CancellationToken ct)
    {
        if (ct.IsCancellationRequested)
            return new CallResult<BinanceFuturesUsdWebSocketSession>(new CancellationRequestedError());

        var guardResult = await __.ServerRateLimitGuard.WaitAsync(ct).ConfigureAwait(false);
        if (!guardResult)
            return guardResult.As<BinanceFuturesUsdWebSocketSession>(null);

        var syncResult = await SyncTimeAsync().ConfigureAwait(false);
        if (!syncResult)
            return syncResult.As<BinanceFuturesUsdWebSocketSession>(null);

        var connectionResult = await GetSessionConnectionAsync(ct).ConfigureAwait(false);
        if (!connectionResult)
            return connectionResult.As<BinanceFuturesUsdWebSocketSession>(null);

        var statusResult = await SendSessionLogonAsync(connectionResult.Data, receiveWindow, ct).ConfigureAwait(false);
        if (!statusResult)
            return statusResult.As<BinanceFuturesUsdWebSocketSession>(null);

        if (sessions.TryGetValue(connectionResult.Data.Id, out var existingSession))
        {
            existingSession.LastStatus = statusResult.Data;
            existingSession.ReceiveWindow = receiveWindow;
            existingSession.LifecycleSubscription.Authenticated = true;
            return statusResult.As(existingSession);
        }

        var lifecycleSubscription = AddSubscription<string>(
            null!,
            $"usd-futures-session-{connectionResult.Data.Id}",
            true,
            connectionResult.Data,
            _ => { },
            true);
        if (lifecycleSubscription == null)
            return new CallResult<BinanceFuturesUsdWebSocketSession>(new InvalidOperationError("Unable to register the USDⓈ-M WebSocket session lifecycle."));

        var session = new BinanceFuturesUsdWebSocketSession(
            connectionResult.Data,
            lifecycleSubscription,
            statusResult.Data,
            receiveWindow);
        sessions[connectionResult.Data.Id] = session;
        connectionResult.Data.ConnectionClosed += () => sessions.TryRemove(connectionResult.Data.Id, out var removedSession);
        return statusResult.As(session);
    }

    public async Task<CallResult<BinanceFuturesUsdWebSocketSessionStatus>> LogonAsync(
        BinanceFuturesUsdWebSocketSession session,
        long? receiveWindow = null,
        CancellationToken ct = default)
    {
        ValidateSession(session);
        var normalizedReceiveWindow = ValidateSessionCredentials(receiveWindow);
        return await sessionTransitions.ExecuteAsync(
            () => LogonSessionAsync(session, normalizedReceiveWindow, ct),
            ct).ConfigureAwait(false);
    }

    private async Task<CallResult<BinanceFuturesUsdWebSocketSessionStatus>> LogonSessionAsync(
        BinanceFuturesUsdWebSocketSession session,
        long? receiveWindow,
        CancellationToken ct)
    {
        if (ct.IsCancellationRequested)
            return new CallResult<BinanceFuturesUsdWebSocketSessionStatus>(new CancellationRequestedError());

        var guardResult = await __.ServerRateLimitGuard.WaitAsync(ct).ConfigureAwait(false);
        if (!guardResult)
            return guardResult.As<BinanceFuturesUsdWebSocketSessionStatus>(null);

        var syncResult = await SyncTimeAsync().ConfigureAwait(false);
        if (!syncResult)
            return syncResult.As<BinanceFuturesUsdWebSocketSessionStatus>(null);

        var result = await SendSessionLogonAsync(session.Connection, receiveWindow, ct).ConfigureAwait(false);
        if (result)
        {
            session.LastStatus = result.Data;
            session.ReceiveWindow = receiveWindow;
            session.LifecycleSubscription.Authenticated = true;
        }

        return result;
    }

    public async Task<CallResult<BinanceFuturesUsdWebSocketSessionStatus>> GetSessionStatusAsync(
        BinanceFuturesUsdWebSocketSession session,
        CancellationToken ct = default)
    {
        ValidateSession(session);
        return await sessionTransitions.ExecuteAsync(
            () => GetSessionStatusAsyncCore(session, ct),
            ct).ConfigureAwait(false);
    }

    private async Task<CallResult<BinanceFuturesUsdWebSocketSessionStatus>> GetSessionStatusAsyncCore(
        BinanceFuturesUsdWebSocketSession session,
        CancellationToken ct)
    {
        if (ct.IsCancellationRequested)
            return new CallResult<BinanceFuturesUsdWebSocketSessionStatus>(new CancellationRequestedError());

        var result = await SendSessionRequestAsync<BinanceFuturesUsdWebSocketSessionStatus>(
            session.Connection,
            CreateSessionRequest(SessionStatusMethod),
            ct).ConfigureAwait(false);
        if (result)
            session.LastStatus = result.Data;

        return result;
    }

    public async Task<CallResult<BinanceFuturesUsdWebSocketSessionStatus>> LogoutAsync(
        BinanceFuturesUsdWebSocketSession session,
        CancellationToken ct = default)
    {
        ValidateSession(session);
        return await sessionTransitions.ExecuteAsync(
            () => LogoutSessionAsync(session, ct),
            ct).ConfigureAwait(false);
    }

    private async Task<CallResult<BinanceFuturesUsdWebSocketSessionStatus>> LogoutSessionAsync(
        BinanceFuturesUsdWebSocketSession session,
        CancellationToken ct)
    {
        if (ct.IsCancellationRequested)
            return new CallResult<BinanceFuturesUsdWebSocketSessionStatus>(new CancellationRequestedError());

        var result = await SendSessionRequestAsync<BinanceFuturesUsdWebSocketSessionStatus>(
            session.Connection,
            CreateSessionRequest(SessionLogoutMethod),
            ct).ConfigureAwait(false);
        if (result)
        {
            session.LastStatus = result.Data;
            session.LifecycleSubscription.Authenticated = false;
        }

        return result;
    }

    internal BinanceSocketQuery CreateSessionLogonRequest(long? receiveWindow, long timestamp)
    {
        var normalizedReceiveWindow = ValidateSessionCredentials(receiveWindow);
        var parameters = new Dictionary<string, object>();
        if (normalizedReceiveWindow.HasValue)
            parameters.Add("recvWindow", normalizedReceiveWindow.Value);

        return new BinanceSocketQuery
        {
            Id = ExchangeHelpers.NextId(),
            Method = SessionLogonMethod,
            Params = ((BinanceAuthentication)AuthenticationProvider).AuthenticateSocketParameters(parameters, timestamp)
        };
    }

    internal static BinanceSocketQuery CreateSessionRequest(string method)
        => new()
        {
            Id = ExchangeHelpers.NextId(),
            Method = method
        };

    internal static JToken? GetSessionRevocationPayload(JToken message)
    {
        if (message.Type != JTokenType.Object
            || message["id"]?.Type != JTokenType.Null
            || message["status"]?.Value<int>() != 401)
            return null;

        var error = message["error"];
        return error?["code"]?.Value<int>() == -2015 && error["msg"]?.Type == JTokenType.String
            ? error
            : null;
    }

    private void HandleSessionRevocation(WebSocketMessageEvent message)
    {
        var payload = GetSessionRevocationPayload(message.JsonData);
        if (payload == null || !sessions.TryGetValue(message.Connection.Id, out var session))
            return;

        session.MarkAuthenticationRevoked(new BinanceFuturesUsdWebSocketSessionRevocation
        {
            Status = 401,
            Code = payload["code"]!.Value<int>(),
            Message = payload["msg"]!.Value<string>()!
        });
    }

    private async Task<CallResult<WebSocketConnection>> GetSessionConnectionAsync(CancellationToken ct)
    {
        try
        {
            await Semaphore.WaitAsync(ct).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            return new CallResult<WebSocketConnection>(new CancellationRequestedError());
        }

        try
        {
            var address = BinanceAddress.Default.UsdFuturesSocketApiQueryAddress.AppendPath(SessionPath);
            var connectionResult = await GetWebSocketConnectionAsync(address, false).ConfigureAwait(false);
            if (!connectionResult)
                return connectionResult;

            var connectResult = await ConnectIfNeededAsync(connectionResult.Data, false).ConfigureAwait(false);
            return connectResult
                ? connectionResult
                : new CallResult<WebSocketConnection>(connectResult.Error!);
        }
        finally
        {
            Semaphore.Release();
        }
    }

    private Task<CallResult<BinanceFuturesUsdWebSocketSessionStatus>> SendSessionLogonAsync(
        WebSocketConnection connection,
        long? receiveWindow,
        CancellationToken ct = default)
    {
        var timestamp = DateTime.UtcNow.Add(GetTimeOffset()).ConvertToMilliseconds();
        return SendSessionRequestAsync<BinanceFuturesUsdWebSocketSessionStatus>(
            connection,
            CreateSessionLogonRequest(receiveWindow, timestamp),
            ct);
    }

    private async Task<CallResult<T>> SendSessionRequestAsync<T>(
        WebSocketConnection connection,
        BinanceSocketQuery request,
        CancellationToken ct = default)
    {
        var guardResult = await __.ServerRateLimitGuard.WaitAsync(ct).ConfigureAwait(false);
        if (!guardResult)
            return guardResult.As<T>(default);

        if (!connection.Connected)
            return new CallResult<T>(new WebError("WebSocket session connection is not open"));
        if (connection.PausedActivity)
            return new CallResult<T>(new ServerError("WebSocket is paused"));

        var result = await QueryAndWaitAsync<BinanceResultWithRateLimits<T>>(
            connection,
            request).ConfigureAwait(false);
        return result
            ? result.As(result.Data.Result)
            : result.As<T>(default);
    }

    private long? ValidateSessionCredentials(long? receiveWindow)
    {
        var normalizedReceiveWindow = __.ReceiveWindow(receiveWindow);
        if (normalizedReceiveWindow is < 0 or > 60_000)
            throw new ArgumentOutOfRangeException(nameof(receiveWindow), "Receive window must be between 0 and 60000 milliseconds.");
        if (!hasApiKey || !hasApiSecret)
            throw new InvalidOperationException("API credentials are required for session.logon.");
        if (apiCredentialsType != ApiCredentialsType.Ed25519)
            throw new NotSupportedException("Binance USDⓈ-M session.logon supports only Ed25519 API keys.");

        return normalizedReceiveWindow;
    }

    private void ValidateSession(BinanceFuturesUsdWebSocketSession session)
    {
        if (session == null)
            throw new ArgumentNullException(nameof(session));
        if (!ReferenceEquals(session.Connection.ApiClient, this))
            throw new ArgumentException("The session belongs to a different USDⓈ-M WebSocket API client.", nameof(session));
    }
}
