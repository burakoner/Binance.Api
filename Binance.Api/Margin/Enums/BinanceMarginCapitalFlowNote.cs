namespace Binance.Api.Margin;

/// <summary>Institutional-loan origin of a Margin capital flow.</summary>
public enum BinanceMarginCapitalFlowNote : byte
{
    /// <summary>Institutional-loan transfer.</summary>
    [Map("INSTITUTIONAL_LOAN_TRANSFER")]
    InstitutionalLoanTransfer = 1,

    /// <summary>Institutional-loan borrow.</summary>
    [Map("INSTITUTIONAL_LOAN_BORROW")]
    InstitutionalLoanBorrow,

    /// <summary>Institutional-loan repayment.</summary>
    [Map("INSTITUTIONAL_LOAN_REPAY")]
    InstitutionalLoanRepay
}
