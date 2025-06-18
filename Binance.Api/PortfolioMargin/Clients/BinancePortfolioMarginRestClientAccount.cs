namespace Binance.Api.PortfolioMargin;

internal partial class BinancePortfolioMarginRestClient
{
    public Task<RestCallResult<List<BinancePortfolioMarginAccount>>> GetAccountAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinancePortfolioMarginAccountInformation>> GetAccountInformationAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinancePortfolioMarginAutoRepay>> GetAutoRepayFuturesStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<bool>> SetAutoRepayFuturesStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<bool>> RepayFuturesNegativeBalanceAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<List<BinancePortfolioMarginNegativeBalanceInterest>>> GetNegativeBalanceInterestHistoryAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<bool>> FundAutoCollectionAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
        // BinanceMessage
    }

    public Task<RestCallResult<bool>> FundAutoCollectionAsync(string asset, int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
        // BinanceMessage
    }

    public Task<RestCallResult<BinancePortfolioMarginTransactionId>> BnbTransferAsync(string asset, int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
        // BinanceMessage
    }

    public Task<RestCallResult<List<BinanceRateLimit>>> GetRateLimitsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceRateLimit>>(GetUrl(papi, v1, "rateLimit/order"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceRowsResult<BinancePortfolioMarginNegativeBalanceAutoExchange>>> GetNegativeBalanceAutoExchangeHistoryAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}