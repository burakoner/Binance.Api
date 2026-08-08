namespace Binance.Api.Spot;

internal partial class BinanceSpotSocketClient
{
    public async Task<CallResult<BinanceSpotUserDataStreamSubscription>> SubscribeToUserDataStreamAsync(
        Action<WebSocketDataEvent<BinanceSpotStreamOrderUpdate>>? onOrderUpdated = null,
        Action<WebSocketDataEvent<BinanceSpotStreamOrderListUpdate>>? onOrderListUpdated = null,
        Action<WebSocketDataEvent<BinanceSpotStreamPositionsUpdate>>? onAccountUpdated = null,
        Action<WebSocketDataEvent<BinanceSpotStreamBalanceUpdate>>? onBalanceUpdated = null,
        Action<WebSocketDataEvent<BinanceSpotStreamExternalLockUpdate>>? onBalanceLockUpdated = null,
        Action<WebSocketDataEvent<BinanceSpotStreamUpdate>>? onUserDataStreamTerminated = null,
        decimal? receiveWindow = null,
        CancellationToken ct = default)
    {
        ValidateUserDataStreamReceiveWindow(receiveWindow);
        if (AuthenticationProvider == null)
            throw new InvalidOperationException("No credentials provided for authenticated endpoint");

        var request = new BinanceSpotUserDataStreamRequest
        {
            Method = "userDataStream.subscribe.signature",
            ReceiveWindow = receiveWindow
        };

        var refreshResult = await RefreshUserDataStreamRequestAsync(request).ConfigureAwait(false);
        if (!refreshResult)
            return refreshResult.As<BinanceSpotUserDataStreamSubscription>(null);

        var handler = new Action<WebSocketDataEvent<string>>(data => HandleUserDataStreamEvent(
            data,
            onOrderUpdated,
            onOrderListUpdated,
            onAccountUpdated,
            onBalanceUpdated,
            onBalanceLockUpdated,
            onUserDataStreamTerminated));

        var result = await base.SubscribeAsync<string>(
            BinanceAddress.Default.SpotSocketApiQueryAddress.AppendPath("ws-api/v3"),
            request,
            string.Empty,
            false,
            handler,
            ct).ConfigureAwait(false);

        if (!result)
            return result.As<BinanceSpotUserDataStreamSubscription>(null);

        return result.As(new BinanceSpotUserDataStreamSubscription(request, result.Data));
    }

    public async Task<CallResult<bool>> UnsubscribeFromUserDataStreamAsync(BinanceSpotUserDataStreamSubscription subscription, CancellationToken ct = default)
    {
        if (subscription == null)
            throw new ArgumentNullException(nameof(subscription));
        if (ct.IsCancellationRequested)
            return new CallResult<bool>(new CancellationRequestedError());

        var socketSubscription = subscription.SocketSubscription;
        var connection = socketSubscription.GetConnection();
        var localSubscription = socketSubscription.GetSubscription();
        var result = await SendUserDataStreamUnsubscribeAsync(connection, subscription.SubscriptionId).ConfigureAwait(false);
        if (!result)
            return result;

        localSubscription.Confirmed = false;
        await connection.CloseAsync(localSubscription).ConfigureAwait(false);
        return result;
    }

    public async Task<CallResult<bool>> UnsubscribeAllUserDataStreamsAsync(BinanceSpotUserDataStreamSubscription sessionSubscription, CancellationToken ct = default)
    {
        if (sessionSubscription == null)
            throw new ArgumentNullException(nameof(sessionSubscription));
        if (ct.IsCancellationRequested)
            return new CallResult<bool>(new CancellationRequestedError());

        var connection = sessionSubscription.SocketSubscription.GetConnection();
        var result = await SendUserDataStreamUnsubscribeAsync(connection, null).ConfigureAwait(false);
        if (!result)
            return result;

        foreach (var localSubscription in connection.Subscriptions
            .Where(item => item.Request is BinanceSpotUserDataStreamRequest)
            .ToArray())
        {
            localSubscription.Confirmed = false;
            await connection.CloseAsync(localSubscription).ConfigureAwait(false);
        }
        return result;
    }

    internal BinanceSpotUserDataStreamRequest CreateUserDataStreamRequest(decimal? receiveWindow, long timestamp)
    {
        ValidateUserDataStreamReceiveWindow(receiveWindow);
        if (AuthenticationProvider == null)
            throw new InvalidOperationException("No credentials provided for authenticated endpoint");

        var request = new BinanceSpotUserDataStreamRequest
        {
            Method = "userDataStream.subscribe.signature",
            ReceiveWindow = receiveWindow
        };
        RefreshUserDataStreamRequest(request, timestamp);
        return request;
    }

    internal static BinanceSocketQuery CreateUserDataStreamUnsubscribeRequest(int? subscriptionId)
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

    internal static CallResult<int> ParseUserDataStreamSubscriptionResponse(JToken message)
    {
        var status = message["status"]?.Value<int>();
        var subscriptionId = message["result"]?["subscriptionId"]?.Value<int>();
        if (status == 200 && subscriptionId.HasValue)
            return new CallResult<int>(subscriptionId.Value);

        var errorCode = message["error"]?["code"]?.Value<int>() ?? status ?? 0;
        var errorMessage = message["error"]?["msg"]?.Value<string>()
            ?? (status == 200 ? "User data subscription response did not contain subscriptionId" : "Undefined error");
        return new CallResult<int>(new ServerError(errorCode, errorMessage));
    }

    internal static void ValidateUserDataStreamReceiveWindow(decimal? receiveWindow)
    {
        if (receiveWindow > 60_000m)
            throw new ArgumentOutOfRangeException(nameof(receiveWindow), "Receive window cannot exceed 60000 milliseconds.");
        if (receiveWindow.HasValue && (decimal.GetBits(receiveWindow.Value)[3] >> 16 & 0xFF) > 3)
            throw new ArgumentException("Receive window supports at most three decimal places.", nameof(receiveWindow));
    }

    internal void HandleUserDataStreamEvent(
        WebSocketDataEvent<string> data,
        Action<WebSocketDataEvent<BinanceSpotStreamOrderUpdate>>? onOrderUpdated,
        Action<WebSocketDataEvent<BinanceSpotStreamOrderListUpdate>>? onOrderListUpdated,
        Action<WebSocketDataEvent<BinanceSpotStreamPositionsUpdate>>? onAccountUpdated,
        Action<WebSocketDataEvent<BinanceSpotStreamBalanceUpdate>>? onBalanceUpdated,
        Action<WebSocketDataEvent<BinanceSpotStreamExternalLockUpdate>>? onBalanceLockUpdated,
        Action<WebSocketDataEvent<BinanceSpotStreamUpdate>>? onUserDataStreamTerminated)
    {
        var envelope = JToken.Parse(data.Data);
        var subscriptionId = envelope["subscriptionId"]?.Value<int>();
        var eventToken = envelope["event"];
        var eventType = eventToken?["e"]?.Value<string>();
        if (subscriptionId == null || eventToken == null || eventType == null)
        {
            Logger.Log(LogLevel.Warning, "Received invalid Spot user data stream envelope: " + data.Data);
            return;
        }

        switch (eventType)
        {
            case "outboundAccountPosition":
                DispatchUserDataEvent(eventToken, subscriptionId.Value, data, onAccountUpdated, "account position");
                break;
            case "balanceUpdate":
                DispatchUserDataEvent(eventToken, subscriptionId.Value, data, onBalanceUpdated, "balance", result => result.Asset);
                break;
            case "externalLockUpdate":
                DispatchUserDataEvent(eventToken, subscriptionId.Value, data, onBalanceLockUpdated, "balance lock", result => result.Asset);
                break;
            case "executionReport":
                DispatchUserDataEvent(eventToken, subscriptionId.Value, data, onOrderUpdated, "order", result => result.Id.ToString(BinanceConstants.CI));
                break;
            case "listStatus":
                DispatchUserDataEvent(eventToken, subscriptionId.Value, data, onOrderListUpdated, "order list", result => result.Id.ToString(BinanceConstants.CI));
                break;
            case "eventStreamTerminated":
                DispatchUserDataEvent(eventToken, subscriptionId.Value, data, onUserDataStreamTerminated, "stream termination");
                break;
            default:
                Logger.Log(LogLevel.Warning, $"Received unknown Spot user data event {eventType}: {data.Data}");
                break;
        }
    }

    private async Task<CallResult<bool>> RefreshUserDataStreamRequestAsync(BinanceSpotUserDataStreamRequest request)
    {
        var syncResult = await SyncTimeAsync().ConfigureAwait(false);
        if (!syncResult)
            return syncResult;
        if (AuthenticationProvider == null)
            throw new InvalidOperationException("No credentials provided for authenticated endpoint");

        RefreshUserDataStreamRequest(request, DateTime.UtcNow.Add(GetTimeOffset()).ConvertToMilliseconds());
        return new CallResult<bool>(true);
    }

    internal void RefreshUserDataStreamRequest(BinanceSpotUserDataStreamRequest request, long timestamp)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", request.ReceiveWindow);
        request.Params = ((BinanceAuthentication)AuthenticationProvider!).AuthenticateSocketParameters(parameters, timestamp);
        request.Id = ExchangeHelpers.NextId();
        request.SubscriptionId = null;
    }

    private void DispatchUserDataEvent<T>(
        JToken eventToken,
        int subscriptionId,
        WebSocketDataEvent<string> source,
        Action<WebSocketDataEvent<T>>? handler,
        string eventName,
        Func<T, string>? topic = null)
        where T : BinanceSpotUserDataStreamEvent
    {
        if (handler == null)
            return;

        var result = Deserialize<T>(eventToken);
        if (!result)
        {
            Logger.Log(LogLevel.Warning, $"Could not deserialize Spot user data {eventName} event: {result.Error}");
            return;
        }

        result.Data.SubscriptionId = subscriptionId;
        handler(topic == null ? source.As(result.Data) : source.As(result.Data, topic(result.Data)));
    }

    private async Task<CallResult<bool>> SendUserDataStreamUnsubscribeAsync(WebSocketConnection connection, int? subscriptionId)
    {
        if (!connection.Connected)
            return new CallResult<bool>(true);

        var request = CreateUserDataStreamUnsubscribeRequest(subscriptionId);
        var response = new CallResult<bool>(new ServerError("No response on user data stream unsubscribe request received"));
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
