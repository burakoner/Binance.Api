namespace Binance.Api.Margin;

/// <summary>
/// Side effect for a margin order
/// </summary>
public enum BinanceMarginSideEffectType : byte
{
    /// <summary>
    /// Normal trade
    /// </summary>
    [Map("NO_SIDE_EFFECT")]
    NoSideEffect = 0,

    /// <summary>
    /// Margin trade order
    /// </summary>
    [Map("MARGIN_BUY")]
    MarginBuy,

    /// <summary>
    /// Make auto repayment after order is filled
    /// </summary>
    [Map("AUTO_REPAY")]
    AutoRepay,

    /// <summary>
    /// Automatic borrowing and repayment, simultaneously
    /// </summary>
    [Map("AUTO_BORROW_REPAY")]
    AutoBorrowRepay,
}
