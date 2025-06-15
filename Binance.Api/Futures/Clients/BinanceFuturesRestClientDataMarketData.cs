namespace Binance.Api.Futures;

internal partial class BinanceFuturesRestClientData
{
    public async Task<RestCallResult<List<BinanceFuturesDataLink>>> GetFuturesDataLinkAsync(string symbol, BinanceFuturesDataType type, DateTime startTime, DateTime endTime, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddEnum("dataType", type);
        parameters.AddMilliseconds("startTime", startTime);
        parameters.AddMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceResponse<List<BinanceFuturesDataLink>>>(GetUrl(sapi, v1, "futures/histDataLink"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 200).ConfigureAwait(false);
        if (!result.Success) return result.AsError<List<BinanceFuturesDataLink>>(result.Error!);
        if (result.Data?.Data == null || result.Data.Data.Count == 0) return result.AsError<List<BinanceFuturesDataLink>>(new ServerError("No data found for the specified parameters."));

        return result.As(result.Data.Data);
    }
}