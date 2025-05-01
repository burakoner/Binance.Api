namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientUsd
{
    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceFuturesStreamAggregatedTrade>> onMessage,
        CancellationToken ct = default)
        => SubscribeToAggregatedTradeUpdatesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(
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

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(
        string symbol,
        int? updateInterval,
        Action<WebSocketDataEvent<BinanceFuturesUsdtStreamMarkPrice>> onMessage,
        CancellationToken ct = default)
        => SubscribeToMarkPriceUpdatesAsync([symbol], updateInterval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceUpdatesAsync(
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

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllMarkPriceUpdatesAsync(
        int? updateInterval,
        Action<WebSocketDataEvent<IEnumerable<BinanceFuturesUsdtStreamMarkPrice>>> onMessage,
        CancellationToken ct = default)
    {
        updateInterval?.ValidateIntValues(nameof(updateInterval), 1000, 3000);

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<IEnumerable<BinanceFuturesUsdtStreamMarkPrice>>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(["!markPrice@arr" + (updateInterval == 1000 ? "@1s" : "")], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(
        string symbol,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage,
        bool premiumIndex = false,
        CancellationToken ct = default)
        => SubscribeToKlineUpdatesAsync([symbol], interval, onMessage, premiumIndex, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(
        string symbol,
        IEnumerable<BinanceKlineInterval> intervals,
        Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage,
        bool premiumIndex = false,
        CancellationToken ct = default)
        => SubscribeToKlineUpdatesAsync([symbol], intervals, onMessage, premiumIndex, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(
        IEnumerable<string> symbols,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage,
        bool premiumIndex = false,
        CancellationToken ct = default)
        => SubscribeToKlineUpdatesAsync(symbols, [interval], onMessage, premiumIndex, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(
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

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(string pair,
        BinanceFuturesContractType contractType,
        BinanceKlineInterval interval,
        Action<WebSocketDataEvent<BinanceFuturesStreamKline>> onMessage,
        CancellationToken ct = default)
        => SubscribeToContinuousContractKlineUpdatesAsync([pair], contractType, interval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToContinuousContractKlineUpdatesAsync(
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
        pairs = pairs.Select(a => a.ToLower(BinanceConstants.CI) + "_" + MapConverter.GetString(contractType).ToLower() + "@continuousKline_" + MapConverter.GetString(interval)).ToArray();
        return SubscribeAsync(pairs, false, handler, ct);
    }

    /*
    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(string symbol, Action<WebSocketDataEvent<IBinanceMiniTick>> onMessage, CancellationToken ct = default) => SubscribeToMiniTickerUpdatesAsync(new[] { symbol }, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<IBinanceMiniTick>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceStreamMiniTick>>>(data =>
        {
            onMessage(data.As<IBinanceMiniTick>(data.Data.Data));
        });
        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@miniTicker").ToArray();
        return SubscribeAsync(symbols, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<WebSocketDataEvent<IEnumerable<IBinance24HPrice>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<IEnumerable<BinanceStreamTick>>>>(data =>
        {
            onMessage(data.As<IEnumerable<IBinance24HPrice>>(data.Data.Data));
        });
        return SubscribeAsync(["!ticker@arr"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<WebSocketDataEvent<IBinance24HPrice>> onMessage, CancellationToken ct = default) => SubscribeToTickerUpdatesAsync(new[] { symbol }, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<IBinance24HPrice>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceStreamTick>>>(data =>
        {
            onMessage(data.As<IBinance24HPrice>(data.Data.Data));
        });
        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@ticker").ToArray();
        return SubscribeAsync(symbols, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllMiniTickerUpdatesAsync(Action<WebSocketDataEvent<IEnumerable<IBinanceMiniTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<IEnumerable<BinanceStreamMiniTick>>>>(data =>
        {
            onMessage(data.As<IEnumerable<IBinanceMiniTick>>(data.Data.Data));
        });
        return SubscribeAsync(["!miniTicker@arr"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickerUpdatesAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage,
        CancellationToken ct = default)
        => SubscribeToBookTickerUpdatesAsync(new[] { symbol }, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickerUpdatesAsync(
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

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllBookTickerUpdatesAsync(Action<WebSocketDataEvent<BinanceFuturesStreamBookPrice>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamBookPrice>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(["!bookTicker"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationUpdatesAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage,
        CancellationToken ct = default)
        => SubscribeToLiquidationUpdatesAsync(new[] { symbol }, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToLiquidationUpdatesAsync(
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

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllLiquidationUpdatesAsync(Action<WebSocketDataEvent<BinanceFuturesStreamLiquidation>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamLiquidationData>>>(data =>
        {
            onMessage(data.As(data.Data.Data.Data));
        });
        return SubscribeAsync(["!forceOrder@arr"], false, handler, ct);
    }

    /*
    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(
        string symbol,
        int levels,
        int? updateInterval,
        Action<WebSocketDataEvent<IBinanceFuturesEventOrderBook>> onMessage,
        CancellationToken ct = default)
        => SubscribeToPartialOrderBookUpdatesAsync([symbol], levels, updateInterval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(
        IEnumerable<string> symbols,
        int levels,
        int? updateInterval,
        Action<WebSocketDataEvent<IBinanceFuturesEventOrderBook>> onMessage,
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        levels.ValidateIntValues(nameof(levels), 5, 10, 20);
        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 250, 500);

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamOrderBookDepth>>>(data =>
        {
            data.Data.Data.Symbol = data.Data.Stream?.Split('@')[0] ?? "";
            onMessage(data.As<IBinanceFuturesEventOrderBook>(data.Data.Data));
        });

        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@depth" + levels + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return SubscribeAsync(symbols, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBookUpdatesAsync(
        string symbol,
        int? updateInterval,
        Action<WebSocketDataEvent<IBinanceFuturesEventOrderBook>> onMessage,
        CancellationToken ct = default)
        => SubscribeToOrderBookUpdatesAsync([symbol], updateInterval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBookUpdatesAsync(
        IEnumerable<string> symbols,
        int? updateInterval,
        Action<WebSocketDataEvent<IBinanceFuturesEventOrderBook>> onMessage,
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 250, 500);
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamOrderBookDepth>>>(data =>
            onMessage(data.As<IBinanceFuturesEventOrderBook>(data.Data.Data)));
        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@depth" + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return SubscribeAsync(symbols, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToCompositeIndexUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceFuturesStreamCompositeIndex>> onMessage, CancellationToken ct = default)
    {
        var action = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamCompositeIndex>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync([symbol.ToLower(BinanceConstants.CI) + "@compositeIndex"], false, action, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToSymbolUpdatesAsync(
        Action<WebSocketDataEvent<BinanceFuturesStreamSymbolUpdate>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamSymbolUpdate>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(["!contractInfo"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAssetIndexUpdatesAsync(
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

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAssetIndexUpdatesAsync(
        Action<WebSocketDataEvent<IEnumerable<BinanceFuturesStreamAssetIndexUpdate>>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<IEnumerable<BinanceFuturesStreamAssetIndexUpdate>>>>(data =>
        {
            onMessage(data.As(data.Data.Data));
        });
        return SubscribeAsync(["!assetIndex@arr"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol,
        Action<WebSocketDataEvent<BinanceStreamTrade>> onMessage,
        bool filterOutNonTradeUpdates = true,
        CancellationToken ct = default)
        => SubscribeToTradeUpdatesAsync([symbol], onMessage, filterOutNonTradeUpdates, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradeUpdatesAsync(
        IEnumerable<string> symbols,
        Action<WebSocketDataEvent<BinanceStreamTrade>> onMessage,
        bool filterOutNonTradeUpdates = true,
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceFuturesStreamCombinedStream<BinanceStreamTrade>>>(data =>
        {
            if (filterOutNonTradeUpdates && data.Data.Data.Type != "MARKET")
                return;

            onMessage(data.As(data.Data.Data));
        });
        symbols = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@trade").ToArray();
        return SubscribeAsync(symbols, false, handler, ct);
    }
    */
}