namespace Binance.Api.Spot;

internal partial class BinanceSpotSocketClient
{
    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceStreamAggregatedTrade>> onMessage, CancellationToken ct = default)
        => SubscribeToAggregatedTradeUpdatesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceStreamAggregatedTrade>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamAggregatedTrade>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + "@aggTrade").ToArray();
        return SubscribeAsync(topics, "", false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceStreamTrade>> onMessage, CancellationToken ct = default)
        => SubscribeToTradeUpdatesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceStreamTrade>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamTrade>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + "@trade").ToArray();
        return SubscribeAsync(topics, "", false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, BinanceKlineInterval interval, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default)
        => SubscribeToKlineUpdatesAsync([symbol], interval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default)
        => SubscribeToKlineUpdatesAsync([symbol], intervals, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols,
        BinanceKlineInterval interval, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default)
        => SubscribeToKlineUpdatesAsync(symbols, [interval], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamKlineData>>>(data =>
        {
            onMessage(data.As<IBinanceStreamKlineData>(data.Data.Data, data.Data.Data.Symbol));
        }
        );

        var topics = symbols.SelectMany(a => intervals.Select(i => a.ToLower(CultureInfo.InvariantCulture) + "@kline" + "_" + MapConverter.GetString(i))).ToArray();
        return SubscribeAsync(topics, "", false, handler, ct);
    }

    // TODO: Kline/Candlestick Streams with timezone offset

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceStreamMiniTick>> onMessage, CancellationToken ct = default)
        => SubscribeToMiniTickerUpdatesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceStreamMiniTick>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamMiniTick>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + "@miniTicker").ToArray();
        return SubscribeAsync(topics, "", false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllMiniTickerUpdatesAsync(Action<WebSocketDataEvent<IEnumerable<BinanceStreamMiniTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<IEnumerable<BinanceStreamMiniTick>>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Stream ?? ""));
        });

        return SubscribeAsync(["!miniTicker@arr"], "", false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<WebSocketDataEvent<IBinanceTick>> onMessage, CancellationToken ct = default)
        => SubscribeToTickerUpdatesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<IBinanceTick>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamTick>>>(data =>
        {
            onMessage(data.As<IBinanceTick>(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + "@ticker").ToArray();
        return SubscribeAsync(topics, "", false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<WebSocketDataEvent<IEnumerable<BinanceStreamTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<IEnumerable<BinanceStreamTick>>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Stream ?? ""));
        });

        return SubscribeAsync(["!ticker@arr"], "", false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToRollingWindowTickerUpdatesAsync(string symbol, TimeSpan windowSize, Action<WebSocketDataEvent<BinanceStreamRollingWindowTick>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamRollingWindowTick>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Stream ?? ""));
        });

        var windowString = windowSize < TimeSpan.FromDays(1) ? windowSize.TotalHours + "h" : windowSize.TotalDays + "d";
        return SubscribeAsync([$"{symbol.ToLowerInvariant()}@ticker_{windowString}"], "", false, handler, ct);
    }

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllRollingWindowTickerUpdatesAsync(TimeSpan windowSize, Action<WebSocketDataEvent<IEnumerable<BinanceStreamRollingWindowTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<IEnumerable<BinanceStreamRollingWindowTick>>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Stream ?? ""));
        });

        var windowString = windowSize < TimeSpan.FromDays(1) ? windowSize.TotalHours + "h" : windowSize.TotalDays + "d";
        return await SubscribeAsync([$"!ticker_{windowString}@arr"], "", false, handler, ct).ConfigureAwait(false);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickerUpdatesAsync(string symbol, Action<WebSocketDataEvent<BinanceStreamBookPrice>> onMessage, CancellationToken ct = default)
        => SubscribeToBookTickerUpdatesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickerUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceStreamBookPrice>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamBookPrice>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + "@bookTicker").ToArray();
        return SubscribeAsync(topics, "", false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol, int levels, int? updateInterval, Action<WebSocketDataEvent<BinanceSpotOrderBook>> onMessage, CancellationToken ct = default)
        => SubscribeToPartialOrderBookUpdatesAsync([symbol], levels, updateInterval, onMessage, ct);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(IEnumerable<string> symbols, int levels, int? updateInterval, Action<WebSocketDataEvent<BinanceSpotOrderBook>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        levels.ValidateIntValues(nameof(levels), 5, 10, 20);
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 1000);

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceSpotOrderBook>>>(data =>
        {
            data.Data.Data.Symbol = data.Data.Stream.Split('@')[0];
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + "@depth" + levels + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return await SubscribeAsync(topics, "", false, handler, ct).ConfigureAwait(false);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int? updateInterval, Action<WebSocketDataEvent<IBinanceEventOrderBook>> onMessage, CancellationToken ct = default)
        => SubscribeToOrderBookUpdatesAsync(new[] { symbol }, updateInterval, onMessage, ct);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols, int? updateInterval, Action<WebSocketDataEvent<IBinanceEventOrderBook>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 1000);
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceEventOrderBook>>>(data =>
        {
            onMessage(data.As<IBinanceEventOrderBook>(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + "@depth" + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return await SubscribeAsync(topics, "", false, handler, ct);
    }
}