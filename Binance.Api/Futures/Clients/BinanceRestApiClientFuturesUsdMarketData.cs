using ApiSharp;
using Binance.Api.Spot;
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

    #region Exchange Information

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesUsdtExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceFuturesUsdtExchangeInfo>(GetUrl(fapi, v1, "exchangeInfo"), HttpMethod.Get, ct, requestWeight: 1).ConfigureAwait(false);
        if (!result) return result;

        _baseClient._exchangeInfo = exchangeInfoResult.Data;
        _baseClient._lastExchangeInfoUpdate = DateTime.UtcNow;
        _logger.Log(LogLevel.Information, "Trade rules updated");
        return exchangeInfoResult;
    }

    #endregion

    #region Order Book

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntValues(nameof(limit), 5, 10, 20, 50, 100, 500, 1000);
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var requestWeight = limit == null ? 10 : limit <= 50 ? 2 : limit == 100 ? 5 : limit == 500 ? 10 : 20;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/depth", BinanceExchange.RateLimiter.FuturesRest, requestWeight);
        var result = await _baseClient.SendAsync<BinanceFuturesOrderBook>(request, parameters, ct, requestWeight).ConfigureAwait(false);
        if (result && string.IsNullOrEmpty(result.Data.Symbol))
            result.Data.Symbol = symbol;
        return result.As(result.Data);
    }

    #endregion



    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<IBinanceRecentTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/trades", BinanceExchange.RateLimiter.FuturesRest, 5);
        var result = await _baseClient.SendAsync<IEnumerable<BinanceRecentTradeQuote>>(request, parameters, ct).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceRecentTrade>>(result.Data);
    }

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<IBinanceRecentTrade>>> GetTradeHistoryAsync(string symbol, int? limit = null, long? fromId = null,
        CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/historicalTrades", BinanceExchange.RateLimiter.FuturesRest, 20);
        var result = await _baseClient.SendAsync<IEnumerable<BinanceRecentTradeQuote>>(request, parameters, ct).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceRecentTrade>>(result.Data);
    }

    #region Compressed/Aggregate Trades List

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceAggregatedTrade>>> GetAggregatedTradeHistoryAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/aggTrades", BinanceExchange.RateLimiter.FuturesRest, 20);
        return await _baseClient.SendAsync<IEnumerable<BinanceAggregatedTrade>>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    #region Kline/Candlestick Data

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<IBinanceKline>>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new ParameterCollection {
            { "symbol", symbol },
        };
        parameters.AddEnum("interval", interval);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/klines", BinanceExchange.RateLimiter.FuturesRest, requestWeight);
        var result = await _baseClient.SendAsync<IEnumerable<BinanceFuturesUsdtKline>>(request, parameters, ct, requestWeight).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceKline>>(result.Data);
    }

    #endregion

    #region Continuous contract Kline Data

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<IBinanceKline>>> GetContinuousContractKlinesAsync(string pair, ContractType contractType, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new ParameterCollection {
            { "pair", pair },
        };
        parameters.AddEnum("interval", interval);
        parameters.AddEnum("contractType", contractType);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/continuousKlines", BinanceExchange.RateLimiter.FuturesRest, requestWeight);
        var result = await _baseClient.SendAsync<IEnumerable<BinanceFuturesUsdtKline>>(request, parameters, ct, requestWeight).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceKline>>(result.Data);
    }

    #endregion

    #region Index Price Kline Data

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<IBinanceKline>>> GetIndexPriceKlinesAsync(string pair, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new ParameterCollection {
            { "pair", pair },
        };
        parameters.AddEnum("interval", interval);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/indexPriceKlines", BinanceExchange.RateLimiter.FuturesRest, requestWeight);
        var result = await _baseClient.SendAsync<IEnumerable<BinanceFuturesUsdtKline>>(request, parameters, ct, requestWeight).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceKline>>(result.Data);
    }

    #endregion

    #region Mark Price Kline/Candlestick Data

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceMarkIndexKline>>> GetMarkPriceKlinesAsync(string symbol, KlineInterval interval, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);

        var parameters = new ParameterCollection {
            { "symbol", symbol },
        };

        parameters.AddEnum("interval", interval);
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/markPriceKlines", BinanceExchange.RateLimiter.FuturesRest, requestWeight);
        return await _baseClient.SendAsync<IEnumerable<BinanceMarkIndexKline>>(request, parameters, ct, requestWeight).ConfigureAwait(false);
    }

    #endregion

    #region Kline/Premium Index

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceMarkIndexKline>>> GetPremiumIndexKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new ParameterCollection {
            { "symbol", symbol },
        };
        parameters.AddEnum("interval", interval);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/premiumIndexKlines", BinanceExchange.RateLimiter.FuturesRest, requestWeight);
        return await _baseClient.SendAsync<IEnumerable<BinanceMarkIndexKline>>(request, parameters, ct, requestWeight).ConfigureAwait(false);
    }

    #endregion

    #region Mark Price

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesMarkPrice>> GetMarkPriceAsync(string symbol,
        CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("symbol", symbol);

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/premiumIndex", BinanceExchange.RateLimiter.FuturesRest, 1);
        return await _baseClient.SendAsync<BinanceFuturesMarkPrice>(request, parameters, ct).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceFuturesMarkPrice>>> GetMarkPricesAsync(CancellationToken ct = default)
    {
        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/premiumIndex", BinanceExchange.RateLimiter.FuturesRest, 10);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesMarkPrice>>(request, null, ct).ConfigureAwait(false);
    }
    #endregion

    #region Get Funding Rate History

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceFuturesFundingRateHistory>>> GetFundingRatesAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        var parameters = new ParameterCollection {
            { "symbol", symbol }
        };
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/fundingRate", BinanceExchange.RateLimiter.EndpointLimit, 1, false,
            limitGuard: new SingleLimitGuard(500, TimeSpan.FromMinutes(5), RateLimitWindowType.Sliding));
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesFundingRateHistory>>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion



    #region Get Funding Info

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceFuturesFundingInfo>>> GetFundingInfoAsync(CancellationToken ct = default)
    {
        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/fundingInfo", BinanceExchange.RateLimiter.FuturesRest, 0);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesFundingInfo>>(request, null, ct).ConfigureAwait(false);
    }

    #endregion

    #region 24h statistics
    /// <inheritdoc />
    public async Task<RestCallResult<IBinance24HPrice>> GetTickerAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("symbol", symbol);

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
        parameters.AddOptionalParameter("symbol", symbol);

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
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
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
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
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
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
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
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
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
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
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
        parameters.AddOptionalParameter("symbol", symbol);

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