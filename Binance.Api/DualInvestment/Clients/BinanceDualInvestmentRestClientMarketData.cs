using Binance.Api.Options;

namespace Binance.Api.DualInvestment;

internal partial class BinanceDualInvestmentRestClient
{
    public Task<RestCallResult<BinanceListTotalResponse<BinanceDualInvestmentProduct>>> GetProductsAsync(BinanceOptionsSide side, string exercisedCoin, string investCoin, int? pageSize = null, int? pageIndex = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddEnum("optionType", side);
        parameters.AddParameter("exercisedCoin", exercisedCoin);
        parameters.AddParameter("investCoin", investCoin);
        parameters.AddOptional("pageSize", pageSize);
        parameters.AddOptional("pageIndex", pageIndex);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceListTotalResponse<BinanceDualInvestmentProduct>>(GetUrl(sapi, v1, "dci/product/list"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 1);
    }
}