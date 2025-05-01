namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientUsd
{
    private const string _klineStreamEndpoint = "@kline";
    private const string _continuousContractKlineStreamEndpoint = "@continuousKline";
    private const string _markPriceStreamEndpoint = "@markPrice";
    private const string _allMarkPriceStreamEndpoint = "!markPrice@arr";
    private const string _symbolMiniTickerStreamEndpoint = "@miniTicker";
    private const string _allMiniTickerStreamEndpoint = "!miniTicker@arr";
    private const string _symbolTickerStreamEndpoint = "@ticker";
    private const string _allTickerStreamEndpoint = "!ticker@arr";
    private const string _compositeIndexEndpoint = "@compositeIndex";

    private const string _aggregatedTradesStreamEndpoint = "@aggTrade";
    private const string _tradesStreamEndpoint = "@trade";
    private const string _bookTickerStreamEndpoint = "@bookTicker";
    private const string _allBookTickerStreamEndpoint = "!bookTicker";
    private const string _liquidationStreamEndpoint = "@forceOrder";
    private const string _allLiquidationStreamEndpoint = "!forceOrder@arr";
    private const string _partialBookDepthStreamEndpoint = "@depth";
    private const string _depthStreamEndpoint = "@depth";

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(string symbol, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesUsdtStreamMarkPrice>> onMessage, CancellationToken ct = default) => SubscribeToMarkPriceUpdatesAsync(new[] { symbol }, updateInterval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(IEnumerable<string> symbols, int? updateInterval, Action<WebSocketDataEvent<BinanceFuturesUsdtStreamMarkPrice>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesUsdtStreamMarkPrice>>>(data =>
            onMessage(data.As(data.Data.Data)
            .WithStreamId(data.Data.Stream)
            .WithSymbol(data.Data.Data.Symbol)
            .WithDataTimestamp(data.Data.Data.EventTime)));
        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + _markPriceStreamEndpoint + (updateInterval == 1000 ? "@1s" : "")).ToArray();
        return SubscribeAsync(symbols, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllMarkPriceUpdatesAsync(int? updateInterval, Action<WebSocketDataEvent<IEnumerable<BinanceFuturesUsdtStreamMarkPrice>>> onMessage, CancellationToken ct = default)
    {
        updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<IEnumerable<BinanceFuturesUsdtStreamMarkPrice>>>>(data =>
            onMessage(data.As(data.Data.Data)
            .WithStreamId(data.Data.Stream)
            .WithDataTimestamp(data.Data.Data.Max(x => x.EventTime))));
        return SubscribeAsync(new[] { _allMarkPriceStreamEndpoint + (updateInterval == 1000 ? "@1s" : "") }, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, KlineInterval interval, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, bool premiumIndex = false, CancellationToken ct = default) => SubscribeToKlineUpdatesAsync(new[] { symbol }, interval, onMessage, premiumIndex, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, IEnumerable<KlineInterval> intervals, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, bool premiumIndex = false, CancellationToken ct = default) => SubscribeToKlineUpdatesAsync(new[] { symbol }, intervals, onMessage, premiumIndex, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, KlineInterval interval, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, bool premiumIndex = false, CancellationToken ct = default) =>
        SubscribeToKlineUpdatesAsync(symbols, new[] { interval }, onMessage, premiumIndex, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, IEnumerable<KlineInterval> intervals, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, bool premiumIndex = false, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceStreamKlineData>>>(data =>
            onMessage(data.As<IBinanceStreamKlineData>(data.Data.Data)
            .WithStreamId(data.Data.Stream)
            .WithSymbol(data.Data.Data.Symbol)
            .WithDataTimestamp(data.Data.Data.EventTime)));
        symbols = symbols.SelectMany(a => intervals.Select(i => (premiumIndex ? "p" + a.ToUpper(BinanceConstants.CI) : a.ToLower(BinanceConstants.CI)) + _klineStreamEndpoint + "_" + MapConverter.GetString(i))).ToArray();
        return SubscribeAsync(symbols, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(string pair, ContractType contractType, KlineInterval interval, Action<WebSocketDataEvent<BinanceStreamContinuousKlineData>> onMessage, CancellationToken ct = default) => SubscribeToContinuousContractKlineUpdatesAsync(new[] { pair }, contractType, interval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(IEnumerable<string> pairs, ContractType contractType, KlineInterval interval, Action<WebSocketDataEvent<BinanceStreamContinuousKlineData>> onMessage, CancellationToken ct = default)
    {
        pairs.ValidateNotNull(nameof(pairs));
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceStreamContinuousKlineData>>>(data =>
            onMessage(data.As(data.Data.Data)
            .WithStreamId(data.Data.Stream)
            .WithSymbol(data.Data.Data.Symbol)
            .WithDataTimestamp(data.Data.Data.EventTime)));
        pairs = pairs.Select(a => a.ToLower(BinanceConstants.CI) +
                                  "_" +
                                  MapConverter.GetString(contractType).ToLower() +
                                  _continuousContractKlineStreamEndpoint +
                                  "_" +
                                  MapConverter.GetString(interval)).ToArray();
        return SubscribeAsync(pairs, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(string symbol, Action<WebSocketDataEvent<IBinanceMiniTick>> onMessage, CancellationToken ct = default) => SubscribeToMiniTickerUpdatesAsync(new[] { symbol }, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<IBinanceMiniTick>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceStreamMiniTick>>>(data =>
            onMessage(data.As<IBinanceMiniTick>(data.Data.Data)
            .WithStreamId(data.Data.Stream)
            .WithSymbol(data.Data.Data.Symbol)
            .WithDataTimestamp(data.Data.Data.EventTime)));
        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + _symbolMiniTickerStreamEndpoint).ToArray();
        return SubscribeAsync(symbols, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllMiniTickerUpdatesAsync(Action<WebSocketDataEvent<IEnumerable<IBinanceMiniTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<IEnumerable<BinanceStreamMiniTick>>>>(data =>
            onMessage(data.As<IEnumerable<IBinanceMiniTick>>(data.Data.Data)
            .WithStreamId(data.Data.Stream)
            .WithDataTimestamp(data.Data.Data.Max(x => x.EventTime))));
        return SubscribeAsync(new[] { _allMiniTickerStreamEndpoint }, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<WebSocketDataEvent<IBinance24HPrice>> onMessage, CancellationToken ct = default) => SubscribeToTickerUpdatesAsync(new[] { symbol }, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<IBinance24HPrice>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceStreamTick>>>(data =>
            onMessage(data.As<IBinance24HPrice>(data.Data.Data)
            .WithStreamId(data.Data.Stream)
            .WithSymbol(data.Data.Data.Symbol)
            .WithDataTimestamp(data.Data.Data.EventTime)));
        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + _symbolTickerStreamEndpoint).ToArray();
        return SubscribeAsync(symbols, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToCompositeIndexUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamCompositeIndex>> onMessage, CancellationToken ct = default)
    {
        var action = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamCompositeIndex>>>(data =>
        {
            onMessage(data.As(data.Data.Data)
                .WithStreamId(data.Data.Stream)
                .WithSymbol(data.Data.Data.Symbol)
                .WithDataTimestamp(data.Data.Data.EventTime));
        });
        return SubscribeAsync(new[] { symbol.ToLower(BinanceConstants.CI) + _compositeIndexEndpoint }, action, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<WebSocketDataEvent<IEnumerable<IBinance24HPrice>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<IEnumerable<BinanceStreamTick>>>>(data =>
            onMessage(data.As<IEnumerable<IBinance24HPrice>>(data.Data.Data)
            .WithStreamId(data.Data.Stream)
            .WithDataTimestamp(data.Data.Data.Max(x => x.EventTime))));
        return SubscribeAsync(new[] { _allTickerStreamEndpoint }, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceStreamAggregatedTrade>> onMessage, CancellationToken ct = default) => SubscribeToAggregatedTradeUpdatesAsync(new[] { symbol }, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceStreamAggregatedTrade>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceStreamAggregatedTrade>>>(data =>
            onMessage(data.As(data.Data.Data)
            .WithStreamId(data.Data.Stream)
            .WithSymbol(data.Data.Data.Symbol)
            .WithDataTimestamp(data.Data.Data.EventTime)));
        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + _aggregatedTradesStreamEndpoint).ToArray();
        return SubscribeAsync(symbols, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol,
        Action<WebSocketDataEvent<BinanceStreamTrade>> onMessage, bool filterOutNonTradeUpdates = true, CancellationToken ct = default) =>
        SubscribeToTradeUpdatesAsync(new[] { symbol }, onMessage, filterOutNonTradeUpdates, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols,         Action<WebSocketDataEvent<BinanceStreamTrade>> onMessage, bool filterOutNonTradeUpdates = true, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceStreamTrade>>>(data =>
        {
            if (filterOutNonTradeUpdates && data.Data.Data.Type != "MARKET")
                return;

            onMessage(data.As(data.Data.Data)
                .WithStreamId(data.Data.Stream)
                .WithSymbol(data.Data.Data.Symbol)
                .WithDataTimestamp(data.Data.Data.EventTime));
        });
        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + _tradesStreamEndpoint).ToArray();
        return SubscribeAsync(symbols, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickerUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default) => SubscribeToBookTickerUpdatesAsync(new[] { symbol }, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickerUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamBookPrice>>>(data =>
            onMessage(data.As(data.Data.Data)
            .WithStreamId(data.Data.Stream)
            .WithSymbol(data.Data.Data.Symbol)
            .WithDataTimestamp(data.Data.Data.EventTime)));
        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + _bookTickerStreamEndpoint).ToArray();
        return SubscribeAsync(symbols, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllBookTickerUpdatesAsync(Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamBookPrice>>>(data =>
            onMessage(data.As(data.Data.Data)
            .WithStreamId(data.Data.Stream)
            .WithSymbol(data.Data.Data.Symbol)
            .WithDataTimestamp(data.Data.Data.EventTime)));
        return SubscribeAsync(new[] { _allBookTickerStreamEndpoint }, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default) => SubscribeToLiquidationUpdatesAsync(new[] { symbol }, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamLiquidationData>>>(data =>
            onMessage(data.As(data.Data.Data.Data)
            .WithStreamId(data.Data.Stream)
            .WithSymbol(data.Data.Data.Data.Symbol)
            .WithDataTimestamp(data.Data.Data.EventTime)));
        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + _liquidationStreamEndpoint).ToArray();
        return SubscribeAsync(symbols, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllLiquidationUpdatesAsync(Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamLiquidationData>>>(data =>
            onMessage(data.As(data.Data.Data.Data)
            .WithStreamId(data.Data.Stream)
            .WithSymbol(data.Data.Data.Data.Symbol)
            .WithDataTimestamp(data.Data.Data.EventTime)));
        return SubscribeAsync(new[] { _allLiquidationStreamEndpoint }, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, int levels, int? updateInterval, Action<WebSocketDataEvent<IBinanceFuturesEventOrderBook>> onMessage, CancellationToken ct = default) => SubscribeToPartialOrderBookUpdatesAsync(new[] { symbol }, levels, updateInterval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(IEnumerable<string> symbols, int levels, int? updateInterval, Action<WebSocketDataEvent<IBinanceFuturesEventOrderBook>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        levels.ValidateIntValues(nameof(levels), 5, 10, 20);
        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 250, 500);

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamOrderBookDepth>>>(data =>
        {
            data.Data.Data.Symbol = data.Data.Stream.Split('@')[0];
            onMessage(data.As<IBinanceFuturesEventOrderBook>(data.Data.Data)
                .WithStreamId(data.Data.Stream)
                .WithSymbol(data.Data.Data.Symbol)
                .WithDataTimestamp(data.Data.Data.EventTime));
        });

        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + _partialBookDepthStreamEndpoint + levels + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return SubscribeAsync(symbols, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int? updateInterval, Action<WebSocketDataEvent<IBinanceFuturesEventOrderBook>> onMessage, CancellationToken ct = default) => SubscribeToOrderBookUpdatesAsync(new[] { symbol }, updateInterval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int? updateInterval, Action<WebSocketDataEvent<IBinanceFuturesEventOrderBook>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 250, 500);
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamOrderBookDepth>>>(data =>
            onMessage(data.As<IBinanceFuturesEventOrderBook>(data.Data.Data)
            .WithStreamId(data.Data.Stream)
            .WithSymbol(data.Data.Data.Symbol)
            .WithDataTimestamp(data.Data.Data.EventTime)));
        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + _depthStreamEndpoint + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return SubscribeAsync(symbols, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToSymbolUpdatesAsync(Action<WebSocketDataEvent<BinanceFuturesStreamSymbolUpdate>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamSymbolUpdate>>>(data =>
            onMessage(data.As(data.Data.Data)
            .WithStreamId(data.Data.Stream)
            .WithSymbol(data.Data.Data.Symbol)
            .WithDataTimestamp(data.Data.Data.EventTime)));
        return SubscribeAsync(new[] { "!contractInfo" }, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAssetIndexUpdatesAsync(Action<WebSocketDataEvent<IEnumerable<BinanceFuturesStreamAssetIndexUpdate>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<IEnumerable<BinanceFuturesStreamAssetIndexUpdate>>>>(data =>
            onMessage(data.As(data.Data.Data)
            .WithStreamId(data.Data.Stream)
            .WithDataTimestamp(data.Data.Data.Max(x => x.EventTime))));
        return SubscribeAsync(new[] { "!assetIndex@arr" }, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAssetIndexUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamAssetIndexUpdate>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamAssetIndexUpdate>>>(data =>
            onMessage(data.As(data.Data.Data)
            .WithStreamId(data.Data.Stream)
            .WithSymbol(data.Data.Data.Symbol)
            .WithDataTimestamp(data.Data.Data.EventTime)));
        return SubscribeAsync(new[] { symbol.ToLowerInvariant() + "@assetIndex" }, handler, ct);
    }
}