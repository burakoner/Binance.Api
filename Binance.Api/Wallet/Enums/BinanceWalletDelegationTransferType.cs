namespace Binance.Api.Wallet;

/// <summary>
/// Binance Wallet Delegation Transfer Type
/// </summary>
public enum BinanceWalletDelegationTransferType : byte
{
    /// <summary>
    /// Delegate
    /// </summary>
    [Map("Delegate")]
    Delegate = 1,

    /// <summary>
    /// Undelegate
    /// </summary>
    [Map("Undelegate")]
    Undelegate = 2,
}
