namespace Binance.Api.DualInvestment;

/// <summary>
/// Binance Dual Investment Status
/// </summary>
public enum BinanceDualInvestmentStatus : byte
{
    /// <summary>
    /// Products are purchasing, will give results later
    /// </summary>>
    [Map("PENDING")]
    Pending = 0,

    /// <summary>
    /// purchase successfully
    /// </summary>>
    [Map("PURCHASE_SUCCESS")]
    Success = 1,

    /// <summary>
    /// fail to purchase
    /// </summary>>
    [Map("PURCHASE_FAIL")]
    Failed = 2,

    /// <summary>
    /// Products are settling
    /// </summary>>
    [Map("SETTLING")]
    Settling = 3,

    /// <summary>
    /// Products are finish settling
    /// </summary>>
    [Map("SETTLED")]
    Settled = 4,

    /// <summary>
    /// refund ongoing
    /// </summary>>
    [Map("REFUNDING")]
    Refunding = 5,

    /// <summary>
    /// refund to spot account successfully
    /// </summary>>
    [Map("REFUND_SUCCESS")]
    RefundSuccess = 6,
}