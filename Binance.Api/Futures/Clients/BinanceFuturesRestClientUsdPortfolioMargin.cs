namespace Binance.Api.Futures;

internal partial class BinanceFuturesRestClientUsd
{
    public Task<RestCallResult<BinanceFuturesPortfolioMarginAccount>> GetPortfolioMarginAccountInfoAsync(string asset, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "asset", asset }
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesPortfolioMarginAccount>(GetUrl(fapi, v1, "pmAccountInfo"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 5);
    }
}