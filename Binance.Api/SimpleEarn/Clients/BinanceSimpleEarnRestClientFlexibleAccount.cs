namespace Binance.Api.SimpleEarn;

internal partial class BinanceSimpleEarnRestClientFlexible
{
    public Task<RestCallResult<BinanceSimpleEarnAccount>> GetAccountAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSimpleEarnAccount>(GetUrl(sapi, v1, "simple-earn/account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceQueryRecords<BinanceSimpleEarnFlexibleProduct>>> GetProductsAsync(string? asset = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("asset", asset);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceQueryRecords<BinanceSimpleEarnFlexibleProduct>>(GetUrl(sapi, v1, "simple-earn/flexible/list"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceQueryRecords<BinanceSimpleEarnFlexiblePosition>>> GetPositionsAsync(string? asset = null, string? productId = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("asset", asset);
        parameters.AddOptional("productId", productId);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceQueryRecords<BinanceSimpleEarnFlexiblePosition>>(GetUrl(sapi, v1, "simple-earn/flexible/position"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceSimpleEarnQuota>> GetQuotaAsync(string productId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "productId", productId }
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSimpleEarnQuota>(GetUrl(sapi, v1, "simple-earn/flexible/personalLeftQuota"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

}