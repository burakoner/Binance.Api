namespace Binance.Api.Staking;

/// <summary>
/// SOL staking quota
/// </summary>
public record BinanceSolStakingQuota
{
    /// <summary>
    /// Staking quota left
    /// </summary>
    public decimal LeftStakingPersonalQuota { get; set; }

    /// <summary>
    /// Redemption quota left
    /// </summary>
    public decimal LeftRedemptionPersonalQuota { get; set; }

    /// <summary>
    /// Min staking amount
    /// </summary>
    [JsonProperty("minStakeAmount")]
    public decimal MinimumStakeAmount { get; set; }

    /// <summary>
    /// Min redeem amount
    /// </summary>
    [JsonProperty("minRedeemAmount")]
    public decimal MinimumRedeemAmount { get; set; }

    /// <summary>
    /// Redeem period
    /// </summary>
    public int RedeemPeriod { get; set; }

    /// <summary>
    /// Is stakeable
    /// </summary>
    public bool Stakeable { get; set; }

    /// <summary>
    /// Is redeemable
    /// </summary>
    public bool Redeemable { get; set; }

    /// <summary>
    /// Sold out
    /// </summary>
    public bool SoldOut { get; set; }

    /// <summary>
    /// Commission fee
    /// </summary>
    public decimal CommissionFee { get; set; }

    /// <summary>
    /// Next time
    /// </summary>
    public DateTime NextEpochTime { get; set; }

    /// <summary>
    /// Calculating
    /// </summary>
    public bool Calculating { get; set; }
}
