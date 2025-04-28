using Binance.Api.Futures;
using Binance.Api.Models.WebSocketApi.Futures;

namespace Binance.Api.Clients.StreamApi.CoinFutures;

public class BinanceStreamCoinFuturesMarketDataClient
{
    private const string klineStreamEndpoint = "@kline";
    private const string markPriceStreamEndpoint = "@markPrice";
    private const string indexPriceStreamEndpoint = "@indexPrice";
    private const string continuousKlineStreamEndpoint = "@continuousKline";
    private const string indexKlineStreamEndpoint = "@indexPriceKline";
    private const string markKlineStreamEndpoint = "@markPriceKline";
    private const string symbolMiniTickerStreamEndpoint = "@miniTicker";
    private const string allMiniTickerStreamEndpoint = "!miniTicker@arr";
    private const string symbolTickerStreamEndpoint = "@ticker";
    private const string allTickerStreamEndpoint = "!ticker@arr";

    private const string aggregatedTradesStreamEndpoint = "@aggTrade";
    private const string bookTickerStreamEndpoint = "@bookTicker";
    private const string allBookTickerStreamEndpoint = "!bookTicker";
    private const string liquidationStreamEndpoint = "@forceOrder";
    private const string allLiquidationStreamEndpoint = "!forceOrder@arr";
    private const string partialBookDepthStreamEndpoint = "@depth";
    private const string depthStreamEndpoint = "@depth";

    // Internal References
    internal BinanceStreamCoinFuturesClient MainClient { get; }
    internal ILogger Log { get => MainClient.Logger; }
    internal string BaseAddress { get => Options.BaseAddress; }
    internal BinanceSocketApiClientOptions Options { get => MainClient.RootClient.SocketOptions; }
    internal CallResult<T> Deserialize<T>(string data, JsonSerializer serializer = null, int? requestId = null) => MainClient.Deserializer<T>(data, serializer, requestId);
    internal CallResult<T> Deserialize<T>(JToken obj, JsonSerializer serializer = null, int? requestId = null) => MainClient.Deserializer<T>(obj, serializer, requestId);
    internal Task<CallResult<WebSocketUpdateSubscription>> SubscribeAsync<T>(string url, IEnumerable<string> topics, Action<WebSocketDataEvent<T>> onData, CancellationToken ct)
    => MainClient.SubscribeAsync<T>(url, topics, onData, ct);

    internal BinanceStreamCoinFuturesMarketDataClient(BinanceStreamCoinFuturesClient main)
    {
        MainClient = main;
    }

    #region Kline/Candlestick Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, BinanceKlineInterval interval, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default) => await SubscribeToKlineUpdatesAsync(new[] { symbol }, new[] { interval }, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default) => await SubscribeToKlineUpdatesAsync(new[] { symbol }, intervals, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, BinanceKlineInterval interval, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default)
        => await SubscribeToKlineUpdatesAsync(symbols, new[] { interval }, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceFuturesStreamCoinKlineData>>>(data =>
        {
            var result = data.Data.Data;
            onMessage(data.As<IBinanceStreamKlineData>(result, result.Symbol));
        });
        symbols = symbols.SelectMany(a => intervals.Select(i => a.ToLower(CultureInfo.InvariantCulture) + klineStreamEndpoint + "_" + MapConverter.GetString(i))).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Index Price Stream
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexPriceUpdatesAsync(string pair, int? updateInterval, Action<WebSocketDataEvent<IEnumerable<BinanceFuturesStreamIndexPrice>>> onMessage, CancellationToken ct = default) => await SubscribeToIndexPriceUpdatesAsync(new[] { pair }, updateInterval, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexPriceUpdatesAsync(IEnumerable<string> pairs, int? updateInterval, Action<WebSocketDataEvent<IEnumerable<BinanceFuturesStreamIndexPrice>>> onMessage, CancellationToken ct = default)
    {
        pairs.ValidateNotNull(nameof(pairs));
        updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

        var internalHandler = new Action<WebSocketDataEvent<JToken>>(data => HandlePossibleSingleData(data, onMessage));
        pairs = pairs.Select(a => a.ToLower(CultureInfo.InvariantCulture) + indexPriceStreamEndpoint + (updateInterval == 1000 ? "@1s" : "")).ToArray();
        return await SubscribeAsync(BaseAddress, pairs, internalHandler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Mark Price Stream
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(string symbol, int? updateInterval, Action<WebSocketDataEvent<IEnumerable<BinanceFuturesCoinStreamMarkPrice>>> onMessage, CancellationToken ct = default) => await SubscribeToMarkPriceUpdatesAsync(new[] { symbol }, updateInterval, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(IEnumerable<string> symbols, int? updateInterval, Action<WebSocketDataEvent<IEnumerable<BinanceFuturesCoinStreamMarkPrice>>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

        var internalHandler = new Action<WebSocketDataEvent<JToken>>(data => HandlePossibleSingleData(data, onMessage));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + markPriceStreamEndpoint + (updateInterval == 1000 ? "@1s" : "")).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, internalHandler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Continuous contract kline/Candlestick Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(string pair, BinanceFuturesContractType contractType, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceStreamKlineData>> onMessage, CancellationToken ct = default) => await SubscribeToContinuousContractKlineUpdatesAsync(new[] { pair }, contractType, interval, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(IEnumerable<string> pairs, BinanceFuturesContractType contractType, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceStreamKlineData>> onMessage, CancellationToken ct = default)
    {
        pairs.ValidateNotNull(nameof(pairs));
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamKlineData>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        pairs = pairs.Select(a => a.ToLower(CultureInfo.InvariantCulture) +
                                  "_" +
                                  MapConverter.GetString(contractType).ToLower() +
                                  continuousKlineStreamEndpoint +
                                  "_" +
                                  MapConverter.GetString(interval)).ToArray();
        return await SubscribeAsync(BaseAddress, pairs, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Index kline/Candlestick Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexKlineUpdatesAsync(string pair, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceStreamIndexKlineData>> onMessage, CancellationToken ct = default) => await SubscribeToIndexKlineUpdatesAsync(new[] { pair }, interval, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexKlineUpdatesAsync(IEnumerable<string> pairs, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceStreamIndexKlineData>> onMessage, CancellationToken ct = default)
    {
        pairs.ValidateNotNull(nameof(pairs));
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamIndexKlineData>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        pairs = pairs.Select(a => a.ToLower(CultureInfo.InvariantCulture) +
                                  indexKlineStreamEndpoint +
                                  "_" +
                                  MapConverter.GetString(interval)).ToArray();
        return await SubscribeAsync(BaseAddress, pairs, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Mark price kline/Candlestick Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceKlineUpdatesAsync(string symbol, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceStreamIndexKlineData>> onMessage, CancellationToken ct = default) => await SubscribeToMarkPriceKlineUpdatesAsync(new[] { symbol }, interval, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceKlineUpdatesAsync(IEnumerable<string> symbols, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceStreamIndexKlineData>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamIndexKlineData>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) +
                                      markKlineStreamEndpoint +
                                     "_" +
                                     MapConverter.GetString(interval)).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Individual Symbol Mini Ticker Stream
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(string symbol, Action<WebSocketDataEvent<IBinanceMiniTick>> onMessage, CancellationToken ct = default) => await SubscribeToMiniTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<IBinanceMiniTick>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamCoinMiniTick>>>(data => onMessage(data.As<IBinanceMiniTick>(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + symbolMiniTickerStreamEndpoint).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region All Market Mini Tickers Stream
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllMiniTickerUpdatesAsync(Action<WebSocketDataEvent<IEnumerable<IBinanceMiniTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<IEnumerable<BinanceStreamCoinMiniTick>>>>(data => onMessage(data.As<IEnumerable<IBinanceMiniTick>>(data.Data.Data, data.Data.Stream)));
        return await SubscribeAsync(BaseAddress, new[] { allMiniTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Individual Symbol Ticker Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<WebSocketDataEvent<IBinance24HPrice>> onMessage, CancellationToken ct = default) => await SubscribeToTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<IBinance24HPrice>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamCoinTick>>>(data => onMessage(data.As<IBinance24HPrice>(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + symbolTickerStreamEndpoint).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region All Market Tickers Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<WebSocketDataEvent<IEnumerable<IBinance24HPrice>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<IEnumerable<BinanceStreamCoinTick>>>>(data => onMessage(data.As<IEnumerable<IBinance24HPrice>>(data.Data.Data, data.Data.Stream)));
        return await SubscribeAsync(BaseAddress, new[] { allTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Aggregate Trade Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceStreamAggregatedTrade>> onMessage, CancellationToken ct = default) => await SubscribeToAggregatedTradeUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceStreamAggregatedTrade>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamAggregatedTrade>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + aggregatedTradesStreamEndpoint).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Individual Symbol Book Ticker Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickerUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default) => await SubscribeToBookTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickerUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceFuturesStreamBookPrice>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + bookTickerStreamEndpoint).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region All Book Tickers Stream
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllBookTickerUpdatesAsync(Action<WebSocketDataEvent<BinanceStreamBookPrice>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamBookPrice>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        return await SubscribeAsync(BaseAddress, new[] { allBookTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Liquidation Order Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default) => await SubscribeToLiquidationUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceFuturesStreamLiquidationData>>>(data => onMessage(data.As(data.Data.Data.Data, data.Data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + liquidationStreamEndpoint).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }

    #endregion

    #region All Market Liquidation Order Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllLiquidationUpdatesAsync(Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceFuturesStreamLiquidationData>>>(data => onMessage(data.As(data.Data.Data.Data, data.Data.Data.Data.Symbol)));
        return await SubscribeAsync(BaseAddress, new[] { allLiquidationStreamEndpoint }, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Partial Book Depth Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, int levels, int? updateInterval, Action<WebSocketDataEvent<IBinanceFuturesEventOrderBook>> onMessage, CancellationToken ct = default) => await SubscribeToPartialOrderBookUpdatesAsync(new[] { symbol }, levels, updateInterval, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(IEnumerable<string> symbols, int levels, int? updateInterval, Action<WebSocketDataEvent<IBinanceFuturesEventOrderBook>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        levels.ValidateIntValues(nameof(levels), 5, 10, 20);
        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 250, 500);

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceFuturesStreamOrderBookDepth>>>(data =>
        {
            data.Data.Data.Symbol = data.Data.Stream.Split('@')[0];
            onMessage(data.As<IBinanceFuturesEventOrderBook>(data.Data.Data, data.Data.Data.Symbol));
        });

        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + partialBookDepthStreamEndpoint + levels + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Diff. Book Depth Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int? updateInterval, Action<WebSocketDataEvent<IBinanceFuturesEventOrderBook>> onMessage, CancellationToken ct = default) => await SubscribeToOrderBookUpdatesAsync(new[] { symbol }, updateInterval, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int? updateInterval, Action<WebSocketDataEvent<IBinanceFuturesEventOrderBook>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 250, 500);
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceFuturesStreamOrderBookDepth>>>(data => onMessage(data.As<IBinanceFuturesEventOrderBook>(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + depthStreamEndpoint + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    private void HandlePossibleSingleData<T>(WebSocketDataEvent<JToken> data, Action<WebSocketDataEvent<IEnumerable<T>>> onMessage)
    {
        var internalData = data.Data["data"];
        if (internalData == null)
            return;
        if (internalData.Type == JTokenType.Array)
        {
            var firstItemTopic = internalData.First()["i"]?.ToString() ?? internalData.First()["s"]?.ToString();
            var deserialized = Deserialize<BinanceCombinedStream<IEnumerable<T>>>(data.Data);
            if (!deserialized)
                return;
            onMessage(data.As(deserialized.Data.Data, firstItemTopic));
        }
        else
        {
            var symbol = internalData["i"]?.ToString() ?? internalData["s"]?.ToString();
            var deserialized = Deserialize<BinanceCombinedStream<T>>(
                    data.Data);
            if (!deserialized)
                return;
            onMessage(data.As<IEnumerable<T>>(new[] { deserialized.Data.Data }, symbol));
        }
    }

}