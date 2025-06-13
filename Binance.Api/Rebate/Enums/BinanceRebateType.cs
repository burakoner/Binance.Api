namespace Binance.Api.Rebate;

/// <summary>
/// Rebate Type
/// </summary>
public enum BinanceRebateType : byte
{
    /// <summary>
    /// Commission Rebate
    /// </summary>
    [Map("1")]
    CommissionRebate = 1,

    /// <summary>
    /// Referral Kickback
    /// </summary>>
    [Map("2")]
    ReferralKickback = 2,
}
