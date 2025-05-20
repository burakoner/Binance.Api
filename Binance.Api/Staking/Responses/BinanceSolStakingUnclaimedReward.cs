namespace Binance.Api.Staking;

/// <summary>
/// Unclaimed rewards info
/// </summary>
public record BinanceSolStakingUnclaimedReward
{
    /// <summary>
    /// Amount
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Reward asset
    /// </summary>
    [JsonProperty("rewardsAsset")]
    public string RewardsAsset { get; set; } = string.Empty;
}
