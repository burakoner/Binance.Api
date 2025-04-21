namespace Binance.Api.Wallet.Enums;

/// <summary>
/// Wallet type
/// </summary>
public enum WalletType : byte
{
    /// <summary>
    /// Spot wallet
    /// </summary>
    [Map("0")]
    Spot = 0,

    /// <summary>
    /// Funding wallet
    /// </summary>
    [Map("1")]
    Funding = 1
}
