namespace Binance.Api.Options;

internal partial class BinanceOptionsSocketClient
{
    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToUserDataStreamAsync(
        string listenKey,
        Action<WebSocketDataEvent<BinanceOptionsStreamAccount>>? onAccountUpdated = null,
        Action<WebSocketDataEvent<BinanceOptionsStreamOrder>>? onOrderUpdated = null,
        Action<WebSocketDataEvent<BinanceOptionsStreamRiskLevel>>? onRiskLevelUpdated = null,
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
                case "ACCOUNT_UPDATE":
                    {
                        var result = Deserialize<BinanceOptionsStreamAccount>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onAccountUpdated?.Invoke(data.As(result.Data));
                        }
                        else Logger.Log(LogLevel.Warning, "Couldn't deserialize data received from account position stream: " + result.Error);
                        break;
                    }

                case "ORDER_TRADE_UPDATE":
                    {
                        var result = Deserialize<BinanceOptionsStreamOrder>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onOrderUpdated?.Invoke(data.As(result.Data));
                        }
                        else Logger.Log(LogLevel.Warning, "Couldn't deserialize data received from account position stream: " + result.Error);
                        break;
                    }

                case "RISK_LEVEL_CHANGE":
                    {
                        var result = Deserialize<BinanceOptionsStreamRiskLevel>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onRiskLevelUpdated?.Invoke(data.As(result.Data));
                        }
                        else Logger.Log(LogLevel.Warning, "Couldn't deserialize data received from account position stream: " + result.Error);
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