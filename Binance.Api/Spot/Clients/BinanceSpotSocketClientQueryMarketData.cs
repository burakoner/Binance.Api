namespace Binance.Api.Spot;

internal partial class BinanceSpotSocketClient
{
    public async Task<CallResult<BinanceSpotOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptionalParameter("limit", limit);
        int weight = limit <= 100 ? 5 : limit <= 500 ? 25 : limit <= 1000 ? 50 : 250;
        var result = await RequestAsync<BinanceSpotOrderBook>("ws-api/v3", $"depth", parameters, weight: weight, ct: ct).ConfigureAwait(false);
        if (result) result.Data.Symbol = symbol;
        return result;
    }

    public Task<CallResult<List<BinanceSpotTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptionalParameter("limit", limit);
        return RequestAsync<List<BinanceSpotTrade>>("ws-api/v3", $"trades.recent", parameters, weight: 25, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotTrade>>> GetHistoricalTradesAsync(string symbol, long? fromId = null, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptionalParameter("limit", limit);
        parameters.AddOptionalParameter("fromId", fromId);
        return RequestAsync<List<BinanceSpotTrade>>("ws-api/v3", $"trades.historical", parameters, false, weight: 25, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotStreamAggregatedTrade>>> GetAggregatedTradesAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptionalParameter("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptionalParameter("fromId", fromId);
        return RequestAsync<List<BinanceSpotStreamAggregatedTrade>>("ws-api/v3", $"trades.aggregate", parameters, false, weight: 2, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotKline>>> GetKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddEnum("interval", interval);
        parameters.AddOptionalParameter("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        return RequestAsync<List<BinanceSpotKline>>("ws-api/v3", $"klines", parameters, false, weight: 2, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotKline>>> GetUIKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddEnum("interval", interval);
        parameters.AddOptionalParameter("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        return RequestAsync<List<BinanceSpotKline>>("ws-api/v3", $"uiKlines", parameters, false, weight: 2, ct: ct);
    }

    public Task<CallResult<BinanceSpotAveragePrice>> GetAveragePriceAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddParameter("symbol", symbol);
        return RequestAsync<BinanceSpotAveragePrice>("ws-api/v3", $"avgPrice", parameters, false, weight: 2, ct: ct);
    }

    public Task<CallResult<BinanceSpotTicker>> GetTickerAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "type", "FULL" }
        };
        return RequestAsync<BinanceSpotTicker>("ws-api/v3", $"ticker.24hr", parameters, false, weight: 2, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotTicker>>> GetTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" },
            { "type", "FULL" }
        };
        var symbolCount = symbols?.Count();
        int weight = symbolCount == null || symbolCount > 100 ? 80 : symbolCount <= 20 ? 2 : 40;
        return RequestAsync<List<BinanceSpotTicker>>("ws-api/v3", $"ticker.24hr", parameters, false, weight: weight, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotTicker>>> GetTickersAsync(CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "FULL" }
        };
        return RequestAsync<List<BinanceSpotTicker>>("ws-api/v3", $"ticker.24hr", parameters, false, weight: 80, ct: ct);
    }

    public Task<CallResult<BinanceSpotMiniTicker>> GetMiniTickerAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "type", "MINI" }
        };
        return RequestAsync<BinanceSpotMiniTicker>("ws-api/v3", $"ticker.24hr", parameters, false, weight: 2, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotMiniTicker>>> GetMiniTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" },
            { "type", "MINI" }
        };
        var symbolCount = symbols?.Count();
        int weight = symbolCount == null || symbolCount > 100 ? 80 : symbolCount <= 20 ? 2 : 40;
        return RequestAsync<List<BinanceSpotMiniTicker>>("ws-api/v3", $"ticker.24hr", parameters, false, weight: weight, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotMiniTicker>>> GetMiniTickersAsync(CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "MINI" }
        };
        return RequestAsync<List<BinanceSpotMiniTicker>>("ws-api/v3", $"ticker.24hr", parameters, false, weight: 80, ct: ct);
    }

    public Task<CallResult<BinanceSpotTradingDayTicker>> GetTradingDayTickerAsync(string symbol, string? timeZone = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "type", "FULL" }
        };
        parameters.AddOptional("timeZone", timeZone);

        return RequestAsync<BinanceSpotTradingDayTicker>("ws-api/v3", $"ticker.tradingDay", parameters, false, weight: 4, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotTradingDayTicker>>> GetTradingDayTickersAsync(IEnumerable<string> symbols, string? timeZone = null, CancellationToken ct = default)
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
        return RequestAsync<List<BinanceSpotTradingDayTicker>>("ws-api/v3", $"ticker.tradingDay", parameters, false, weight: weight, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotTradingDayTicker>>> GetTradingDayTickersAsync(string? timeZone = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "FULL" }
        };
        parameters.AddOptional("timeZone", timeZone);

        return RequestAsync<List<BinanceSpotTradingDayTicker>>("ws-api/v3", $"ticker.tradingDay", parameters, false, weight: 80, ct: ct);
    }

    public Task<CallResult<BinanceSpotTradingDayTicker>> GetTradingDayMiniTickerAsync(string symbol, string? timeZone = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "type", "MINI" }
        };
        parameters.AddOptional("timeZone", timeZone);

        return RequestAsync<BinanceSpotTradingDayTicker>("ws-api/v3", $"ticker.tradingDay", parameters, false, weight: 4, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotTradingDayTicker>>> GetTradingDayMiniTickersAsync(IEnumerable<string> symbols, string? timeZone = null, CancellationToken ct = default)
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
        return RequestAsync<List<BinanceSpotTradingDayTicker>>("ws-api/v3", $"ticker.tradingDay", parameters, false, weight: weight, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotTradingDayTicker>>> GetTradingDayMiniTickersAsync(string? timeZone = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "MINI" }
        };
        parameters.AddOptional("timeZone", timeZone);

        return RequestAsync<List<BinanceSpotTradingDayTicker>>("ws-api/v3", $"ticker.tradingDay", parameters, false, weight: 80, ct: ct);
    }

    public Task<CallResult<BinanceSpotTicker>> GetRollingWindowTickerAsync(string symbol, TimeSpan? windowSize = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
        };
        parameters.AddOptional("windowSize", windowSize == null ? null : GetWindowSize(windowSize.Value));
        return RequestAsync<BinanceSpotTicker>("ws-api/v3", $"ticker", parameters, false, weight: 2, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotTicker>>> GetRollingWindowTickersAsync(IEnumerable<string> symbols, TimeSpan? windowSize = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" },
        };
        parameters.AddOptional("windowSize", windowSize == null ? null : GetWindowSize(windowSize.Value));
        var symbolCount = symbols?.Count();
        int weight = symbolCount == null || symbolCount > 100 ? 80 : symbolCount <= 20 ? 2 : 40;
        return RequestAsync<List<BinanceSpotTicker>>("ws-api/v3", $"ticker", parameters, false, weight: weight, ct: ct);
    }

    public Task<CallResult<BinanceSpotPriceTicker>> GetPriceTickerAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };

        return RequestAsync<BinanceSpotPriceTicker>("ws-api/v3", $"ticker.price", parameters, false, weight: 2, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotPriceTicker>>> GetPriceTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };
        return RequestAsync<List<BinanceSpotPriceTicker>>("ws-api/v3", $"ticker.price", parameters, false, weight: 4, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotPriceTicker>>> GetPriceTickersAsync(CancellationToken ct = default)
    {
        return RequestAsync<List<BinanceSpotPriceTicker>>("ws-api/v3", $"ticker.price", [], false, weight: 4, ct: ct);
    }

    public Task<CallResult<BinanceSpotBookTicker>> GetBookTickerAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
        };
        return RequestAsync<BinanceSpotBookTicker>("ws-api/v3", $"ticker.book", parameters, false, weight: 2, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotBookTicker>>> GetBookTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" },
        };
        return RequestAsync<List<BinanceSpotBookTicker>>("ws-api/v3", $"ticker.book", parameters, false, weight: 4, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotBookTicker>>> GetBookTickersAsync(CancellationToken ct = default)
    {
        return RequestAsync<List<BinanceSpotBookTicker>>("ws-api/v3", $"ticker.book", [], false, weight: 4, ct: ct);
    }

    private string GetWindowSize(TimeSpan timeSpan)
    {
        if (timeSpan.TotalHours < 1) return timeSpan.TotalMinutes + "m";
        else if (timeSpan.TotalHours < 24) return timeSpan.TotalHours + "h";
        return timeSpan.TotalDays + "d";
    }
}