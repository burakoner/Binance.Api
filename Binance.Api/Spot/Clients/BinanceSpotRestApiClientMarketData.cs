namespace Binance.Api.Spot;

internal partial class BinanceSpotRestApiClient
{
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

    public Task<RestCallResult<IEnumerable<BinanceSpotTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalString("limit", limit);

        return RequestAsync<IEnumerable<BinanceSpotTrade>>(GetUrl(api, v3, "trades"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 25);
    }

    public Task<RestCallResult<IEnumerable<BinanceSpotTrade>>> GetTradeHistoryAsync(string symbol, int? limit = null, long? fromId = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalString("limit", limit);
        parameters.AddOptionalString("fromId", fromId);

        return RequestAsync<IEnumerable<BinanceSpotTrade>>(GetUrl(api, v3, "historicalTrades"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 25);
    }

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

    public async Task<RestCallResult<BinanceAveragePrice>> GetAveragePriceAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };

        return await RequestAsync<BinanceAveragePrice>(GetUrl(api, v3, "avgPrice"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2).ConfigureAwait(false);
    }

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

    public Task<RestCallResult<IEnumerable<BinanceFullTicker>>> GetFullTickersAsync(CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "FULL" }
        };

        return RequestAsync<IEnumerable<BinanceFullTicker>>(GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 80);
    }

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

    public Task<RestCallResult<IEnumerable<BinanceMiniTicker>>> GetMiniTickersAsync(CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "MINI" }
        };

        return RequestAsync<IEnumerable<BinanceMiniTicker>>(GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 80);
    }

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

    public Task<RestCallResult<IEnumerable<BinanceTradingDayFullTicker>>> GetTradingDayFullTickersAsync(string? timeZone = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "FULL" }
        };
        parameters.AddOptional("timeZone", timeZone);

        return RequestAsync<IEnumerable<BinanceTradingDayFullTicker>>(GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 80);
    }

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

    public Task<RestCallResult<IEnumerable<BinanceTradingDayMiniTicker>>> GetTradingDayMiniTickersAsync(string? timeZone = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "MINI" }
        };
        parameters.AddOptional("timeZone", timeZone);

        return RequestAsync<IEnumerable<BinanceTradingDayMiniTicker>>(GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 80);
    }

    public Task<RestCallResult<BinancePriceTicker>> GetPriceTickerAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };

        return RequestAsync<BinancePriceTicker>(GetUrl(api, v3, "ticker/price"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    public Task<RestCallResult<IEnumerable<BinancePriceTicker>>> GetPriceTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };
        return RequestAsync<IEnumerable<BinancePriceTicker>>(GetUrl(api, v3, "ticker/price"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 4);
    }

    public Task<RestCallResult<IEnumerable<BinancePriceTicker>>> GetPriceTickersAsync(CancellationToken ct = default)
    {
        return RequestAsync<IEnumerable<BinancePriceTicker>>(GetUrl(api, v3, "ticker/price"), HttpMethod.Get, ct, requestWeight: 4);
    }

    public Task<RestCallResult<BinanceBookTicker>> GetBookTickerAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };

        return RequestAsync<BinanceBookTicker>(GetUrl(api, v3, "ticker/bookTicker"), HttpMethod.Get, ct, false, queryParameters: parameters);
    }

    public Task<RestCallResult<IEnumerable<BinanceBookTicker>>> GetBookTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };

        return RequestAsync<IEnumerable<BinanceBookTicker>>(GetUrl(api, v3, "ticker/bookTicker"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    public Task<RestCallResult<IEnumerable<BinanceBookTicker>>> GetBookTickersAsync(CancellationToken ct = default)
    {
        return RequestAsync<IEnumerable<BinanceBookTicker>>(GetUrl(api, v3, "ticker/bookTicker"), HttpMethod.Get, ct, requestWeight: 2);
    }

    public Task<RestCallResult<BinanceFullTicker>> GetRollingWindowTickerAsync(string symbol, TimeSpan? windowSize = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("windowSize", windowSize == null ? null : GetWindowSize(windowSize.Value));

        return RequestAsync<BinanceFullTicker>(GetUrl(api, v3, "ticker"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

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
}