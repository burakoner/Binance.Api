namespace Binance.Api.Staking;

/// <summary>
/// Rewards history
/// </summary>
public record BinanceEthStakingReward
{
    /// <summary>
    /// Asset
    /// </summary>
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Amount
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("time")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// BETH holding balance
    /// </summary>
    public decimal Holding { get; set; }

    /// <summary>
    /// Annual percentage rate
    /// </summary>
    public decimal AnnualPercentageRate { get; set; }
}
