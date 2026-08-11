namespace Binance.Api.Options;

internal partial class BinanceOptionsRestClient
{
    public Task<RestCallResult<List<BinanceOptionsTicker>>> GetTickersAsync(CancellationToken ct = default)
    {
        return RequestAsync<List<BinanceOptionsTicker>>(GetUrl(eapi, v1, "ticker"), HttpMethod.Get, ct, false, requestWeight: 40);
    }

    public async Task<RestCallResult<BinanceOptionsTicker>> GetTickersAsync(string symbol, CancellationToken ct = default)
    {
        ValidateMarketDataRequiredValue(symbol, nameof(symbol));

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
        };

        var result = await RequestAsync<List<BinanceOptionsTicker>>(GetUrl(eapi, v1, "ticker"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 1);
        if (!result) return result.AsError<BinanceOptionsTicker>(result.Error!);
        if (result.Data.Count == 0) return result.AsError<BinanceOptionsTicker>(new ServerError("No data found"));

        return result.As(result.Data.FirstOrDefault()!);
    }

    public Task<RestCallResult<List<BinanceOptionsPublicExercise>>> GetPublicExerciseRecordsAsync(string? underlying = null, DateTime? startTime = null, DateTime? endTime = null, long? limit = null, CancellationToken ct = default)
    {
        ValidateMarketDataOptionalValue(underlying, nameof(underlying));
        ValidateMarketDataMaximum(limit, 100, nameof(limit));

        var parameters = new ParameterCollection();
        parameters.AddOptional("underlying", underlying);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);

        return RequestAsync<List<BinanceOptionsPublicExercise>>(GetUrl(eapi, v1, "exerciseHistory"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 3);
    }

    public Task<RestCallResult<List<BinanceOptionsOpenInterest>>> GetOpenInterestAsync(string underlying, DateTime expiration, CancellationToken ct = default)
    {
        ValidateMarketDataRequiredValue(underlying, nameof(underlying));

        var parameters = new ParameterCollection();
        parameters.AddParameter("underlyingAsset", underlying);
        parameters.AddParameter("expiration", expiration.ToString("yyMMdd", BinanceConstants.CI));

        return RequestAsync<List<BinanceOptionsOpenInterest>>(GetUrl(eapi, v1, "openInterest"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceOptionsOrderBook>> GetOrderBookAsync(string symbol, long? limit = null, CancellationToken ct = default)
    {
        ValidateMarketDataRequiredValue(symbol, nameof(symbol));
        if (limit is not null and not (5 or 10 or 20 or 50 or 100 or 500 or 1000))
            throw new ArgumentOutOfRangeException(nameof(limit), limit, "Allowed values are 5, 10, 20, 50, 100, 500, and 1000.");

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("limit", limit);

        var requestWeight = limit switch
        {
            null or 100 => 5,
            500 => 10,
            1000 => 20,
            _ => 1
        };
        return RequestAsync<BinanceOptionsOrderBook>(GetUrl(eapi, v1, "depth"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight);
    }

    public Task<RestCallResult<List<BinanceOptionsPublicTrade>>> GetRecentTradesAsync(string symbol, long? limit = null, CancellationToken ct = default)
    {
        ValidateMarketDataRequiredValue(symbol, nameof(symbol));
        ValidateMarketDataMaximum(limit, 500, nameof(limit));

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("limit", limit);

        return RequestAsync<List<BinanceOptionsPublicTrade>>(GetUrl(eapi, v1, "trades"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<List<BinanceOptionsBlockTrade>>> GetRecentBlockTradesAsync(string? symbol = null, long? limit = null, CancellationToken ct = default)
    {
        ValidateMarketDataOptionalValue(symbol, nameof(symbol));
        ValidateMarketDataMaximum(limit, 500, nameof(limit));

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("limit", limit);

        return RequestAsync<List<BinanceOptionsBlockTrade>>(GetUrl(eapi, v1, "blockTrades"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<BinanceOptionsIndexPrice>> GetIndexPriceAsync(string underlying, CancellationToken ct = default)
    {
        ValidateMarketDataRequiredValue(underlying, nameof(underlying));

        var parameters = new ParameterCollection();
        parameters.AddParameter("underlying", underlying);

        return RequestAsync<BinanceOptionsIndexPrice>(GetUrl(eapi, v1, "index"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceOptionsKline>>> GetKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, long? limit = null, CancellationToken ct = default)
    {
        ValidateMarketDataRequiredValue(symbol, nameof(symbol));
        ValidateMarketDataKlineInterval(interval);
        ValidateMarketDataMaximum(limit, 1500, nameof(limit));

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddEnum("interval", interval);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<List<BinanceOptionsKline>>(GetUrl(eapi, v1, "klines"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceOptionsMarkPrice>>> GetMarkPriceAsync(string? symbol = null, CancellationToken ct = default)
    {
        ValidateMarketDataOptionalValue(symbol, nameof(symbol));

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);

        return RequestAsync<List<BinanceOptionsMarkPrice>>(GetUrl(eapi, v1, "mark"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 5);
    }

    private static void ValidateMarketDataRequiredValue(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null, empty, or whitespace.", parameterName);
    }

    private static void ValidateMarketDataOptionalValue(string? value, string parameterName)
    {
        if (value is not null)
            ValidateMarketDataRequiredValue(value, parameterName);
    }

    private static void ValidateMarketDataMaximum(long? value, long maximum, string parameterName)
    {
        if (value > maximum)
            throw new ArgumentOutOfRangeException(parameterName, value, $"Value cannot exceed {maximum}.");
    }

    private static void ValidateMarketDataKlineInterval(BinanceKlineInterval interval)
    {
        if (!Enum.IsDefined(typeof(BinanceKlineInterval), interval) || interval == BinanceKlineInterval.OneSecond)
            throw new ArgumentOutOfRangeException(nameof(interval), interval, "The interval is not supported by Options klines.");
    }
}
