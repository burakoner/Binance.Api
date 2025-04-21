namespace Binance.Net.Enums;

/// <summary>
/// Cloud mining payment status
/// </summary>
public enum CloudMiningPaymentStatus : int
{
    /// <summary>
    /// Payment
    /// </summary>
    [Map("248")]
    Payment = 248,

    /// <summary>
    /// Refund
    /// </summary>
    [Map("249")]
    Refund = 249
}
