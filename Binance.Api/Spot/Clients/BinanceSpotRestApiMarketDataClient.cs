namespace Binance.Api.Spot;

public class BinanceSpotRestApiMarketDataClient(BinanceSpotRestApiClient parent)
{
    // Api
    private const string api = "api";
    private const string v1 = "1";
    private const string v3 = "3";

    // Parent Clients
    internal BinanceRestApiClient _ { get; } = parent._;
    internal BinanceSpotRestApiClient __ { get; } = parent;
    internal ILogger? Logger { get; } = parent.Logger;

    public async Task<RestCallResult<BinanceOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 5000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalString("limit", limit);

        var requestWeight = limit == null ? 1 : limit <= 100 ? 5 : limit <= 500 ? 25 : limit <= 1000 ? 50 : 250;
        var result = await __.SendRequestInternal<BinanceOrderBook>(__.GetUrl(api, v3, "depth"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
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

        return __.SendRequestInternal<IEnumerable<BinanceSpotTrade>>(__.GetUrl(api, v3, "trades"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 25);
    }

    public Task<RestCallResult<IEnumerable<BinanceSpotTrade>>> GetTradeHistoryAsync(string symbol, int? limit = null, long? fromId = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalString("limit", limit);
        parameters.AddOptionalString("fromId", fromId);

        return __.SendRequestInternal<IEnumerable<BinanceSpotTrade>>(__.GetUrl(api, v3, "historicalTrades"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 25);
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

        return __.SendRequestInternal<IEnumerable<BinanceAggregatedTrade>>(__.GetUrl(api, v3, "aggTrades"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 4);
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

        return __.SendRequestInternal<IEnumerable<BinanceSpotKline>>(__.GetUrl(api, v3, "klines"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
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

        return __.SendRequestInternal<IEnumerable<BinanceSpotKline>>(__.GetUrl(api, v3, "uiKlines"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    public async Task<RestCallResult<BinanceAveragePrice>> GetAveragePriceAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };

        return await __.SendRequestInternal<BinanceAveragePrice>(__.GetUrl(api, v3, "avgPrice"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2).ConfigureAwait(false);
    }


    public Task<RestCallResult<BinanceFullTicker>> GetFullTickerAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "type", "FULL" }
        };

        return __.SendRequestInternal<BinanceFullTicker>(__.GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
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
        return __.SendRequestInternal<IEnumerable<BinanceFullTicker>>(__.GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<IEnumerable<BinanceFullTicker>>> GetFullTickersAsync(CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "FULL" }
        };

        return __.SendRequestInternal<IEnumerable<BinanceFullTicker>>(__.GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 80);
    }



    public Task<RestCallResult<BinanceMiniTicker>> GetMiniTickerAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "type", "MINI" }
        };

        return __.SendRequestInternal<BinanceMiniTicker>(__.GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
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
        return __.SendRequestInternal<IEnumerable<BinanceMiniTicker>>(__.GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<IEnumerable<BinanceMiniTicker>>> GetMiniTickersAsync(CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "MINI" }
        };

        return __.SendRequestInternal<IEnumerable<BinanceMiniTicker>>(__.GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 80);
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

        return __.SendRequestInternal<BinanceTradingDayFullTicker>(__.GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 4);
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
        return __.SendRequestInternal<IEnumerable<BinanceTradingDayFullTicker>>(__.GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<IEnumerable<BinanceTradingDayFullTicker>>> GetTradingDayFullTickersAsync(string? timeZone = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "FULL" }
        };
        parameters.AddOptional("timeZone", timeZone);

        return __.SendRequestInternal<IEnumerable<BinanceTradingDayFullTicker>>(__.GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 80);
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

        return __.SendRequestInternal<BinanceTradingDayMiniTicker>(__.GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 4);
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
        return __.SendRequestInternal<IEnumerable<BinanceTradingDayMiniTicker>>(__.GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<IEnumerable<BinanceTradingDayMiniTicker>>> GetTradingDayMiniTickersAsync(string? timeZone = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "MINI" }
        };
        parameters.AddOptional("timeZone", timeZone);

        return __.SendRequestInternal<IEnumerable<BinanceTradingDayMiniTicker>>(__.GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 80);
    }

    public Task<RestCallResult<BinancePriceTicker>> GetPriceAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };

        return __.SendRequestInternal<BinancePriceTicker>(__.GetUrl(api, v3, "ticker/price"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    public Task<RestCallResult<IEnumerable<BinancePriceTicker>>> GetPricesAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };
        return __.SendRequestInternal<IEnumerable<BinancePriceTicker>>(__.GetUrl(api, v3, "ticker/price"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 4);
    }

    public Task<RestCallResult<IEnumerable<BinancePriceTicker>>> GetPricesAsync(CancellationToken ct = default)
    {
        return __.SendRequestInternal<IEnumerable<BinancePriceTicker>>(__.GetUrl(api, v3, "ticker/price"), HttpMethod.Get, ct, requestWeight: 4);
    }

    public Task<RestCallResult<BinanceBookTicker>> GetBookPriceAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };

        return __.SendRequestInternal<BinanceBookTicker>(__.GetUrl(api, v3, "ticker/bookTicker"), HttpMethod.Get, ct, false, queryParameters: parameters);
    }

    public Task<RestCallResult<IEnumerable<BinanceBookTicker>>> GetBookPricesAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };

        return __.SendRequestInternal<IEnumerable<BinanceBookTicker>>(__.GetUrl(api, v3, "ticker/bookTicker"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    public Task<RestCallResult<IEnumerable<BinanceBookTicker>>> GetBookPricesAsync(CancellationToken ct = default)
    {
        return __.SendRequestInternal<IEnumerable<BinanceBookTicker>>(__.GetUrl(api, v3, "ticker/bookTicker"), HttpMethod.Get, ct, requestWeight: 2);
    }

    public Task<RestCallResult<BinanceFullTicker>> GetRollingWindowTickerAsync(string symbol, TimeSpan? windowSize = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("windowSize", windowSize == null ? null : GetWindowSize(windowSize.Value));

        return __.SendRequestInternal<BinanceFullTicker>(__.GetUrl(api, v3, "ticker"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    public Task<RestCallResult<IEnumerable<BinanceFullTicker>>> GetRollingWindowTickersAsync(IEnumerable<string> symbols, TimeSpan? windowSize = null, CancellationToken ct = default)
    {
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };
        parameters.AddOptional("windowSize", windowSize == null ? null : GetWindowSize(windowSize.Value));
        var symbolCount = symbols.Count();
        var weight = Math.Min(symbolCount * 4, 200);
        return __.SendRequestInternal<IEnumerable<BinanceFullTicker>>(__.GetUrl(api, v3, "ticker"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }

    private string GetWindowSize(TimeSpan timeSpan)
    {
        if (timeSpan.TotalHours < 1) return timeSpan.TotalMinutes + "m";
        else if (timeSpan.TotalHours < 24) return timeSpan.TotalHours + "h";
        return timeSpan.TotalDays + "d";
    }


}