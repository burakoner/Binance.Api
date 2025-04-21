namespace Binance.Api.Spot;

/// <summary>
/// Binance Spot Rest API Client
/// </summary>
/// <param name="root">Parent</param>
public class BinanceSpotRestApiClient(BinanceRestApiClient root)
{
    // Api
    private const string v1 = "1";
    private const string v3 = "3";
    private const string api = "api";
    private const string sapi = "sapi";

    // Parent
    internal BinanceRestApiClient _ { get; } = root;

    // Internal
    internal ILogger Logger => Logger;
    internal BinanceRestApiClientOptions Options => Options;
    internal DateTime? LastExchangeInfoUpdate { get; private set; }
    internal BinanceExchangeInfo? ExchangeInfo { get; private set; }

    internal void InvokeOrderPlaced(long id) => OnOrderPlaced?.Invoke(id);
    internal void InvokeOrderCanceled(long id) => OnOrderCanceled?.Invoke(id);

    /// <summary>
    /// Event triggered when an order is placed via this client. Only available for Spot orders
    /// </summary>
    public event Action<long>? OnOrderPlaced;

    /// <summary>
    /// Event triggered when an order is canceled via this client. 
    /// Note that this does not trigger when using CancelAllOrdersAsync. Only available for Spot orders
    /// </summary>
    public event Action<long>? OnOrderCanceled;

    #region Internal Methods
    internal Task<RestCallResult<T>> RequestAsync<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object>? queryParameters = null,
        Dictionary<string, object>? bodyParameters = null,
        Dictionary<string, string>? headerParameters = null,
        ArraySerialization? serialization = null,
        JsonSerializer? deserializer = null,
        bool ignoreRatelimit = false,
        int requestWeight = 1) where T : class
        => _.RequestAsync<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal Uri GetUrl(string api, string version, string endpoint)
    {
        var url = Options.BaseAddress.AppendPath(api);
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }

    internal async Task<BinanceTradeRuleResult> CheckTradeRules(string symbol, decimal? quantity, decimal? quoteQuantity, decimal? price, decimal? stopPrice, BinanceSpotOrderType? type, CancellationToken ct)
    {
        if (Options.SpotOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.None)
            return BinanceTradeRuleResult.CreatePassed(quantity, quoteQuantity, price, stopPrice);

        if (ExchangeInfo == null || LastExchangeInfoUpdate == null || (DateTime.UtcNow - LastExchangeInfoUpdate.Value).TotalMinutes > Options.SpotOptions.TradeRulesUpdateInterval.TotalMinutes)
            await GetExchangeInfoAsync(ct).ConfigureAwait(false);

        if (ExchangeInfo == null)
            return BinanceTradeRuleResult.CreateFailed("Unable to retrieve trading rules, validation failed");

        return BinanceHelpers.ValidateTradeRules(Logger, Options.SpotOptions.TradeRulesBehavior, ExchangeInfo, symbol, quantity, quoteQuantity, price, stopPrice, type);
    }
    #endregion

    #region General Methods
    /// <summary>
    /// Pings the Binance API
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#test-connectivity" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if successful ping, false if no response</returns>
    public async Task<RestCallResult<TimeSpan>> PingAsync(CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        var result = await RequestAsync<object>(GetUrl(api, v3, "ping"), HttpMethod.Get, ct).ConfigureAwait(false);
        sw.Stop();

        return result.Success
            ? result.As(sw.Elapsed)
            : result.AsError<TimeSpan>(result.Error!);
    }

    /// <summary>
    /// Requests the server for the local time
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#check-server-time" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Server time</returns>
    public async Task<RestCallResult<DateTime>> GetTimeAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceServerTime>(GetUrl(api, v3, "time"), HttpMethod.Get, ct, ignoreRatelimit: true).ConfigureAwait(false);

        return result.Success
            ? result.As(result.Data?.ServerTime ?? default)
            : result.AsError<DateTime>(result.Error!);
    }

    /// <summary>
    /// Gets information about the exchange including rate limits and symbol list
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#exchange-information" /></para>
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Exchange Info</returns>
    public Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [], ct: ct);

    /// <summary>
    /// Gets information about the exchange including rate limits and symbol list
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#exchange-information" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to get data for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Exchange Info</returns>
    public Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(string symbol, CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [symbol], ct: ct);

    /// <summary>
    /// Gets information about the exchange including rate limits and symbol list
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#exchange-information" /></para>
    /// </summary>
    /// <param name="status">Filter by symbol status, Trading, Halt or Break</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Exchange Info</returns>
    public Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(BinanceSymbolStatus status, CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [], status: status, ct: ct);

    /// <summary>
    /// Gets information about the exchange including rate limits and symbol list
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#exchange-information" /></para>
    /// </summary>
    /// <param name="permission">Permission Type</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Exchange Info</returns>
    public Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(BinancePermissionType permission, CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [], status: null, permissions: [permission], ct: ct);

    /// <summary>
    /// Gets information about the exchange including rate limits and symbol list
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#exchange-information" /></para>
    /// </summary>
    /// <param name="symbols">Symbols to get data for, for example `ETHUSDT`</param>
    /// <param name="status">Filter by symbol status, Trading, Halt or Break</param>
    /// <param name="permissions">Permission Types</param>
    /// <param name="showPermissionSets">Whether or not permission sets should be returned</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Exchange Info</returns>
    public async Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(
        IEnumerable<string> symbols,
        BinanceSymbolStatus? status = null,
        IEnumerable<BinancePermissionType>? permissions = null,
        bool? showPermissionSets = null,
        CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();

        // Symbol(s)
        if (symbols.Count() > 1)
        {
            parameters.Add("symbols", JsonConvert.SerializeObject(symbols));
        }
        else if (symbols.Any())
        {
            parameters.Add("symbol", symbols.First());
        }

        // Permissions
        if (permissions != null && permissions?.Count() > 1)
        {
            var list = new List<string>();
            foreach (var permission in permissions)
            {
                list.Add(permission.ToString().ToUpper());
            }

            parameters.Add("permissions", JsonConvert.SerializeObject(list));
        }
        else if (permissions != null && permissions.Any())
        {
            parameters.Add("permissions", permissions.First().ToString().ToUpper());
        }

        // Permission Sets
        parameters.AddOptional("showPermissionSets", showPermissionSets?.ToString().ToLowerInvariant());
        parameters.AddOptionalEnum("symbolStatus", status);

        var exchangeInfoResult = await RequestAsync<BinanceExchangeInfo>(GetUrl(api, v3, "exchangeInfo"), HttpMethod.Get, ct, queryParameters: parameters, serialization: ArraySerialization.Array, requestWeight: 20).ConfigureAwait(false);
        if (!exchangeInfoResult)
            return exchangeInfoResult;

        ExchangeInfo = exchangeInfoResult.Data;
        LastExchangeInfoUpdate = DateTime.UtcNow;
        Logger.Log(LogLevel.Information, "Trade rules updated");
        return exchangeInfoResult;
    }
    #endregion

    #region Market Data Methods
    /// <summary>
    /// Gets the order book for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#order-book" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the order book for, for example `ETHUSDT`</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The order book for the symbol</returns>
    public async Task<RestCallResult<BinanceOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 5000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalString("limit", limit);

        var requestWeight = limit == null ? 1 : limit <= 100 ? 5 : limit <= 500 ? 25 : limit <= 1000 ? 50 : 250;
        var result = await RequestAsync<BinanceOrderBook>(GetUrl(api, v3, "depth"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
        if (!result) return result;

        result.Data.Symbol = symbol;
        return result;
    }

    /// <summary>
    /// Gets the recent trades for a symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#recent-trades-list" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get recent trades for, for example `ETHUSDT`</param>
    /// <param name="limit">Result limit</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of recent trades</returns>
    public Task<RestCallResult<IEnumerable<BinanceSpotTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalString("limit", limit);

        return RequestAsync<IEnumerable<BinanceSpotTrade>>(GetUrl(api, v3, "trades"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 25);
    }

    /// <summary>
    /// Gets the historical trades for a symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#old-trade-lookup" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get recent trades for, for example `ETHUSDT`</param>
    /// <param name="limit">Result limit</param>
    /// <param name="fromId">From which trade id on results should be retrieved</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of recent trades</returns>
    public Task<RestCallResult<IEnumerable<BinanceSpotTrade>>> GetTradeHistoryAsync(string symbol, int? limit = null, long? fromId = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalString("limit", limit);
        parameters.AddOptionalString("fromId", fromId);

        return RequestAsync<IEnumerable<BinanceSpotTrade>>(GetUrl(api, v3, "historicalTrades"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 25);
    }

    /// <summary>
    /// Gets compressed, aggregate trades. Trades that fill at the same time, from the same order, with the same price will have the quantity aggregated.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#compressedaggregate-trades-list" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the trades for, for example `ETHUSDT`</param>
    /// <param name="fromId">ID to get aggregate trades from INCLUSIVE.</param>
    /// <param name="startTime">Time to start getting trades from</param>
    /// <param name="endTime">Time to stop getting trades from</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The aggregated trades list for the symbol</returns>
    public Task<RestCallResult<IEnumerable<BinanceAggregatedTrade>>> GetAggregatedTradeHistoryAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<IEnumerable<BinanceAggregatedTrade>>(GetUrl(api, v3, "aggTrades"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 4);
    }

    /// <summary>
    /// Get candlestick data for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#klinecandlestick-data" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="interval">The candlestick timespan</param>
    /// <param name="startTime">Start time to get candlestick data</param>
    /// <param name="endTime">End time to get candlestick data</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The candlestick data for the provided symbol</returns>
    public Task<RestCallResult<IEnumerable<BinanceSpotKline>>> GetKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddEnum("interval", interval);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<IEnumerable<BinanceSpotKline>>(GetUrl(api, v3, "klines"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    /// <summary>
    /// Get candlestick data for the provided symbol. Returns modified kline data, optimized for the presentation of candlestick charts
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#uiklines" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="interval">The candlestick timespan</param>
    /// <param name="startTime">Start time to get candlestick data</param>
    /// <param name="endTime">End time to get candlestick data</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The candlestick data for the provided symbol</returns>
    public Task<RestCallResult<IEnumerable<BinanceSpotKline>>> GetUiKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddEnum("interval", interval);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<IEnumerable<BinanceSpotKline>>(GetUrl(api, v3, "uiKlines"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    /// <summary>
    /// Gets current average price for a symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#current-average-price" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public async Task<RestCallResult<BinanceAveragePrice>> GetAveragePriceAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };

        return await RequestAsync<BinanceAveragePrice>(GetUrl(api, v3, "avgPrice"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2).ConfigureAwait(false);
    }

    /// <summary>
    /// Get data regarding the last 24 hours for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Data over the last 24 hours</returns>
    public Task<RestCallResult<BinanceFullTicker>> GetFullTickerAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "type", "FULL" }
        };

        return RequestAsync<BinanceFullTicker>(GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    /// <summary>
    /// Get data regarding the last 24 hours for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to get the data for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Data over the last 24 hours</returns>
    public Task<RestCallResult<IEnumerable<BinanceFullTicker>>> GetFullTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" },
            { "type", "FULL" }
        };

        var symbolCount = symbols.Count();
        var weight = symbolCount <= 20 ? 2 : symbolCount <= 100 ? 40 : 80;
        return RequestAsync<IEnumerable<BinanceFullTicker>>(GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }

    /// <summary>
    /// Get data regarding the last 24 hours for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Data over the last 24 hours</returns>
    public Task<RestCallResult<IEnumerable<BinanceFullTicker>>> GetFullTickersAsync(CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "FULL" }
        };

        return RequestAsync<IEnumerable<BinanceFullTicker>>(GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 80);
    }

    /// <summary>
    /// Get data regarding the last 24 hours for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Data over the last 24 hours</returns>
    public Task<RestCallResult<BinanceMiniTicker>> GetMiniTickerAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "type", "MINI" }
        };

        return RequestAsync<BinanceMiniTicker>(GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    /// <summary>
    /// Get data regarding the last 24 hours for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to get the data for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Data over the last 24 hours</returns>
    public Task<RestCallResult<IEnumerable<BinanceMiniTicker>>> GetMiniTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" },
            { "type", "MINI" }
        };

        var symbolCount = symbols.Count();
        var weight = symbolCount <= 20 ? 2 : symbolCount <= 100 ? 40 : 80;
        return RequestAsync<IEnumerable<BinanceMiniTicker>>(GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }

    /// <summary>
    /// Get data regarding the last 24 hours for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#24hr-ticker-price-change-statistics" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Data over the last 24 hours</returns>
    public Task<RestCallResult<IEnumerable<BinanceMiniTicker>>> GetMiniTickersAsync(CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "MINI" }
        };

        return RequestAsync<IEnumerable<BinanceMiniTicker>>(GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 80);
    }

    /// <summary>
    /// Get price change stats for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="timeZone">The timezone offset, for example -3 for UTC-3 or 5 for UTC+5</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public Task<RestCallResult<BinanceTradingDayFullTicker>> GetTradingDayFullTickerAsync(string symbol, string? timeZone = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "type", "FULL" }
        };
        parameters.AddOptional("timeZone", timeZone);

        return RequestAsync<BinanceTradingDayFullTicker>(GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 4);
    }

    /// <summary>
    /// Get price change stats for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="timeZone">The timezone offset, for example -3 for UTC-3 or 5 for UTC+5</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public Task<RestCallResult<IEnumerable<BinanceTradingDayFullTicker>>> GetTradingDayFullTickersAsync(IEnumerable<string> symbols, string? timeZone = null, CancellationToken ct = default)
    {
        if (symbols.Count() > 100) throw new ArgumentException("The maximum number of symbols is 100", nameof(symbols));
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" },
            { "type", "FULL" }
        };
        parameters.AddOptional("timeZone", timeZone);

        var symbolCount = symbols.Count();
        var weight = Math.Min(symbolCount * 4, 200);
        return RequestAsync<IEnumerable<BinanceTradingDayFullTicker>>(GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }

    /// <summary>
    /// Get price change stats for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="timeZone">The timezone offset, for example -3 for UTC-3 or 5 for UTC+5</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public Task<RestCallResult<IEnumerable<BinanceTradingDayFullTicker>>> GetTradingDayFullTickersAsync(string? timeZone = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "FULL" }
        };
        parameters.AddOptional("timeZone", timeZone);

        return RequestAsync<IEnumerable<BinanceTradingDayFullTicker>>(GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 80);
    }

    /// <summary>
    /// Get price change stats for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="timeZone">The timezone offset, for example -3 for UTC-3 or 5 for UTC+5</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public Task<RestCallResult<BinanceTradingDayMiniTicker>> GetTradingDayMiniTickerAsync(string symbol, string? timeZone = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "type", "MINI" }
        };
        parameters.AddOptional("timeZone", timeZone);

        return RequestAsync<BinanceTradingDayMiniTicker>(GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 4);
    }

    /// <summary>
    /// Get price change stats for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="symbols">The symbols, for example `ETHUSDT`</param>
    /// <param name="timeZone">The timezone offset, for example -3 for UTC-3 or 5 for UTC+5</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public Task<RestCallResult<IEnumerable<BinanceTradingDayMiniTicker>>> GetTradingDayMiniTickersAsync(IEnumerable<string> symbols, string? timeZone = null, CancellationToken ct = default)
    {
        if (symbols.Count() > 100) throw new ArgumentException("The maximum number of symbols is 100", nameof(symbols));
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" },
            { "type", "MINI" }
        };
        parameters.AddOptional("timeZone", timeZone);

        var symbolCount = symbols.Count();
        var weight = Math.Min(symbolCount * 4, 200);
        return RequestAsync<IEnumerable<BinanceTradingDayMiniTicker>>(GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }

    /// <summary>
    /// Get price change stats for a trading day
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#trading-day-ticker" /></para>
    /// </summary>
    /// <param name="timeZone">The timezone offset, for example -3 for UTC-3 or 5 for UTC+5</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public Task<RestCallResult<IEnumerable<BinanceTradingDayMiniTicker>>> GetTradingDayMiniTickersAsync(string? timeZone = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "MINI" }
        };
        parameters.AddOptional("timeZone", timeZone);

        return RequestAsync<IEnumerable<BinanceTradingDayMiniTicker>>(GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 80);
    }

    /// <summary>
    /// Gets the price of a symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#symbol-price-ticker" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the price for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Price of symbol</returns>
    public Task<RestCallResult<BinancePriceTicker>> GetPriceTickerAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };

        return RequestAsync<BinancePriceTicker>(GetUrl(api, v3, "ticker/price"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    /// <summary>
    ///  Gets the prices of symbols
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#symbol-price-ticker" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to get the price for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of prices</returns>
    public Task<RestCallResult<IEnumerable<BinancePriceTicker>>> GetPriceTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };
        return RequestAsync<IEnumerable<BinancePriceTicker>>(GetUrl(api, v3, "ticker/price"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 4);
    }

    /// <summary>
    ///  Gets the prices of symbols
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#symbol-price-ticker" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of prices</returns>
    public Task<RestCallResult<IEnumerable<BinancePriceTicker>>> GetPriceTickersAsync(CancellationToken ct = default)
    {
        return RequestAsync<IEnumerable<BinancePriceTicker>>(GetUrl(api, v3, "ticker/price"), HttpMethod.Get, ct, requestWeight: 4);
    }

    /// <summary>
    /// Gets the best price/quantity on the order book for a symbol.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#symbol-order-book-ticker" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to get book price for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of book prices</returns>
    public Task<RestCallResult<BinanceBookTicker>> GetBookTickerAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };

        return RequestAsync<BinanceBookTicker>(GetUrl(api, v3, "ticker/bookTicker"), HttpMethod.Get, ct, false, queryParameters: parameters);
    }

    /// <summary>
    /// Gets the best price/quantity on the order book for a symbol.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#symbol-order-book-ticker" /></para>
    /// </summary>
    /// <param name="symbols">Symbols to get book price for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of book prices</returns>
    public Task<RestCallResult<IEnumerable<BinanceBookTicker>>> GetBookTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };

        return RequestAsync<IEnumerable<BinanceBookTicker>>(GetUrl(api, v3, "ticker/bookTicker"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    /// <summary>
    /// Gets the best price/quantity on the order book for a symbol.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#symbol-order-book-ticker" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of book prices</returns>
    public Task<RestCallResult<IEnumerable<BinanceBookTicker>>> GetBookTickersAsync(CancellationToken ct = default)
    {
        return RequestAsync<IEnumerable<BinanceBookTicker>>(GetUrl(api, v3, "ticker/bookTicker"), HttpMethod.Get, ct, requestWeight: 2);
    }

    /// <summary>
    /// Get data based on the last x time, specified as windowSize
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#rolling-window-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbol">The symbols to get data for, for example `ETHUSDT`</param>
    /// <param name="windowSize">The window size to use</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public Task<RestCallResult<BinanceFullTicker>> GetRollingWindowTickerAsync(string symbol, TimeSpan? windowSize = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("windowSize", windowSize == null ? null : GetWindowSize(windowSize.Value));

        return RequestAsync<BinanceFullTicker>(GetUrl(api, v3, "ticker"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    /// <summary>
    /// Get data based on the last x time, specified as windowSize
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints#rolling-window-price-change-statistics" /></para>
    /// </summary>
    /// <param name="symbols">The symbols to get data for, for example `ETHUSDT`</param>
    /// <param name="windowSize">The window size to use</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public Task<RestCallResult<IEnumerable<BinanceFullTicker>>> GetRollingWindowTickersAsync(IEnumerable<string> symbols, TimeSpan? windowSize = null, CancellationToken ct = default)
    {
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };
        parameters.AddOptional("windowSize", windowSize == null ? null : GetWindowSize(windowSize.Value));
        var symbolCount = symbols.Count();
        var weight = Math.Min(symbolCount * 4, 200);
        return RequestAsync<IEnumerable<BinanceFullTicker>>(GetUrl(api, v3, "ticker"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }

    private string GetWindowSize(TimeSpan timeSpan)
    {
        if (timeSpan.TotalHours < 1) return timeSpan.TotalMinutes + "m";
        else if (timeSpan.TotalHours < 24) return timeSpan.TotalHours + "h";
        return timeSpan.TotalDays + "d";
    }
    #endregion

    #region Trading Methods
    public async Task<RestCallResult<BinanceSpotOrder>> PlaceOrderAsync(
        string symbol,
        BinanceSpotOrderSide side,
        BinanceSpotOrderType type,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        decimal? price = null,
        decimal? stopPrice = null,
        decimal? icebergQuantity = null,
        string? newClientOrderId = null,
        BinanceSpotTimeInForce? timeInForce = null,
        BinanceSpotOrderResponseType? orderResponseType = null,
        BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
        long? trailingDelta = null,
        long? strategyId = null,
        int? strategyType = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {

        if (quoteQuantity != null && type != BinanceSpotOrderType.Market)
            throw new ArgumentException("quoteQuantity is only valid for market orders");

        if (quantity == null && quoteQuantity == null || quantity != null && quoteQuantity != null)
            throw new ArgumentException("1 of either should be specified, quantity or quoteOrderQuantity");

        var rulesCheck = await CheckTradeRules(symbol, quantity, quoteQuantity, price, stopPrice, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceSpotOrder>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;
        quoteQuantity = rulesCheck.QuoteQuantity;
        var clientOrderId = BinanceHelpers.ApplyBrokerId(newClientOrderId, BinanceConstants.ClientOrderIdSpot, 36, Options.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
        };
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddOptional("quantity", quantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("quoteOrderQty", quoteQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("price", price?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("stopPrice", stopPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("icebergQty", icebergQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("newClientOrderId", clientOrderId);
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptional("trailingDelta", trailingDelta);
        parameters.AddOptional("strategyId", strategyId);
        parameters.AddOptional("strategyType", strategyType);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceSpotOrder>(GetUrl(api, v3, "order"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        if (result) InvokeOrderPlaced(result.Data.Id);

        return result;
    }

    public async Task<RestCallResult<BinanceSpotOrderTest>> PlaceTestOrderAsync(
        string symbol,
        BinanceSpotOrderSide side,
        BinanceSpotOrderType type,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        decimal? price = null,
        decimal? stopPrice = null,
        decimal? icebergQuantity = null,
        string? newClientOrderId = null,
        BinanceSpotTimeInForce? timeInForce = null,
        BinanceSpotOrderResponseType? orderResponseType = null,
        BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
        long? trailingDelta = null,
        long? strategyId = null,
        int? strategyType = null,
        int? receiveWindow = null,
        bool? computeFeeRates = null,
        CancellationToken ct = default)
    {
        if (quoteQuantity != null && type != BinanceSpotOrderType.Market)
            throw new ArgumentException("quoteQuantity is only valid for market orders");

        if (quantity == null && quoteQuantity == null || quantity != null && quoteQuantity != null)
            throw new ArgumentException("1 of either should be specified, quantity or quoteOrderQuantity");

        var rulesCheck = await CheckTradeRules(symbol, quantity, quoteQuantity, price, stopPrice, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceSpotOrderTest>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;
        quoteQuantity = rulesCheck.QuoteQuantity;

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
        };
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddOptional("quantity", quantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("quoteOrderQty", quoteQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("newClientOrderId", newClientOrderId);
        parameters.AddOptional("price", price?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptional("stopPrice", stopPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("icebergQty", icebergQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptional("trailingDelta", trailingDelta);
        parameters.AddOptional("strategyId", strategyId);
        parameters.AddOptional("strategyType", strategyType);
        parameters.AddOptional("computeCommissionRates", computeFeeRates?.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var weight = computeFeeRates == true ? 20 : 1;
        return await RequestAsync<BinanceSpotOrderTest>(GetUrl(api, v3, "order/test"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: weight).ConfigureAwait(false);
    }

    public Task<RestCallResult<BinanceSpotOrder>> GetOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalString("orderId", orderId);
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSpotOrder>(GetUrl(api, v3, "order"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 4);
    }

    public async Task<RestCallResult<BinanceSpotOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, string? newClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        if (!orderId.HasValue && string.IsNullOrEmpty(origClientOrderId))
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptionalString("orderId", orderId);
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("newClientOrderId", newClientOrderId);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceSpotOrder>(GetUrl(api, v3, "order"), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
        if (result) InvokeOrderCanceled(result.Data.Id);
        return result;
    }

    public async Task<RestCallResult<IEnumerable<BinanceSpotOrder>>> CancelOrdersAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<IEnumerable<BinanceSpotOrder>>(GetUrl(api, v3, "openOrders"), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
        if (result) foreach (var order in result.Data) InvokeOrderCanceled(order.Id);
        return result;
    }

    public async Task<RestCallResult<BinanceReplaceOrderResult>> ReplaceOrderAsync(
        string symbol,
        BinanceSpotOrderSide side,
        BinanceSpotOrderType type,
        BinanceSpotOrderCancelReplaceMode mode,
        long? cancelOrderId = null,
        string? cancelClientOrderId = null,
        string? newClientOrderId = null,
        string? newCancelClientOrderId = null,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        decimal? price = null,
        decimal? stopPrice = null,
        decimal? icebergQuantity = null,
        BinanceSpotTimeInForce? timeInForce = null,
        BinanceSpotOrderResponseType? orderResponseType = null,
        BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
        BinanceSpotOrderCancelRestriction? cancelRestriction = null,
        long? trailingDelta = null,
        long? strategyId = null,
        int? strategyType = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (cancelOrderId == null && cancelClientOrderId == null || cancelOrderId != null && cancelClientOrderId != null)
            throw new ArgumentException("1 of either should be specified, cancelOrderId or cancelClientOrderId");

        if (quoteQuantity != null && type != BinanceSpotOrderType.Market)
            throw new ArgumentException("quoteQuantity is only valid for market orders");

        if (quantity == null && quoteQuantity == null || quantity != null && quoteQuantity != null)
            throw new ArgumentException("1 of either should be specified, quantity or quoteOrderQuantity");

        var rulesCheck = await CheckTradeRules(symbol, quantity, quoteQuantity, price, stopPrice, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceReplaceOrderResult>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;
        quoteQuantity = rulesCheck.QuoteQuantity;

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
        };
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddEnum("cancelReplaceMode", mode);
        parameters.AddOptional("cancelOrderId", cancelOrderId);
        parameters.AddOptional("cancelOrigClientOrderId", cancelClientOrderId);
        parameters.AddOptional("newClientOrderId", newClientOrderId);
        parameters.AddOptional("cancelNewClientOrderId", newCancelClientOrderId);
        parameters.AddOptional("quantity", quantity);
        parameters.AddOptional("quoteOrderQty", quoteQuantity);
        parameters.AddOptional("price", price);
        parameters.AddOptional("stopPrice", stopPrice);
        parameters.AddOptional("icebergQty", icebergQuantity);
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptionalEnum("cancelRestrictions", cancelRestriction);
        parameters.AddOptional("trailingDelta", trailingDelta);
        parameters.AddOptional("strategyId", strategyId);
        parameters.AddOptional("strategyType", strategyType);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceReplaceOrderResult>(GetUrl(api, v3, "order/cancelReplace"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        if (!result && result.Raw != null)
        {
            // Attempt to parse the error
            var jsonData = result.Raw.ToJToken(Logger);
            if (jsonData != null)
            {
                var dataNode = jsonData["data"];
                if (dataNode == null)
                    return result;

                var error = dataNode?["cancelResult"]?.ToString() == "FAILURE" ? dataNode!["cancelResponse"] : jsonData["data"]!["newOrderResponse"];
                if (error != null && error.HasValues)
                    return result.AsError<BinanceReplaceOrderResult>(new ServerError(error!.Value<int>("code"), error.Value<string>("msg")!));
            }
        }

        if (result && result.Data.NewOrderResult == OrderOperationResult.Success)
            InvokeOrderPlaced(result.Data.NewOrderResponse!.Id);

        return result;
    }

    // TODO: Order Amend Keep Priority (TRADE)

    public async Task<RestCallResult<IEnumerable<BinanceSpotOrder>>> GetOpenOrdersAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return await RequestAsync<IEnumerable<BinanceSpotOrder>>(GetUrl(api, v3, "openOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: symbol == null ? 40 : 3).ConfigureAwait(false);
    }

    public Task<RestCallResult<IEnumerable<BinanceSpotOrder>>> GetOrdersAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("startTime", startTime?.ConvertToMilliseconds());
        parameters.AddOptional("endTime", endTime?.ConvertToMilliseconds());
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceSpotOrder>>(GetUrl(api, v3, "allOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    // TODO: New order using SOR (TRADE)

    #endregion

    #region Account Methods
    public Task<RestCallResult<BinanceSpotAccount>> GetAccountAsync(bool? omitZeroBalances = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("omitZeroBalances", omitZeroBalances?.ToString().ToLowerInvariant());
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSpotAccount>(GetUrl(api, v3, "account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<IEnumerable<BinanceSpotUserTrade>>> GetUserTradesAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var weight = orderId.HasValue ? 5 : 20;
        return RequestAsync<IEnumerable<BinanceSpotUserTrade>>(GetUrl(api, v3, "myTrades"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<IEnumerable<BinanceOrderRateLimit>>> GetOrderRateLimitStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceOrderRateLimit>>(GetUrl(api, v3, "rateLimit/order"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<IEnumerable<BinancePreventedTrade>>> GetPreventedTradesAsync(string symbol, long? orderId = null, long? preventedMatchId = null, long? fromPreventedMatchId = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("preventedMatchId", preventedMatchId);
        parameters.AddOptional("fromPreventedMatchId", fromPreventedMatchId);
        parameters.AddOptional("size", limit);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var weight = preventedMatchId.HasValue ? 2 : 20;
        if (orderId.HasValue) weight = 20;
        return RequestAsync<IEnumerable<BinancePreventedTrade>>(GetUrl(api, v3, "myPreventedMatches"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: weight);
    }

    // TODO: Query Allocations (USER_DATA)
    // TODO: Query Commission Rates (USER_DATA)
    // TODO: Query Order Amendments (USER_DATA)

    #endregion

    #region User Data Stream
    [Obsolete("[!IMPORTANT] " +
        "These requests have been deprecated, which means we will remove them in the future. Please subscribe to the User Data Stream through the WebSocket API instead. " +
        "https://developers.binance.com/docs/binance-spot-api-docs/rest-api/user-data-stream-endpoints-deprecated")]
    public async Task<RestCallResult<string>> StartUserStreamAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceListenKey>(GetUrl(api, v3, "userDataStream"), HttpMethod.Post, ct, true, requestWeight: 2);
        return result.As(result.Data?.ListenKey!);
    }

    [Obsolete("[!IMPORTANT] " +
        "These requests have been deprecated, which means we will remove them in the future. Please subscribe to the User Data Stream through the WebSocket API instead. " +
        "https://developers.binance.com/docs/binance-spot-api-docs/rest-api/user-data-stream-endpoints-deprecated")]
    public async Task<RestCallResult<bool>> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new ParameterCollection
        {
            { "listenKey", listenKey }
        };

        var result = await RequestAsync<object>(GetUrl(api, v3, "userDataStream"), HttpMethod.Put, ct, true, bodyParameters: parameters, requestWeight: 2);
        return result.As(result.Success);
    }

    [Obsolete("[!IMPORTANT] " +
        "These requests have been deprecated, which means we will remove them in the future. Please subscribe to the User Data Stream through the WebSocket API instead. " +
        "https://developers.binance.com/docs/binance-spot-api-docs/rest-api/user-data-stream-endpoints-deprecated")]
    public async Task<RestCallResult<bool>> StopUserStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new ParameterCollection
        {
            { "listenKey", listenKey }
        };

        var result = await RequestAsync<object>(GetUrl(api, v3, "userDataStream"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 2);
        return result.As(result.Success);
    }

    #endregion

}