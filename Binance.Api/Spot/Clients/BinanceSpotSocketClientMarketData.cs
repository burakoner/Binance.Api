namespace Binance.Api.Spot;

public partial class BinanceSpotSocketClient
{
    // Endpoints
    private const string depthStreamEndpoint = "@depth";
    private const string tradesStreamEndpoint = "@trade";
    private const string bookTickerStreamEndpoint = "@bookTicker";
    private const string allBookTickerStreamEndpoint = "!bookTicker";
    private const string klineStreamEndpoint = "@kline";
    private const string aggregatedTradesStreamEndpoint = "@aggTrade";
    private const string symbolTickerStreamEndpoint = "@ticker";
    private const string allSymbolTickerStreamEndpoint = "!ticker@arr";
    private const string partialBookDepthStreamEndpoint = "@depth";
    private const string symbolMiniTickerStreamEndpoint = "@miniTicker";
    private const string allSymbolMiniTickerStreamEndpoint = "!miniTicker@arr";
    private const string SocketClientApiAddress = "wss://ws-api.binance.com:443/";


    #region Queries

    public async Task<CallResult<TimeSpan>> PingAsync(CancellationToken ct = default)
    {

        var sw = Stopwatch.StartNew();
        var result = await BinanceQueryAsync<object>("ws-api/v3", $"ping", [], ct: ct).ConfigureAwait(false);
        sw.Stop();

        return result.Success
            ? result.As(sw.Elapsed)
            : result.AsError<TimeSpan>(result.Error!);
    }

    public async Task<CallResult<DateTime>> GetTimeAsync(CancellationToken ct = default)
    {
        var result = await BinanceQueryAsync<BinanceServerTime>("ws-api/v3", $"time", [], false, ct: ct).ConfigureAwait(false);
        if (!result) return result.AsError<DateTime>(result.Error!);

        return result.As(result.Data.ServerTime);
    }

    public async Task<CallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(IEnumerable<string>? symbols = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("symbols", symbols);
        var result = await BinanceQueryAsync<BinanceSpotExchangeInfo>("ws-api/v3", $"exchangeInfo", parameters, weight: 20, ct: ct).ConfigureAwait(false);
        if (!result) return result;

        ExchangeInfo = result.Data;
        LastExchangeInfoUpdate = DateTime.UtcNow;
        _logger.Log(LogLevel.Information, "Trade rules updated");
        return result;
    }

    public async Task<CallResult<BinanceSpotOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptionalParameter("limit", limit);
        int weight = limit <= 100 ? 5 : limit <= 500 ? 25 : limit <= 1000 ? 50 : 250;
        var result = await BinanceQueryAsync<BinanceSpotOrderBook>("ws-api/v3", $"depth", parameters, weight: weight, ct: ct).ConfigureAwait(false);
        if (result) result.Data.Symbol = symbol;
        return result;
    }

    public async Task<CallResult<IEnumerable<BinanceSpotTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptionalParameter("limit", limit);
        return await BinanceQueryAsync<IEnumerable<BinanceSpotTrade>>("ws-api/v3", $"trades.recent", parameters, weight: 25, ct: ct).ConfigureAwait(false);
    }

    public async Task<CallResult<IEnumerable<BinanceSpotTrade>>> GetTradeHistoryAsync(string symbol, long? fromId = null, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptionalParameter("limit", limit);
        parameters.AddOptionalParameter("fromId", fromId);
        return await BinanceQueryAsync<IEnumerable<BinanceSpotTrade>>("ws-api/v3", $"trades.historical", parameters, false, weight: 25, ct: ct).ConfigureAwait(false);
    }

    public async Task<CallResult<IEnumerable<BinanceStreamAggregatedTrade>>> GetAggregatedTradeHistoryAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptionalParameter("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptionalParameter("fromId", fromId);
        return await BinanceQueryAsync<IEnumerable<BinanceStreamAggregatedTrade>>("ws-api/v3", $"trades.aggregate", parameters, false, weight: 2, ct: ct).ConfigureAwait(false);
    }

    public async Task<CallResult<IEnumerable<BinanceSpotKline>>> GetKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddEnum("interval", interval);
        parameters.AddOptionalParameter("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        return await BinanceQueryAsync<IEnumerable<BinanceSpotKline>>("ws-api/v3", $"klines", parameters, false, weight: 2, ct: ct).ConfigureAwait(false);
    }

    public async Task<CallResult<IEnumerable<BinanceSpotKline>>> GetUIKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddEnum("interval", interval);
        parameters.AddOptionalParameter("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        return await BinanceQueryAsync<IEnumerable<BinanceSpotKline>>("ws-api/v3", $"uiKlines", parameters, false, weight: 2, ct: ct).ConfigureAwait(false);
    }

    public async Task<CallResult<BinanceSpotAveragePrice>> GetAveragePriceAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddParameter("symbol", symbol);
        return await BinanceQueryAsync<BinanceSpotAveragePrice>("ws-api/v3", $"avgPrice", parameters, false, weight: 2, ct: ct).ConfigureAwait(false);
    }

    public async Task<CallResult<BinanceSpotTicker>> GetTickerAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "type", "FULL" }
        };
        return await BinanceQueryAsync<BinanceSpotTicker>("ws-api/v3", $"ticker.24hr", parameters, false, weight: 2, ct: ct).ConfigureAwait(false);
    }

    public async Task<CallResult<IEnumerable<BinanceSpotTicker>>> GetTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" },
            { "type", "FULL" }
        };
        var symbolCount = symbols?.Count();
        int weight = symbolCount == null || symbolCount > 100 ? 80 : symbolCount <= 20 ? 2 : 40;
        return await BinanceQueryAsync<IEnumerable<BinanceSpotTicker>>("ws-api/v3", $"ticker.24hr", parameters, false, weight: weight, ct: ct).ConfigureAwait(false);
    }

    public async Task<CallResult<IEnumerable<BinanceSpotTicker>>> GetTickersAsync(CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "FULL" }
        };
        return await BinanceQueryAsync<IEnumerable<BinanceSpotTicker>>("ws-api/v3", $"ticker.24hr", parameters, false, weight: 80, ct: ct).ConfigureAwait(false);
    }

    public async Task<CallResult<BinanceSpotMiniTicker>> GetMiniTickerAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "type", "MINI" }
        };
        return await BinanceQueryAsync<BinanceSpotMiniTicker>("ws-api/v3", $"ticker.24hr", parameters, false, weight: 2, ct: ct).ConfigureAwait(false);
    }

    public async Task<CallResult<IEnumerable<BinanceSpotMiniTicker>>> GetMiniTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" },
            { "type", "MINI" }
        };
        var symbolCount = symbols?.Count();
        int weight = symbolCount == null || symbolCount > 100 ? 80 : symbolCount <= 20 ? 2 : 40;
        return await BinanceQueryAsync<IEnumerable<BinanceSpotMiniTicker>>("ws-api/v3", $"ticker.24hr", parameters, false, weight: weight, ct: ct).ConfigureAwait(false);
    }

    public async Task<CallResult<IEnumerable<BinanceSpotMiniTicker>>> GetMiniTickersAsync(CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "MINI" }
        };
        return await BinanceQueryAsync<IEnumerable<BinanceSpotMiniTicker>>("ws-api/v3", $"ticker.24hr", parameters, false, weight: 80, ct: ct).ConfigureAwait(false);
    }

    public async Task<CallResult<BinanceSpotTicker>> GetRollingWindowTickerAsync(string symbol,  TimeSpan? windowSize = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
        };
        parameters.AddOptional("windowSize", windowSize == null ? null : GetWindowSize(windowSize.Value));
        return await BinanceQueryAsync<BinanceSpotTicker>("ws-api/v3", $"ticker", parameters, false, weight: 2, ct: ct).ConfigureAwait(false);
    }

    public async Task<CallResult<IEnumerable<BinanceSpotTicker>>> GetRollingWindowTickersAsync(IEnumerable<string> symbols,  TimeSpan? windowSize = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" },
        };
        parameters.AddOptional("windowSize", windowSize == null ? null : GetWindowSize(windowSize.Value));
        var symbolCount = symbols?.Count();
        int weight = symbolCount == null || symbolCount > 100 ? 80 : symbolCount <= 20 ? 2 : 40;
        return await BinanceQueryAsync<IEnumerable<BinanceSpotTicker>>("ws-api/v3", $"ticker", parameters, false, weight: weight, ct: ct).ConfigureAwait(false);
    }



    public async Task<CallResult<BinanceSpotBookTicker>> GetBookTickerAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
        };
        return await BinanceQueryAsync<BinanceSpotBookTicker>("ws-api/v3", $"ticker.book", parameters, false, weight: 2, ct: ct).ConfigureAwait(false);
    }

    public async Task<CallResult<IEnumerable<BinanceSpotBookTicker>>> GetBookTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" },
        };
        return await BinanceQueryAsync<IEnumerable<BinanceSpotBookTicker>>("ws-api/v3", $"ticker.book", parameters, false, weight: 4, ct: ct).ConfigureAwait(false);
    }

    public async Task<CallResult<IEnumerable<BinanceSpotBookTicker>>> GetBookTickersAsync(CancellationToken ct = default)
    {
        return await BinanceQueryAsync<IEnumerable<BinanceSpotBookTicker>>("ws-api/v3", $"ticker.book", [], false, weight: 4, ct: ct).ConfigureAwait(false);
    }


    private string GetWindowSize(TimeSpan timeSpan)
    {
        if (timeSpan.TotalHours < 1) return timeSpan.TotalMinutes + "m";
        else if (timeSpan.TotalHours < 24) return timeSpan.TotalHours + "h";
        return timeSpan.TotalDays + "d";
    }
















    #region Diff. Depth Stream
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol,
        int? updateInterval, Action<WebSocketDataEvent<IBinanceEventOrderBook>> onMessage, CancellationToken ct = default) =>
        await SubscribeToOrderBookUpdatesAsync(new[] { symbol }, updateInterval, onMessage, ct).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols,
        int? updateInterval, Action<WebSocketDataEvent<IBinanceEventOrderBook>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 1000);
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceEventOrderBook>>>(data => onMessage(data.As<IBinanceEventOrderBook>(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a =>
            a.ToLower(CultureInfo.InvariantCulture) + depthStreamEndpoint +
            (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return await SubscribeAsync(symbols, "", false, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Aggregate Trade Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(string symbol,
        Action<WebSocketDataEvent<BinanceStreamAggregatedTrade>> onMessage, CancellationToken ct = default) =>
        await SubscribeToAggregatedTradeUpdatesAsync([symbol], onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(
        IEnumerable<string> symbols, Action<WebSocketDataEvent<BinanceStreamAggregatedTrade>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamAggregatedTrade>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + aggregatedTradesStreamEndpoint)
            .ToArray();
        return await SubscribeAsync(symbols, "", false, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Trade Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol,
        Action<WebSocketDataEvent<BinanceStreamTrade>> onMessage, CancellationToken ct = default) =>
        await SubscribeToTradeUpdatesAsync([symbol], onMessage, ct).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols,
        Action<WebSocketDataEvent<BinanceStreamTrade>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamTrade>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + tradesStreamEndpoint).ToArray();
        return await SubscribeAsync(symbols, "", false, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Kline/Candlestick Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol,
        BinanceKlineInterval interval, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default) =>
        await SubscribeToKlineUpdatesAsync([symbol], interval, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol,
        IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default) =>
        await SubscribeToKlineUpdatesAsync([symbol], intervals, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols,
        BinanceKlineInterval interval, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default) =>
        await SubscribeToKlineUpdatesAsync(symbols, [interval], onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols,
        IEnumerable<BinanceKlineInterval> intervals, Action<WebSocketDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamKlineData>>>(data => onMessage(data.As<IBinanceStreamKlineData>(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.SelectMany(a =>
            intervals.Select(i =>
                a.ToLower(CultureInfo.InvariantCulture) + klineStreamEndpoint + "_" +
                MapConverter.GetString(i))).ToArray();
        return await SubscribeAsync(symbols, "", false, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Individual Symbol Mini Ticker Stream
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(string symbol,
        Action<WebSocketDataEvent<IBinanceMiniTick>> onMessage, CancellationToken ct = default) =>
        await SubscribeToMiniTickerUpdatesAsync([symbol], onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(
        IEnumerable<string> symbols, Action<WebSocketDataEvent<IBinanceMiniTick>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamMiniTick>>>(data => onMessage(data.As<IBinanceMiniTick>(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + symbolMiniTickerStreamEndpoint)
            .ToArray();

        return await SubscribeAsync(symbols, "", false, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region All Market Mini Tickers Stream
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllMiniTickerUpdatesAsync(
        Action<WebSocketDataEvent<IEnumerable<IBinanceMiniTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<IEnumerable<BinanceStreamMiniTick>>>>(data => onMessage(data.As<IEnumerable<IBinanceMiniTick>>(data.Data.Data, data.Data.Stream)));
        return await SubscribeAsync([allSymbolMiniTickerStreamEndpoint], "", false, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Individual Market Rolling Window Tickers Stream
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToRollingWindowTickerUpdatesAsync(string symbol, TimeSpan windowSize,
        Action<WebSocketDataEvent<BinanceStreamRollingWindowTick>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamRollingWindowTick>>>(data => onMessage(data.As(data.Data.Data, data.Data.Stream)));
        var windowString = windowSize < TimeSpan.FromDays(1) ? windowSize.TotalHours + "h" : windowSize.TotalDays + "d";
        return await SubscribeAsync([$"{symbol.ToLowerInvariant()}@ticker_{windowString}"], "", false, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region All Market Rolling Window Tickers Stream
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllRollingWindowTickerUpdatesAsync(TimeSpan windowSize,
        Action<WebSocketDataEvent<IEnumerable<BinanceStreamRollingWindowTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<IEnumerable<BinanceStreamRollingWindowTick>>>>(data => onMessage(data.As(data.Data.Data, data.Data.Stream)));
        var windowString = windowSize < TimeSpan.FromDays(1) ? windowSize.TotalHours + "h" : windowSize.TotalDays + "d";
        return await SubscribeAsync([$"!ticker_{windowString}@arr"], "", false, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Individual Symbol Book Ticker Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickerUpdatesAsync(string symbol,
        Action<WebSocketDataEvent<BinanceStreamBookPrice>> onMessage, CancellationToken ct = default) =>
        await SubscribeToBookTickerUpdatesAsync([symbol], onMessage, ct).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBookTickerUpdatesAsync(IEnumerable<string> symbols,
        Action<WebSocketDataEvent<BinanceStreamBookPrice>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamBookPrice>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + bookTickerStreamEndpoint).ToArray();
        return await SubscribeAsync(symbols, "", false, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region All Book Tickers Stream
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllBookTickerUpdatesAsync(
        Action<WebSocketDataEvent<BinanceStreamBookPrice>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamBookPrice>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        return await SubscribeAsync([allBookTickerStreamEndpoint], "", false, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Partial Book Depth Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol,
        int levels, int? updateInterval, Action<WebSocketDataEvent<BinanceSpotOrderBook>> onMessage, CancellationToken ct = default) =>
        await SubscribeToPartialOrderBookUpdatesAsync([symbol], levels, updateInterval, onMessage, ct)
            .ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(
        IEnumerable<string> symbols, int levels, int? updateInterval, Action<WebSocketDataEvent<BinanceSpotOrderBook>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        levels.ValidateIntValues(nameof(levels), 5, 10, 20);
        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 1000);

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceSpotOrderBook>>>(data =>
        {
            data.Data.Data.Symbol = data.Data.Stream.Split('@')[0];
            onMessage(data.As(data.Data.Data, data.Data.Data.Symbol));
        });

        symbols = symbols.Select(a =>
            a.ToLower(CultureInfo.InvariantCulture) + partialBookDepthStreamEndpoint + levels +
            (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return await SubscribeAsync(symbols, "", false, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Individual Symbol Ticker Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<WebSocketDataEvent<IBinanceTick>> onMessage, CancellationToken ct = default) => await SubscribeToTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<WebSocketDataEvent<IBinanceTick>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamTick>>>(data => onMessage(data.As<IBinanceTick>(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + symbolTickerStreamEndpoint).ToArray();
        return await SubscribeAsync(symbols, "", false, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region All Market Tickers Streams
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<WebSocketDataEvent<IEnumerable<IBinanceTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<IEnumerable<BinanceStreamTick>>>>(data => onMessage(data.As<IEnumerable<IBinanceTick>>(data.Data.Data, data.Data.Stream)));
        return await SubscribeAsync([allSymbolTickerStreamEndpoint], "", false, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #endregion

}