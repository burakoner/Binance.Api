namespace Binance.Api.Futures;

internal partial class BinanceFuturesRestClientCoin
{
    public async Task<RestCallResult<TimeSpan>> PingAsync(CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        var result = await RequestAsync<object>(GetUrl(dapi, v1, "ping"), HttpMethod.Get, ct).ConfigureAwait(false);
        sw.Stop();

        return result.Success
            ? result.As(sw.Elapsed)
            : result.AsError<TimeSpan>(result.Error!);
    }

    public async Task<RestCallResult<DateTime>> GetTimeAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceServerTime>(GetUrl(dapi, v1, "time"), HttpMethod.Get, ct, ignoreRatelimit: true).ConfigureAwait(false);

        return result.Success
            ? result.As(result.Data?.ServerTime ?? default)
            : result.AsError<DateTime>(result.Error!);
    }

    public async Task<RestCallResult<BinanceFuturesCoinExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceFuturesCoinExchangeInfo>(GetUrl(dapi, v1, "exchangeInfo"), HttpMethod.Get, ct, requestWeight: 1).ConfigureAwait(false);
        if (!result) return result;

        ExchangeInfo = result.Data;
        LastExchangeInfoUpdate = DateTime.UtcNow;
        _.Logger.Log(LogLevel.Information, "Trade rules updated");

        return result;
    }

    public async Task<RestCallResult<BinanceFuturesOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntValues(nameof(limit), 5, 10, 20, 50, 100, 500, 1000);
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("limit", limit);

        var weight = limit == null ? 10 : limit <= 50 ? 2 : limit == 100 ? 5 : limit == 500 ? 10 : 20;
        var result = await RequestAsync<BinanceFuturesOrderBook>(GetUrl(dapi, v1, "depth"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: weight).ConfigureAwait(false);
        if (result && string.IsNullOrEmpty(result.Data.Symbol)) result.Data.Symbol = symbol;
        return result.As(result.Data);
    }

    public Task<RestCallResult<List<BinanceFuturesCoinTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("limit", limit);

        return RequestAsync<List<BinanceFuturesCoinTrade>>(GetUrl(dapi, v1, "trades"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<List<BinanceFuturesCoinTrade>>> GetHistoricalTradesAsync(string symbol, int? limit = null, long? fromId = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("fromId", fromId);

        return RequestAsync<List<BinanceFuturesCoinTrade>>(GetUrl(dapi, v1, "historicalTrades"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<List<BinanceFuturesAggregatedTrade>>> GetAggregatedTradesAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);

        return RequestAsync<List<BinanceFuturesAggregatedTrade>>(GetUrl(dapi, v1, "aggTrades"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<List<BinanceFuturesCoinMarkPrice>>> GetMarkPricesAsync(string? symbol = null, string? pair = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("pair", pair);

        return RequestAsync<List<BinanceFuturesCoinMarkPrice>>(GetUrl(dapi, v1, "premiumIndex"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<List<BinanceFuturesFundingRate>>> GetFundingRatesAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);

        return RequestAsync<List<BinanceFuturesFundingRate>>(GetUrl(dapi, v1, "fundingRate"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceFuturesFundingInfo>>> GetFundingInfoAsync(CancellationToken ct = default)
    {
        return RequestAsync<List<BinanceFuturesFundingInfo>>(GetUrl(dapi, v1, "fundingInfo"), HttpMethod.Get, ct, requestWeight: 0);
    }

    public Task<RestCallResult<List<BinanceFuturesCoinKline>>> GetKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
        };
        parameters.AddEnum("interval", interval);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);

        var weight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        return RequestAsync<List<BinanceFuturesCoinKline>>(GetUrl(dapi, v1, "klines"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<List<BinanceFuturesCoinKline>>> GetContinuousContractKlinesAsync(string pair, BinanceFuturesContractType contractType, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new ParameterCollection
        {
            { "pair", pair }
        };
        parameters.AddEnum("interval", interval);
        parameters.AddEnum("contractType", contractType);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);

        var weight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        return RequestAsync<List<BinanceFuturesCoinKline>>(GetUrl(dapi, v1, "continuousKlines"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<List<BinanceFuturesKline>>> GetIndexPriceKlinesAsync(string pair, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new ParameterCollection
        {
            { "pair", pair }
        };
        parameters.AddEnum("interval", interval);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);

        var weight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        return RequestAsync<List<BinanceFuturesKline>>(GetUrl(dapi, v1, "indexPriceKlines"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<List<BinanceFuturesKline>>> GetMarkPriceKlinesAsync(string symbol, BinanceKlineInterval interval, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
        };

        parameters.AddEnum("interval", interval);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        var weight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        return RequestAsync<List<BinanceFuturesKline>>(GetUrl(dapi, v1, "markPriceKlines"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<List<BinanceFuturesKline>>> GetPremiumIndexKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
        };
        parameters.AddEnum("interval", interval);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);

        var weight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        return RequestAsync<List<BinanceFuturesKline>>(GetUrl(dapi, v1, "premiumIndexKlines"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<List<BinanceFuturesCoinTicker>>> GetTickersAsync(string? symbol = null, string? pair = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("pair", pair);

        var weight = symbol == null ? 40 : 1;
        return RequestAsync<List<BinanceFuturesCoinTicker>>(GetUrl(dapi, v1, "ticker/24hr"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<List<BinanceFuturesCoinPrice>>> GetPricesAsync(string? symbol = null, string? pair = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("pair", pair);

        var weight = symbol == null ? 2 : 1;
        return RequestAsync<List<BinanceFuturesCoinPrice>>(GetUrl(dapi, v1, "ticker/price"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<List<BinanceFuturesCoinBookTicker>>> GetBookPricesAsync(string? symbol = null, string? pair = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("pair", pair);

        var weight = symbol == null ? 5 : 2;
        return RequestAsync<List<BinanceFuturesCoinBookTicker>>(GetUrl(dapi, v1, "ticker/bookTicker"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<BinanceFuturesCoinOpenInterest>> GetOpenInterestAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "symbol", symbol }
        };

        return RequestAsync<BinanceFuturesCoinOpenInterest>(GetUrl(dapi, v1, "openInterest"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceFuturesCoinOpenInterestHistory>>> GetOpenInterestHistoryAsync(string pair, BinanceFuturesContractType contractType, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection
        {
            { "pair", pair }
        };

        parameters.AddEnum("period", period);
        parameters.AddEnum("contractType", contractType);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<List<BinanceFuturesCoinOpenInterestHistory>>(GetUrl("", "", "futures/data/openInterestHist"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceFuturesLongShortRatio>>> GetTopLongShortPositionRatioAsync(string symbolPair, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection
        {
            { "pair", symbolPair }
        };

        parameters.AddEnum("period", period);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<List<BinanceFuturesLongShortRatio>>(GetUrl("", "", "futures/data/topLongShortPositionRatio"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceFuturesLongShortRatio>>> GetTopLongShortAccountRatioAsync(string symbolPair, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection
        {
            { "pair", symbolPair },
        };
        parameters.AddEnum("period", period);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<List<BinanceFuturesLongShortRatio>>(GetUrl("", "", "futures/data/topLongShortAccountRatio"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceFuturesLongShortRatio>>> GetGlobalLongShortAccountRatioAsync(string symbolPair, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection
        {
            { "pair", symbolPair }
        };

        parameters.AddEnum("period", period);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<List<BinanceFuturesLongShortRatio>>(GetUrl("", "", "futures/data/globalLongShortAccountRatio"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceFuturesCoinBuySellVolumeRatio>>> GetTakerBuySellVolumeRatioAsync(string pair, BinanceFuturesContractType contractType, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection
        {
            { "pair", pair }
        };

        parameters.AddEnum("period", period);
        parameters.AddEnum("contractType", contractType);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<List<BinanceFuturesCoinBuySellVolumeRatio>>(GetUrl("", "", "futures/data/takerBuySellVol"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceFuturesBasis>>> GetBasisAsync(string pair, BinanceFuturesContractType contractType, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection
        {
            { "pair", pair }
        };

        parameters.AddEnum("period", period);
        parameters.AddEnum("contractType", contractType);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<List<BinanceFuturesBasis>>(GetUrl("", "", "futures/data/basis"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceFuturesIndexPriceConstituents>> GetIndexPriceConstituentsAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };

        return RequestAsync<BinanceFuturesIndexPriceConstituents>(GetUrl(dapi, v1, "constituents"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 1);
    }
}