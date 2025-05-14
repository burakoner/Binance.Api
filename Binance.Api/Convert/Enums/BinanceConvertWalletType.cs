namespace Binance.Api.Convert;

/// <summary>
/// Wallet type
/// </summary>
public enum BinanceConvertWalletType : byte
{
    /// <summary>
    /// Spot wallet
    /// </summary>
    [Map("SPOT")]
    Spot = 0,

    /// <summary>
    /// Funding wallet
    /// </summary>
    [Map("FUNDING")]
    Funding = 1,

    /// <summary>
    /// Spot Funding
    /// </summary>
    [Map("SPOT_FUNDING")]
    SpotFunding = 2,
}
