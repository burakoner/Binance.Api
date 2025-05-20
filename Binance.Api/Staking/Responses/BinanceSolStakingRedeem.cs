namespace Binance.Api.Staking;

/// <summary>
/// Staking Redeem Result
/// </summary>
public record BinanceSolStakingRedeem
{
    /// <summary>
    /// Successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// SOL Quantity
    /// </summary>
    [JsonProperty("solAmount")]
    public decimal SolQuantity { get; set; }

    /// <summary>
    /// Exchange Ratio
    /// </summary>
    [JsonProperty("exchangeRate")]
    public decimal ExchangeRate { get; set; }

    /// <summary>
    /// Arrival timestamp
    /// </summary>
    [JsonProperty("arrivalTime")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime ArrivalTime { get; set; }
}
