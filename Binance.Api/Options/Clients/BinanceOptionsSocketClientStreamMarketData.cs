namespace Binance.Api.Options;

internal partial class BinanceOptionsSocketClient
{
    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToNewSymbolsAsync(Action<WebSocketDataEvent<BinanceOptionsStreamSymbol>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceOptionsStreamSymbol>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        return SubscribeAsync(["option_pair"], false, handler, ct);
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
        return SubscribeAsync(topics, false, handler, ct);
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
        return SubscribeAsync(topics, false, handler, ct);
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
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(string asset, DateTime expiration, Action<WebSocketDataEvent<BinanceOptionsStreamTicker>> onMessage, CancellationToken ct = default)
        => SubscribeToTickersAsync([(asset,expiration)], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(IEnumerable<(string UnderlyingAsset, DateTime ExpirationDate)> tuples, Action<WebSocketDataEvent<BinanceOptionsStreamTicker>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceOptionsStreamTicker>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = tuples.Select(a => $"{a.UnderlyingAsset}@ticker@{a.ExpirationDate.ToString("MMddyy")}").ToArray();
        return SubscribeAsync(topics, false, handler, ct);
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
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(string symbol, Action<WebSocketDataEvent<BinanceOptionsStreamTicker>> onMessage, CancellationToken ct = default)
        => SubscribeToTickersAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickersAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceOptionsStreamTicker>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceOptionsStreamTicker>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbols.Select(a => $"{a}@ticker").ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradesAsync(string symbol, Action<WebSocketDataEvent<BinanceOptionsStreamTrade>> onMessage, CancellationToken ct = default)
        => SubscribeToTradesAsync([symbol], onMessage, ct);

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceOptionsStreamTrade>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceOptionsStreamTrade>>>(data =>
        {
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbols.Select(a => $"{a}@trade").ToArray();
        return SubscribeAsync(topics, false, handler, ct);
    }

    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBooksAsync(string symbol, int levels, int? updateInterval, Action<WebSocketDataEvent<BinanceOptionsStreamOrderBook>> onMessage, CancellationToken ct = default)
        => SubscribeToPartialOrderBooksAsync([symbol], levels, updateInterval, onMessage, ct);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBooksAsync(IEnumerable<string> symbols, int levels, int? updateInterval, Action<WebSocketDataEvent<BinanceOptionsStreamOrderBook>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        levels.ValidateIntValues(nameof(levels), 10, 20, 50,100);
        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 1000);

        var handler = new Action<WebSocketDataEvent<BinanceSocketCombinedStream<BinanceOptionsStreamOrderBook>>>(data =>
        {
            data.Data.Data.Symbol = data.Data.Stream?.Split('@')[0] ?? "";
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        var topics = symbols.Select(a => a.ToLower(BinanceConstants.CI) + "@depth" + levels + (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return await SubscribeAsync(topics, false, handler, ct).ConfigureAwait(false);
    }

}