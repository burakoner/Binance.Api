namespace Binance.Api.Spot;

internal partial class BinanceSpotSocketClient
{
    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradesAsync(string symbol, Action<WebSocketDataEvent<BinanceSpotStreamAggregatedTrade>> onMessage, CancellationToken ct = default)
        => SubscribeToAggregatedTradesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceSpotStreamAggregatedTrade>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceSpotStreamCombinedStream<BinanceSpotStreamAggregatedTrade>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@aggTrade").ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradesAsync(string symbol, Action<WebSocketDataEvent<BinanceSpotStreamTrade>> onMessage, CancellationToken ct = default)
        => SubscribeToTradesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceSpotStreamTrade>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceSpotStreamCombinedStream<BinanceSpotStreamTrade>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@trade").ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(string symbol, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceSpotStreamKlineData>> onMessage, CancellationToken ct = default)
        => SubscribeToKlinesAsync([symbol], interval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(string symbol, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<BinanceSpotStreamKlineData>> onMessage, CancellationToken ct = default)
        => SubscribeToKlinesAsync([symbol], intervals, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(IEnumerable<string> symbols, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceSpotStreamKlineData>> onMessage, CancellationToken ct = default)
        => SubscribeToKlinesAsync(symbols, [interval], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(IEnumerable<string> symbols, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<BinanceSpotStreamKlineData>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceSpotStreamCombinedStream<BinanceSpotStreamKlineData>>>(data =>
        {
            onMessage(data.As<BinanceSpotStreamKlineData>(data.Data.Data, data.Data.Data.Symbol));
        }
        );

        var topics = symbols.SelectMany(a => intervals.Select(i => a.ToLower(BinanceConstants.CI) + "@kline" + "_" + MapConverter.GetString(i))).ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    // TODO: Kline/Candlestick Streams with timezone offset

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceSpotStreamMiniTick>> onMessage, CancellationToken ct = default)
        => SubscribeToMiniTickersAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceSpotStreamMiniTick>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceSpotStreamCombinedStream<BinanceSpotStreamMiniTick>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbols.Select(a => a.ToLower(BinanceConstants.CI  ) + "@miniTicker").ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickersAsync(Action<WebSocketDataEvent<IEnumerable<BinanceSpotStreamMiniTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSpotStreamCombinedStream<IEnumerable<BinanceSpotStreamMiniTick>>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Stream ?? ""));
        });

        return SubscribeAsync(["!miniTicker@arr"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceSpotStreamTick>> onMessage, CancellationToken ct = default)
        => SubscribeToTickersAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceSpotStreamTick>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceSpotStreamCombinedStream<BinanceSpotStreamTick>>>(data =>
        {
            onMessage(data.As<BinanceSpotStreamTick>(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@ticker").ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(Action<WebSocketDataEvent<IEnumerable<BinanceSpotStreamTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSpotStreamCombinedStream<IEnumerable<BinanceSpotStreamTick>>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Stream ?? ""));
        });

        return SubscribeAsync(["!ticker@arr"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToRollingWindowTickersAsync(string symbol, TimeSpan windowSize, Action<WebSocketDataEvent<BinanceSpotStreamRollingWindowTick>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSpotStreamCombinedStream<BinanceSpotStreamRollingWindowTick>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Stream ?? ""));
        });

        var windowString = windowSize < TimeSpan.FromDays(1) ? windowSize.TotalHours + "h" : windowSize.TotalDays + "d";
        return SubscribeAsync([$"{symbol.ToLowerInvariant()}@ticker_{windowString}"], false, handler, ct);
    }

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToRollingWindowTickersAsync(TimeSpan windowSize, Action<WebSocketDataEvent<IEnumerable<BinanceSpotStreamRollingWindowTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSpotStreamCombinedStream<IEnumerable<BinanceSpotStreamRollingWindowTick>>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Stream ?? ""));
        });

        var windowString = windowSize < TimeSpan.FromDays(1) ? windowSize.TotalHours + "h" : windowSize.TotalDays + "d";
        return await SubscribeAsync([$"!ticker_{windowString}@arr"], false, handler, ct).ConfigureAwait(false);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceSpotStreamBookPrice>> onMessage, CancellationToken ct = default)
        => SubscribeToBookTickersAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceSpotStreamBookPrice>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceSpotStreamCombinedStream<BinanceSpotStreamBookPrice>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@bookTicker").ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBooksAsync(string symbol, int levels, int? updateInterval, Action<WebSocketDataEvent<BinanceSpotOrderBook>> onMessage, CancellationToken ct = default)
        => SubscribeToPartialOrderBooksAsync([symbol], levels, updateInterval, onMessage, ct);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBooksAsync(IEnumerable<string> symbols, int levels, int? updateInterval, Action<WebSocketDataEvent<BinanceSpotOrderBook>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        levels.ValidateIntValues(nameof(levels), 5, 10, 20);
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 1000);

        var handler = new Action<WebSocketDataEvent<BinanceSpotStreamCombinedStream<BinanceSpotOrderBook>>>(data =>
        {
            data.Data.Data.Symbol = data.Data.Stream?.Split('@')[0] ?? "";
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@depth" + levels + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return await SubscribeAsync(topics, false, handler, ct).ConfigureAwait(false);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBooksAsync(string symbol, int? updateInterval, Action<WebSocketDataEvent<BinanceSpotStreamOrderBook>> onMessage, CancellationToken ct = default)
        => SubscribeToOrderBooksAsync([symbol], updateInterval, onMessage, ct);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBooksAsync(IEnumerable<string> symbols, int? updateInterval, Action<WebSocketDataEvent<BinanceSpotStreamOrderBook>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 1000);
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceSpotStreamCombinedStream<BinanceSpotStreamOrderBook>>>(data =>
        {
            onMessage(data.As<BinanceSpotStreamOrderBook>(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@depth" + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return await SubscribeAsync(topics, false, handler, ct);
    }
}