using Binance.Api.Futures;
using Binance.Api.Models.WebSocketApi.Futures;

namespace Binance.Api.Clients.StreamApi.UsdtFutures;

public class BinanceStreamUsdtFuturesMarketDataClient
{
    private const string klineStreamEndpoint = "@kline";
    private const string continuousContractKlineStreamEndpoint = "@continuousKline";
    private const string markPriceStreamEndpoint = "@markPrice";
    private const string allMarkPriceStreamEndpoint = "!markPrice@arr";
    private const string symbolMiniTickerStreamEndpoint = "@miniTicker";
    private const string allMiniTickerStreamEndpoint = "!miniTicker@arr";
    private const string symbolTickerStreamEndpoint = "@ticker";
    private const string allTickerStreamEndpoint = "!ticker@arr";
    private const string compositeIndexEndpoint = "@compositeIndex";

    private const string aggregatedTradesStreamEndpoint = "@aggTrade";
    private const string bookTickerStreamEndpoint = "@bookTicker";
    private const string allBookTickerStreamEndpoint = "!bookTicker";
    private const string liquidationStreamEndpoint = "@forceOrder";
    private const string allLiquidationStreamEndpoint = "!forceOrder@arr";
    private const string partialBookDepthStreamEndpoint = "@depth";
    private const string depthStreamEndpoint = "@depth";

    // Internal References
    internal BinanceStreamUsdtFuturesClient MainClient { get; }
    internal ILogger Logger { get => MainClient.Logger; }
    internal string BaseAddress { get => Options.BaseAddress; }
    internal BinanceSocketApiClientOptions Options { get => MainClient.RootClient.SocketOptions; }
    internal CallResult<T> Deserialize<T>(string data, JsonSerializer serializer = null, int? requestId = null) => MainClient.Deserializer<T>(data, serializer, requestId);
    internal CallResult<T> Deserialize<T>(JToken obj, JsonSerializer serializer = null, int? requestId = null) => MainClient.Deserializer<T>(obj, serializer, requestId);
    internal Task<CallResult<WebSocketUpdateSubscription>> SubscribeAsync<T>(string url, IEnumerable<string> topics, Action<WebSocketDataEvent<T>> onData, CancellationToken ct)
    => MainClient.SubscribeAsync<T>(url, topics, onData, ct);

    internal BinanceStreamUsdtFuturesMarketDataClient(BinanceStreamUsdtFuturesClient main)
    {
        MainClient = main;
    }

    #region Mark Price Stream
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(string symbol, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesUsdStreamMarkPrice>> onMessage, CancellationToken ct = default) => await SubscribeToMarkPriceUpdatesAsync(new[] { symbol }, updateInterval, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(IEnumerable<string> symbols, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesUsdStreamMarkPrice>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceFuturesUsdStreamMarkPrice>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + markPriceStreamEndpoint + (updateInterval == 1000 ? "@1s" : "")).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Mark Price Stream for All market
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllMarkPriceUpdatesAsync(int? updateInterval, Action<WebSocketDataEvent<IEnumerable<BinanceFuturesStreamMarkPrice>>> onMessage, CancellationToken ct = default)
    {
        updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<IEnumerable<BinanceFuturesStreamMarkPrice>>>>(data => onMessage(data.As(data.Data.Data, data.Data.Stream)));
        return await SubscribeAsync(BaseAddress, new[] { allMarkPriceStreamEndpoint + (updateInterval == 1000 ? "@1s" : "") }, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Kline/Candlestick Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, BinanceKlineInterval interval, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default) => await SubscribeToKlineUpdatesAsync(new[] { symbol }, interval, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default) => await SubscribeToKlineUpdatesAsync(new[] { symbol }, intervals, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, BinanceKlineInterval interval, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default) =>
        await SubscribeToKlineUpdatesAsync(symbols, new[] { interval }, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamKlineData>>>(data => onMessage(data.As<IBinanceStreamKlineData>(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.SelectMany(a => intervals.Select(i => a.ToLower(CultureInfo.InvariantCulture) + klineStreamEndpoint + "_" + MapConverter.GetString(i))).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Continuous contract kline/Candlestick Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(string pair, BinanceFuturesContractType contractType, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceStreamContinuousKlineData>> onMessage, CancellationToken ct = default) => await SubscribeToContinuousContractKlineUpdatesAsync(new[] { pair }, contractType, interval, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(IEnumerable<string> pairs, BinanceFuturesContractType contractType, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceStreamContinuousKlineData>> onMessage, CancellationToken ct = default)
    {
        pairs.ValidateNotNull(nameof(pairs));
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamContinuousKlineData>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        pairs = pairs.Select(a => a.ToLower(CultureInfo.InvariantCulture) +
                                  "_" +
                                  MapConverter.GetString(contractType).ToLower() +
                                  continuousContractKlineStreamEndpoint +
                                  "_" +
                                  MapConverter.GetString(interval)).ToArray();
        return await SubscribeAsync(BaseAddress, pairs, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Individual Symbol Mini Ticker Stream
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceStreamMiniTick>> onMessage, CancellationToken ct = default) => await SubscribeToMiniTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceStreamMiniTick>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamMiniTick>>>(data => onMessage(data.As<BinanceStreamMiniTick>(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + symbolMiniTickerStreamEndpoint).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region All Market Mini Tickers Stream
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllMiniTickerUpdatesAsync(Action<WebSocketDataEvent<IEnumerable<BinanceStreamMiniTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<IEnumerable<BinanceStreamMiniTick>>>>(data => onMessage(data.As<IEnumerable<BinanceStreamMiniTick>>(data.Data.Data, data.Data.Stream)));
        return await SubscribeAsync(BaseAddress, new[] { allMiniTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Individual Symbol Ticker Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<WebSocketDataEvent<IBinance24HPrice>> onMessage, CancellationToken ct = default) => await SubscribeToTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<IBinance24HPrice>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamTick>>>(data => onMessage(data.As<IBinance24HPrice>(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + symbolTickerStreamEndpoint).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Composite index Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToCompositeIndexUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamCompositeIndex>> onMessage, CancellationToken ct = default)
    {
        var action = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceFuturesStreamCompositeIndex>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });
        return await SubscribeAsync(BaseAddress, new[] { symbol + compositeIndexEndpoint }, action, ct).ConfigureAwait(false);
    }
    #endregion

    #region All Market Tickers Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<WebSocketDataEvent<IEnumerable<IBinance24HPrice>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<IEnumerable<BinanceStreamTick>>>>(data => onMessage(data.As<IEnumerable<IBinance24HPrice>>(data.Data.Data, data.Data.Stream)));
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
}