namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Portfolio Margin Loan Status
/// </summary>
public enum BinancePortfolioMarginLoanStatus : byte
{
    /// <summary>
    /// Pending
    /// </summary>
    [Map("PENDING")]
    Pending = 1,

    /// <summary>
    /// Confirmed
    /// </summary>
    [Map("CONFIRMED")]
    Confirmed = 2,

    /// <summary>
    /// Failed
    /// </summary>
    [Map("FAILED")]
    Failed = 3,
}
