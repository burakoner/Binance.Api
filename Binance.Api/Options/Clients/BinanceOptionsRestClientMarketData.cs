namespace Binance.Api.Options;

internal partial class BinanceOptionsRestClient
{
    public Task<RestCallResult<List<BinanceOptionsTicker>>> GetTickersAsync(CancellationToken ct = default)
    {
        return RequestAsync<List<BinanceOptionsTicker>>(GetUrl(eapi, v1, "ticker"), HttpMethod.Get, ct, false, requestWeight: 5);
    }

    public async Task<RestCallResult<BinanceOptionsTicker>> GetTickersAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
        };

        var result = await RequestAsync<List<BinanceOptionsTicker>>(GetUrl(eapi, v1, "ticker"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 5);
        if (!result) return result.AsError<BinanceOptionsTicker>(result.Error!);
        if (result.Data.Count == 0) return result.AsError<BinanceOptionsTicker>(new ServerError("No data found"));

        return result.As(result.Data.FirstOrDefault()!);
    }

    public Task<RestCallResult<List<BinanceOptionsPublicExercise>>> GetPublicExerciseRecordsAsync(string? underlying = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("underlying", underlying);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);

        return RequestAsync<List<BinanceOptionsPublicExercise>>(GetUrl(eapi, v1, "exerciseHistory"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 3);
    }

    public Task<RestCallResult<List<BinanceOptionsOpenInterest>>> GetOpenInterestAsync(string underlying, DateTime expiration, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("underlyingAsset", underlying);
        parameters.AddParameter("expiration", expiration.ToString("MMddyy"));

        return RequestAsync<List<BinanceOptionsOpenInterest>>(GetUrl(eapi, v1, "openInterest"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 0);
    }

    public async Task<RestCallResult<BinanceOptionsOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntValues(nameof(limit), 5, 10, 20, 50, 100, 500, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalString("limit", limit);

        var requestWeight = limit == null ? 5 : limit <= 100 ? 2 : limit <= 500 ? 10 : limit <= 1000 ? 20 : 50;
        var result = await RequestAsync<BinanceOptionsOrderBook>(GetUrl(eapi, v1, "depth"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
        if (!result) return result;

        result.Data.Symbol = symbol;
        return result;
    }

    public Task<RestCallResult<List<BinanceOptionsPublicTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalString("limit", limit);

        return RequestAsync<List<BinanceOptionsPublicTrade>>(GetUrl(eapi, v1, "trades"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<List<BinanceOptionsBlockTrade>>> GetRecentBlockTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalString("limit", limit);

        return RequestAsync<List<BinanceOptionsBlockTrade>>(GetUrl(eapi, v1, "blockTrades"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<BinanceOptionsIndexPrice>> GetIndexPriceAsync(string underlying, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("underlying", underlying);

        return RequestAsync<BinanceOptionsIndexPrice>(GetUrl(eapi, v1, "index"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceOptionsKline>>> GetKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddEnum("interval", interval);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<List<BinanceOptionsKline>>(GetUrl(eapi, v1, "klines"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2);
    }

    public async Task<RestCallResult<List<BinanceOptionsBlockTrade>>> GetHistoricalTradesAsync(string symbol, int? limit = null, long? fromId = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalString("limit", limit);
        parameters.AddOptionalString("fromId", fromId);

        var result = await RequestAsync<List<BinanceOptionsBlockTrade>>(GetUrl(eapi, v1, "historicalTrades"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 20);
        if (!result) return result;

        foreach (var trade in result.Data) trade.Symbol = symbol;
        return result;
    }

    public Task<RestCallResult<List<BinanceOptionsMarkPrice>>> GetMarkPriceAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);

        return RequestAsync<List<BinanceOptionsMarkPrice>>(GetUrl(eapi, v1, "mark"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 5);
    }
}