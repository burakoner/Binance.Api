namespace Binance.Api.Staking;

/// <summary>
/// Eth staking quota
/// </summary>
public record BinanceEthStakingQuota
{
    /// <summary>
    /// Staking quota left
    /// </summary>
    public decimal LeftStakingPersonalQuota { get; set; }

    /// <summary>
    /// Redemption quota left
    /// </summary>
    public decimal LeftRedemptionPersonalQuota { get; set; }
}
