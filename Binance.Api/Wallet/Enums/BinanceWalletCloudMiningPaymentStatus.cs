namespace Binance.Api.Wallet;

/// <summary>
/// Cloud mining payment status
/// </summary>
public enum BinanceWalletCloudMiningPaymentStatus : int
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
