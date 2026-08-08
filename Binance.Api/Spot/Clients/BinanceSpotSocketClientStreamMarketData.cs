namespace Binance.Api.Spot;

internal partial class BinanceSpotSocketClient
{
    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToReferencePriceAsync(string symbol, Action<WebSocketDataEvent<BinanceSpotStreamReferencePrice>> onMessage, CancellationToken ct = default)
        => SubscribeToReferencePriceAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToReferencePriceAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceSpotStreamReferencePrice>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceSpotStreamReferencePrice>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = BinanceSpotMarketDataValidation.SymbolStreamTopics(symbols, "@referencePrice");
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradesAsync(string symbol, Action<WebSocketDataEvent<BinanceSpotStreamAggregatedTrade>> onMessage, CancellationToken ct = default)
        => SubscribeToAggregatedTradesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceSpotStreamAggregatedTrade>> onMessage, CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols, 1024);

        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceSpotStreamAggregatedTrade>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbolList.Select(a => a.ToLower(BinanceConstants.CI) + "@aggTrade");
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradesAsync(string symbol, Action<WebSocketDataEvent<BinanceSpotStreamTrade>> onMessage, CancellationToken ct = default)
        => SubscribeToTradesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceSpotStreamTrade>> onMessage, CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols, 1024);

        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceSpotStreamTrade>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbolList.Select(a => a.ToLower(BinanceConstants.CI) + "@trade");
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBlockTradesAsync(string symbol, Action<WebSocketDataEvent<BinanceSpotStreamBlockTrade>> onMessage, CancellationToken ct = default)
        => SubscribeToBlockTradesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBlockTradesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceSpotStreamBlockTrade>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceSpotStreamBlockTrade>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = BinanceSpotMarketDataValidation.SymbolStreamTopics(symbols, "@blockTrade");
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(string symbol, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceSpotStreamKline>> onMessage, CancellationToken ct = default)
        => SubscribeToKlinesAsync([symbol], interval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(string symbol, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<BinanceSpotStreamKline>> onMessage, CancellationToken ct = default)
        => SubscribeToKlinesAsync([symbol], intervals, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(IEnumerable<string> symbols, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceSpotStreamKline>> onMessage, CancellationToken ct = default)
        => SubscribeToKlinesAsync(symbols, [interval], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(IEnumerable<string> symbols, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<BinanceSpotStreamKline>> onMessage, CancellationToken ct = default)
        => SubscribeToKlinesAsync(symbols, intervals, utc8: false, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToUtc8KlinesAsync(string symbol, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceSpotStreamKline>> onMessage, CancellationToken ct = default)
        => SubscribeToKlinesAsync([symbol], [interval], utc8: true, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToUtc8KlinesAsync(string symbol, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<BinanceSpotStreamKline>> onMessage, CancellationToken ct = default)
        => SubscribeToKlinesAsync([symbol], intervals, utc8: true, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToUtc8KlinesAsync(IEnumerable<string> symbols, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceSpotStreamKline>> onMessage, CancellationToken ct = default)
        => SubscribeToKlinesAsync(symbols, [interval], utc8: true, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToUtc8KlinesAsync(IEnumerable<string> symbols, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<BinanceSpotStreamKline>> onMessage, CancellationToken ct = default)
        => SubscribeToKlinesAsync(symbols, intervals, utc8: true, onMessage, ct);

    private Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(
        IEnumerable<string> symbols,
        IEnumerable<BinanceKlineInterval> intervals,
        bool utc8,
        Action<WebSocketDataEvent<BinanceSpotStreamKline>> onMessage,
        CancellationToken ct)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceSpotStreamKlineWrapper>>>(data =>
        {
            onMessage(data.As(data.Data.Data.Kline, data.Data.Data.Symbol));
        });

        var topics = BinanceSpotMarketDataValidation.KlineStreamTopics(symbols, intervals, utc8);
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceSpotStreamMiniTick>> onMessage, CancellationToken ct = default)
        => SubscribeToMiniTickersAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceSpotStreamMiniTick>> onMessage, CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols, 1024);

        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceSpotStreamMiniTick>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbolList.Select(a => a.ToLower(BinanceConstants.CI) + "@miniTicker");
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(Action<WebSocketDataEvent<IEnumerable<BinanceSpotStreamMiniTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<IEnumerable<BinanceSpotStreamMiniTick>>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Stream ?? ""));
        });

        return SubscribeAsync(["!miniTicker@arr"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceSpotStreamTick>> onMessage, CancellationToken ct = default)
        => SubscribeToTickersAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceSpotStreamTick>> onMessage, CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols, 1024);

        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceSpotStreamTick>>>(data =>
        {
            onMessage(data.As<BinanceSpotStreamTick>(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbolList.Select(a => a.ToLower(BinanceConstants.CI) + "@ticker");
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToRollingWindowTickersAsync(string symbol, TimeSpan windowSize, Action<WebSocketDataEvent<BinanceSpotStreamRollingWindowTick>> onMessage, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceSpotStreamRollingWindowTick>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Stream ?? ""));
        });

        var windowString = BinanceSpotMarketDataValidation.RollingStreamWindow(windowSize);
        return SubscribeAsync([$"{symbol.ToLowerInvariant()}@ticker_{windowString}"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToRollingWindowTickersAsync(TimeSpan windowSize, Action<WebSocketDataEvent<IEnumerable<BinanceSpotStreamRollingWindowTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<IEnumerable<BinanceSpotStreamRollingWindowTick>>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Stream ?? ""));
        });

        var windowString = BinanceSpotMarketDataValidation.RollingStreamWindow(windowSize);
        return SubscribeAsync([$"!ticker_{windowString}@arr"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceSpotStreamBookPrice>> onMessage, CancellationToken ct = default)
        => SubscribeToBookTickersAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceSpotStreamBookPrice>> onMessage, CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols, 1024);

        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceSpotStreamBookPrice>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbolList.Select(a => a.ToLower(BinanceConstants.CI) + "@bookTicker");
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAveragePriceAsync(string symbol, Action<WebSocketDataEvent<BinanceSpotStreamAveragePrice>> onMessage, CancellationToken ct = default)
        => SubscribeToAveragePriceAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAveragePriceAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceSpotStreamAveragePrice>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceSpotStreamAveragePrice>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = BinanceSpotMarketDataValidation.SymbolStreamTopics(symbols, "@avgPrice");
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBooksAsync(string symbol, int levels, int? updateInterval, Action<WebSocketDataEvent<BinanceSpotOrderBook>> onMessage, CancellationToken ct = default)
        => SubscribeToPartialOrderBooksAsync([symbol], levels, updateInterval, onMessage, ct);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBooksAsync(IEnumerable<string> symbols, int levels, int? updateInterval, Action<WebSocketDataEvent<BinanceSpotOrderBook>> onMessage, CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols, 1024);
        levels.ValidateIntValues(nameof(levels), 5, 10, 20);

        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 1000);

        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceSpotOrderBook>>>(data =>
        {
            data.Data.Data.Symbol = data.Data.Stream?.Split('@')[0].ToUpperInvariant() ?? "";
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbolList.Select(a => a.ToLower(BinanceConstants.CI) + "@depth" + levels + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : ""));
        return await SubscribeAsync(topics, false, handler, ct).ConfigureAwait(false);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBooksAsync(string symbol, int? updateInterval, Action<WebSocketDataEvent<BinanceSpotStreamOrderBook>> onMessage, CancellationToken ct = default)
        => SubscribeToOrderBooksAsync([symbol], updateInterval, onMessage, ct);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBooksAsync(IEnumerable<string> symbols, int? updateInterval, Action<WebSocketDataEvent<BinanceSpotStreamOrderBook>> onMessage, CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols, 1024);
        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 1000);

        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceSpotStreamOrderBook>>>(data =>
        {
            onMessage(data.As<BinanceSpotStreamOrderBook>(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbolList.Select(a => a.ToLower(BinanceConstants.CI) + "@depth" + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : ""));
        return await SubscribeAsync(topics, false, handler, ct);
    }
}
