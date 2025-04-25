using Binance.Api.Shared.Responses;

namespace Binance.Api.Futures;

internal partial class BinanceFuturesRestClientUsd : IBinanceFuturesRestClientUsd
{
    public async Task<RestCallResult<TimeSpan>> PingAsync(CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        var result = await RequestAsync<object>(GetUrl(fapi, v1, "ping"), HttpMethod.Get, ct).ConfigureAwait(false);
        sw.Stop();

        return result.Success
            ? result.As(sw.Elapsed)
            : result.AsError<TimeSpan>(result.Error!);
    }

    public async Task<RestCallResult<DateTime>> GetTimeAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceServerTime>(GetUrl(fapi, v1, "time"), HttpMethod.Get, ct, ignoreRatelimit: true).ConfigureAwait(false);

        return result.Success
            ? result.As(result.Data?.ServerTime ?? default)
            : result.AsError<DateTime>(result.Error!);
    }

    public async Task<RestCallResult<BinanceFuturesUsdExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceFuturesUsdExchangeInfo>(GetUrl(fapi, v1, "exchangeInfo"), HttpMethod.Get, ct, requestWeight: 1).ConfigureAwait(false);
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

        var requestWeight = limit == null ? 10 : limit <= 50 ? 2 : limit == 100 ? 5 : limit == 500 ? 10 : 20;
        var result = await RequestAsync<BinanceFuturesOrderBook>(GetUrl(fapi, v1, "depth"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
        if (result && string.IsNullOrEmpty(result.Data.Symbol)) result.Data.Symbol = symbol;

        return result.As(result.Data);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesUsdTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("limit", limit);

        return RequestAsync<IEnumerable<BinanceFuturesUsdTrade>>(GetUrl(fapi, v1, "trades"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesUsdTrade>>> GetTradeHistoryAsync(string symbol, int? limit = null, long? fromId = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("fromId", fromId);

        return RequestAsync<IEnumerable<BinanceFuturesUsdTrade>>(GetUrl(fapi, v1, "historicalTrades"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesAggregatedTrade>>> GetAggregatedTradeHistoryAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);

        return RequestAsync<IEnumerable<BinanceFuturesAggregatedTrade>>(GetUrl(fapi, v1, "aggTrades"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesUsdKline>>> GetKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new ParameterCollection {
            { "symbol", symbol },
        };
        parameters.AddEnum("interval", interval);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        return RequestAsync<IEnumerable<BinanceFuturesUsdKline>>(GetUrl(fapi, v1, "klines"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: requestWeight);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesUsdKline>>> GetContinuousContractKlinesAsync(string pair, BinanceFuturesContractType contractType, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new ParameterCollection {
            { "pair", pair },
        };
        parameters.AddEnum("interval", interval);
        parameters.AddEnum("contractType", contractType);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        return RequestAsync<IEnumerable<BinanceFuturesUsdKline>>(GetUrl(fapi, v1, "continuousKlines"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: requestWeight);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesKline>>> GetIndexPriceKlinesAsync(string pair, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new ParameterCollection {
            { "pair", pair },
        };
        parameters.AddEnum("interval", interval);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        return RequestAsync<IEnumerable<BinanceFuturesKline>>(GetUrl(fapi, v1, "indexPriceKlines"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: requestWeight);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesKline>>> GetMarkPriceKlinesAsync(string symbol, BinanceKlineInterval interval, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);

        var parameters = new ParameterCollection {
            { "symbol", symbol },
        };

        parameters.AddEnum("interval", interval);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        return RequestAsync<IEnumerable<BinanceFuturesKline>>(GetUrl(fapi, v1, "markPriceKlines"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: requestWeight);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesKline>>> GetPremiumIndexKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new ParameterCollection {
            { "symbol", symbol },
        };
        parameters.AddEnum("interval", interval);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        return RequestAsync<IEnumerable<BinanceFuturesKline>>(GetUrl(fapi, v1, "premiumIndexKlines"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: requestWeight);
    }

    public Task<RestCallResult<BinanceFuturesMarkPrice>> GetMarkPriceAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);

        return RequestAsync<BinanceFuturesMarkPrice>(GetUrl(fapi, v1, "premiumIndex"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesMarkPrice>>> GetMarkPricesAsync(CancellationToken ct = default)
    {
        return RequestAsync<IEnumerable<BinanceFuturesMarkPrice>>(GetUrl(fapi, v1, "premiumIndex"), HttpMethod.Get, ct, requestWeight: 10);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesFundingRate>>> GetFundingRatesAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        var parameters = new ParameterCollection {
            { "symbol", symbol }
        };
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);

        return RequestAsync<IEnumerable<BinanceFuturesFundingRate>>(GetUrl(fapi, v1, "fundingRate"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesFundingInfo>>> GetFundingInfoAsync(CancellationToken ct = default)
    {
        return RequestAsync<IEnumerable<BinanceFuturesFundingInfo>>(GetUrl(fapi, v1, "fundingInfo"), HttpMethod.Get, ct, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceFuturesUsdTicker>> GetTickerAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);

        return RequestAsync<BinanceFuturesUsdTicker>(GetUrl(fapi, v1, "ticker/24hr"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesUsdTicker>>> GetTickersAsync(CancellationToken ct = default)
    {
        return RequestAsync<IEnumerable<BinanceFuturesUsdTicker>>(GetUrl(fapi, v1, "ticker/24hr"), HttpMethod.Get, ct, requestWeight: 40);
    }

    public Task<RestCallResult<BinanceFuturesPrice>> GetPriceAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };

        return RequestAsync<BinanceFuturesPrice>(GetUrl(fapi, v2, "ticker/price"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesPrice>>> GetPricesAsync(CancellationToken ct = default)
    {
        return RequestAsync<IEnumerable<BinanceFuturesPrice>>(GetUrl(fapi, v2, "ticker/price"), HttpMethod.Get, ct, requestWeight: 2);
    }

    public Task<RestCallResult<BinanceFuturesBookTicker>> GetBookPriceAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);

        return RequestAsync<BinanceFuturesBookTicker>(GetUrl(fapi, v1, "ticker/bookTicker"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesBookTicker>>> GetBookPricesAsync(CancellationToken ct = default)
    {
        return RequestAsync<IEnumerable<BinanceFuturesBookTicker>>(GetUrl(fapi, v1, "ticker/bookTicker"), HttpMethod.Get, ct, requestWeight: 5);
    }

    // TODO: Quarterly Contract Settlement Price

    public Task<RestCallResult<BinanceFuturesOpenInterest>> GetOpenInterestAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "symbol", symbol }
        };

        return RequestAsync<BinanceFuturesOpenInterest>(GetUrl(fapi, v1, "openInterest"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesOpenInterestHistory>>> GetOpenInterestHistoryAsync(string symbol, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection {
            { "symbol", symbol },
        };

        parameters.AddEnum("period", period);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<IEnumerable<BinanceFuturesOpenInterestHistory>>(GetUrl(fapi, "", "futures/data/openInterestHist"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 1000);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesLongShortRatio>>> GetTopLongShortPositionRatioAsync(string symbol, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection {
            { "symbol", symbol },
        };

        parameters.AddEnum("period", period);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<IEnumerable<BinanceFuturesLongShortRatio>>(GetUrl(fapi, "", "futures/data/topLongShortPositionRatio"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 1000);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesLongShortRatio>>> GetTopLongShortAccountRatioAsync(string symbol, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection {
            { "symbol", symbol },
        };

        parameters.AddEnum("period", period);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<IEnumerable<BinanceFuturesLongShortRatio>>(GetUrl(fapi, "", "futures/data/topLongShortAccountRatio"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 1000);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesLongShortRatio>>> GetGlobalLongShortAccountRatioAsync(string symbol, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection {
            { "symbol", symbol },
        };

        parameters.AddEnum("period", period);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<IEnumerable<BinanceFuturesLongShortRatio>>(GetUrl(fapi, "", "futures/data/globalLongShortAccountRatio"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesBuySellVolumeRatio>>> GetTakerBuySellVolumeRatioAsync(string symbol, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection {
            { "symbol", symbol },
        };

        parameters.AddEnum("period", period);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<IEnumerable<BinanceFuturesBuySellVolumeRatio>>(GetUrl(fapi, "", "futures/data/takerlongshortRatio"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesBasis>>> GetBasisAsync(string pair, BinanceFuturesContractType contractType, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "pair", pair }
        };
        parameters.AddEnum("contractType", contractType);
        parameters.AddEnum("period", period);
        parameters.AddOptional("limit", limit ?? 30);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<IEnumerable<BinanceFuturesBasis>>(GetUrl(fapi, "", "futures/data/basis"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesCompositeIndexInfo>>> GetCompositeIndexInfoAsync(string? symbol = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);

        var weight = symbol == null ? 10 : 1;
        return RequestAsync<IEnumerable<BinanceFuturesCompositeIndexInfo>>(GetUrl(fapi, v1, "indexInfo"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<BinanceFuturesAssetIndex>> GetAssetIndexAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "symbol", symbol }
        };

        return RequestAsync<BinanceFuturesAssetIndex>(GetUrl(fapi, v1, "assetIndex"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesAssetIndex>>> GetAssetIndexesAsync(CancellationToken ct = default)
    {
        return RequestAsync<IEnumerable<BinanceFuturesAssetIndex>>(GetUrl(fapi, v1, "assetIndex"), HttpMethod.Get, ct, requestWeight: 1);
    }
}