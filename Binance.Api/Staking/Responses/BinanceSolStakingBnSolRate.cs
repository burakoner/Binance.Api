namespace Binance.Api.Staking;

/// <summary>
/// Rate history
/// </summary>
public record BinanceSolStakingBnSolRate
{
    /// <summary>
    /// Exchange rate
    /// </summary>
    public decimal ExchangeRate { get; set; }

    /// <summary>
    /// Annual percentage rate
    /// </summary>
    public decimal AnnualPercentageRate { get; set; }
    /// <summary>
    /// Boost rewards
    /// </summary>
    public List<BinanceSolStakingBnSolRateReward> BoostRewards { get; set; } = [];

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("time")]
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Reward info
/// </summary>
public record BinanceSolStakingBnSolRateReward
{
    /// <summary>
    /// Boost APR
    /// </summary>
    public decimal BoostApr { get; set; }

    /// <summary>
    /// Reward asset
    /// </summary>
    public string RewardsAsset { get; set; } = string.Empty;
}
