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

    internal async Task<CallResult<T>> BinanceQueryAsync<T>(string url, string method, Dictionary<string, object> parameters, bool authenticated = false, bool sign = false, int weight = 1, CancellationToken ct = default)
    {
        if (authenticated)
        {
            if (AuthenticationProvider == null)
                throw new InvalidOperationException("No credentials provided for authenticated endpoint");

            var authProvider = (BinanceAuthentication)AuthenticationProvider;
            if (sign) parameters = authProvider.AuthenticateSocketParameters(parameters);
            else parameters.Add("apiKey", authProvider.Credentials.Key.GetString());
        }

        var request = new BinanceSocketQuery
        {
            Method = method,
            Params = parameters,
            Id = ExchangeHelpers.NextId()
        };

        var result = await base.QueryAsync<BinanceResponse<T>>(url, request, sign).ConfigureAwait(false);
        if (!result.Success && result.Error is BinanceRateLimitError rle)
        {
            /*
            if (rle.RetryAfter != null && RateLimiter != null && ClientOptions.RateLimiterEnabled)
            {
                _logger.LogWarning("Ratelimit error from server, pausing requests until {Until}", rle.RetryAfter.Value);
                await RateLimiter.SetRetryAfterGuardAsync(rle.RetryAfter.Value).ConfigureAwait(false);
            }
            */
        }

        return result.As(result.Data.Result);
    }

    public async Task<CallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
    {
        // var result = await QueryAsync<BinanceServerTime>(BinanceAddress.Default.SpotSocketClientAddress.AppendPath("ws-api/v3"), new Dictionary<string, object>(), false).ConfigureAwait(false);
        // var result = await QueryAsync<BinanceServerTime>(SocketClientApiAddress.AppendPath("ws-api/v3"), new Dictionary<string, object>(), false).ConfigureAwait(false);
        var result = await BinanceQueryAsync<BinanceServerTime>(SocketClientApiAddress.AppendPath("ws-api/v3"), $"time", [], false).ConfigureAwait(false);
        if (!result) return result.AsError<DateTime>(result.Error!);

        return result.As(result.Data.ServerTime);
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

}