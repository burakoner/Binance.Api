using Binance.Net.Objects.Models.Futures;

namespace Binance.Api.Futures;

internal partial class BinanceRestApiClientFuturesCoin
{
    /*
    
    public async Task<RestCallResult<long>> PingAsync(CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/ping", BinanceExchange.RateLimiter.FuturesRest, 1);
        var result = await _baseClient.SendAsync<object>(request, null, ct).ConfigureAwait(false);
        sw.Stop();
        return result ? result.As(sw.ElapsedMilliseconds) : result.As<long>(default!);
    }

    public async Task<RestCallResult<DateTime>> GetServerTimeAsync(bool resetAutoTimestamp = false, CancellationToken ct = default)
    {
        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/time", BinanceExchange.RateLimiter.FuturesRest, 1);
        var result = await _baseClient.SendAsync<BinanceCheckTime>(request, null, ct).ConfigureAwait(false);
        return result.As(result.Data?.ServerTime ?? default);
    }

    public async Task<RestCallResult<BinanceFuturesCoinExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
    {
        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/exchangeInfo", BinanceExchange.RateLimiter.FuturesRest, 1);
        var exchangeInfoResult = await _baseClient.SendAsync<BinanceFuturesCoinExchangeInfo>(request, null, ct).ConfigureAwait(false);
        if (!exchangeInfoResult)
            return exchangeInfoResult;

        _baseClient._exchangeInfo = exchangeInfoResult.Data;
        _baseClient._lastExchangeInfoUpdate = DateTime.UtcNow;
        _logger.Log(LogLevel.Information, "Trade rules updated");
        return exchangeInfoResult;
    }

    public async Task<RestCallResult<BinanceFuturesOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntValues(nameof(limit), 5, 10, 20, 50, 100, 500, 1000);
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var requestWeight = limit == null ? 10 : limit <= 50 ? 2 : limit == 100 ? 5 : limit == 500 ? 10 : 20;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/depth", BinanceExchange.RateLimiter.FuturesRest, requestWeight);
        var result = await _baseClient.SendAsync<BinanceFuturesOrderBook>(request, parameters, ct, requestWeight).ConfigureAwait(false);
        if (result && string.IsNullOrEmpty(result.Data.Symbol))
            result.Data.Symbol = symbol;
        return result.As(result.Data);
    }

    public async Task<RestCallResult<IEnumerable<IBinanceRecentTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/trades", BinanceExchange.RateLimiter.FuturesRest, 5);
        var result = await _baseClient.SendAsync<IEnumerable<BinanceRecentTradeBase>>(request, parameters, ct).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceRecentTrade>>(result.Data);
    }

    public async Task<RestCallResult<IEnumerable<IBinanceRecentTrade>>> GetTradeHistoryAsync(string symbol, int? limit = null, long? fromId = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/historicalTrades", BinanceExchange.RateLimiter.FuturesRest, 20);
        var result = await _baseClient.SendAsync<IEnumerable<BinanceRecentTradeBase>>(request, parameters, ct).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceRecentTrade>>(result.Data);
    }

    public async Task<RestCallResult<IEnumerable<BinanceAggregatedTrade>>> GetAggregatedTradeHistoryAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/aggTrades", BinanceExchange.RateLimiter.FuturesRest, 20);
        return await _baseClient.SendAsync<IEnumerable<BinanceAggregatedTrade>>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesCoinMarkPrice>>> GetMarkPricesAsync(string? symbol = null, string? pair = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("pair", pair);

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/premiumIndex", BinanceExchange.RateLimiter.FuturesRest, 10);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesCoinMarkPrice>>(request, parameters, ct).ConfigureAwait(false);

    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesFundingRateHistory>>> GetFundingRatesAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        var parameters = new ParameterCollection {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/fundingRate", BinanceExchange.RateLimiter.FuturesRest, 1);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesFundingRateHistory>>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesFundingInfo>>> GetFundingInfoAsync(CancellationToken ct = default)
    {
        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/fundingInfo", BinanceExchange.RateLimiter.FuturesRest, 0);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesFundingInfo>>(request, null, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<IBinanceKline>>> GetKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new ParameterCollection {
                { "symbol", symbol },
            };
        parameters.AddEnum("interval", interval);
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/klines", BinanceExchange.RateLimiter.FuturesRest, requestWeight);
        var result = await _baseClient.SendAsync<IEnumerable<BinanceFuturesCoinKline>>(request, parameters, ct, requestWeight).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceKline>>(result.Data);
    }

    public async Task<RestCallResult<IEnumerable<IBinanceKline>>> GetContinuousContractKlinesAsync(string pair, BinanceContractType contractType, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new ParameterCollection {
                { "pair", pair }
            };
        parameters.AddEnum("interval", interval);
        parameters.AddEnum("contractType", contractType);
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/continuousKlines", BinanceExchange.RateLimiter.FuturesRest, requestWeight);
        var result = await _baseClient.SendAsync<IEnumerable<BinanceFuturesCoinKline>>(request, parameters, ct, requestWeight).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceKline>>(result.Data);
    }

    public async Task<RestCallResult<IEnumerable<BinanceMarkIndexKline>>> GetIndexPriceKlinesAsync(string pair, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new ParameterCollection {
                { "pair", pair }
            };
        parameters.AddEnum("interval", interval);
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/indexPriceKlines", BinanceExchange.RateLimiter.FuturesRest, requestWeight);
        return await _baseClient.SendAsync<IEnumerable<BinanceMarkIndexKline>>(request, parameters, ct, requestWeight).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceMarkIndexKline>>> GetMarkPriceKlinesAsync(string symbol, BinanceKlineInterval interval, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);

        var parameters = new ParameterCollection {
                { "symbol", symbol },
            };

        parameters.AddEnum("interval", interval);
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/markPriceKlines", BinanceExchange.RateLimiter.FuturesRest, requestWeight);
        return await _baseClient.SendAsync<IEnumerable<BinanceMarkIndexKline>>(request, parameters, ct, requestWeight).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceMarkIndexKline>>> GetPremiumIndexKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new ParameterCollection {
                { "symbol", symbol },
            };
        parameters.AddEnum("interval", interval);
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/premiumIndexKlines", BinanceExchange.RateLimiter.FuturesRest, requestWeight);
        return await _baseClient.SendAsync<IEnumerable<BinanceMarkIndexKline>>(request, parameters, ct, requestWeight).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<IBinance24HPrice>>> GetTickersAsync(string? symbol = null, string? pair = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("pair", pair);

        var requestWeight = symbol == null ? 40 : 1;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/ticker/24hr", BinanceExchange.RateLimiter.FuturesRest, requestWeight);
        var result = await _baseClient.SendAsync<IEnumerable<BinanceFuturesCoin24HPrice>>(request, parameters, ct, requestWeight).ConfigureAwait(false);
        return result.As<IEnumerable<IBinance24HPrice>>(result.Success ? result.Data : null);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesCoinPrice>>> GetPricesAsync(string? symbol = null, string? pair = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("pair", pair);

        var weight = symbol == null ? 2 : 1;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/ticker/price", BinanceExchange.RateLimiter.FuturesRest, weight);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesCoinPrice>>(request, parameters, ct, weight).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesBookPrice>>> GetBookPricesAsync(string? symbol = null, string? pair = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("pair", pair);

        var requestWeight = symbol == null ? 5 : 2;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/ticker/bookTicker", BinanceExchange.RateLimiter.FuturesRest, requestWeight);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesBookPrice>>(request, parameters, ct, requestWeight).ConfigureAwait(false);
    }

    public async Task<RestCallResult<BinanceFuturesCoinOpenInterest>> GetOpenInterestAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
            {
                { "symbol", symbol }
            };

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/openInterest", BinanceExchange.RateLimiter.FuturesRest, 1);
        return await _baseClient.SendAsync<BinanceFuturesCoinOpenInterest>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesCoinOpenInterestHistory>>> GetOpenInterestHistoryAsync(string pair, BinanceContractType contractType, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection {
                { "pair", pair }
            };

        parameters.AddEnum("period", period);
        parameters.AddEnum("contractType", contractType);
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "futures/data/openInterestHist", BinanceExchange.RateLimiter.EndpointLimit, 1, false,
            limitGuard: new SingleLimitGuard(1000, TimeSpan.FromMinutes(5), RateLimitWindowType.Sliding));
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesCoinOpenInterestHistory>>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesLongShortRatio>>> GetTopLongShortPositionRatioAsync(string symbolPair, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection {
                { "pair", symbolPair }
            };

        parameters.AddEnum("period", period);
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "futures/data/topLongShortPositionRatio", BinanceExchange.RateLimiter.EndpointLimit, 1, false,
            limitGuard: new SingleLimitGuard(1000, TimeSpan.FromMinutes(5), RateLimitWindowType.Sliding));
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesLongShortRatio>>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesLongShortRatio>>> GetTopLongShortAccountRatioAsync(string symbolPair, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection {
                { "pair", symbolPair },
            };
        parameters.AddEnum("period", period);
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "futures/data/topLongShortAccountRatio", BinanceExchange.RateLimiter.EndpointLimit, 1, false,
            limitGuard: new SingleLimitGuard(1000, TimeSpan.FromMinutes(5), RateLimitWindowType.Sliding));
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesLongShortRatio>>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesLongShortRatio>>> GetGlobalLongShortAccountRatioAsync(string symbolPair, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection {
                { "pair", symbolPair }
            };

        parameters.AddEnum("period", period);
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "futures/data/globalLongShortAccountRatio", BinanceExchange.RateLimiter.EndpointLimit, 1, false,
            limitGuard: new SingleLimitGuard(1000, TimeSpan.FromMinutes(5), RateLimitWindowType.Sliding));
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesLongShortRatio>>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesCoinBuySellVolumeRatio>>> GetTakerBuySellVolumeRatioAsync(string pair, BinanceContractType contractType, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection {
                { "pair", pair }
            };

        parameters.AddEnum("period", period);
        parameters.AddEnum("contractType", contractType);
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "futures/data/takerBuySellVol", BinanceExchange.RateLimiter.EndpointLimit, 1, false,
            limitGuard: new SingleLimitGuard(1000, TimeSpan.FromMinutes(5), RateLimitWindowType.Sliding));
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesCoinBuySellVolumeRatio>>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesBasis>>> GetBasisAsync(string pair, BinanceContractType contractType, BinancePeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection {
                { "pair", pair }
            };

        parameters.AddEnum("period", period);
        parameters.AddEnum("contractType", contractType);
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "futures/data/basis", BinanceExchange.RateLimiter.FuturesRest, 1);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesBasis>>(request, parameters, ct).ConfigureAwait(false);
    }

    // TODO: Query Index Price Constituents

    // -------------------------------------
    // user data stream



    public async Task<RestCallResult<string>> StartUserStreamAsync(CancellationToken ct = default)
    {
        var request = _definitions.GetOrCreate(HttpMethod.Post, "dapi/v1/listenKey", BinanceExchange.RateLimiter.FuturesRest, 1);
        var result = await _baseClient.SendAsync<BinanceListenKey>(request, null, ct).ConfigureAwait(false);
        return result.As(result.Data?.ListenKey!);
    }

    public async Task<WebCallResult> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));

        var parameters = new ParameterCollection
            {
                { "listenKey", listenKey }
            };

        var request = _definitions.GetOrCreate(HttpMethod.Put, "dapi/v1/listenKey", BinanceExchange.RateLimiter.FuturesRest, 1);
        return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<WebCallResult> StopUserStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new ParameterCollection
            {
                { "listenKey", listenKey }
            };

        var request = _definitions.GetOrCreate(HttpMethod.Delete, "dapi/v1/listenKey", BinanceExchange.RateLimiter.FuturesRest, 1);
        return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
    }



    // -----------------------------------------
    // Account


    public async Task<RestCallResult<IEnumerable<BinanceCoinFuturesAccountBalance>>> GetBalancesAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/balance", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<IEnumerable<BinanceCoinFuturesAccountBalance>>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<BinanceFuturesAccountUserCommissionRate>> GetUserCommissionRateAsync(string symbol, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
            {
                { "symbol", symbol}
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/commissionRate", BinanceExchange.RateLimiter.FuturesRest, 20, true);
        return await _baseClient.SendAsync<BinanceFuturesAccountUserCommissionRate>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<BinanceFuturesCoinAccountInfo>> GetAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/account", BinanceExchange.RateLimiter.FuturesRest, 5, true);
        return await _baseClient.SendAsync<BinanceFuturesCoinAccountInfo>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesSymbolBracket>>> GetBracketsAsync(string? symbolOrPair = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("pair", symbolOrPair);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "/dapi/v2/leverageBracket", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesSymbolBracket>>(request, parameters, ct).ConfigureAwait(false);
    }

    // TODO: Notional Bracket for Pair(USER_DATA)

    public async Task<RestCallResult<BinanceFuturesPositionMode>> GetPositionModeAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/positionSide/dual", BinanceExchange.RateLimiter.FuturesRest, 30, true);
        return await _baseClient.SendAsync<BinanceFuturesPositionMode>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<BinanceResult>> ModifyPositionModeAsync(bool dualPositionSide, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
            {
                { "dualSidePosition", dualPositionSide.ToString().ToLower() }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Post, "dapi/v1/positionSide/dual", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<BinanceResult>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesIncomeHistory>>> GetIncomeHistoryAsync(string? symbol = null, string? incomeType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("incomeType", incomeType);
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/income", BinanceExchange.RateLimiter.FuturesRest, 20, true);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesIncomeHistory>>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForTransactionHistoryAsync(DateTime startTime, DateTime endTime, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
            {
                { "startTime", DateTimeConverter.ConvertToMilliseconds(startTime) },
                { "endTime", DateTimeConverter.ConvertToMilliseconds(endTime) },
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/income/asyn", BinanceExchange.RateLimiter.FuturesRest, 5, true);
        return await _baseClient.SendAsync<BinanceFuturesDownloadIdInfo>(request, parameters, ct).ConfigureAwait(false);
    }
    
    public async Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForTransactionHistoryAsync(string downloadId, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
            {
                { "downloadId", downloadId }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/income/asyn/id", BinanceExchange.RateLimiter.FuturesRest, 5, true);
        return await _baseClient.SendAsync<BinanceFuturesDownloadLink>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForOrderHistoryAsync(DateTime startTime, DateTime endTime, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
            {
                { "startTime", DateTimeConverter.ConvertToMilliseconds(startTime) },
                { "endTime", DateTimeConverter.ConvertToMilliseconds(endTime) },
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/order/asyn", BinanceExchange.RateLimiter.FuturesRest, 5, true);
        return await _baseClient.SendAsync<BinanceFuturesDownloadIdInfo>(request, parameters, ct).ConfigureAwait(false);
    }
    
    public async Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForOrderHistoryAsync(string downloadId, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
            {
                { "downloadId", downloadId }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/order/asyn/id", BinanceExchange.RateLimiter.FuturesRest, 5, true);
        return await _baseClient.SendAsync<BinanceFuturesDownloadLink>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForTradeHistoryAsync(DateTime startTime, DateTime endTime, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
            {
                { "startTime", DateTimeConverter.ConvertToMilliseconds(startTime) },
                { "endTime", DateTimeConverter.ConvertToMilliseconds(endTime) },
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/trade/asyn", BinanceExchange.RateLimiter.FuturesRest, 5, true);
        return await _baseClient.SendAsync<BinanceFuturesDownloadIdInfo>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForTradeHistoryAsync(string downloadId, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
            {
                { "downloadId", downloadId }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/trade/asyn/id", BinanceExchange.RateLimiter.FuturesRest, 5, true);
        return await _baseClient.SendAsync<BinanceFuturesDownloadLink>(request, parameters, ct).ConfigureAwait(false);
    }

    // -------------------------------------------

    // Portfolio Margin
    // TODO: Classic Portfolio Margin Account Information(USER_DATA)



    // ---------------------------------

    // Trade


    public async Task<RestCallResult<BinanceFuturesOrder>> PlaceOrderAsync(
        string symbol,
        OrderSide side,
        FuturesOrderType type,
        decimal? quantity,
        decimal? price = null,
        PositionSide? positionSide = null,
        TimeInForce? timeInForce = null,
        bool? reduceOnly = null,
        string? newClientOrderId = null,
        decimal? stopPrice = null,
        decimal? activationPrice = null,
        decimal? callbackRate = null,
        WorkingType? workingType = null,
        bool? closePosition = null,
        OrderResponseType? orderResponseType = null,
        bool? priceProtect = null,
        PriceMatch? priceMatch = null,
        SelfTradePreventionMode? selfTradePreventionMode = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (closePosition == true && positionSide != null)
        {
            if (positionSide == Enums.PositionSide.Short && side == Enums.OrderSide.Sell)
                throw new ArgumentException("Can't close short position with order side sell");
            if (positionSide == Enums.PositionSide.Long && side == Enums.OrderSide.Buy)
                throw new ArgumentException("Can't close long position with order side buy");
        }

        if (orderResponseType == OrderResponseType.Full)
            throw new ArgumentException("OrderResponseType.Full is not supported in Futures");

        var rulesCheck = await _baseClient.CheckTradeRules(symbol, quantity, null, price, stopPrice, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            _logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new WebCallResult<BinanceFuturesOrder>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;

        var clientOrderId = LibraryHelpers.ApplyBrokerId(newClientOrderId, BinanceExchange.ClientOrderIdFutures, 36, _baseClient.ClientOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection()
            {
                { "symbol", symbol },
            };

        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddOptionalParameter("quantity", quantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("newClientOrderId", clientOrderId);
        parameters.AddOptionalParameter("price", price?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptionalEnum("positionSide", positionSide);
        parameters.AddOptionalParameter("stopPrice", stopPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("activationPrice", activationPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("callbackRate", callbackRate?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalEnum("workingType", workingType);
        parameters.AddOptionalParameter("reduceOnly", reduceOnly?.ToString().ToLower());
        parameters.AddOptionalParameter("closePosition", closePosition?.ToString().ToLower());
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalEnum("priceMatch", priceMatch);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptionalParameter("priceProtect", priceProtect?.ToString().ToUpper());

        var request = _definitions.GetOrCreate(HttpMethod.Post, "dapi/v1/order", BinanceExchange.RateLimiter.FuturesRest, 0, true);
        var result = await _baseClient.SendAsync<BinanceFuturesOrder>(request, parameters, ct).ConfigureAwait(false);
        if (result)
            _baseClient.InvokeOrderPlaced(new OrderId { SourceObject = result.Data, Id = result.Data.Id.ToString(CultureInfo.InvariantCulture) });
        return result;
    }

    public async Task<RestCallResult<IEnumerable<CallResult<BinanceFuturesOrder>>>> PlaceMultipleOrdersAsync(
        IEnumerable<BinanceFuturesBatchOrder> orders,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (orders.Count() <= 0 || orders.Count() > 5)
            throw new ArgumentException("Order list should be at least 1 and max 5 orders");

        if (_baseClient.ApiOptions.TradeRulesBehaviour != TradeRulesBehaviour.None)
        {
            foreach (var order in orders)
            {
                var rulesCheck = await _baseClient.CheckTradeRules(order.Symbol, order.Quantity, null, order.Price, order.StopPrice, order.Type, ct).ConfigureAwait(false);
                if (!rulesCheck.Passed)
                {
                    _logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
                    return new WebCallResult<IEnumerable<CallResult<BinanceFuturesOrder>>>(new ArgumentError(rulesCheck.ErrorMessage!));
                }

                order.Quantity = rulesCheck.Quantity;
                order.Price = rulesCheck.Price;
                order.StopPrice = rulesCheck.StopPrice;
            }
        }

        var parameters = new ParameterCollection();

        var parameterOrders = new ParameterCollection[orders.Count()];
        int i = 0;
        foreach (var order in orders)
        {
            var orderParameters = new ParameterCollection()
                {
                    { "symbol", order.Symbol },
                    { "newOrderRespType", "RESULT" }
                };

            orderParameters.AddEnum("side", order.Side);
            orderParameters.AddEnum("type", order.Type);
            var clientOrderId = LibraryHelpers.ApplyBrokerId(order.NewClientOrderId, BinanceExchange.ClientOrderIdFutures, 36, _baseClient.ClientOptions.AllowAppendingClientOrderId);
            orderParameters.AddOptionalParameter("quantity", order.Quantity?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalParameter("newClientOrderId", clientOrderId);
            orderParameters.AddOptionalParameter("price", order.Price?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalEnum("timeInForce", order.TimeInForce);
            orderParameters.AddOptionalEnum("positionSide", order.PositionSide);
            orderParameters.AddOptionalParameter("stopPrice", order.StopPrice?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalParameter("activationPrice", order.ActivationPrice?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalParameter("callbackRate", order.CallbackRate?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalEnum("workingType", order.WorkingType);
            orderParameters.AddOptionalParameter("reduceOnly", order.ReduceOnly?.ToString().ToLower());
            orderParameters.AddOptionalParameter("priceProtect", order.PriceProtect?.ToString().ToUpper());
            orderParameters.AddOptionalEnum("selfTradePreventionMode", order.SelfTradePreventionMode);
            orderParameters.AddOptionalEnum("priceMatch", order.PriceMatch);
            parameterOrders[i] = orderParameters;
            i++;
        }

        parameters.Add("batchOrders", JsonSerializer.Serialize(parameterOrders));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Post, "dapi/v1/batchOrders", BinanceExchange.RateLimiter.FuturesRest, 5, true);
        var response = await _baseClient.SendAsync<IEnumerable<BinanceFuturesMultipleOrderPlaceResult>>(request, parameters, ct).ConfigureAwait(false);
        if (!response.Success)
            return response.As<IEnumerable<CallResult<BinanceFuturesOrder>>>(default);

        var result = new List<CallResult<BinanceFuturesOrder>>();
        foreach (var item in response.Data)
        {
            result.Add(item.Code != 0
                ? new CallResult<BinanceFuturesOrder>(new ServerError(item.Code, item.Message))
                : new CallResult<BinanceFuturesOrder>(item));
        }

        return response.As<IEnumerable<CallResult<BinanceFuturesOrder>>>(result);
    }

    // TODO: Modify Order (TRADE)
    // TODO: Modify Multiple Orders(TRADE)
    // TODO: Get Order Modify History (USER_DATA)

    public async Task<RestCallResult<BinanceFuturesOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (!orderId.HasValue && string.IsNullOrEmpty(origClientOrderId))
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        if (origClientOrderId != null)
            origClientOrderId = LibraryHelpers.ApplyBrokerId(origClientOrderId, BinanceExchange.ClientOrderIdFutures, 36, _baseClient.ClientOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Delete, "dapi/v1/order", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        var result = await _baseClient.SendAsync<BinanceFuturesOrder>(request, parameters, ct).ConfigureAwait(false);
        if (result)
            _baseClient.InvokeOrderCanceled(new OrderId { SourceObject = result.Data, Id = result.Data.Id.ToString(CultureInfo.InvariantCulture) });
        return result;
    }

    public async Task<RestCallResult<IEnumerable<CallResult<BinanceFuturesOrder>>>> CancelMultipleOrdersAsync(string symbol, IEnumerable<long>? orderIdList = null, IEnumerable<string>? origClientOrderIdList = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderIdList == null && origClientOrderIdList == null)
            throw new ArgumentException("Either orderIdList or origClientOrderIdList must be sent");

        if (orderIdList?.Count() > 10)
            throw new ArgumentException("orderIdList cannot contain more than 10 items");

        if (origClientOrderIdList?.Count() > 10)
            throw new ArgumentException("origClientOrderIdList cannot contain more than 10 items");

        var convertClientOrderIdList = origClientOrderIdList?.Select(x => LibraryHelpers.ApplyBrokerId(x, BinanceExchange.ClientOrderIdFutures, 36, _baseClient.ClientOptions.AllowAppendingClientOrderId));

        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };

        if (orderIdList != null)
            parameters.AddOptionalParameter("orderIdList", $"[{string.Join(",", orderIdList)}]");

        if (origClientOrderIdList != null)
            parameters.AddOptionalParameter("origClientOrderIdList", $"[{string.Join(",", origClientOrderIdList.Select(id => $"\"{id}\""))}]");

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Delete, "dapi/v1/batchOrders", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        var response = await _baseClient.SendAsync<IEnumerable<BinanceFuturesMultipleOrderCancelResult>>(request, parameters, ct).ConfigureAwait(false);

        if (!response.Success)
            return response.As<IEnumerable<CallResult<BinanceFuturesOrder>>>(default);

        var result = new List<CallResult<BinanceFuturesOrder>>();
        foreach (var item in response.Data)
        {
            result.Add(item.Code != 0
                ? new CallResult<BinanceFuturesOrder>(new ServerError(item.Code, item.Message))
                : new CallResult<BinanceFuturesOrder>(item));
        }

        return response.As<IEnumerable<CallResult<BinanceFuturesOrder>>>(result);
    }

    public async Task<RestCallResult<BinanceFuturesCancelAllOrders>> CancelAllOrdersAsync(string symbol, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Delete, "dapi/v1/allOpenOrders", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<BinanceFuturesCancelAllOrders>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<BinanceFuturesCountDownResult>> CancelAllOrdersAfterTimeoutAsync(string symbol, TimeSpan countDownTime, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
            {
                { "symbol", symbol },
                { "countdownTime", (int)countDownTime.TotalMilliseconds }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Post, "dapi/v1/countdownCancelAll", BinanceExchange.RateLimiter.FuturesRest, 10, true);
        return await _baseClient.SendAsync<BinanceFuturesCountDownResult>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<BinanceFuturesOrder>> GetOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        if (origClientOrderId != null)
            origClientOrderId = LibraryHelpers.ApplyBrokerId(origClientOrderId, BinanceExchange.ClientOrderIdFutures, 36, _baseClient.ClientOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/order", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<BinanceFuturesOrder>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesOrder>>> GetOrdersAsync(string? symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var weight = symbol == null ? 40 : 20;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/allOrders", BinanceExchange.RateLimiter.FuturesRest, weight, true);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesOrder>>(request, parameters, ct, weight).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesOrder>>> GetOpenOrdersAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("symbol", symbol);

        var weight = symbol == null ? 40 : 1;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/openOrders", BinanceExchange.RateLimiter.FuturesRest, weight, true);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesOrder>>(request, parameters, ct, weight).ConfigureAwait(false);
    }

    public async Task<RestCallResult<BinanceFuturesOrder>> GetOpenOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        if (origClientOrderId != null)
            origClientOrderId = LibraryHelpers.ApplyBrokerId(origClientOrderId, BinanceExchange.ClientOrderIdFutures, 36, _baseClient.ClientOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/openOrder", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<BinanceFuturesOrder>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesOrder>>> GetForcedOrdersAsync(string? symbol = null, AutoCloseType? closeType = null, DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalEnum("autoCloseType", closeType);
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));

        var weight = symbol == null ? 50 : 20;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/forceOrders", BinanceExchange.RateLimiter.FuturesRest, weight, true);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesOrder>>(request, parameters, ct, weight).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesCoinTrade>>> GetUserTradesAsync(string? symbol = null, string? pair = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, long? orderId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("pair", pair);
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var weight = symbol == null ? 40 : 20;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/userTrades", BinanceExchange.RateLimiter.FuturesRest, weight, true);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesCoinTrade>>(request, parameters, ct, weight).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinancePositionDetailsCoin>>> GetPositionInformationAsync(string? marginAsset = null, string? pair = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();

        parameters.AddOptionalParameter("marginAsset", marginAsset);
        parameters.AddOptionalParameter("pair", pair);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/positionRisk", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<IEnumerable<BinancePositionDetailsCoin>>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<BinanceFuturesPositionMode>> GetPositionModeAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/positionSide/dual", BinanceExchange.RateLimiter.FuturesRest, 30, true);
        return await _baseClient.SendAsync<BinanceFuturesPositionMode>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<BinanceResult>> ModifyPositionModeAsync(bool dualPositionSide, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
            {
                { "dualSidePosition", dualPositionSide.ToString().ToLower() }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Post, "dapi/v1/positionSide/dual", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<BinanceResult>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<BinanceFuturesChangeMarginTypeResult>> ChangeMarginTypeAsync(string symbol, BinanceFuturesMarginType marginType, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
        parameters.AddEnum("marginType", marginType);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Post, "dapi/v1/marginType", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<BinanceFuturesChangeMarginTypeResult>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<BinanceFuturesInitialLeverageChangeResult>> ChangeInitialLeverageAsync(string symbol, int leverage, long? receiveWindow = null, CancellationToken ct = default)
    {
        leverage.ValidateIntBetween(nameof(leverage), 1, 125);

        var parameters = new ParameterCollection
            {
                { "symbol", symbol },
                { "leverage", leverage }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Post, "dapi/v1/leverage", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<BinanceFuturesInitialLeverageChangeResult>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesQuantileEstimation>>> GetPositionAdlQuantileEstimationAsync(string? symbol = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/adlQuantile", BinanceExchange.RateLimiter.FuturesRest, 5, true);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesQuantileEstimation>>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<WebCallResult<BinanceFuturesPositionMarginResult>> ModifyPositionMarginAsync(string symbol, decimal quantity, BinanceFuturesMarginChangeDirectionType type, BinancePositionSide? positionSide = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
            {
                { "symbol", symbol },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
            };
        parameters.AddEnum("type", type);
        parameters.AddOptionalEnum("positionSide", positionSide);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Post, "dapi/v1/positionMargin", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<BinanceFuturesPositionMarginResult>(request, parameters, ct).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesMarginChangeHistoryResult>>> GetMarginChangeHistoryAsync(string symbol, BinanceFuturesMarginChangeDirectionType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalEnum("type", type);
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "dapi/v1/positionMargin/history", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesMarginChangeHistoryResult>>(request, parameters, ct).ConfigureAwait(false);
    }

    */
}