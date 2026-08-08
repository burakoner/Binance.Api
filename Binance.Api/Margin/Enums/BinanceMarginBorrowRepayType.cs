namespace Binance.Api.Margin;

/// <summary>Margin borrowing or repayment operation.</summary>
public enum BinanceMarginBorrowRepayType : byte
{
    /// <summary>Borrow from Margin.</summary>
    [Map("BORROW")]
    Borrow = 1,

    /// <summary>Repay a Margin liability.</summary>
    [Map("REPAY")]
    Repay = 2
}
