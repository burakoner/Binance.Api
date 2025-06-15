namespace Binance.Api.Staking;

/// <summary>
/// SOL rewards
/// </summary>
public record BinanceSolStakingRewards : BinanceRowsResult<BinanceSolStakingReward>
{
    /// <summary>
    /// Estimated rewards in SOL
    /// </summary>
    [JsonProperty("estRewardsInSOL")]
    public decimal EstimatedRewardsInSol { get; set; }
}

/// <summary>
/// SOL reward
/// </summary>
public record BinanceSolStakingReward
{
    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("time")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Amount in SOL
    /// </summary>
    public decimal AmountInSol { get; set; }

    /// <summary>
    /// Holding
    /// </summary>
    public decimal Holding { get; set; }

    /// <summary>
    /// Holding in SOL
    /// </summary>
    public decimal HoldingInSol { get; set; }

    /// <summary>
    /// Annual percetage ratge
    /// </summary>
    public decimal AnnualPercentageRate { get; set; }
}
