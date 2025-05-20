namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientUsd
{
    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradesAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceFuturesStreamAggregatedTrade>> onMessage,
        CancellationToken ct = default)
        => SubscribeToAggregatedTradesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradesAsync(
        IEnumerable<string> symbols,
        Action<WebSocketDataEvent<BinanceFuturesStreamAggregatedTrade>> onMessage,
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamAggregatedTrade>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@aggTrade").ToArray();
        return SubscribeAsync(symbols, false, handler, ct);
    }

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
        symbols.ValidateNotNull(nameof(symbols));
        updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesUsdtStreamMarkPrice>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@markPrice" + (updateInterval == 1000 ? "@1s" : "")).ToArray();
        return SubscribeAsync(symbols, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPricesAsync(
        int? updateInterval,
        Action<WebSocketDataEvent<List<BinanceFuturesUsdtStreamMarkPrice>>> onMessage,
        CancellationToken ct = default)
    {
        updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<List<BinanceFuturesUsdtStreamMarkPrice>>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(["!markPrice@arr" + (updateInterval == 1000 ? "@1s" : "")], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(
        string symbol,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage,
        bool premiumIndex = false,
        CancellationToken ct = default)
        => SubscribeToKlinesAsync([symbol], interval, onMessage, premiumIndex, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(
        string symbol,
        IEnumerable<BinanceKlineInterval> intervals,
        Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage,
        bool premiumIndex = false,
        CancellationToken ct = default)
        => SubscribeToKlinesAsync([symbol], intervals, onMessage, premiumIndex, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(
        IEnumerable<string> symbols,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage,
        bool premiumIndex = false,
        CancellationToken ct = default)
        => SubscribeToKlinesAsync(symbols, [interval], onMessage, premiumIndex, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(
        IEnumerable<string> symbols,
        IEnumerable<BinanceKlineInterval> intervals,
        Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage,
        bool premiumIndex = false,
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamKlineData>>>(data =>
        {
            onMessage(data.As(data.Data.Data.Data));
        });
        symbols = symbols.SelectMany(a => intervals.Select(i => (premiumIndex ? "p" + a.ToUpper(BinanceConstants.CI) : a.ToLower(BinanceConstants.CI)) + "@kline_" + MapConverter.GetString(i))).ToArray();
        return SubscribeAsync(symbols, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlinesAsync(string pair,
        BinanceFuturesContractType contractType,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage,
        CancellationToken ct = default)
        => SubscribeToContinuousContractKlinesAsync([pair], contractType, interval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlinesAsync(
        IEnumerable<string> pairs,
        BinanceFuturesContractType contractType,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage, CancellationToken ct = default)
    {
        pairs.ValidateNotNull(nameof(pairs));
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceStreamContinuousKlineData>>>(data =>
        {
            onMessage(data.As(data.Data.Data.Data));
        });
        pairs = pairs.Select(a => a.ToLower(BinanceConstants.CI) + "_" + MapConverter.GetString(contractType)!.ToLower() + "@continuousKline_" + MapConverter.GetString(interval)).ToArray();
        return SubscribeAsync(pairs, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamMiniTick>> onMessage, CancellationToken ct = default) => SubscribeToMiniTickersAsync(new[] { symbol }, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamMiniTick>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamMiniTick>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@miniTicker").ToArray();
        return SubscribeAsync(symbols, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(Action<WebSocketDataEvent<List<BinanceFuturesStreamTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<List<BinanceFuturesStreamTick>>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(["!ticker@arr"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamTick>> onMessage, CancellationToken ct = default) => SubscribeToTickersAsync(new[] { symbol }, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceFuturesStreamTick>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamTick>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@ticker").ToArray();
        return SubscribeAsync(symbols, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(Action<WebSocketDataEvent<List<BinanceFuturesStreamMiniTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<List<BinanceFuturesStreamMiniTick>>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(["!miniTicker@arr"], false, handler, ct);
    }

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
        return SubscribeAsync(symbols, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickersAsync(Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamBookPrice>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(["!bookTicker"], false, handler, ct);
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
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamLiquidationData>>>(data =>
        {
            onMessage(data.As(data.Data.Data.Data));
        });
        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@forceOrder").ToArray();
        return SubscribeAsync(symbols, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationsAsync(Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamLiquidationData>>>(data =>
        {
            onMessage(data.As(data.Data.Data.Data));
        });
        return SubscribeAsync(["!forceOrder@arr"], false, handler, ct);
    }

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
        symbols.ValidateNotNull(nameof(symbols));
        levels.ValidateIntValues(nameof(levels), 5, 10, 20);
        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 250, 500);

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamOrderBookDepth>>>(data =>
        {
            data.Data.Data.Symbol = data.Data.Stream?.Split('@')[0] ?? "";
            onMessage(data.As(data.Data.Data));
        });

        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@depth" + levels + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return SubscribeAsync(symbols, false, handler, ct);
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
        symbols.ValidateNotNull(nameof(symbols));

        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 250, 500);
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamOrderBookDepth>>>(data =>
            onMessage(data.As(data.Data.Data)));
        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@depth" + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return SubscribeAsync(symbols, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToCompositeIndexesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamCompositeIndex>> onMessage, CancellationToken ct = default)
    {
        var action = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamCompositeIndex>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync([symbol.ToLower(BinanceConstants.CI) + "@compositeIndex"], false, action, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToSymbolsAsync(
        Action<WebSocketDataEvent<BinanceFuturesStreamSymbolUpdate>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamSymbolUpdate>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(["!contractInfo"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAssetIndexesAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceFuturesStreamAssetIndexUpdate>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamAssetIndexUpdate>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync([symbol.ToLowerInvariant() + "@assetIndex"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAssetIndexesAsync(
        Action<WebSocketDataEvent<List<BinanceFuturesStreamAssetIndexUpdate>>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<List<BinanceFuturesStreamAssetIndexUpdate>>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(["!assetIndex@arr"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradesAsync(string symbol,
        Action<WebSocketDataEvent<BinanceFuturesStreamTrade>> onMessage,
        bool filterOutNonTradeUpdates = true,
        CancellationToken ct = default)
        => SubscribeToTradesAsync([symbol], onMessage, filterOutNonTradeUpdates, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradesAsync(
        IEnumerable<string> symbols,
        Action<WebSocketDataEvent<BinanceFuturesStreamTrade>> onMessage,
        bool filterOutNonTradeUpdates = true,
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamTrade>>>(data =>
        {
            if (filterOutNonTradeUpdates && data.Data.Data.Type != "MARKET")
                return;

            onMessage(data.As(data.Data.Data));
        });
        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@trade").ToArray();
        return SubscribeAsync(symbols, false, handler, ct);
    }
}