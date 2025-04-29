namespace Binance.Api.Spot;

internal partial class BinanceSpotSocketClient
{
    // User Stream
    private const string executionUpdateEvent = "executionReport";
    private const string ocoOrderUpdateEvent = "listStatus";
    private const string accountPositionUpdateEvent = "outboundAccountPosition";
    private const string balanceUpdateEvent = "balanceUpdate";

    /*
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToUserDataUpdatesAsync(
        string listenKey,
        Action<WebSocketDataEvent<BinanceStreamOrderUpdate>> onOrderUpdateMessage,
        Action<WebSocketDataEvent<BinanceStreamOrderList>> onOcoOrderUpdateMessage,
        Action<WebSocketDataEvent<BinanceStreamPositionsUpdate>> onAccountPositionMessage,
        Action<WebSocketDataEvent<BinanceStreamBalanceUpdate>> onAccountBalanceUpdate,
        CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));

        var handler = new Action<WebSocketDataEvent<string>>(data =>
        {
            var combinedToken = JToken.Parse(data.Data);
            var token = combinedToken["data"];
            if (token == null)
                return;

            var evnt = token["e"]?.ToString();
            if (evnt == null)
                return;

            switch (evnt)
            {
                case executionUpdateEvent:
                    {
                        var result = Deserialize<BinanceStreamOrderUpdate>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onOrderUpdateMessage?.Invoke(data.As(result.Data, result.Data.Id.ToString()));
                        }
                        else
                            MainClient.Logger.Log(LogLevel.Warning,
                                "Couldn't deserialize data received from order stream: " + result.Error);
                        break;
                    }
                case ocoOrderUpdateEvent:
                    {
                        var result = Deserialize<BinanceStreamOrderList>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onOcoOrderUpdateMessage?.Invoke(data.As(result.Data, result.Data.Id.ToString()));
                        }
                        else
                            MainClient.Logger.Log(LogLevel.Warning,
                                "Couldn't deserialize data received from oco order stream: " + result.Error);
                        break;
                    }
                case accountPositionUpdateEvent:
                    {
                        var result = Deserialize<BinanceStreamPositionsUpdate>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onAccountPositionMessage?.Invoke(data.As(result.Data));
                        }
                        else
                            MainClient.Logger.Log(LogLevel.Warning,
                                "Couldn't deserialize data received from account position stream: " + result.Error);
                        break;
                    }
                case balanceUpdateEvent:
                    {
                        var result = Deserialize<BinanceStreamBalanceUpdate>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onAccountBalanceUpdate?.Invoke(data.As(result.Data, result.Data.Asset));
                        }
                        else
                            MainClient.Logger.Log(LogLevel.Warning,
                                "Couldn't deserialize data received from account position stream: " + result.Error);
                        break;
                    }
                default:
                    MainClient.Logger.Log(LogLevel.Warning, $"Received unknown user data event {evnt}: " + data);
                    break;
            }
        });

        return await SubscribeAsync(BaseAddress, new[] { listenKey }, handler, ct).ConfigureAwait(false);
    }
    */
}