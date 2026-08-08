namespace Binance.Api.Margin;

internal class BinanceMarginSocketClient : WebSocketApiClient, IBinanceMarginSocketClient
{
    private ILogger Logger => _logger;

    internal BinanceMarginSocketClient(BinanceSocketApiClient root) : base(root.Logger, root.ApiOptions)
    {
        RateLimitPerConnectionPerSecond = 4;
        SetDataInterpreter(data => string.Empty, null);
    }

    public async Task<CallResult<BinanceMarginUserDataStreamSubscription>> SubscribeToUserDataStreamAsync(
        string listenToken,
        Action<WebSocketDataEvent<BinanceMarginStreamOrderUpdate>>? onOrderUpdated = null,
        Action<WebSocketDataEvent<BinanceMarginStreamOrderListUpdate>>? onOrderListUpdated = null,
        Action<WebSocketDataEvent<BinanceMarginStreamPositionsUpdate>>? onAccountUpdated = null,
        Action<WebSocketDataEvent<BinanceMarginStreamBalanceUpdate>>? onBalanceUpdated = null,
        Action<WebSocketDataEvent<BinanceMarginStreamUpdate>>? onUserDataStreamTerminated = null,
        CancellationToken ct = default)
    {
        listenToken.ValidateNotNull(nameof(listenToken));
        var request = CreateSubscriptionRequest(listenToken);
        var handler = new Action<WebSocketDataEvent<string>>(data => HandleUserDataStreamEvent(
            data,
            onOrderUpdated,
            onOrderListUpdated,
            onAccountUpdated,
            onBalanceUpdated,
            onUserDataStreamTerminated));

        var result = await base.SubscribeAsync<string>(
            BinanceAddress.Default.SpotSocketApiQueryAddress.AppendPath("ws-api/v3"),
            request,
            string.Empty,
            false,
            handler,
            ct).ConfigureAwait(false);
        if (!result)
            return result.As<BinanceMarginUserDataStreamSubscription>(null);

        return result.As(new BinanceMarginUserDataStreamSubscription(request, result.Data));
    }

    public async Task<CallResult<BinanceMarginUserDataStreamSubscription>> ExtendUserDataStreamAsync(
        BinanceMarginUserDataStreamSubscription subscription,
        string listenToken,
        CancellationToken ct = default)
    {
        if (subscription == null)
            throw new ArgumentNullException(nameof(subscription));
        listenToken.ValidateNotNull(nameof(listenToken));
        if (ct.IsCancellationRequested)
            return new CallResult<BinanceMarginUserDataStreamSubscription>(new CancellationRequestedError());

        var connection = subscription.SocketSubscription.GetConnection();
        if (!connection.Connected)
            return new CallResult<BinanceMarginUserDataStreamSubscription>(new WebError("WebSocket is not connected"));

        var replacementRequest = CreateSubscriptionRequest(listenToken);
        var result = await SendSubscriptionRequestAsync(connection, replacementRequest).ConfigureAwait(false);
        if (!result)
            return result.As<BinanceMarginUserDataStreamSubscription>(null);

        ApplySubscriptionExtension(subscription.GetRequest(), replacementRequest, result.Data);
        return result.As(subscription);
    }

    public async Task<CallResult<bool>> UnsubscribeFromUserDataStreamAsync(BinanceMarginUserDataStreamSubscription subscription, CancellationToken ct = default)
    {
        if (subscription == null)
            throw new ArgumentNullException(nameof(subscription));
        if (ct.IsCancellationRequested)
            return new CallResult<bool>(new CancellationRequestedError());

        var socketSubscription = subscription.SocketSubscription;
        var connection = socketSubscription.GetConnection();
        var localSubscription = socketSubscription.GetSubscription();
        var result = await SendUnsubscribeRequestAsync(connection, subscription.SubscriptionId).ConfigureAwait(false);
        if (!result)
            return result;

        localSubscription.Confirmed = false;
        await connection.CloseAsync(localSubscription).ConfigureAwait(false);
        return result;
    }

    public async Task<CallResult<bool>> UnsubscribeAllUserDataStreamsAsync(BinanceMarginUserDataStreamSubscription sessionSubscription, CancellationToken ct = default)
    {
        if (sessionSubscription == null)
            throw new ArgumentNullException(nameof(sessionSubscription));
        if (ct.IsCancellationRequested)
            return new CallResult<bool>(new CancellationRequestedError());

        var connection = sessionSubscription.SocketSubscription.GetConnection();
        var result = await SendUnsubscribeRequestAsync(connection, null).ConfigureAwait(false);
        if (!result)
            return result;

        foreach (var localSubscription in connection.Subscriptions
            .Where(item => item.Request is BinanceMarginUserDataStreamRequest)
            .ToArray())
        {
            localSubscription.Confirmed = false;
            await connection.CloseAsync(localSubscription).ConfigureAwait(false);
        }
        return result;
    }

    internal static BinanceMarginUserDataStreamRequest CreateSubscriptionRequest(string listenToken)
    {
        return new BinanceMarginUserDataStreamRequest
        {
            Id = ExchangeHelpers.NextId(),
            Method = "userDataStream.subscribe.listenToken",
            Params = new ParameterCollection { { "listenToken", listenToken } }
        };
    }

    internal static BinanceSocketQuery CreateUnsubscribeRequest(int? subscriptionId)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("subscriptionId", subscriptionId);
        return new BinanceSocketQuery
        {
            Id = ExchangeHelpers.NextId(),
            Method = "userDataStream.unsubscribe",
            Params = parameters
        };
    }

    internal static CallResult<BinanceMarginUserDataStreamStatus> ParseSubscriptionResponse(JToken message)
    {
        var status = message["status"]?.Value<int>();
        var subscriptionId = message["result"]?["subscriptionId"]?.Value<int>();
        var expirationTime = message["result"]?["expirationTime"]?.Value<long>();
        if (status == 200 && subscriptionId.HasValue && expirationTime.HasValue)
            return new CallResult<BinanceMarginUserDataStreamStatus>(
                new BinanceMarginUserDataStreamStatus(subscriptionId.Value, expirationTime.Value));

        var errorCode = message["error"]?["code"]?.Value<int>() ?? status ?? 0;
        var errorMessage = message["error"]?["msg"]?.Value<string>()
            ?? (status == 200 ? "Margin subscription response did not contain subscriptionId and expirationTime" : "Undefined error");
        return new CallResult<BinanceMarginUserDataStreamStatus>(new ServerError(errorCode, errorMessage));
    }

    internal static void ApplySubscriptionExtension(
        BinanceMarginUserDataStreamRequest activeRequest,
        BinanceMarginUserDataStreamRequest replacementRequest,
        BinanceMarginUserDataStreamStatus status)
    {
        activeRequest.Id = replacementRequest.Id;
        activeRequest.Params = replacementRequest.Params;
        activeRequest.SubscriptionId = status.SubscriptionId;
        activeRequest.ExpirationTime = status.ExpirationTime;
    }

    internal void HandleUserDataStreamEvent(
        WebSocketDataEvent<string> data,
        Action<WebSocketDataEvent<BinanceMarginStreamOrderUpdate>>? onOrderUpdated,
        Action<WebSocketDataEvent<BinanceMarginStreamOrderListUpdate>>? onOrderListUpdated,
        Action<WebSocketDataEvent<BinanceMarginStreamPositionsUpdate>>? onAccountUpdated,
        Action<WebSocketDataEvent<BinanceMarginStreamBalanceUpdate>>? onBalanceUpdated,
        Action<WebSocketDataEvent<BinanceMarginStreamUpdate>>? onUserDataStreamTerminated)
    {
        var envelope = JToken.Parse(data.Data);
        var subscriptionId = envelope["subscriptionId"]?.Value<int>();
        var eventToken = envelope["event"];
        var eventType = eventToken?["e"]?.Value<string>();
        if (subscriptionId == null || eventToken == null || eventType == null)
        {
            Logger.Log(LogLevel.Warning, "Received invalid Margin user data stream envelope: " + data.Data);
            return;
        }

        switch (eventType)
        {
            case "executionReport":
                DispatchEvent(eventToken, subscriptionId.Value, data, onOrderUpdated, "order", result => result.Id.ToString(BinanceConstants.CI));
                break;
            case "listStatus":
                DispatchEvent(eventToken, subscriptionId.Value, data, onOrderListUpdated, "order list", result => result.Id.ToString(BinanceConstants.CI));
                break;
            case "outboundAccountPosition":
                DispatchEvent(eventToken, subscriptionId.Value, data, onAccountUpdated, "account position");
                break;
            case "balanceUpdate":
                DispatchEvent(eventToken, subscriptionId.Value, data, onBalanceUpdated, "balance", result => result.Asset);
                break;
            case "eventStreamTerminated":
                DispatchEvent(eventToken, subscriptionId.Value, data, onUserDataStreamTerminated, "stream termination");
                break;
            default:
                Logger.Log(LogLevel.Warning, $"Received unknown Margin user data event {eventType}: {data.Data}");
                break;
        }
    }

    protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
        => new BinanceAuthentication(credentials);

    protected override bool HandleQueryResponse<T>(WebSocketConnection connection, object request, JToken data, out CallResult<T>? callResult)
    {
        callResult = null;
        return false;
    }

    protected override bool HandleSubscriptionResponse(WebSocketConnection connection, WebSocketSubscription subscription, object request, JToken data, out CallResult<object>? callResult)
    {
        callResult = null;
        if (request is not BinanceMarginUserDataStreamRequest marginRequest
            || data.Type != JTokenType.Object
            || data["id"]?.Value<int>() != marginRequest.Id)
            return false;

        var response = ParseSubscriptionResponse(data);
        if (!response)
        {
            callResult = new CallResult<object>(response.Error!);
            return true;
        }

        marginRequest.SubscriptionId = response.Data.SubscriptionId;
        marginRequest.ExpirationTime = response.Data.ExpirationTime;
        callResult = new CallResult<object>(new object());
        return true;
    }

    protected override bool MessageMatchesHandler(WebSocketConnection connection, JToken message, object request)
    {
        return request is BinanceMarginUserDataStreamRequest marginRequest
            && marginRequest.SubscriptionId.HasValue
            && message["subscriptionId"]?.Value<int>() == marginRequest.SubscriptionId.Value
            && message["event"] != null;
    }

    protected override bool MessageMatchesHandler(WebSocketConnection connection, JToken message, string identifier) => false;

    protected override Task<CallResult<bool>> AuthenticateAsync(WebSocketConnection connection)
        => Task.FromResult(new CallResult<bool>(true));

    protected override async Task<bool> UnsubscribeAsync(WebSocketConnection connection, WebSocketSubscription subscription)
    {
        if (subscription.Request is not BinanceMarginUserDataStreamRequest request)
            return false;
        var result = await SendUnsubscribeRequestAsync(connection, request.SubscriptionId).ConfigureAwait(false);
        return result.Success;
    }

    private void DispatchEvent<T>(
        JToken eventToken,
        int subscriptionId,
        WebSocketDataEvent<string> source,
        Action<WebSocketDataEvent<T>>? handler,
        string eventName,
        Func<T, string>? topic = null)
        where T : BinanceMarginStreamUpdate
    {
        if (handler == null)
            return;

        var result = Deserialize<T>(eventToken);
        if (!result)
        {
            Logger.Log(LogLevel.Warning, $"Could not deserialize Margin user data {eventName} event: {result.Error}");
            return;
        }

        result.Data.SubscriptionId = subscriptionId;
        handler(topic == null ? source.As(result.Data) : source.As(result.Data, topic(result.Data)));
    }

    private async Task<CallResult<BinanceMarginUserDataStreamStatus>> SendSubscriptionRequestAsync(WebSocketConnection connection, BinanceMarginUserDataStreamRequest request)
    {
        var response = new CallResult<BinanceMarginUserDataStreamStatus>(new ServerError("No response on Margin subscription request received"));
        await connection.SendAndWaitAsync(request, ClientOptions.ResponseTimeout, data =>
        {
            if (data.Type != JTokenType.Object || data["id"]?.Value<int>() != request.Id)
                return false;
            response = ParseSubscriptionResponse(data);
            return true;
        }).ConfigureAwait(false);
        return response;
    }

    private async Task<CallResult<bool>> SendUnsubscribeRequestAsync(WebSocketConnection connection, int? subscriptionId)
    {
        if (!connection.Connected)
            return new CallResult<bool>(true);

        var request = CreateUnsubscribeRequest(subscriptionId);
        var response = new CallResult<bool>(new ServerError("No response on Margin unsubscribe request received"));
        await connection.SendAndWaitAsync(request, ClientOptions.ResponseTimeout, data =>
        {
            if (data.Type != JTokenType.Object || data["id"]?.Value<int>() != request.Id)
                return false;

            var status = data["status"]?.Value<int>();
            if (status == 200)
            {
                response = new CallResult<bool>(true);
                return true;
            }

            var errorCode = data["error"]?["code"]?.Value<int>() ?? status ?? 0;
            var errorMessage = data["error"]?["msg"]?.Value<string>() ?? "Undefined error";
            response = new CallResult<bool>(new ServerError(errorCode, errorMessage));
            return true;
        }).ConfigureAwait(false);
        return response;
    }
}
