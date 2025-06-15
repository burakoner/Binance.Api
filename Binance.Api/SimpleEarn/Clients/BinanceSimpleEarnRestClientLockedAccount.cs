namespace Binance.Api.SimpleEarn;

internal partial class BinanceSimpleEarnRestClientLocked
{
    public Task<RestCallResult<BinanceSimpleEarnAccount>> GetAccountAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSimpleEarnAccount>(GetUrl(sapi, v1, "simple-earn/account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceSimpleEarnLockedProduct>>> GetProductsAsync(string? asset = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("asset", asset);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceSimpleEarnLockedProduct>>(GetUrl(sapi, v1, "simple-earn/locked/list"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceSimpleEarnLockedPosition>>> GetPositionsAsync(string? asset = null, string? positionId = null, string? projectId = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("asset", asset);
        parameters.AddOptional("positionId", positionId);
        parameters.AddOptional("projectId", projectId);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceSimpleEarnLockedPosition>>(GetUrl(sapi, v1, "simple-earn/locked/position"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceSimpleEarnQuota>> GetQuotaAsync(string projectId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "projectId", projectId }
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSimpleEarnQuota>(GetUrl(sapi, v1, "simple-earn/locked/personalLeftQuota"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }
}