namespace Binance.Api.Pay;

/// <summary>
/// Binance Pay Wallet Type
/// </summary>
public enum BinancePayWalletType : byte
{
    /// <summary>
    /// Funding Wallet
    /// </summary>
    [Map("1")]
    Funding = 1,

    /// <summary>
    /// Spot Wallet
    /// </summary>
    [Map("2")]
    Spot = 2,

    /// <summary>
    /// Fiat Wallet
    /// </summary>
    [Map("3")]
    Fiat = 3,

    /// <summary>
    /// Card Payment Wallet
    /// </summary>
    [Map("4", "6")]
    CardPayment = 4,

    /// <summary>
    /// Earn Wallet
    /// </summary>
    [Map("5")]
    Earn = 5
}
