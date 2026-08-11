namespace Binance.Api.Options;

internal partial class BinanceOptionsSocketClient
{
    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToNewSymbolsAsync(Action<WebSocketDataEvent<BinanceOptionsStreamSymbol>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceOptionsStreamSymbol>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        return SubscribeMarketAsync(["option_pair"], false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOpenInterestAsync(string asset, DateTime expiration, Action<WebSocketDataEvent<BinanceOptionsStreamOpenInterest>> onMessage, CancellationToken ct = default)
        => SubscribeToOpenInterestAsync([(asset, expiration)], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOpenInterestAsync(IEnumerable<(string UnderlyingAsset, DateTime ExpirationDate)> tuples, Action<WebSocketDataEvent<BinanceOptionsStreamOpenInterest>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<List<BinanceOptionsStreamOpenInterest>>>>(data =>
        {
            foreach (var item in data.Data.Data)
            {
                onMessage(data.As(item, item.Symbol));
            }
        });

        var topics = tuples.Select(a => $"{a.UnderlyingAsset}@openInterest{a.ExpirationDate.ToString("MMddyy")}").ToArray();
        return SubscribeMarketAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceAsync(string asset, Action<WebSocketDataEvent<BinanceOptionsStreamMarkPrice>> onMessage, CancellationToken ct = default)
        => SubscribeToMarkPriceAsync([asset], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMarkPriceAsync(IEnumerable<string> assets, Action<WebSocketDataEvent<BinanceOptionsStreamMarkPrice>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<List<BinanceOptionsStreamMarkPrice>>>>(data =>
        {
            foreach (var item in data.Data.Data)
            {
                onMessage(data.As(item, item.Symbol));
            }
        });

        var topics = assets.Select(a => $"{a}@markPrice").ToArray();
        return SubscribeMarketAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(string symbol, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceOptionsStreamKline>> onMessage, CancellationToken ct = default)
        => SubscribeToKlinesAsync([symbol], interval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(string symbol, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<BinanceOptionsStreamKline>> onMessage, CancellationToken ct = default)
        => SubscribeToKlinesAsync([symbol], intervals, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(IEnumerable<string> symbols, BinanceKlineInterval interval, Action<WebSocketDataEvent<BinanceOptionsStreamKline>> onMessage, CancellationToken ct = default)
        => SubscribeToKlinesAsync(symbols, [interval], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlinesAsync(IEnumerable<string> symbols, IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<BinanceOptionsStreamKline>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceOptionsStreamKlineWrapper>>>(data =>
        {
            onMessage(data.As(data.Data.Data.Kline, data.Data.Data.Symbol));
        });

        var topics = symbols.SelectMany(a => intervals.Select(i => a + "@kline" + "_" + MapConverter.GetString(i))).ToArray();
        return SubscribeMarketAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(string symbol, DateTime expiration, Action<WebSocketDataEvent<BinanceOptionsStreamTicker>> onMessage, CancellationToken ct = default)
        => SubscribeToTickersAsync([(symbol, expiration)], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(IEnumerable<(string Symbol, DateTime ExpirationDate)> tuples, Action<WebSocketDataEvent<BinanceOptionsStreamTicker>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceOptionsStreamTicker>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        return SubscribePublicAsync(TickerStreamTopics(tuples), false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexPricesAsync(string symbol, Action<WebSocketDataEvent<BinanceOptionsStreamIndexPrice>> onMessage, CancellationToken ct = default)
        => SubscribeToIndexPricesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToIndexPricesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceOptionsStreamIndexPrice>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceOptionsStreamIndexPrice>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbols.Select(a => a + "@index").ToArray();
        return SubscribeMarketAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceOptionsStreamTicker>> onMessage, CancellationToken ct = default)
        => SubscribeToTickersAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceOptionsStreamTicker>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceOptionsStreamTicker>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        return SubscribePublicAsync(TickerStreamTopics(symbols), false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradesAsync(string symbol, Action<WebSocketDataEvent<BinanceOptionsStreamTrade>> onMessage, CancellationToken ct = default)
        => SubscribeToTradesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceOptionsStreamTrade>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceOptionsStreamTrade>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        return SubscribePublicAsync(TradeStreamTopics(symbols), false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBooksAsync(
        string symbol,
        int updateInterval,
        Action<WebSocketDataEvent<BinanceOptionsStreamOrderBook>> onMessage,
        CancellationToken ct = default)
        => SubscribeToOrderBooksAsync([symbol], updateInterval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBooksAsync(
        IEnumerable<string> symbols,
        int updateInterval,
        Action<WebSocketDataEvent<BinanceOptionsStreamOrderBook>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceOptionsStreamOrderBook>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        return SubscribePublicAsync(DiffDepthStreamTopics(symbols, updateInterval), false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickersAsync(
        string symbol,
        Action<WebSocketDataEvent<BinanceOptionsStreamBookTicker>> onMessage,
        CancellationToken ct = default)
        => SubscribeToBookTickersAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickersAsync(
        IEnumerable<string> symbols,
        Action<WebSocketDataEvent<BinanceOptionsStreamBookTicker>> onMessage,
        CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceOptionsStreamBookTicker>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        return SubscribePublicAsync(BookTickerStreamTopics(symbols), false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBooksAsync(string symbol, int levels, int updateInterval, Action<WebSocketDataEvent<BinanceOptionsStreamOrderBook>> onMessage, CancellationToken ct = default)
        => SubscribeToPartialOrderBooksAsync([symbol], levels, updateInterval, onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBooksAsync(IEnumerable<string> symbols, int levels, int updateInterval, Action<WebSocketDataEvent<BinanceOptionsStreamOrderBook>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceOptionsStreamOrderBook>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        return SubscribePublicAsync(PartialDepthStreamTopics(symbols, levels, updateInterval), false, handler, ct);
    }

    internal static string[] DiffDepthStreamTopics(IEnumerable<string> symbols, int updateInterval)
    {
        ValidateUpdateInterval(updateInterval);
        return SymbolStreamTopics(symbols, $"@depth@{updateInterval}ms");
    }

    internal static string[] BookTickerStreamTopics(IEnumerable<string> symbols)
        => SymbolStreamTopics(symbols, "@bookTicker");

    internal static string[] PartialDepthStreamTopics(IEnumerable<string> symbols, int levels, int updateInterval)
    {
        levels.ValidateIntValues(nameof(levels), 5, 10, 20);
        ValidateUpdateInterval(updateInterval);
        return SymbolStreamTopics(symbols, $"@depth{levels}@{updateInterval}ms");
    }

    internal static string[] TickerStreamTopics(IEnumerable<string> symbols)
        => SymbolStreamTopics(symbols, "@optionTicker");

    internal static string[] TickerStreamTopics(IEnumerable<(string Symbol, DateTime ExpirationDate)> tuples)
    {
        if (tuples == null)
            throw new ArgumentNullException(nameof(tuples));
        var tupleList = tuples.ToArray();
        if (tupleList.Length == 0)
            throw new ArgumentException("At least one symbol and expiration date must be provided.", nameof(tuples));
        foreach (var tuple in tupleList)
            if (string.IsNullOrWhiteSpace(tuple.Symbol))
                throw new ArgumentException("Symbols cannot be null or blank.", nameof(tuples));

        return tupleList
            .Select(tuple => tuple.Symbol.ToLower(BinanceConstants.CI) + "@optionTicker" + tuple.ExpirationDate.ToString("yyMMdd", BinanceConstants.CI))
            .ToArray();
    }

    internal static string[] TradeStreamTopics(IEnumerable<string> symbols)
        => SymbolStreamTopics(symbols, "@optionTrade");

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

        return symbolList.Select(symbol => symbol.ToLower(BinanceConstants.CI) + suffix).ToArray();
    }

    private static void ValidateUpdateInterval(int updateInterval)
        => updateInterval.ValidateIntValues(nameof(updateInterval), 100, 500);

}
