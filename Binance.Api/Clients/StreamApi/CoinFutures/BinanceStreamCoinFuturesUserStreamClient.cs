using Binance.Api.Models.StreamApi.Futures;

namespace Binance.Api.Clients.StreamApi.CoinFutures;

public class BinanceStreamCoinFuturesUserStreamClient
{
    private const string configUpdateEvent = "ACCOUNT_CONFIG_UPDATE";
    private const string marginUpdateEvent = "MARGIN_CALL";
    private const string accountUpdateEvent = "ACCOUNT_UPDATE";
    private const string orderUpdateEvent = "ORDER_TRADE_UPDATE";
    private const string listenKeyExpiredEvent = "listenKeyExpired";

    // Internal References
    internal BinanceStreamCoinFuturesClient MainClient { get; }
    internal Log Log { get => MainClient.Log; }
    internal string BaseAddress{ get => Options.BaseAddress; }
    internal BinanceStreamClientOptions Options { get => MainClient.RootClient.Options; }
    internal CallResult<T> Deserialize<T>(string data, JsonSerializer serializer = null, int? requestId = null) => MainClient.Deserializer<T>(data, serializer, requestId);
    internal CallResult<T> Deserialize<T>(JToken obj, JsonSerializer serializer = null, int? requestId = null) => MainClient.Deserializer<T>(obj, serializer, requestId);
    internal Task<CallResult<UpdateSubscription>> SubscribeAsync<T>(string url, IEnumerable<string> topics, Action<StreamDataEvent<T>> onData, CancellationToken ct)
    => MainClient.SubscribeAsync<T>(url, topics, onData, ct);

    internal BinanceStreamCoinFuturesUserStreamClient(BinanceStreamCoinFuturesClient main)
    {
        MainClient = main;
    }

    #region User Data Streams
    public async Task<CallResult<UpdateSubscription>> SubscribeToUserDataUpdatesAsync(
        string listenKey,
        Action<StreamDataEvent<BinanceFuturesStreamConfigUpdate>> onConfigUpdate,
        Action<StreamDataEvent<BinanceFuturesStreamMarginUpdate>> onMarginUpdate,
        Action<StreamDataEvent<BinanceFuturesStreamAccountUpdate>> onAccountUpdate,
        Action<StreamDataEvent<BinanceFuturesStreamOrderUpdate>> onOrderUpdate,
        Action<StreamDataEvent<BinanceStreamEvent>> onListenKeyExpired,
        CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));

        var handler = new Action<StreamDataEvent<string>>(data =>
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
                case configUpdateEvent:
                    {
                        var result = Deserialize<BinanceFuturesStreamConfigUpdate>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onConfigUpdate?.Invoke(data.As(result.Data, result.Data.LeverageUpdateData?.Symbol));
                        }
                        else
                            Log.Write(LogLevel.Warning, "Couldn't deserialize data received from config stream: " + result.Error);

                        break;
                    }
                case marginUpdateEvent:
                    {
                        var result = Deserialize<BinanceFuturesStreamMarginUpdate>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onMarginUpdate?.Invoke(data.As(result.Data));
                        }
                        else
                            Log.Write(LogLevel.Warning, "Couldn't deserialize data received from order stream: " + result.Error);
                        break;
                    }
                case accountUpdateEvent:
                    {
                        var result = Deserialize<BinanceFuturesStreamAccountUpdate>(token);
                        if (result.Success)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onAccountUpdate?.Invoke(data.As(result.Data));
                        }
                        else
                            Log.Write(LogLevel.Warning, "Couldn't deserialize data received from account stream: " + result.Error);

                        break;
                    }
                case orderUpdateEvent:
                    {
                        var result = Deserialize<BinanceFuturesStreamOrderUpdate>(token);
                        if (result)
                        {
                            result.Data.ListenKey = combinedToken["stream"]!.Value<string>()!;
                            onOrderUpdate?.Invoke(data.As(result.Data, result.Data.UpdateData.Symbol));
                        }
                        else
                        {
                            Log.Write(LogLevel.Warning, "Couldn't deserialize data received from order stream: " + result.Error);
                        }
                        break;
                    }
                case listenKeyExpiredEvent:
                    {
                        var result = Deserialize<BinanceStreamEvent>(token);
                        if (result)
                            onListenKeyExpired?.Invoke(data.As(result.Data, combinedToken["stream"]!.Value<string>()));
                        else
                            Log.Write(LogLevel.Warning, "Couldn't deserialize data received from the expired listen key event: " + result.Error);
                        break;
                    }
                default:
                    Log.Write(LogLevel.Warning, $"Received unknown user data event {evnt}: " + data);
                    break;
            }
        });

        return await SubscribeAsync(BaseAddress, new[] { listenKey }, handler, ct).ConfigureAwait(false);
    }
    #endregion

}