namespace Binance.Api.Options;

internal partial class BinanceOptionsSocketClient
{
    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToUserDataStreamAsync(
        string listenKey,
        Action<WebSocketDataEvent<BinanceOptionsStreamAccount>>? onAccountUpdated = null,
        Action<WebSocketDataEvent<BinanceOptionsStreamOrder>>? onOrderUpdated = null,
        Action<WebSocketDataEvent<BinanceOptionsStreamRiskLevel>>? onRiskLevelUpdated = null,
        Action<WebSocketDataEvent<BinanceOptionsStreamBalancePosition>>? onBalancePositionUpdated = null,
        Action<WebSocketDataEvent<BinanceOptionsStreamGreek>>? onGreekUpdated = null,
        Action<WebSocketDataEvent<BinanceOptionsStreamListenKeyExpired>>? onListenKeyExpired = null,
        CancellationToken ct = default)
    {
        var address = UserDataStreamAddress(listenKey);
        var handler = new Action<WebSocketDataEvent<string>>(data =>
        {
            JToken token;
            try
            {
                token = ParseUserDataPayload(data.Data);
            }
            catch (JsonException exception)
            {
                Logger.Log(LogLevel.Warning, "Couldn't parse Options user data event: " + exception.Message);
                return;
            }

            var result = DeserializeUserDataEvent(token, listenKey);
            if (result.Data == null)
            {
                if (result.Error == null)
                    Logger.Log(LogLevel.Warning, $"Received unknown Options user data event {token["e"]}: {token}");
                else
                    Logger.Log(LogLevel.Warning, "Couldn't deserialize Options user data event: " + result.Error);
                return;
            }

            switch (result.Data)
            {
                case BinanceOptionsStreamAccount value:
                    onAccountUpdated?.Invoke(data.As(value));
                    break;
                case BinanceOptionsStreamOrder value:
                    onOrderUpdated?.Invoke(data.As(value));
                    break;
                case BinanceOptionsStreamRiskLevel value:
                    onRiskLevelUpdated?.Invoke(data.As(value));
                    break;
                case BinanceOptionsStreamBalancePosition value:
                    onBalancePositionUpdated?.Invoke(data.As(value));
                    break;
                case BinanceOptionsStreamGreek value:
                    onGreekUpdated?.Invoke(data.As(value));
                    break;
                case BinanceOptionsStreamListenKeyExpired value:
                    onListenKeyExpired?.Invoke(data.As(value));
                    break;
            }
        });

        return SubscribeAsync(address, null!, listenKey, false, handler, ct);
    }

    internal static string UserDataStreamAddress(string listenKey)
    {
        if (string.IsNullOrWhiteSpace(listenKey))
            throw new ArgumentException("listenKey must be provided", nameof(listenKey));

        return BinanceAddress.Default.EuropeanOptionsPrivateSocketApiStreamAddress
            .AppendPath("ws")
            .AppendPath(listenKey);
    }

    internal static JToken ParseUserDataPayload(string data)
    {
        var envelope = JToken.Parse(data);
        return envelope["data"] ?? envelope;
    }

    internal (object? Data, Error? Error) DeserializeUserDataEvent(JToken token, string listenKey)
    {
        switch (token["e"]?.Value<string>())
        {
            case "ACCOUNT_UPDATE":
                {
                    var result = Deserialize<BinanceOptionsStreamAccount>(token);
                    if (!result) return (null, result.Error);
                    result.Data.ListenKey = listenKey;
                    return (result.Data, null);
                }
            case "ORDER_TRADE_UPDATE":
                {
                    var result = Deserialize<BinanceOptionsStreamOrder>(token);
                    if (!result) return (null, result.Error);
                    result.Data.ListenKey = listenKey;
                    return (result.Data, null);
                }
            case "RISK_LEVEL_CHANGE":
                {
                    var result = Deserialize<BinanceOptionsStreamRiskLevel>(token);
                    if (!result) return (null, result.Error);
                    result.Data.ListenKey = listenKey;
                    return (result.Data, null);
                }
            case "BALANCE_POSITION_UPDATE":
                {
                    var result = Deserialize<BinanceOptionsStreamBalancePosition>(token);
                    if (!result) return (null, result.Error);
                    result.Data.ListenKey = listenKey;
                    return (result.Data, null);
                }
            case "GREEK_UPDATE":
                {
                    var result = Deserialize<BinanceOptionsStreamGreek>(token);
                    if (!result) return (null, result.Error);
                    result.Data.ListenKey = listenKey;
                    return (result.Data, null);
                }
            case "listenKeyExpired":
                {
                    var result = Deserialize<BinanceOptionsStreamListenKeyExpired>(token);
                    return result ? (result.Data, null) : (null, result.Error);
                }
            default:
                return (null, null);
        }
    }
}
