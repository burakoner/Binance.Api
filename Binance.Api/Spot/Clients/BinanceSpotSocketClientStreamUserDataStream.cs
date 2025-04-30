namespace Binance.Api.Spot;

internal partial class BinanceSpotSocketClient
{
    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToUserDataStreamAsync(
        string listenKey,
        Action<WebSocketDataEvent<BinanceSpotStreamOrderUpdate>>? onOrderUpdated = null,
        Action<WebSocketDataEvent<BinanceSpotStreamOrderListUpdate>>? onOrderListUpdated = null,
        Action<WebSocketDataEvent<BinanceSpotStreamPositionsUpdate>>? onAccountUpdated = null,
        Action<WebSocketDataEvent<BinanceSpotStreamBalanceUpdate>>? onBalanceUpdated = null,
        Action<WebSocketDataEvent<BinanceSpotStreamBalanceUpdate>>? onBalanceLockUpdated = null,
        Action<WebSocketDataEvent<BinanceSpotStreamUpdate>>? onUserDataStreamTerminated = null,
        Action<WebSocketDataEvent<BinanceSpotStreamUpdate>>? onListenKeyExpired = null,
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
                case "outboundAccountPosition":
                    {
                        var result = Deserialize<BinanceSpotStreamPositionsUpdate>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onAccountUpdated?.Invoke(data.As(result.Data));
                        }
                        else Logger.Log(LogLevel.Warning, "Couldn't deserialize data received from account position stream: " + result.Error);
                        break;
                    }

                // Balance Update
                case "balanceUpdate":
                    {
                        var result = Deserialize<BinanceSpotStreamBalanceUpdate>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onBalanceUpdated?.Invoke(data.As(result.Data, result.Data.Asset));
                        }
                        else Logger.Log(LogLevel.Warning, "Couldn't deserialize data received from account position stream: " + result.Error);
                        break;
                    }

                // Balance Lock Update
                case "externalLockUpdate":
                    {
                        var result = Deserialize<BinanceSpotStreamBalanceUpdate>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onBalanceLockUpdated?.Invoke(data.As(result.Data, result.Data.Asset));
                        }
                        else Logger.Log(LogLevel.Warning, "Couldn't deserialize data received from account position stream: " + result.Error);
                        break;
                    }

                // Order Update
                case "executionReport":
                    {
                        var result = Deserialize<BinanceSpotStreamOrderUpdate>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onOrderUpdated?.Invoke(data.As(result.Data, result.Data.Id.ToString()));
                        }
                        else Logger.Log(LogLevel.Warning, "Couldn't deserialize data received from order stream: " + result.Error);
                        break;
                    }

                // Order List Update
                case "listStatus":
                    {
                        var result = Deserialize<BinanceSpotStreamOrderListUpdate>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onOrderListUpdated?.Invoke(data.As(result.Data, result.Data.Id.ToString()));
                        }
                        else Logger.Log(LogLevel.Warning, "Couldn't deserialize data received from oco order stream: " + result.Error);
                        break;
                    }

                // Listen Key Expired
                case "listenKeyExpired":
                    {
                        var result = Deserialize<BinanceSpotStreamUpdate>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onListenKeyExpired?.Invoke(data.As(result.Data));
                        }
                        else Logger.Log(LogLevel.Warning, "Couldn't deserialize data received from oco order stream: " + result.Error);
                        break;
                    }

                // Event Stream Terminated
                case "eventStreamTerminated":
                    {
                        var result = Deserialize<BinanceSpotStreamUpdate>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onUserDataStreamTerminated?.Invoke(data.As(result.Data));
                        }
                        else Logger.Log(LogLevel.Warning, "Couldn't deserialize data received from oco order stream: " + result.Error);
                        break;
                    }

                // Default
                default:
                    Logger.Log(LogLevel.Warning, $"Received unknown user data event {evnt}: " + data);
                    break;
            }
        });

        return SubscribeAsync([listenKey], false, handler, ct);
    }
}