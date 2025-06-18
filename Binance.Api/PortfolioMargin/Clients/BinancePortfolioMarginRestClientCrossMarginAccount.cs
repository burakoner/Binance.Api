namespace Binance.Api.PortfolioMargin;

internal partial class BinancePortfolioMarginRestClientCrossMargin
{
    public Task<RestCallResult<BinancePortfolioMarginCrossBorrowable>> GetMaximumBorrowableAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinancePortfolioMarginQuantity>> GetMaximumWithdrawalAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinanceRowsResult<BinancePortfolioMarginCrossLoan>>> GetLoansAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinanceRowsResult<BinancePortfolioMarginCrossRepayRecord>>> GetRepaysAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinanceRowsResult<BinancePortfolioMarginCrossInterestRecord>>> GetMarginInterestHistoryRepaysAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}