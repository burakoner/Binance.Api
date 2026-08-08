namespace Binance.Api.Spot;

internal partial class BinanceSpotSocketClient
{
    public async Task<CallResult<BinanceSpotOrderBook>> GetOrderBookAsync(
        string symbol,
        int? limit = null,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 5000);
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalEnum("symbolStatus", status);
        var weight = BinanceSpotMarketDataValidation.DepthWeight(limit);
        var result = await RequestAsync<BinanceSpotOrderBook>("ws-api/v3", $"depth", parameters, weight: weight, ct: ct).ConfigureAwait(false);
        if (result) result.Data.Symbol = symbol;
        return result;
    }

    public Task<CallResult<List<BinanceSpotTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("limit", limit);
        return RequestAsync<List<BinanceSpotTrade>>("ws-api/v3", $"trades.recent", parameters, weight: 25, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotTrade>>> GetHistoricalTradesAsync(string symbol, long? fromId = null, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        if (fromId < 0)
            throw new ArgumentOutOfRangeException(nameof(fromId), "fromId cannot be negative.");
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("fromId", fromId);
        return RequestAsync<List<BinanceSpotTrade>>("ws-api/v3", $"trades.historical", parameters, false, weight: 25, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotBlockTrade>>> GetHistoricalBlockTradesAsync(
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
        return RequestAsync<List<BinanceSpotBlockTrade>>("ws-api/v3", "blockTrades.historical", parameters, weight: 25, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotAggregatedTrade>>> GetAggregatedTradesAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        BinanceSpotMarketDataValidation.TimeRange(startTime, endTime);
        if (fromId.HasValue && (startTime.HasValue || endTime.HasValue))
            throw new ArgumentException("fromId cannot be combined with startTime or endTime.", nameof(fromId));
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("fromId", fromId);
        return RequestAsync<List<BinanceSpotAggregatedTrade>>("ws-api/v3", $"trades.aggregate", parameters, false, weight: 4, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotKline>>> GetKlinesAsync(
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
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddEnum("interval", interval);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("timeZone", timeZone);
        return RequestAsync<List<BinanceSpotKline>>("ws-api/v3", $"klines", parameters, false, weight: 2, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotKline>>> GetUIKlinesAsync(
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
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddEnum("interval", interval);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("timeZone", timeZone);
        return RequestAsync<List<BinanceSpotKline>>("ws-api/v3", $"uiKlines", parameters, false, weight: 2, ct: ct);
    }

    public Task<CallResult<BinanceSpotAveragePrice>> GetAveragePriceAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new Dictionary<string, object>();
        parameters.AddParameter("symbol", symbol);
        return RequestAsync<BinanceSpotAveragePrice>("ws-api/v3", $"avgPrice", parameters, false, weight: 2, ct: ct);
    }

    public Task<CallResult<BinanceSpotReferencePrice>> GetReferencePriceAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };
        return RequestAsync<BinanceSpotReferencePrice>("ws-api/v3", "referencePrice", parameters, weight: 2, ct: ct);
    }

    public Task<CallResult<BinanceSpotReferencePriceCalculation>> GetReferencePriceCalculationAsync(
        string symbol,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalEnum("symbolStatus", status);
        return RequestAsync<BinanceSpotReferencePriceCalculation>("ws-api/v3", "referencePrice.calculation", parameters, weight: 2, ct: ct);
    }

    public Task<CallResult<BinanceSpotTicker>> GetTickerAsync(
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
        return RequestAsync<BinanceSpotTicker>("ws-api/v3", $"ticker.24hr", parameters, false, weight: 2, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotTicker>>> GetTickersAsync(
        IEnumerable<string> symbols,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols);
        var parameters = new ParameterCollection
        {
            { "symbols", symbolList },
            { "type", "FULL" }
        };
        parameters.AddOptionalEnum("symbolStatus", status);
        var weight = BinanceSpotMarketDataValidation.Ticker24HourWeight(symbolList.Length);
        return RequestAsync<List<BinanceSpotTicker>>("ws-api/v3", $"ticker.24hr", parameters, false, weight: weight, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotTicker>>> GetTickersAsync(
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "FULL" }
        };
        parameters.AddOptionalEnum("symbolStatus", status);
        return RequestAsync<List<BinanceSpotTicker>>("ws-api/v3", $"ticker.24hr", parameters, false, weight: 80, ct: ct);
    }

    public Task<CallResult<BinanceSpotMiniTicker>> GetMiniTickerAsync(
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
        return RequestAsync<BinanceSpotMiniTicker>("ws-api/v3", $"ticker.24hr", parameters, false, weight: 2, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotMiniTicker>>> GetMiniTickersAsync(
        IEnumerable<string> symbols,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols);
        var parameters = new ParameterCollection
        {
            { "symbols", symbolList },
            { "type", "MINI" }
        };
        parameters.AddOptionalEnum("symbolStatus", status);
        var weight = BinanceSpotMarketDataValidation.Ticker24HourWeight(symbolList.Length);
        return RequestAsync<List<BinanceSpotMiniTicker>>("ws-api/v3", $"ticker.24hr", parameters, false, weight: weight, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotMiniTicker>>> GetMiniTickersAsync(
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "type", "MINI" }
        };
        parameters.AddOptionalEnum("symbolStatus", status);
        return RequestAsync<List<BinanceSpotMiniTicker>>("ws-api/v3", $"ticker.24hr", parameters, false, weight: 80, ct: ct);
    }

    public Task<CallResult<BinanceSpotTradingDayTicker>> GetTradingDayTickerAsync(
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

        return RequestAsync<BinanceSpotTradingDayTicker>("ws-api/v3", $"ticker.tradingDay", parameters, false, weight: 4, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotTradingDayTicker>>> GetTradingDayTickersAsync(
        IEnumerable<string> symbols,
        string? timeZone = null,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols);
        timeZone = BinanceSpotMarketDataValidation.TimeZone(timeZone);

        var parameters = new ParameterCollection
        {
            { "symbols", symbolList },
            { "type", "FULL" }
        };
        parameters.AddOptional("timeZone", timeZone);
        parameters.AddOptionalEnum("symbolStatus", status);

        var symbolCount = symbolList.Length;
        var weight = BinanceSpotMarketDataValidation.RollingTickerWeight(symbolCount);
        return RequestAsync<List<BinanceSpotTradingDayTicker>>("ws-api/v3", $"ticker.tradingDay", parameters, false, weight: weight, ct: ct);
    }

    public Task<CallResult<BinanceTradingDayMiniTicker>> GetTradingDayMiniTickerAsync(
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

        return RequestAsync<BinanceTradingDayMiniTicker>("ws-api/v3", $"ticker.tradingDay", parameters, false, weight: 4, ct: ct);
    }

    public Task<CallResult<List<BinanceTradingDayMiniTicker>>> GetTradingDayMiniTickersAsync(
        IEnumerable<string> symbols,
        string? timeZone = null,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols);
        timeZone = BinanceSpotMarketDataValidation.TimeZone(timeZone);

        var parameters = new ParameterCollection
        {
            { "symbols", symbolList },
            { "type", "MINI" }
        };
        parameters.AddOptional("timeZone", timeZone);
        parameters.AddOptionalEnum("symbolStatus", status);

        var symbolCount = symbolList.Length;
        var weight = BinanceSpotMarketDataValidation.RollingTickerWeight(symbolCount);
        return RequestAsync<List<BinanceTradingDayMiniTicker>>("ws-api/v3", $"ticker.tradingDay", parameters, false, weight: weight, ct: ct);
    }

    public Task<CallResult<BinanceSpotRollingWindowTicker>> GetRollingWindowTickerAsync(
        string symbol,
        TimeSpan? windowSize = null,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "type", "FULL" }
        };
        parameters.AddOptional("windowSize", BinanceSpotMarketDataValidation.WindowSize(windowSize));
        parameters.AddOptionalEnum("symbolStatus", status);
        return RequestAsync<BinanceSpotRollingWindowTicker>("ws-api/v3", $"ticker", parameters, false, weight: 4, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotRollingWindowTicker>>> GetRollingWindowTickersAsync(
        IEnumerable<string> symbols,
        TimeSpan? windowSize = null,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols, 200);
        var parameters = new ParameterCollection
        {
            { "symbols", symbolList },
            { "type", "FULL" }
        };
        parameters.AddOptional("windowSize", BinanceSpotMarketDataValidation.WindowSize(windowSize));
        parameters.AddOptionalEnum("symbolStatus", status);
        var weight = BinanceSpotMarketDataValidation.RollingTickerWeight(symbolList.Length);
        return RequestAsync<List<BinanceSpotRollingWindowTicker>>("ws-api/v3", $"ticker", parameters, false, weight: weight, ct: ct);
    }

    public Task<CallResult<BinanceSpotMiniTicker>> GetRollingWindowMiniTickerAsync(
        string symbol,
        TimeSpan? windowSize = null,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol }, { "type", "MINI" } };
        parameters.AddOptional("windowSize", BinanceSpotMarketDataValidation.WindowSize(windowSize));
        parameters.AddOptionalEnum("symbolStatus", status);
        return RequestAsync<BinanceSpotMiniTicker>("ws-api/v3", "ticker", parameters, weight: 4, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotMiniTicker>>> GetRollingWindowMiniTickersAsync(
        IEnumerable<string> symbols,
        TimeSpan? windowSize = null,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols, 200);
        var parameters = new ParameterCollection { { "symbols", symbolList }, { "type", "MINI" } };
        parameters.AddOptional("windowSize", BinanceSpotMarketDataValidation.WindowSize(windowSize));
        parameters.AddOptionalEnum("symbolStatus", status);
        var weight = BinanceSpotMarketDataValidation.RollingTickerWeight(symbolList.Length);
        return RequestAsync<List<BinanceSpotMiniTicker>>("ws-api/v3", "ticker", parameters, weight: weight, ct: ct);
    }

    public Task<CallResult<BinanceSpotPriceTicker>> GetPriceTickerAsync(
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

        return RequestAsync<BinanceSpotPriceTicker>("ws-api/v3", $"ticker.price", parameters, false, weight: 2, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotPriceTicker>>> GetPriceTickersAsync(
        IEnumerable<string> symbols,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols);

        var parameters = new ParameterCollection { { "symbols", symbolList } };
        parameters.AddOptionalEnum("symbolStatus", status);
        return RequestAsync<List<BinanceSpotPriceTicker>>("ws-api/v3", $"ticker.price", parameters, false, weight: 4, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotPriceTicker>>> GetPriceTickersAsync(
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalEnum("symbolStatus", status);
        return RequestAsync<List<BinanceSpotPriceTicker>>("ws-api/v3", $"ticker.price", parameters, false, weight: 4, ct: ct);
    }

    public Task<CallResult<BinanceSpotBookTicker>> GetBookTickerAsync(
        string symbol,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
        };
        parameters.AddOptionalEnum("symbolStatus", status);
        return RequestAsync<BinanceSpotBookTicker>("ws-api/v3", $"ticker.book", parameters, false, weight: 2, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotBookTicker>>> GetBookTickersAsync(
        IEnumerable<string> symbols,
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var symbolList = BinanceSpotMarketDataValidation.Symbols(symbols);
        var parameters = new ParameterCollection
        {
            { "symbols", symbolList },
        };
        parameters.AddOptionalEnum("symbolStatus", status);
        return RequestAsync<List<BinanceSpotBookTicker>>("ws-api/v3", $"ticker.book", parameters, false, weight: 4, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotBookTicker>>> GetBookTickersAsync(
        BinanceSpotSymbolStatusFilter? status = null,
        CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalEnum("symbolStatus", status);
        return RequestAsync<List<BinanceSpotBookTicker>>("ws-api/v3", $"ticker.book", parameters, false, weight: 4, ct: ct);
    }
}
