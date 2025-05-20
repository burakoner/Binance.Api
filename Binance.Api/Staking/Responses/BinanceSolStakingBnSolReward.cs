namespace Binance.Api.Staking;

/// <summary>
/// SOL rewards history
/// </summary>
public record BinanceSolStakingBnSolReward
{
    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("time")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Asset name
    /// </summary>
    [JsonProperty("token")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Amount
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// BN SOL holding
    /// </summary>
    [JsonProperty("bnsolHolding")]
    public decimal BnSolHolding { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public string Status { get; set; } = string.Empty;
}
