namespace Binance.Api.Wallet;

/// <summary>
/// Wallet type
/// </summary>
public enum BinanceWalletType : byte
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
