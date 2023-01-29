namespace Binance.Api.Clients.RestApi.Spot;

public class BinanceRestApiSpotMarketDataClient
{
    // Api
    protected const string api = "api";
    protected const string publicVersion = "3";

    // Market Data
    private const string orderBookEndpoint = "depth";
    private const string recentTradesEndpoint = "trades";
    private const string historicalTradesEndpoint = "historicalTrades";
    private const string aggregatedTradesEndpoint = "aggTrades";
    private const string klinesEndpoint = "klines";
    private const string uiKlinesEndpoint = "uiKlines";
    private const string averagePriceEndpoint = "avgPrice";
    private const string price24HEndpoint = "ticker/24hr";
    private const string allPricesEndpoint = "ticker/price";
    private const string bookPricesEndpoint = "ticker/bookTicker";
    private const string rollingWindowPriceEndpoint = "ticker";

    // Internal References
    internal BinanceRestApiSpotClient MainClient { get; }
    internal Log Log { get => MainClient.Log; }
    internal BinanceRestApiClientOptions Options { get => MainClient.RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiSpotMarketDataClient(BinanceRestApiSpotClient main)
    {
        MainClient = main;
    }

    #region Order Book
    public async Task<RestCallResult<BinanceOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 5000);
        var parameters = new Dictionary<string, object> { { "symbol", symbol } };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        var requestWeight = limit == null ? 1 : limit <= 100 ? 1 : limit <= 500 ? 5 : limit <= 1000 ? 10 : 50;
        var result = await SendRequestInternal<BinanceOrderBook>(GetUrl(orderBookEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: requestWeight).ConfigureAwait(false);
        if (result)
            result.Data.Symbol = symbol;
        return result;
    }
    #endregion

    #region Recent Trades List
    public async Task<RestCallResult<IEnumerable<IBinanceRecentTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object> { { "symbol", symbol } };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        var result = await SendRequestInternal<IEnumerable<BinanceRecentTradeQuote>>(GetUrl(recentTradesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceRecentTrade>>(result.Data);
    }
    #endregion

    #region Old Trade Lookup
    public async Task<RestCallResult<IEnumerable<IBinanceRecentTrade>>> GetTradeHistoryAsync(string symbol, int? limit = null, long? fromId = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        var parameters = new Dictionary<string, object> { { "symbol", symbol } };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<IEnumerable<BinanceRecentTradeQuote>>(GetUrl(historicalTradesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: 5).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceRecentTrade>>(result.Data);
    }
    #endregion

    #region Compressed/Aggregate Trades List
    public async Task<RestCallResult<IEnumerable<BinanceAggregatedTrade>>> GetAggregatedTradeHistoryAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object> { { "symbol", symbol } };
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceAggregatedTrade>>(GetUrl(aggregatedTradesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
    }
    #endregion

    #region Kline/Candlestick Data
    public async Task<RestCallResult<IEnumerable<IBinanceKline>>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new Dictionary<string, object> {
                { "symbol", symbol },
                { "interval", JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false)) }
            };
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<IEnumerable<BinanceSpotKline>>(GetUrl(klinesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceKline>>(result.Data);
    }
    #endregion

    #region UI Kline Data
    public async Task<RestCallResult<IEnumerable<IBinanceKline>>> GetUiKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new Dictionary<string, object> {
                { "symbol", symbol },
                { "interval", JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false)) }
            };
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<IEnumerable<BinanceSpotKline>>(GetUrl(uiKlinesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceKline>>(result.Data);
    }
    #endregion

    #region Current Average Price
    public async Task<RestCallResult<BinanceAveragePrice>> GetCurrentAvgPriceAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new Dictionary<string, object> { { "symbol", symbol } };

        return await SendRequestInternal<BinanceAveragePrice>(GetUrl(averagePriceEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
    }
    #endregion

    #region 24hr Ticker Price Change Statistics
    public async Task<RestCallResult<IBinanceTick>> GetTickerAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new Dictionary<string, object> { { "symbol", symbol } };

        var result = await SendRequestInternal<Binance24HPrice>(GetUrl(price24HEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: 1).ConfigureAwait(false);
        return result.As<IBinanceTick>(result.Data);
    }

    public async Task<RestCallResult<IEnumerable<IBinanceTick>>> GetTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var parameters = new Dictionary<string, object> { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };
        var symbolCount = symbols.Count();
        var weight = symbolCount <= 20 ? 1 : symbolCount <= 100 ? 20 : 40;
        var result = await SendRequestInternal<IEnumerable<Binance24HPrice>>(GetUrl(price24HEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: weight).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceTick>>(result.Data);
    }

    public async Task<RestCallResult<IEnumerable<IBinanceTick>>> GetTickersAsync(CancellationToken ct = default)
    {
        var result = await SendRequestInternal<IEnumerable<Binance24HPrice>>(GetUrl(price24HEndpoint, api, publicVersion), HttpMethod.Get, ct, weight: 40).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceTick>>(result.Data);
    }
    #endregion

    #region Symbol Price Ticker
    public async Task<RestCallResult<BinancePrice>> GetPriceAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };

        return await SendRequestInternal<BinancePrice>(GetUrl(allPricesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinancePrice>>> GetPricesAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var parameters = new Dictionary<string, object> { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };
        return await SendRequestInternal<IEnumerable<BinancePrice>>(GetUrl(allPricesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: 2).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinancePrice>>> GetPricesAsync(CancellationToken ct = default)
    {
        return await SendRequestInternal<IEnumerable<BinancePrice>>(GetUrl(allPricesEndpoint, api, publicVersion), HttpMethod.Get, ct, weight: 2).ConfigureAwait(false);
    }
    #endregion

    #region Symbol Order Book Ticker
    public async Task<RestCallResult<BinanceBookPrice>> GetBookPriceAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new Dictionary<string, object> { { "symbol", symbol } };

        return await SendRequestInternal<BinanceBookPrice>(GetUrl(bookPricesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceBookPrice>>> GetBookPricesAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();
        var parameters = new Dictionary<string, object> { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };

        return await SendRequestInternal<IEnumerable<BinanceBookPrice>>(GetUrl(bookPricesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: 2).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceBookPrice>>> GetBookPricesAsync(CancellationToken ct = default)
    {
        return await SendRequestInternal<IEnumerable<BinanceBookPrice>>(GetUrl(bookPricesEndpoint, api, publicVersion), HttpMethod.Get, ct, weight: 2).ConfigureAwait(false);
    }
    #endregion

    #region Rolling Window Price Change Ticker
    public async Task<RestCallResult<IBinance24HPrice>> GetRollingWindowTickerAsync(string symbol, TimeSpan? windowSize = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new Dictionary<string, object> { { "symbol", symbol } };
        parameters.AddOptionalParameter("windowSize", windowSize == null ? null : GetWindowSize(windowSize.Value));

        var result = await SendRequestInternal<Binance24HPrice>(GetUrl(rollingWindowPriceEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: 2).ConfigureAwait(false);
        return result.As<IBinance24HPrice>(result.Data);
    }

    public async Task<RestCallResult<IEnumerable<IBinance24HPrice>>> GetRollingWindowTickersAsync(IEnumerable<string> symbols, TimeSpan? windowSize = null, CancellationToken ct = default)
    {
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var parameters = new Dictionary<string, object> { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };
        parameters.AddOptionalParameter("windowSize", windowSize == null ? null : GetWindowSize(windowSize.Value));
        var symbolCount = symbols.Count();
        var weight = Math.Min(symbolCount * 2, 100);
        var result = await SendRequestInternal<IEnumerable<Binance24HPrice>>(GetUrl(rollingWindowPriceEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: weight).ConfigureAwait(false);
        return result.As<IEnumerable<IBinance24HPrice>>(result.Data);
    }

    private string GetWindowSize(TimeSpan timeSpan)
    {
        if (timeSpan.TotalHours < 1)
            return timeSpan.TotalMinutes + "m";
        else if (timeSpan.TotalHours < 24)
            return timeSpan.TotalHours + "h";
        return timeSpan.TotalDays + "d";
    }
    #endregion

}