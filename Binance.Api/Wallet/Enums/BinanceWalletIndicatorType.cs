namespace Binance.Api.Wallet;

/// <summary>
/// Types of indicators
/// </summary>
public enum BinanceWalletIndicatorType : byte
{
    /// <summary>
    /// Unfilled ratio
    /// </summary>
    [Map("UFR")]
    UnfilledRatio = 1,

    /// <summary>
    /// Expired orders ratio
    /// </summary>
    [Map("IFER")]
    ExpirationRatio = 2,

    /// <summary>
    /// Canceled orders ratio
    /// </summary>
    [Map("GCR")]
    CancelationRatio = 3
}
