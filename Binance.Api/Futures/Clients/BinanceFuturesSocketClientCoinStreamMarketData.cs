namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientCoin
{
    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceFuturesCoinStreamAggregatedTrade>> onMessage,
        CancellationToken ct = default)
        => SubscribeToAggregatedTradeUpdatesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(
        IEnumerable<string> symbols,
        Action<WebSocketDataEvent<BinanceFuturesCoinStreamAggregatedTrade>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesCoinStreamAggregatedTrade>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(AggregateTradeStreamTopics(symbols), false, handler, ct);
    }

    internal static string[] AggregateTradeStreamTopics(IEnumerable<string> symbols)
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
            .Select(symbol => symbol.ToLower(BinanceConstants.CI) + "@aggTrade")
            .ToArray();
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexPriceUpdatesAsync(
        string pair,
        int? updateInterval,
        Action<WebSocketDataEvent<BinanceFuturesStreamIndexPrice>> onMessage,
        CancellationToken ct = default)
        => SubscribeToIndexPriceUpdatesAsync([pair], updateInterval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexPriceUpdatesAsync(
        IEnumerable<string> pairs,
        int? updateInterval,
        Action<WebSocketDataEvent<BinanceFuturesStreamIndexPrice>> onMessage,
        CancellationToken ct = default)
    {
        pairs.ValidateNotNull(nameof(pairs));
        updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamIndexPrice>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });

        var topics = pairs.Select(a => a.ToLower(BinanceConstants.CI) + "@indexPrice" + (updateInterval == 1000 ? "@1s" : "")).ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(
        string symbol,
        int? updateInterval,
        Action<WebSocketDataEvent<BinanceFuturesCoinStreamMarkPrice>> onMessage,
        CancellationToken ct = default)
        => SubscribeToMarkPriceUpdatesAsync([symbol], updateInterval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(
        IEnumerable<string> symbols,
        int? updateInterval,
        Action<WebSocketDataEvent<BinanceFuturesCoinStreamMarkPrice>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesCoinStreamMarkPrice>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(MarkPriceStreamTopics(symbols, updateInterval), false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllMarkPriceUpdatesAsync(
        Action<WebSocketDataEvent<List<BinanceFuturesStreamAllMarketMarkPrice>>> onMessage,
        int? updateInterval = null,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<List<BinanceFuturesStreamAllMarketMarkPrice>>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync([MarkPriceAllMarketStreamTopic(updateInterval)], false, handler, ct);
    }

    internal static string[] MarkPriceStreamTopics(IEnumerable<string> symbols, int? updateInterval)
    {
        if (symbols == null)
            throw new ArgumentNullException(nameof(symbols));

        var symbolList = symbols.ToArray();
        if (symbolList.Length == 0)
            throw new ArgumentException("At least one symbol is required.", nameof(symbols));
        foreach (var symbol in symbolList)
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException("Symbols cannot be null or blank.", nameof(symbols));

        var updateSpeed = MarkPriceUpdateSpeed(updateInterval);
        return symbolList
            .Select(symbol => symbol.ToLower(BinanceConstants.CI) + "@markPrice" + updateSpeed)
            .ToArray();
    }

    internal static string MarkPricePairStreamTopic(string pair, int? updateInterval)
    {
        if (pair == null)
            throw new ArgumentNullException(nameof(pair));
        if (string.IsNullOrWhiteSpace(pair))
            throw new ArgumentException("Pair cannot be null or blank.", nameof(pair));

        return pair.ToLower(BinanceConstants.CI) + "@markPrice" + MarkPriceUpdateSpeed(updateInterval);
    }

    internal static string MarkPriceAllMarketStreamTopic(int? updateInterval)
        => "!markPrice@arr" + MarkPriceUpdateSpeed(updateInterval);

    private static string MarkPriceUpdateSpeed(int? updateInterval)
    {
        updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);
        return updateInterval == 1000 ? "@1s" : string.Empty;
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(
        string symbol,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamCoinKline>> onMessage,
        CancellationToken ct = default)
        => SubscribeToKlineUpdatesAsync([symbol], [interval], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(
        string symbol,
        IEnumerable<BinanceKlineInterval> intervals,
        Action<WebSocketDataEvent<BinanceFuturesStreamCoinKline>> onMessage,
        CancellationToken ct = default)
        => SubscribeToKlineUpdatesAsync([symbol], intervals, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(
        IEnumerable<string> symbols,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamCoinKline>> onMessage,
        CancellationToken ct = default)
        => SubscribeToKlineUpdatesAsync(symbols, [interval], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(
        IEnumerable<string> symbols,
        IEnumerable<BinanceKlineInterval> intervals,
        Action<WebSocketDataEvent<BinanceFuturesStreamCoinKline>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamCoinKlineWrapper>>>(data =>
        {
            onMessage(data.As(StandardKline(data.Data.Data)));
        });

        return SubscribeAsync(KlineStreamTopics(symbols, intervals), false, handler, ct);
    }

    internal static string[] KlineStreamTopics(IEnumerable<string> symbols, IEnumerable<BinanceKlineInterval> intervals)
        => BinanceFuturesStreamValidation.KlineStreamTopics(symbols, intervals);

    internal static BinanceFuturesStreamCoinKline StandardKline(BinanceFuturesStreamCoinKlineWrapper update)
    {
        update.Kline.Event = update.Event;
        update.Kline.EventTime = update.EventTime;
        return update.Kline;
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(
        string pair,
        BinanceFuturesContractType contractType,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamCoinContinuousKline>> onMessage,
        CancellationToken ct = default)
        => SubscribeToContinuousContractKlineUpdatesAsync([pair], contractType, interval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(
        IEnumerable<string> pairs,
        BinanceFuturesContractType contractType,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamCoinContinuousKline>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamCoinContinuousKline>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(ContinuousKlineStreamTopics(pairs, contractType, interval), false, handler, ct);
    }

    internal static string[] ContinuousKlineStreamTopics(
        IEnumerable<string> pairs,
        BinanceFuturesContractType contractType,
        BinanceKlineInterval interval)
        => BinanceFuturesStreamValidation.CoinContinuousKlineStreamTopics(pairs, contractType, interval);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllMarkPriceUpdatesOfAllSymbolsOfPairAsync(
        string pair,
        int? updateInterval,
        Action<WebSocketDataEvent<List<BinanceFuturesCoinStreamMarkPrice>>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<List<BinanceFuturesCoinStreamMarkPrice>>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync([MarkPricePairStreamTopic(pair, updateInterval)], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexKlineUpdatesAsync(
        string pair,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamIndexKline>> onMessage,
        CancellationToken ct = default)
        => SubscribeToIndexKlineUpdatesAsync([pair], interval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexKlineUpdatesAsync(
        IEnumerable<string> pairs,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamIndexKline>> onMessage,
        CancellationToken ct = default)
    {
        pairs.ValidateNotNull(nameof(pairs));
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamIndexKlineWrapper>>>(data =>
        {
            onMessage(data.As(data.Data.Data.Kline));
        });
        var topics = pairs.Select(a => a.ToLower(BinanceConstants.CI) + "@indexPriceKline_" + MapConverter.GetString(interval)).ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceKlineUpdatesAsync(
        string symbol,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamIndexKline>> onMessage,
        CancellationToken ct = default)
        => SubscribeToMarkPriceKlineUpdatesAsync([symbol], interval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceKlineUpdatesAsync(
        IEnumerable<string> symbols,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamIndexKline>> onMessage,
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamIndexKlineWrapper>>>(data =>
        {
            data.Data.Data.Kline.Symbol = data.Data.Data.Symbol;
            onMessage(data.As(data.Data.Data.Kline));
        });
        var topics = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@markPriceKline_" + MapConverter.GetString(interval)).ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceFuturesStreamMiniTick>> onMessage,
        CancellationToken ct = default)
        => SubscribeToMiniTickerUpdatesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(
        IEnumerable<string> symbols,
        Action<WebSocketDataEvent<BinanceFuturesStreamMiniTick>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamMiniTick>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(MiniTickerStreamTopics(symbols), false, handler, ct);
    }

    internal static string[] MiniTickerStreamTopics(IEnumerable<string> symbols)
        => SymbolStreamTopics(symbols, "@miniTicker");

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllMiniTickerUpdatesAsync(
        Action<WebSocketDataEvent<List<BinanceFuturesStreamMiniTick>>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<List<BinanceFuturesStreamMiniTick>>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync([MiniTickerAllMarketStreamTopic], false, handler, ct);
    }

    internal const string MiniTickerAllMarketStreamTopic = "!miniTicker@arr";

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerUpdatesAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceFuturesStreamTick>> onMessage,
        CancellationToken ct = default)
        => SubscribeToTickerUpdatesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerUpdatesAsync(
        IEnumerable<string> symbols,
        Action<WebSocketDataEvent<BinanceFuturesStreamTick>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamTick>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(TickerStreamTopics(symbols), false, handler, ct);
    }

    internal static string[] TickerStreamTopics(IEnumerable<string> symbols)
        => SymbolStreamTopics(symbols, "@ticker");

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllTickerUpdatesAsync(
        Action<WebSocketDataEvent<List<BinanceFuturesStreamTick>>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<List<BinanceFuturesStreamTick>>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync([TickerAllMarketStreamTopic], false, handler, ct);
    }

    internal const string TickerAllMarketStreamTopic = "!ticker@arr";

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

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickerUpdatesAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage,
        CancellationToken ct = default)
        => SubscribeToBookTickerUpdatesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickerUpdatesAsync(
        IEnumerable<string> symbols,
        Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamBookPrice>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(BookTickerStreamTopics(symbols), false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllBookTickerUpdatesAsync(
        Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamBookPrice>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync([BookTickerAllMarketStreamTopic], false, handler, ct);
    }

    internal static string[] BookTickerStreamTopics(IEnumerable<string> symbols)
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
            .Select(symbol => symbol.ToLower(BinanceConstants.CI) + "@bookTicker")
            .ToArray();
    }

    internal const string BookTickerAllMarketStreamTopic = "!bookTicker";

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationUpdatesAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage,
        CancellationToken ct = default)
        => SubscribeToLiquidationUpdatesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationUpdatesAsync(
        IEnumerable<string> symbols,
        Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamLiquidation>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(LiquidationStreamTopics(symbols), false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllLiquidationUpdatesAsync(
        Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamLiquidation>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync([LiquidationAllMarketStreamTopic], false, handler, ct);
    }

    internal static string[] LiquidationStreamTopics(IEnumerable<string> symbols)
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
            .Select(symbol => symbol.ToLower(BinanceConstants.CI) + "@forceOrder")
            .ToArray();
    }

    internal const string LiquidationAllMarketStreamTopic = "!forceOrder@arr";

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToSymbolUpdatesAsync(
        Action<WebSocketDataEvent<BinanceFuturesStreamSymbolUpdate>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamSymbolUpdate>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync([ContractInfoStreamTopic], false, handler, ct);
    }

    internal const string ContractInfoStreamTopic = "!contractInfo";

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(
        string symbol,
        int levels,
        int? updateInterval,
        Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage,
        CancellationToken ct = default)
        => SubscribeToPartialOrderBookUpdatesAsync([symbol], levels, updateInterval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(
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
        return SubscribeAsync(PartialDepthStreamTopics(symbols, levels, updateInterval), false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBookUpdatesAsync(
        string symbol,
        int? updateInterval,
        Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage,
        CancellationToken ct = default)
        => SubscribeToOrderBookUpdatesAsync([symbol], updateInterval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBookUpdatesAsync(
        IEnumerable<string> symbols,
        int? updateInterval,
        Action<WebSocketDataEvent<BinanceFuturesStreamOrderBookDepth>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamOrderBookDepth>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(DiffDepthStreamTopics(symbols, updateInterval), false, handler, ct);
    }

    internal static string[] PartialDepthStreamTopics(IEnumerable<string> symbols, int levels, int? updateInterval)
    {
        levels.ValidateIntValues(nameof(levels), 5, 10, 20);
        return DepthStreamTopics(symbols, $"@depth{levels}", updateInterval);
    }

    internal static string[] DiffDepthStreamTopics(IEnumerable<string> symbols, int? updateInterval)
        => DepthStreamTopics(symbols, "@depth", updateInterval);

    private static string[] DepthStreamTopics(IEnumerable<string> symbols, string suffix, int? updateInterval)
    {
        if (symbols == null)
            throw new ArgumentNullException(nameof(symbols));

        var symbolList = symbols.ToArray();
        if (symbolList.Length == 0)
            throw new ArgumentException("At least one symbol is required.", nameof(symbols));
        foreach (var symbol in symbolList)
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException("Symbols cannot be null or blank.", nameof(symbols));

        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 500);
        var updateSpeed = updateInterval.HasValue ? $"@{updateInterval.Value}ms" : string.Empty;
        return symbolList
            .Select(symbol => symbol.ToLower(BinanceConstants.CI) + suffix + updateSpeed)
            .ToArray();
    }

}
