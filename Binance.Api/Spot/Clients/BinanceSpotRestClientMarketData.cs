namespace Binance.Api.Spot;

internal partial class BinanceSpotRestClient
{
    public async Task<RestCallResult<BinanceSpotOrderBook>> GetOrderBookAsync(
        string symbol,
        int? limit = null,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 5000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalString("limit", limit);
        parameters.AddOptionalEnum("symbolStatus", status);

        var requestWeight = BinanceSpotMarketDataValidation.DepthWeight(limit);
        var result = await RequestAsync<BinanceSpotOrderBook>(GetUrl(api, v3, "depth"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
        if (!result) return result;

        result.Data.Symbol = symbol;
        return result;
    }

    public Task<RestCallResult<List<BinanceSpotTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalString("limit", limit);

        return RequestAsync<List<BinanceSpotTrade>>(GetUrl(api, v3, "trades"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 25);
    }

    public Task<RestCallResult<List<BinanceSpotTrade>>> GetHistoricalTradesAsync(string symbol, int? limit = null, long? fromId = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalString("limit", limit);
        parameters.AddOptionalString("fromId", fromId);

        return RequestAsync<List<BinanceSpotTrade>>(GetUrl(api, v3, "historicalTrades"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 25);
    }

    public Task<RestCallResult<List<BinanceSpotBlockTrade>>> GetHistoricalBlockTradesAsync(
        string symbol,
        long fromId,
        int? limit = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        if (fromId < 0)
            throw new ArgumentOutOfRangeException(nameof(fromId), "fromId cannot be negative.");
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "fromId", fromId }
        };
        parameters.AddOptional("limit", limit);

        return RequestAsync<List<BinanceSpotBlockTrade>>(
            GetUrl(api, v3, "historicalBlockTrades"),
            HttpMethod.Get,
            ct,
            queryParameters: parameters,
            requestWeight: 25);
    }

    public Task<RestCallResult<List<BinanceSpotAggregatedTrade>>> GetAggregatedTradesAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        BinanceSpotMarketDataValidation.TimeRange(startTime, endTime);
        if (fromId.HasValue && (startTime.HasValue || endTime.HasValue))
            throw new ArgumentException("fromId cannot be combined with startTime or endTime.", nameof(fromId));

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<List<BinanceSpotAggregatedTrade>>(GetUrl(api, v3, "aggTrades"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 4);
    }

    public Task<RestCallResult<List<BinanceSpotKline>>> GetKlinesAsync(
        string symbol,
        BinanceKlineInterval interval,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? timeZone = null,
        int? limit = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        BinanceSpotMarketDataValidation.TimeRange(startTime, endTime);
        timeZone = BinanceSpotMarketDataValidation.TimeZone(timeZone);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddEnum("interval", interval);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("timeZone", timeZone);

        return RequestAsync<List<BinanceSpotKline>>(GetUrl(api, v3, "klines"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    public Task<RestCallResult<List<BinanceSpotKline>>> GetUIKlinesAsync(
        string symbol,
        BinanceKlineInterval interval,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? timeZone = null,
        int? limit = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        BinanceSpotMarketDataValidation.TimeRange(startTime, endTime);
        timeZone = BinanceSpotMarketDataValidation.TimeZone(timeZone);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddEnum("interval", interval);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("timeZone", timeZone);

        return RequestAsync<List<BinanceSpotKline>>(GetUrl(api, v3, "uiKlines"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    public async Task<RestCallResult<BinanceSpotAveragePrice>> GetAveragePriceAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };

        return await RequestAsync<BinanceSpotAveragePrice>(GetUrl(api, v3, "avgPrice"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2).ConfigureAwait(false);
    }

    public Task<RestCallResult<BinanceSpotReferencePrice>> GetReferencePriceAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };
        return RequestAsync<BinanceSpotReferencePrice>(
            GetUrl(api, v3, "referencePrice"),
            HttpMethod.Get,
            ct,
            queryParameters: parameters,
            requestWeight: 2);
    }

    public Task<RestCallResult<BinanceSpotReferencePriceCalculation>> GetReferencePriceCalculationAsync(
        string symbol,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalEnum("symbolStatus", status);
        return RequestAsync<BinanceSpotReferencePriceCalculation>(
            GetUrl(api, v3, "referencePrice/calculation"),
            HttpMethod.Get,
            ct,
            queryParameters: parameters,
            requestWeight: 2);
    }

    public Task<RestCallResult<BinanceSpotTicker>> GetTickerAsync(
        string symbol,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "type", "FULL" }
        };
        parameters.AddOptionalEnum("symbolStatus", status);

        return RequestAsync<BinanceSpotTicker>(GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    public Task<RestCallResult<List<BinanceSpotTicker>>> GetTickersAsync(
        IEnumerable<string> symbols,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols, 100);

        var parameters = new ParameterCollection
        {
            { "symbols", JsonConvert.SerializeObject(symbolList) },
            { "type", "FULL" }
        };
        parameters.AddOptionalEnum("symbolStatus", status);

        var symbolCount = symbolList.Length;
        var weight = BinanceSpotMarketDataValidation.Ticker24HourWeight(symbolCount);
        return RequestAsync<List<BinanceSpotTicker>>(GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<List<BinanceSpotTicker>>> GetTickersAsync(
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "FULL" }
        };
        parameters.AddOptionalEnum("symbolStatus", status);

        return RequestAsync<List<BinanceSpotTicker>>(GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 80);
    }

    public Task<RestCallResult<BinanceSpotMiniTicker>> GetMiniTickerAsync(
        string symbol,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "type", "MINI" }
        };
        parameters.AddOptionalEnum("symbolStatus", status);

        return RequestAsync<BinanceSpotMiniTicker>(GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    public Task<RestCallResult<List<BinanceSpotMiniTicker>>> GetMiniTickersAsync(
        IEnumerable<string> symbols,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols, 100);

        var parameters = new ParameterCollection
        {
            { "symbols", JsonConvert.SerializeObject(symbolList) },
            { "type", "MINI" }
        };
        parameters.AddOptionalEnum("symbolStatus", status);

        var symbolCount = symbolList.Length;
        var weight = BinanceSpotMarketDataValidation.Ticker24HourWeight(symbolCount);
        return RequestAsync<List<BinanceSpotMiniTicker>>(GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<List<BinanceSpotMiniTicker>>> GetMiniTickersAsync(
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "MINI" }
        };
        parameters.AddOptionalEnum("symbolStatus", status);

        return RequestAsync<List<BinanceSpotMiniTicker>>(GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 80);
    }

    public Task<RestCallResult<BinanceSpotTradingDayTicker>> GetTradingDayTickerAsync(
        string symbol,
        string? timeZone = null,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        timeZone = BinanceSpotMarketDataValidation.TimeZone(timeZone);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "type", "FULL" }
        };
        parameters.AddOptional("timeZone", timeZone);
        parameters.AddOptionalEnum("symbolStatus", status);

        return RequestAsync<BinanceSpotTradingDayTicker>(GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 4);
    }

    public Task<RestCallResult<List<BinanceSpotTradingDayTicker>>> GetTradingDayTickersAsync(
        IEnumerable<string> symbols,
        string? timeZone = null,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols, 100);
        timeZone = BinanceSpotMarketDataValidation.TimeZone(timeZone);

        var parameters = new ParameterCollection
        {
            { "symbols", JsonConvert.SerializeObject(symbolList) },
            { "type", "FULL" }
        };
        parameters.AddOptional("timeZone", timeZone);
        parameters.AddOptionalEnum("symbolStatus", status);

        var symbolCount = symbolList.Length;
        var weight = BinanceSpotMarketDataValidation.RollingTickerWeight(symbolCount);
        return RequestAsync<List<BinanceSpotTradingDayTicker>>(GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<BinanceTradingDayMiniTicker>> GetTradingDayMiniTickerAsync(
        string symbol,
        string? timeZone = null,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        timeZone = BinanceSpotMarketDataValidation.TimeZone(timeZone);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "type", "MINI" }
        };
        parameters.AddOptional("timeZone", timeZone);
        parameters.AddOptionalEnum("symbolStatus", status);

        return RequestAsync<BinanceTradingDayMiniTicker>(GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 4);
    }

    public Task<RestCallResult<List<BinanceTradingDayMiniTicker>>> GetTradingDayMiniTickersAsync(
        IEnumerable<string> symbols,
        string? timeZone = null,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols, 100);
        timeZone = BinanceSpotMarketDataValidation.TimeZone(timeZone);

        var parameters = new ParameterCollection
        {
            { "symbols", JsonConvert.SerializeObject(symbolList) },
            { "type", "MINI" }
        };
        parameters.AddOptional("timeZone", timeZone);
        parameters.AddOptionalEnum("symbolStatus", status);

        var symbolCount = symbolList.Length;
        var weight = BinanceSpotMarketDataValidation.RollingTickerWeight(symbolCount);
        return RequestAsync<List<BinanceTradingDayMiniTicker>>(GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<BinanceSpotPriceTicker>> GetPriceTickerAsync(
        string symbol,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalEnum("symbolStatus", status);

        return RequestAsync<BinanceSpotPriceTicker>(GetUrl(api, v3, "ticker/price"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    public Task<RestCallResult<List<BinanceSpotPriceTicker>>> GetPriceTickersAsync(
        IEnumerable<string> symbols,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols);

        var parameters = new ParameterCollection { { "symbols", JsonConvert.SerializeObject(symbolList) } };
        parameters.AddOptionalEnum("symbolStatus", status);
        return RequestAsync<List<BinanceSpotPriceTicker>>(GetUrl(api, v3, "ticker/price"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 4);
    }

    public Task<RestCallResult<List<BinanceSpotPriceTicker>>> GetPriceTickersAsync(
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalEnum("symbolStatus", status);
        return RequestAsync<List<BinanceSpotPriceTicker>>(GetUrl(api, v3, "ticker/price"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 4);
    }

    public Task<RestCallResult<BinanceSpotBookTicker>> GetBookTickerAsync(
        string symbol,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalEnum("symbolStatus", status);

        return RequestAsync<BinanceSpotBookTicker>(GetUrl(api, v3, "ticker/bookTicker"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    public Task<RestCallResult<List<BinanceSpotBookTicker>>> GetBookTickersAsync(
        IEnumerable<string> symbols,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols);
        var parameters = new ParameterCollection { { "symbols", JsonConvert.SerializeObject(symbolList) } };
        parameters.AddOptionalEnum("symbolStatus", status);

        return RequestAsync<List<BinanceSpotBookTicker>>(GetUrl(api, v3, "ticker/bookTicker"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 4);
    }

    public Task<RestCallResult<List<BinanceSpotBookTicker>>> GetBookTickersAsync(
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalEnum("symbolStatus", status);
        return RequestAsync<List<BinanceSpotBookTicker>>(GetUrl(api, v3, "ticker/bookTicker"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 4);
    }

    public Task<RestCallResult<BinanceSpotRollingWindowTicker>> GetRollingWindowTickerAsync(
        string symbol,
        TimeSpan? windowSize = null,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("windowSize", BinanceSpotMarketDataValidation.WindowSize(windowSize));
        parameters.Add("type", "FULL");
        parameters.AddOptionalEnum("symbolStatus", status);

        return RequestAsync<BinanceSpotRollingWindowTicker>(GetUrl(api, v3, "ticker"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 4);
    }

    public Task<RestCallResult<List<BinanceSpotRollingWindowTicker>>> GetRollingWindowTickersAsync(
        IEnumerable<string> symbols,
        TimeSpan? windowSize = null,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols, 100);

        var parameters = new ParameterCollection { { "symbols", JsonConvert.SerializeObject(symbolList) }, { "type", "FULL" } };
        parameters.AddOptional("windowSize", BinanceSpotMarketDataValidation.WindowSize(windowSize));
        parameters.AddOptionalEnum("symbolStatus", status);
        var symbolCount = symbolList.Length;
        var weight = BinanceSpotMarketDataValidation.RollingTickerWeight(symbolCount);
        return RequestAsync<List<BinanceSpotRollingWindowTicker>>(GetUrl(api, v3, "ticker"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<BinanceSpotMiniTicker>> GetRollingWindowMiniTickerAsync(
        string symbol,
        TimeSpan? windowSize = null,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol }, { "type", "MINI" } };
        parameters.AddOptional("windowSize", BinanceSpotMarketDataValidation.WindowSize(windowSize));
        parameters.AddOptionalEnum("symbolStatus", status);
        return RequestAsync<BinanceSpotMiniTicker>(GetUrl(api, v3, "ticker"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 4);
    }

    public Task<RestCallResult<List<BinanceSpotMiniTicker>>> GetRollingWindowMiniTickersAsync(
        IEnumerable<string> symbols,
        TimeSpan? windowSize = null,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols, 100);
        var parameters = new ParameterCollection { { "symbols", JsonConvert.SerializeObject(symbolList) }, { "type", "MINI" } };
        parameters.AddOptional("windowSize", BinanceSpotMarketDataValidation.WindowSize(windowSize));
        parameters.AddOptionalEnum("symbolStatus", status);
        var weight = BinanceSpotMarketDataValidation.RollingTickerWeight(symbolList.Length);
        return RequestAsync<List<BinanceSpotMiniTicker>>(GetUrl(api, v3, "ticker"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }
}
