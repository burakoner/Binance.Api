namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientUsd
{
    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradesAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceFuturesUsdtStreamAggregatedTrade>> onMessage,
        CancellationToken ct = default)
        => SubscribeToAggregatedTradesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradesAsync(
        IEnumerable<string> symbols,
        Action<WebSocketDataEvent<BinanceFuturesUsdtStreamAggregatedTrade>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesUsdtStreamAggregatedTrade>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeMarketAsync(AggregateTradeStreamTopics(symbols), false, handler, ct);
    }

    internal static string[] AggregateTradeStreamTopics(IEnumerable<string> symbols)
        => SymbolStreamTopics(symbols, "@aggTrade");

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPricesAsync(
        string symbol,
        int? updateInterval,
        Action<WebSocketDataEvent<BinanceFuturesUsdtStreamMarkPrice>> onMessage,
        CancellationToken ct = default)
        => SubscribeToMarkPricesAsync([symbol], updateInterval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPricesAsync(
        IEnumerable<string> symbols,
        int? updateInterval,
        Action<WebSocketDataEvent<BinanceFuturesUsdtStreamMarkPrice>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesUsdtStreamMarkPrice>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeMarketAsync(MarkPriceStreamTopics(symbols, updateInterval), false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPricesAsync(
        int? updateInterval,
        Action<WebSocketDataEvent<List<BinanceFuturesStreamAllMarketMarkPrice>>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<List<BinanceFuturesStreamAllMarketMarkPrice>>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeMarketAsync([MarkPriceAllMarketStreamTopic(updateInterval)], false, handler, ct);
    }

    internal static string[] MarkPriceStreamTopics(IEnumerable<string> symbols, int? updateInterval)
    {
        updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);
        var updateSpeed = updateInterval == 1000 ? "@1s" : string.Empty;
        return SymbolStreamTopics(symbols, "@markPrice" + updateSpeed);
    }

    internal static string MarkPriceAllMarketStreamTopic(int? updateInterval)
    {
        updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);
        return "!markPrice@arr" + (updateInterval == 1000 ? "@1s" : string.Empty);
    }

    private static string[] SymbolStreamTopics(IEnumerable<string> symbols, string suffix)
    {
        if (symbols == null)
            throw new ArgumentNullException(nameof(symbols));

        var symbolList = symbols.ToArray();
        if (symbolList.Length == 0)
            throw new ArgumentException("At least one symbol is required.", nameof(symbols));
        foreach (var symbol in symbolList)
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException("Symbols cannot be null or blank.", nameof(symbols));

        return symbolList
            .Select(symbol => symbol.ToLower(BinanceConstants.CI) + suffix)
            .ToArray();
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(
        string symbol,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage,
        CancellationToken ct = default)
        => SubscribeToKlinesAsync([symbol], interval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(
        string symbol,
        IEnumerable<BinanceKlineInterval> intervals,
        Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage,
        CancellationToken ct = default)
        => SubscribeToKlinesAsync([symbol], intervals, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(
        IEnumerable<string> symbols,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage,
        CancellationToken ct = default)
        => SubscribeToKlinesAsync(symbols, [interval], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(
        IEnumerable<string> symbols,
        IEnumerable<BinanceKlineInterval> intervals,
        Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamKlineWrapper>>>(data =>
        {
            onMessage(data.As(StandardKline(data.Data.Data)));
        });
        return SubscribeMarketAsync(KlineStreamTopics(symbols, intervals), false, handler, ct);
    }

    internal static string[] KlineStreamTopics(IEnumerable<string> symbols, IEnumerable<BinanceKlineInterval> intervals)
        => BinanceFuturesStreamValidation.KlineStreamTopics(symbols, intervals);

    internal static BinanceFuturesStreamKline StandardKline(BinanceFuturesStreamKlineWrapper update)
    {
        update.Kline.Event = update.Event;
        update.Kline.EventTime = update.EventTime;
        return update.Kline;
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlinesAsync(string pair,
        BinanceFuturesContractType contractType,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamContinuousKline>> onMessage,
        CancellationToken ct = default)
        => SubscribeToContinuousContractKlinesAsync([pair], contractType, interval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlinesAsync(
        IEnumerable<string> pairs,
        BinanceFuturesContractType contractType,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamContinuousKline>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamContinuousKline>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeMarketAsync(ContinuousKlineStreamTopics(pairs, contractType, interval), false, handler, ct);
    }

    internal static string[] ContinuousKlineStreamTopics(
        IEnumerable<string> pairs,
        BinanceFuturesContractType contractType,
        BinanceKlineInterval interval)
        => BinanceFuturesStreamValidation.UsdContinuousKlineStreamTopics(pairs, contractType, interval);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamMiniTick>> onMessage, CancellationToken ct = default) => SubscribeToMiniTickersAsync(new[] { symbol }, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamMiniTick>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamMiniTick>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeMarketAsync(MiniTickerStreamTopics(symbols), false, handler, ct);
    }

    internal static string[] MiniTickerStreamTopics(IEnumerable<string> symbols)
        => SymbolStreamTopics(symbols, "@miniTicker");

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(Action<WebSocketDataEvent<List<BinanceFuturesStreamTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<List<BinanceFuturesStreamTick>>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeMarketAsync([TickerAllMarketStreamTopic], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamTick>> onMessage, CancellationToken ct = default) => SubscribeToTickersAsync(new[] { symbol }, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamTick>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamTick>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeMarketAsync(TickerStreamTopics(symbols), false, handler, ct);
    }

    internal static string[] TickerStreamTopics(IEnumerable<string> symbols)
        => SymbolStreamTopics(symbols, "@ticker");

    internal const string TickerAllMarketStreamTopic = "!ticker@arr";

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(Action<WebSocketDataEvent<List<BinanceFuturesStreamMiniTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<List<BinanceFuturesStreamMiniTick>>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeMarketAsync([MiniTickerAllMarketStreamTopic], false, handler, ct);
    }

    internal const string MiniTickerAllMarketStreamTopic = "!miniTicker@arr";

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickersAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage,
        CancellationToken ct = default)
        => SubscribeToBookTickersAsync(new[] { symbol }, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickersAsync(
        IEnumerable<string> symbols,
        Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage,
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamBookPrice>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@bookTicker").ToArray();
        return SubscribePublicAsync(symbols, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickersAsync(Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamBookPrice>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribePublicAsync(["!bookTicker"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationsAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage,
        CancellationToken ct = default)
        => SubscribeToLiquidationsAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationsAsync(
        IEnumerable<string> symbols,
        Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamLiquidation>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeMarketAsync(LiquidationStreamTopics(symbols), false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationsAsync(Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamLiquidation>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeMarketAsync([LiquidationAllMarketStreamTopic], false, handler, ct);
    }

    internal static string[] LiquidationStreamTopics(IEnumerable<string> symbols)
        => SymbolStreamTopics(symbols, "@forceOrder");

    internal const string LiquidationAllMarketStreamTopic = "!forceOrder@arr";

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBooksAsync(
        string symbol,
        int levels,
        int? updateInterval,
        Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage,
        CancellationToken ct = default)
        => SubscribeToPartialOrderBooksAsync([symbol], levels, updateInterval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBooksAsync(
        IEnumerable<string> symbols,
        int levels,
        int? updateInterval,
        Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamOrderBookDepth>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribePublicAsync(PartialDepthStreamTopics(symbols, levels, updateInterval), false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBooksAsync(
        string symbol,
        int? updateInterval,
        Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage,
        CancellationToken ct = default)
        => SubscribeToOrderBooksAsync([symbol], updateInterval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBooksAsync(
        IEnumerable<string> symbols,
        int? updateInterval,
        Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamOrderBookDepth>>>(data =>
            onMessage(data.As(data.Data.Data)));
        return SubscribePublicAsync(DiffDepthStreamTopics(symbols, updateInterval), false, handler, ct);
    }

    internal static string[] PartialDepthStreamTopics(IEnumerable<string> symbols, int levels, int? updateInterval)
    {
        levels.ValidateIntValues(nameof(levels), 5, 10, 20);
        return DepthStreamTopics(symbols, $"@depth{levels}", updateInterval);
    }

    internal static string[] DiffDepthStreamTopics(IEnumerable<string> symbols, int? updateInterval)
        => DepthStreamTopics(symbols, "@depth", updateInterval);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToRpiOrderBooksAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage,
        CancellationToken ct = default)
        => SubscribeToRpiOrderBooksAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToRpiOrderBooksAsync(
        IEnumerable<string> symbols,
        Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamOrderBookDepth>>>(data =>
            onMessage(data.As(data.Data.Data)));
        return SubscribePublicAsync(RpiDepthStreamTopics(symbols), false, handler, ct);
    }

    internal static string[] RpiDepthStreamTopics(IEnumerable<string> symbols)
        => SymbolStreamTopics(symbols, "@rpiDepth@500ms");

    private static string[] DepthStreamTopics(IEnumerable<string> symbols, string suffix, int? updateInterval)
    {
        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 500);
        var updateSpeed = updateInterval.HasValue ? $"@{updateInterval.Value}ms" : string.Empty;
        return SymbolStreamTopics(symbols, suffix + updateSpeed);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToCompositeIndexesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamCompositeIndex>> onMessage, CancellationToken ct = default)
    {
        var action = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamCompositeIndex>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeMarketAsync([symbol.ToLower(BinanceConstants.CI) + "@compositeIndex"], false, action, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToSymbolsAsync(
        Action<WebSocketDataEvent<BinanceFuturesStreamSymbolUpdate>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamSymbolUpdate>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeMarketAsync([ContractInfoStreamTopic], false, handler, ct);
    }

    internal const string ContractInfoStreamTopic = "!contractInfo";

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAssetIndexesAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceFuturesStreamAssetIndexUpdate>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamAssetIndexUpdate>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeMarketAsync([symbol.ToLowerInvariant() + "@assetIndex"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAssetIndexesAsync(
        Action<WebSocketDataEvent<List<BinanceFuturesStreamAssetIndexUpdate>>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<List<BinanceFuturesStreamAssetIndexUpdate>>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeMarketAsync(["!assetIndex@arr"], false, handler, ct);
    }

}
