namespace Binance.Api.Clients.StreamApi.Spot;

public class BinanceStreamSpotMarketDataClient
{
    // Endpoints
    private const string depthStreamEndpoint = "@depth";
    private const string bookTickerStreamEndpoint = "@bookTicker";
    private const string allBookTickerStreamEndpoint = "!bookTicker";
    private const string klineStreamEndpoint = "@kline";
    private const string tradesStreamEndpoint = "@trade";
    private const string aggregatedTradesStreamEndpoint = "@aggTrade";
    private const string symbolTickerStreamEndpoint = "@ticker";
    private const string allSymbolTickerStreamEndpoint = "!ticker@arr";
    private const string partialBookDepthStreamEndpoint = "@depth";
    private const string symbolMiniTickerStreamEndpoint = "@miniTicker";
    private const string allSymbolMiniTickerStreamEndpoint = "!miniTicker@arr";

    // Internal References
    internal BinanceStreamSpotClient MainClient { get; }
    internal Log Log { get => MainClient.Log; }
    internal string BaseAddress{ get => Options.BaseAddress; }
    internal BinanceStreamClientOptions Options { get => MainClient.RootClient.Options; }
    internal CallResult<T> Deserialize<T>(string data, JsonSerializer serializer = null, int? requestId = null) => MainClient.Deserializer<T>(data, serializer, requestId);
    internal CallResult<T> Deserialize<T>(JToken obj, JsonSerializer serializer = null, int? requestId = null) => MainClient.Deserializer<T>(obj, serializer, requestId);
    internal Task<CallResult<UpdateSubscription>> SubscribeAsync<T>(string url, IEnumerable<string> topics, Action<StreamDataEvent<T>> onData, CancellationToken ct)
    => MainClient.SubscribeAsync<T>(url, topics, onData, ct);

    internal BinanceStreamSpotMarketDataClient(BinanceStreamSpotClient main)
    {
        MainClient = main;
    }

    #region Aggregate Trade Streams
    public async Task<CallResult<UpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(string symbol,
        Action<StreamDataEvent<BinanceStreamAggregatedTrade>> onMessage, CancellationToken ct = default) =>
        await SubscribeToAggregatedTradeUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<UpdateSubscription>> SubscribeToAggregatedTradeUpdatesAsync(
        IEnumerable<string> symbols, Action<StreamDataEvent<BinanceStreamAggregatedTrade>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceStreamAggregatedTrade>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + aggregatedTradesStreamEndpoint)
            .ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Trade Streams
    public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol,
        Action<StreamDataEvent<BinanceStreamTrade>> onMessage, CancellationToken ct = default) =>
        await SubscribeToTradeUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<CallResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(IEnumerable<string> symbols,
        Action<StreamDataEvent<BinanceStreamTrade>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceStreamTrade>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + tradesStreamEndpoint).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Kline/Candlestick Streams
    public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol,
        KlineInterval interval, Action<StreamDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default) =>
        await SubscribeToKlineUpdatesAsync(new[] { symbol }, interval, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(string symbol,
        IEnumerable<KlineInterval> intervals, Action<StreamDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default) =>
        await SubscribeToKlineUpdatesAsync(new[] { symbol }, intervals, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols,
        KlineInterval interval, Action<StreamDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default) =>
        await SubscribeToKlineUpdatesAsync(symbols, new[] { interval }, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(IEnumerable<string> symbols,
        IEnumerable<KlineInterval> intervals, Action<StreamDataEvent<IBinanceStreamKlineData>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceStreamKlineData>>>(data => onMessage(data.As<IBinanceStreamKlineData>(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.SelectMany(a =>
            intervals.Select(i =>
                a.ToLower(CultureInfo.InvariantCulture) + klineStreamEndpoint + "_" +
                JsonConvert.SerializeObject(i, new KlineIntervalConverter(false)))).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Individual Symbol Mini Ticker Stream
    public async Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(string symbol,
        Action<StreamDataEvent<IBinanceMiniTick>> onMessage, CancellationToken ct = default) =>
        await SubscribeToMiniTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<UpdateSubscription>> SubscribeToMiniTickerUpdatesAsync(
        IEnumerable<string> symbols, Action<StreamDataEvent<IBinanceMiniTick>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceStreamMiniTick>>>(data => onMessage(data.As<IBinanceMiniTick>(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + symbolMiniTickerStreamEndpoint)
            .ToArray();

        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region All Market Mini Tickers Stream
    public async Task<CallResult<UpdateSubscription>> SubscribeToAllMiniTickerUpdatesAsync(
        Action<StreamDataEvent<IEnumerable<IBinanceMiniTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<StreamDataEvent<BinanceCombinedStream<IEnumerable<BinanceStreamMiniTick>>>>(data => onMessage(data.As<IEnumerable<IBinanceMiniTick>>(data.Data.Data, data.Data.Stream)));
        return await SubscribeAsync(BaseAddress, new[] { allSymbolMiniTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Individual Market Rolling Window Tickers Stream
    public async Task<CallResult<UpdateSubscription>> SubscribeToRollingWindowTickerUpdatesAsync(string symbol, TimeSpan windowSize,
        Action<StreamDataEvent<BinanceStreamRollingWindowTick>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceStreamRollingWindowTick>>>(data => onMessage(data.As(data.Data.Data, data.Data.Stream)));
        var windowString = windowSize < TimeSpan.FromDays(1) ? windowSize.TotalHours + "h" : windowSize.TotalDays + "d";
        return await SubscribeAsync(BaseAddress, new[] { $"{symbol.ToLowerInvariant()}@ticker_{windowString}" }, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region All Market Rolling Window Tickers Stream
    public async Task<CallResult<UpdateSubscription>> SubscribeToAllRollingWindowTickerUpdatesAsync(TimeSpan windowSize,
        Action<StreamDataEvent<IEnumerable<BinanceStreamRollingWindowTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<StreamDataEvent<BinanceCombinedStream<IEnumerable<BinanceStreamRollingWindowTick>>>>(data => onMessage(data.As(data.Data.Data, data.Data.Stream)));
        var windowString = windowSize < TimeSpan.FromDays(1) ? windowSize.TotalHours + "h" : windowSize.TotalDays + "d";
        return await SubscribeAsync(BaseAddress, new[] { $"!ticker_{windowString}@arr" }, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Individual Symbol Book Ticker Streams
    public async Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(string symbol,
        Action<StreamDataEvent<BinanceStreamBookPrice>> onMessage, CancellationToken ct = default) =>
        await SubscribeToBookTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<CallResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(IEnumerable<string> symbols,
        Action<StreamDataEvent<BinanceStreamBookPrice>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceStreamBookPrice>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + bookTickerStreamEndpoint).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region All Book Tickers Stream
    public async Task<CallResult<UpdateSubscription>> SubscribeToAllBookTickerUpdatesAsync(
        Action<StreamDataEvent<BinanceStreamBookPrice>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceStreamBookPrice>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        return await SubscribeAsync(BaseAddress, new[] { allBookTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Partial Book Depth Streams
    public async Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(string symbol,
        int levels, int? updateInterval, Action<StreamDataEvent<IBinanceOrderBook>> onMessage, CancellationToken ct = default) =>
        await SubscribeToPartialOrderBookUpdatesAsync(new[] { symbol }, levels, updateInterval, onMessage, ct)
            .ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<CallResult<UpdateSubscription>> SubscribeToPartialOrderBookUpdatesAsync(
        IEnumerable<string> symbols, int levels, int? updateInterval, Action<StreamDataEvent<IBinanceOrderBook>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        levels.ValidateIntValues(nameof(levels), 5, 10, 20);
        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 1000);

        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceOrderBook>>>(data =>
        {
            data.Data.Data.Symbol = data.Data.Stream.Split('@')[0];
            onMessage(data.As<IBinanceOrderBook>(data.Data.Data, data.Data.Data.Symbol));
        });

        symbols = symbols.Select(a =>
            a.ToLower(CultureInfo.InvariantCulture) + partialBookDepthStreamEndpoint + levels +
            (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Diff. Depth Stream
    public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol,
        int? updateInterval, Action<StreamDataEvent<IBinanceEventOrderBook>> onMessage, CancellationToken ct = default) =>
        await SubscribeToOrderBookUpdatesAsync(new[] { symbol }, updateInterval, onMessage, ct).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(IEnumerable<string> symbols,
        int? updateInterval, Action<StreamDataEvent<IBinanceEventOrderBook>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        updateInterval?.ValidateIntValues(nameof(updateInterval), 100, 1000);
        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceEventOrderBook>>>(data => onMessage(data.As<IBinanceEventOrderBook>(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a =>
            a.ToLower(CultureInfo.InvariantCulture) + depthStreamEndpoint +
            (updateInterval.HasValue ? $"@{updateInterval.Value}ms" : "")).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Individual Symbol Ticker Streams
    public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, Action<StreamDataEvent<IBinanceTick>> onMessage, CancellationToken ct = default) => await SubscribeToTickerUpdatesAsync(new[] { symbol }, onMessage, ct).ConfigureAwait(false);

    /// <inheritdoc />
    public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(IEnumerable<string> symbols, Action<StreamDataEvent<IBinanceTick>> onMessage, CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));

        var handler = new Action<StreamDataEvent<BinanceCombinedStream<BinanceStreamTick>>>(data => onMessage(data.As<IBinanceTick>(data.Data.Data, data.Data.Data.Symbol)));
        symbols = symbols.Select(a => a.ToLower(CultureInfo.InvariantCulture) + symbolTickerStreamEndpoint).ToArray();
        return await SubscribeAsync(BaseAddress, symbols, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region All Market Tickers Streams
    public async Task<CallResult<UpdateSubscription>> SubscribeToAllTickerUpdatesAsync(Action<StreamDataEvent<IEnumerable<IBinanceTick>>> onMessage, CancellationToken ct = default)
    {
        var handler = new Action<StreamDataEvent<BinanceCombinedStream<IEnumerable<BinanceStreamTick>>>>(data => onMessage(data.As<IEnumerable<IBinanceTick>>(data.Data.Data, data.Data.Stream)));
        return await SubscribeAsync(BaseAddress, new[] { allSymbolTickerStreamEndpoint }, handler, ct).ConfigureAwait(false);
    }
    #endregion

}