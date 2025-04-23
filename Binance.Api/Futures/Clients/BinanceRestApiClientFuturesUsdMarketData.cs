using Binance.Api.Futures.Responses;
using Binance.Net.Objects.Models.Futures;

namespace Binance.Api.Futures;

internal partial class BinanceRestApiClientFuturesUsd
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

    public async Task<RestCallResult<BinanceFuturesUsdtExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceFuturesUsdtExchangeInfo>(GetUrl(fapi, v1, "exchangeInfo"), HttpMethod.Get, ct, requestWeight: 1).ConfigureAwait(false);
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

    public Task<RestCallResult<IEnumerable<BinanceFuturesTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("limit", limit);

        return RequestAsync<IEnumerable<BinanceFuturesTrade>>(GetUrl(fapi, v1, "trades"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesTrade>>> GetTradeHistoryAsync(string symbol, int? limit = null, long? fromId = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("fromId", fromId);

        return RequestAsync<IEnumerable<BinanceFuturesTrade>>(GetUrl(fapi, v1, "historicalTrades"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 20);
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

    public Task<RestCallResult<IEnumerable<BinanceFuturesKline>>> GetKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
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
        return RequestAsync<IEnumerable<BinanceFuturesKline>>(GetUrl(fapi, v1, "klines"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: requestWeight);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesKline>>> GetContinuousContractKlinesAsync(string pair, ContractType contractType, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
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
        return RequestAsync<IEnumerable<BinanceFuturesKline>>(GetUrl(fapi, v1, "continuousKlines"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: requestWeight);
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

    public Task<RestCallResult<IEnumerable<BinanceFuturesMarkPriceKline>>> GetMarkPriceKlinesAsync(string symbol, BinanceKlineInterval interval, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
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
        return RequestAsync<IEnumerable<BinanceFuturesMarkPriceKline>>(GetUrl(fapi, v1, "markPriceKlines"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: requestWeight);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesPremiumKline>>> GetPremiumIndexKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
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
        return RequestAsync<IEnumerable<BinanceFuturesPremiumKline>>(GetUrl(fapi, v1, "premiumIndexKlines"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: requestWeight);
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


    #region 24h statistics
    /// <inheritdoc />
    public async Task<RestCallResult<IBinance24HPrice>> GetTickerAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/ticker/24hr", BinanceExchange.RateLimiter.FuturesRest, 1);
        var result = await _baseClient.SendAsync<Binance24HPrice>(request, parameters, ct).ConfigureAwait(false);
        return result.As<IBinance24HPrice>(result.Data);
    }

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<IBinance24HPrice>>> GetTickersAsync(CancellationToken ct = default)
    {
        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/ticker/24hr", BinanceExchange.RateLimiter.FuturesRest, 40);
        var result = await _baseClient.SendAsync<IEnumerable<Binance24HPrice>>(request, null, ct).ConfigureAwait(false);
        return result.As<IEnumerable<IBinance24HPrice>>(result.Data);
    }
    #endregion

    #region Symbol Order Book Ticker



    #region Get price

    /// <inheritdoc />
    public async Task<RestCallResult<BinancePrice>> GetPriceAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v2/ticker/price", BinanceExchange.RateLimiter.FuturesRest, 1);
        return await _baseClient.SendAsync<BinancePrice>(request, parameters, ct, 1).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinancePrice>>> GetPricesAsync(CancellationToken ct = default)
    {
        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v2/ticker/price", BinanceExchange.RateLimiter.FuturesRest, 2);
        return await _baseClient.SendAsync<IEnumerable<BinancePrice>>(request, null, ct, 2).ConfigureAwait(false);
    }
    #endregion

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceBookPrice>> GetBookPriceAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/ticker/bookTicker", BinanceExchange.RateLimiter.FuturesRest, 2);
        return await _baseClient.SendAsync<BinanceBookPrice>(request, parameters, ct).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceBookPrice>>> GetBookPricesAsync(CancellationToken ct = default)
    {
        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/ticker/bookTicker", BinanceExchange.RateLimiter.FuturesRest, 5);
        return await _baseClient.SendAsync<IEnumerable<BinanceBookPrice>>(request, null, ct).ConfigureAwait(false);
    }


    // TODO: Quarterly Contract Settlement Price

    #region Open Interest

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesOpenInterest>> GetOpenInterestAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "symbol", symbol }
        };

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/openInterest", BinanceExchange.RateLimiter.FuturesRest, 1);
        return await _baseClient.SendAsync<BinanceFuturesOpenInterest>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    #region Open Interest History

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceFuturesOpenInterestHistory>>> GetOpenInterestHistoryAsync(string symbol, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection {
            { "symbol", symbol },
        };

        parameters.AddEnum("period", period);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        var request = _definitions.GetOrCreate(HttpMethod.Get, "futures/data/openInterestHist", BinanceExchange.RateLimiter.EndpointLimit, 1, false,
            limitGuard: new SingleLimitGuard(1000, TimeSpan.FromMinutes(5), RateLimitWindowType.Sliding));
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesOpenInterestHistory>>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    #region Top Trader Long/Short Ratio (Positions)

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceFuturesLongShortRatio>>> GetTopLongShortPositionRatioAsync(string symbolPair, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection {
            { "symbol", symbolPair },
        };

        parameters.AddEnum("period", period);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        var request = _definitions.GetOrCreate(HttpMethod.Get, "futures/data/topLongShortPositionRatio", BinanceExchange.RateLimiter.EndpointLimit, 1, false,
            limitGuard: new SingleLimitGuard(1000, TimeSpan.FromMinutes(5), RateLimitWindowType.Sliding));
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesLongShortRatio>>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion



    #region Top Trader Long/Short Ratio (Accounts)

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceFuturesLongShortRatio>>> GetTopLongShortAccountRatioAsync(string symbolPair, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection {
            { "symbol", symbolPair },
        };

        parameters.AddEnum("period", period);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        var request = _definitions.GetOrCreate(HttpMethod.Get, "futures/data/topLongShortAccountRatio", BinanceExchange.RateLimiter.EndpointLimit, 1, false,
            limitGuard: new SingleLimitGuard(1000, TimeSpan.FromMinutes(5), RateLimitWindowType.Sliding));
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesLongShortRatio>>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion



    #region Global Long/Short Ratio (Accounts)

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceFuturesLongShortRatio>>> GetGlobalLongShortAccountRatioAsync(string symbolPair, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection {
            { "symbol", symbolPair },
        };

        parameters.AddEnum("period", period);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        var request = _definitions.GetOrCreate(HttpMethod.Get, "futures/data/globalLongShortAccountRatio", BinanceExchange.RateLimiter.EndpointLimit, 1, false,
            limitGuard: new SingleLimitGuard(1000, TimeSpan.FromMinutes(5), RateLimitWindowType.Sliding));
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesLongShortRatio>>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    #endregion

    #region Taker Buy/Sell Volume Ratio

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceFuturesBuySellVolumeRatio>>> GetTakerBuySellVolumeRatioAsync(string symbol, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection {
            { "symbol", symbol },
        };

        parameters.AddEnum("period", period);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        var request = _definitions.GetOrCreate(HttpMethod.Get, "futures/data/takerlongshortRatio", BinanceExchange.RateLimiter.EndpointLimit, 1, false,
            limitGuard: new SingleLimitGuard(1000, TimeSpan.FromMinutes(5), RateLimitWindowType.Sliding));
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesBuySellVolumeRatio>>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion


    #region Get Basis

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceFuturesBasis>>> GetBasisAsync(string symbol, ContractType contractType, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "pair", symbol }
        };
        parameters.AddEnum("contractType", contractType);
        parameters.AddEnum("period", period);
        parameters.AddOptional("limit", limit ?? 30);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        var request = _definitions.GetOrCreate(HttpMethod.Get, "futures/data/basis", BinanceExchange.RateLimiter.FuturesRest, 0);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesBasis>>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion







    #region Composite index symbol information

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceFuturesCompositeIndexInfo>>> GetCompositeIndexInfoAsync(string? symbol = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);

        var weight = symbol == null ? 10 : 1;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/indexInfo", BinanceExchange.RateLimiter.FuturesRest, weight);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesCompositeIndexInfo>>(request, parameters, ct, weight).ConfigureAwait(false);
    }

    #endregion

    #region Asset index

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceFuturesAssetIndex>>> GetAssetIndexesAsync(CancellationToken ct = default)
    {
        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/assetIndex", BinanceExchange.RateLimiter.FuturesRest, 10);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesAssetIndex>>(request, null, ct).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesAssetIndex>> GetAssetIndexAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "symbol", symbol }
        };

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/assetIndex", BinanceExchange.RateLimiter.FuturesRest, 1);
        return await _baseClient.SendAsync<BinanceFuturesAssetIndex>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion


    // TODO: Query Index Price Constituents
    // TODO: Query Insurance Fund Balance Snapshot
}