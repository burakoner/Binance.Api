namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientCoin
{
    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToUserDataUpdatesAsync(
        string listenKey,
        Action<WebSocketDataEvent<BinanceFuturesStreamAccountUpdate>>? onAccountUpdated = null,
        Action<WebSocketDataEvent<BinanceFuturesStreamConfigUpdate>>? onLeverageUpdated = null,
        Action<WebSocketDataEvent<BinanceFuturesStreamMarginUpdate>>? onMarginUpdated = null,
        Action<WebSocketDataEvent<BinanceFuturesStreamOrderUpdate>>? onOrderUpdated = null,

        Action<WebSocketDataEvent<BinanceFuturesStreamStrategyUpdate>>? onStrategyUpdated = null,
        Action<WebSocketDataEvent<BinanceFuturesStreamGridUpdate>>? onGridUpdated = null,
        Action<WebSocketDataEvent<BinanceFuturesStreamUpdate>>? onListenKeyExpired = null,

        CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));

        var handler = new Action<WebSocketDataEvent<string>>(data =>
        {
            var combinedToken = JToken.Parse(data.Data);
            var token = combinedToken["data"];
            if (token == null) return;

            var evnt = token["e"]?.ToString();
            if (evnt == null) return;

            switch (evnt)
            {
                // Account Update
                case "ACCOUNT_UPDATE":
                    {
                        var result = Deserialize<BinanceFuturesStreamAccountUpdate>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onAccountUpdated?.Invoke(data.As(result.Data));
                        }
                        else Logger.Log(LogLevel.Warning, "Couldn't deserialize data received from account position stream: {Error}", result.Error);
                        break;
                    }

                // Account Config Update
                case "ACCOUNT_CONFIG_UPDATE":
                    {
                        var result = Deserialize<BinanceFuturesStreamConfigUpdate>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onLeverageUpdated?.Invoke(data.As(result.Data));
                        }
                        else Logger.Log(LogLevel.Warning, "Couldn't deserialize data received from account position stream: {Error}", result.Error);
                        break;
                    }

                // Margin Update
                case "MARGIN_CALL":
                    {
                        var result = Deserialize<BinanceFuturesStreamMarginUpdate>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onMarginUpdated?.Invoke(data.As(result.Data));
                        }
                        else Logger.Log(LogLevel.Warning, "Couldn't deserialize data received from account position stream: {Error}", result.Error);
                        break;
                    }

                // Order Update
                case "ORDER_TRADE_UPDATE":
                    {
                        var result = Deserialize<BinanceFuturesStreamOrderUpdate>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onOrderUpdated?.Invoke(data.As(result.Data));
                        }
                        else Logger.Log(LogLevel.Warning, "Couldn't deserialize data received from order stream: {Error}", result.Error);
                        break;
                    }

                // Strategy Update
                case "STRATEGY_UPDATE":
                    {
                        var result = Deserialize<BinanceFuturesStreamStrategyUpdate>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onStrategyUpdated?.Invoke(data.As(result.Data));
                        }
                        else Logger.Log(LogLevel.Warning, "Couldn't deserialize data received from oco order stream: {Error}", result.Error);
                        break;
                    }

                // Grid Update
                case "GRID_UPDATE":
                    {
                        var result = Deserialize<BinanceFuturesStreamGridUpdate>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onGridUpdated?.Invoke(data.As(result.Data));
                        }
                        else Logger.Log(LogLevel.Warning, "Couldn't deserialize data received from oco order stream: {Error}", result.Error);
                        break;
                    }

                // Listen Key Expired
                case "listenKeyExpired":
                    {
                        var result = Deserialize<BinanceFuturesStreamUpdate>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onListenKeyExpired?.Invoke(data.As(result.Data));
                        }
                        else Logger.Log(LogLevel.Warning, "Couldn't deserialize data received from oco order stream: {Error}", result.Error);
                        break;
                    }

                // Default
                default:
                    Logger.Log(LogLevel.Warning, "Received unknown user data event {Event}: {Data}", evnt, data);
                    break;
            }
        });

        return SubscribeAsync([listenKey], false, handler, ct);
    }
}
