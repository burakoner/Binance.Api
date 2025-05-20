namespace Binance.Api.Staking;

/// <summary>
/// Rewards history container
/// </summary>
public record BinanceEthStakingWbEthRewards: BinanceQueryRecords<BinanceEthStakingWbEthReward>
{
    /// <summary>
    /// Estimated Rewards in ETH
    /// </summary>
    [JsonProperty("estRewardsInETH")]
    public decimal EstimatedRewardsInETH { get; set; }
}

/// <summary>
/// Rewards history
/// </summary>
public record BinanceEthStakingWbEthReward
{
    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("time")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Quantity in ETH
    /// </summary>
    [JsonProperty("amountInETH")]
    public decimal QuantityInETH { get; set; }

    /// <summary>
    /// Holding
    /// </summary>
    [JsonProperty("holding")]
    public decimal Holding { get; set; }

    /// <summary>
    /// Holding in ETH
    /// </summary>
    [JsonProperty("holdingInETH")]
    public decimal HoldingInETH { get; set; }

    /// <summary>
    /// Annual percentage rate
    /// </summary>
    public decimal AnnualPercentageRate { get; set; }
}
