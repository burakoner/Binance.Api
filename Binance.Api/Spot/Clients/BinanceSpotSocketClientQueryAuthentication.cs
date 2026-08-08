namespace Binance.Api.Spot;

internal partial class BinanceSpotSocketClient
{
    public async Task<CallResult<BinanceSpotWebSocketSession>> LogonAsync(decimal? receiveWindow = null, CancellationToken ct = default)
    {
        ValidateSessionCredentials(receiveWindow);
        if (ct.IsCancellationRequested)
            return new CallResult<BinanceSpotWebSocketSession>(new CancellationRequestedError());

        var syncResult = await SyncTimeAsync().ConfigureAwait(false);
        if (!syncResult)
            return syncResult.As<BinanceSpotWebSocketSession>(null);

        var connectionResult = await GetSessionConnectionAsync(ct).ConfigureAwait(false);
        if (!connectionResult)
            return connectionResult.As<BinanceSpotWebSocketSession>(null);

        var statusResult = await SendSessionLogonAsync(connectionResult.Data, receiveWindow).ConfigureAwait(false);
        if (!statusResult)
            return statusResult.As<BinanceSpotWebSocketSession>(null);

        return statusResult.As(new BinanceSpotWebSocketSession(connectionResult.Data, statusResult.Data));
    }

    public async Task<CallResult<BinanceSpotWebSocketSessionStatus>> LogonAsync(
        BinanceSpotWebSocketSession session,
        decimal? receiveWindow = null,
        CancellationToken ct = default)
    {
        ValidateSession(session);
        ValidateSessionCredentials(receiveWindow);
        if (ct.IsCancellationRequested)
            return new CallResult<BinanceSpotWebSocketSessionStatus>(new CancellationRequestedError());

        var syncResult = await SyncTimeAsync().ConfigureAwait(false);
        if (!syncResult)
            return syncResult.As<BinanceSpotWebSocketSessionStatus>(null);

        var result = await SendSessionLogonAsync(session.Connection, receiveWindow).ConfigureAwait(false);
        if (result)
            session.LastStatus = result.Data;
        return result;
    }

    public async Task<CallResult<BinanceSpotWebSocketSessionStatus>> GetSessionStatusAsync(
        BinanceSpotWebSocketSession session,
        CancellationToken ct = default)
    {
        ValidateSession(session);
        if (ct.IsCancellationRequested)
            return new CallResult<BinanceSpotWebSocketSessionStatus>(new CancellationRequestedError());

        var result = await SendSessionRequestAsync(
            session.Connection,
            CreateSessionRequest("session.status")).ConfigureAwait(false);
        if (result)
            session.LastStatus = result.Data;
        return result;
    }

    public async Task<CallResult<BinanceSpotWebSocketSessionStatus>> LogoutAsync(
        BinanceSpotWebSocketSession session,
        CancellationToken ct = default)
    {
        ValidateSession(session);
        if (ct.IsCancellationRequested)
            return new CallResult<BinanceSpotWebSocketSessionStatus>(new CancellationRequestedError());

        var result = await SendSessionRequestAsync(
            session.Connection,
            CreateSessionRequest("session.logout")).ConfigureAwait(false);
        if (result)
            session.LastStatus = result.Data;
        return result;
    }

    internal BinanceSocketQuery CreateSessionLogonRequest(decimal? receiveWindow, long timestamp)
    {
        ValidateSessionCredentials(receiveWindow);
        var parameters = new Dictionary<string, object>();
        if (receiveWindow.HasValue)
            parameters.Add("recvWindow", receiveWindow.Value);

        return new BinanceSocketQuery
        {
            Id = ExchangeHelpers.NextId(),
            Method = "session.logon",
            Params = ((BinanceAuthentication)AuthenticationProvider).AuthenticateSocketParameters(parameters, timestamp)
        };
    }

    internal static BinanceSocketQuery CreateSessionRequest(string method)
        => new()
        {
            Id = ExchangeHelpers.NextId(),
            Method = method
        };

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
            var address = BinanceAddress.Default.SpotSocketApiQueryAddress.AppendPath("ws-api/v3");
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

    private Task<CallResult<BinanceSpotWebSocketSessionStatus>> SendSessionLogonAsync(
        WebSocketConnection connection,
        decimal? receiveWindow)
    {
        var timestamp = DateTime.UtcNow.Add(GetTimeOffset()).ConvertToMilliseconds();
        return SendSessionRequestAsync(connection, CreateSessionLogonRequest(receiveWindow, timestamp));
    }

    private async Task<CallResult<BinanceSpotWebSocketSessionStatus>> SendSessionRequestAsync(
        WebSocketConnection connection,
        BinanceSocketQuery request)
    {
        if (!connection.Connected)
            return new CallResult<BinanceSpotWebSocketSessionStatus>(new WebError("WebSocket session connection is not open"));
        if (connection.PausedActivity)
            return new CallResult<BinanceSpotWebSocketSessionStatus>(new ServerError("WebSocket is paused"));

        var result = await QueryAndWaitAsync<BinanceResultWithRateLimits<BinanceSpotWebSocketSessionStatus>>(
            connection,
            request).ConfigureAwait(false);
        return result
            ? result.As(result.Data.Result)
            : result.As<BinanceSpotWebSocketSessionStatus>(null);
    }

    private void ValidateSessionCredentials(decimal? receiveWindow)
    {
        BinanceSpotAccountValidation.ReceiveWindow(receiveWindow);
        if (!hasApiCredentials)
            throw new InvalidOperationException("API credentials are required for session.logon.");
        if (apiCredentialsType != ApiCredentialsType.Ed25519)
            throw new NotSupportedException("Binance session.logon supports only Ed25519 API keys.");
    }

    private void ValidateSession(BinanceSpotWebSocketSession session)
    {
        if (session == null)
            throw new ArgumentNullException(nameof(session));
        if (!ReferenceEquals(session.Connection.ApiClient, this))
            throw new ArgumentException("The session belongs to a different Spot WebSocket API client.", nameof(session));
    }
}
