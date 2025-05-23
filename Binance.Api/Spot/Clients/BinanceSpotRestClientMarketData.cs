﻿namespace Binance.Api.Spot;

internal partial class BinanceSpotRestClient
{
    public async Task<RestCallResult<BinanceSpotOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 5000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalString("limit", limit);

        var requestWeight = limit == null ? 1 : limit <= 100 ? 5 : limit <= 500 ? 25 : limit <= 1000 ? 50 : 250;
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

    public Task<RestCallResult<List<BinanceSpotAggregatedTrade>>> GetAggregatedTradesAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<List<BinanceSpotAggregatedTrade>>(GetUrl(api, v3, "aggTrades"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 4);
    }

    public Task<RestCallResult<List<BinanceSpotKline>>> GetKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddEnum("interval", interval);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<List<BinanceSpotKline>>(GetUrl(api, v3, "klines"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    public Task<RestCallResult<List<BinanceSpotKline>>> GetUIKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddEnum("interval", interval);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<List<BinanceSpotKline>>(GetUrl(api, v3, "uiKlines"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    public async Task<RestCallResult<BinanceSpotAveragePrice>> GetAveragePriceAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };

        return await RequestAsync<BinanceSpotAveragePrice>(GetUrl(api, v3, "avgPrice"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2).ConfigureAwait(false);
    }

    public Task<RestCallResult<BinanceSpotTicker>> GetTickerAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "type", "FULL" }
        };

        return RequestAsync<BinanceSpotTicker>(GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    public Task<RestCallResult<List<BinanceSpotTicker>>> GetTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" },
            { "type", "FULL" }
        };

        var symbolCount = symbols.Count();
        var weight = symbolCount <= 20 ? 2 : symbolCount <= 100 ? 40 : 80;
        return RequestAsync<List<BinanceSpotTicker>>(GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<List<BinanceSpotTicker>>> GetTickersAsync(CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "FULL" }
        };

        return RequestAsync<List<BinanceSpotTicker>>(GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 80);
    }

    public Task<RestCallResult<BinanceSpotMiniTicker>> GetMiniTickerAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "type", "MINI" }
        };

        return RequestAsync<BinanceSpotMiniTicker>(GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    public Task<RestCallResult<List<BinanceSpotMiniTicker>>> GetMiniTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" },
            { "type", "MINI" }
        };

        var symbolCount = symbols.Count();
        var weight = symbolCount <= 20 ? 2 : symbolCount <= 100 ? 40 : 80;
        return RequestAsync<List<BinanceSpotMiniTicker>>(GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<List<BinanceSpotMiniTicker>>> GetMiniTickersAsync(CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "MINI" }
        };

        return RequestAsync<List<BinanceSpotMiniTicker>>(GetUrl(api, v3, "ticker/24hr"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 80);
    }

    public Task<RestCallResult<BinanceSpotTradingDayTicker>> GetTradingDayTickerAsync(string symbol, string? timeZone = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "type", "FULL" }
        };
        parameters.AddOptional("timeZone", timeZone);

        return RequestAsync<BinanceSpotTradingDayTicker>(GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 4);
    }

    public Task<RestCallResult<List<BinanceSpotTradingDayTicker>>> GetTradingDayTickersAsync(IEnumerable<string> symbols, string? timeZone = null, CancellationToken ct = default)
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
        return RequestAsync<List<BinanceSpotTradingDayTicker>>(GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<List<BinanceSpotTradingDayTicker>>> GetTradingDayTickersAsync(string? timeZone = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "FULL" }
        };
        parameters.AddOptional("timeZone", timeZone);

        return RequestAsync<List<BinanceSpotTradingDayTicker>>(GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 80);
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

    public Task<RestCallResult<List<BinanceTradingDayMiniTicker>>> GetTradingDayMiniTickersAsync(IEnumerable<string> symbols, string? timeZone = null, CancellationToken ct = default)
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
        return RequestAsync<List<BinanceTradingDayMiniTicker>>(GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<List<BinanceTradingDayMiniTicker>>> GetTradingDayMiniTickersAsync(string? timeZone = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "MINI" }
        };
        parameters.AddOptional("timeZone", timeZone);

        return RequestAsync<List<BinanceTradingDayMiniTicker>>(GetUrl(api, v3, "ticker/tradingDay"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 80);
    }

    public Task<RestCallResult<BinanceSpotPriceTicker>> GetPriceTickerAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };

        return RequestAsync<BinanceSpotPriceTicker>(GetUrl(api, v3, "ticker/price"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    public Task<RestCallResult<List<BinanceSpotPriceTicker>>> GetPriceTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };
        return RequestAsync<List<BinanceSpotPriceTicker>>(GetUrl(api, v3, "ticker/price"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 4);
    }

    public Task<RestCallResult<List<BinanceSpotPriceTicker>>> GetPriceTickersAsync(CancellationToken ct = default)
    {
        return RequestAsync<List<BinanceSpotPriceTicker>>(GetUrl(api, v3, "ticker/price"), HttpMethod.Get, ct, requestWeight: 4);
    }

    public Task<RestCallResult<BinanceSpotBookTicker>> GetBookTickerAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };

        return RequestAsync<BinanceSpotBookTicker>(GetUrl(api, v3, "ticker/bookTicker"), HttpMethod.Get, ct, false, queryParameters: parameters);
    }

    public Task<RestCallResult<List<BinanceSpotBookTicker>>> GetBookTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };

        return RequestAsync<List<BinanceSpotBookTicker>>(GetUrl(api, v3, "ticker/bookTicker"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    public Task<RestCallResult<List<BinanceSpotBookTicker>>> GetBookTickersAsync(CancellationToken ct = default)
    {
        return RequestAsync<List<BinanceSpotBookTicker>>(GetUrl(api, v3, "ticker/bookTicker"), HttpMethod.Get, ct, requestWeight: 2);
    }

    public Task<RestCallResult<BinanceSpotTicker>> GetRollingWindowTickerAsync(string symbol, TimeSpan? windowSize = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("windowSize", windowSize == null ? null : GetWindowSize(windowSize.Value));

        return RequestAsync<BinanceSpotTicker>(GetUrl(api, v3, "ticker"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    public Task<RestCallResult<List<BinanceSpotTicker>>> GetRollingWindowTickersAsync(IEnumerable<string> symbols, TimeSpan? windowSize = null, CancellationToken ct = default)
    {
        foreach (var symbol in symbols) symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };
        parameters.AddOptional("windowSize", windowSize == null ? null : GetWindowSize(windowSize.Value));
        var symbolCount = symbols.Count();
        var weight = Math.Min(symbolCount * 4, 200);
        return RequestAsync<List<BinanceSpotTicker>>(GetUrl(api, v3, "ticker"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight);
    }

    private string GetWindowSize(TimeSpan timeSpan)
    {
        if (timeSpan.TotalHours < 1) return timeSpan.TotalMinutes + "m";
        else if (timeSpan.TotalHours < 24) return timeSpan.TotalHours + "h";
        return timeSpan.TotalDays + "d";
    }
}