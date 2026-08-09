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
    /// Spot and Funding wallets
    /// </summary>
    [Map("SPOT_FUNDING")]
    SpotFunding = 2,

    /// <summary>
    /// Earn wallet
    /// </summary>
    [Map("EARN")]
    Earn = 3,

    /// <summary>
    /// Funding and Earn wallets
    /// </summary>
    [Map("FUNDING_EARN")]
    FundingEarn = 4,

    /// <summary>
    /// Spot, Funding, and Earn wallets
    /// </summary>
    [Map("SPOT_FUNDING_EARN")]
    SpotFundingEarn = 5,

    /// <summary>
    /// Spot and Earn wallets
    /// </summary>
    [Map("SPOT_EARN")]
    SpotEarn = 6,
}
