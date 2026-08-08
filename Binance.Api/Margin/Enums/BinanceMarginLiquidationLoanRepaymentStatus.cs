namespace Binance.Api.Margin;

/// <summary>Liquidation-loan repayment state.</summary>
public enum BinanceMarginLiquidationLoanRepaymentStatus : byte
{
    /// <summary>The repayment completed.</summary>
    [Map("SUCCESS")]
    Success = 1,

    /// <summary>The repayment is still processing.</summary>
    [Map("PENDING")]
    Pending = 2
}
