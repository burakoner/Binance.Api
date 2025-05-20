namespace Binance.Api.Staking;

/// <summary>
/// Staking Redeem Result
/// </summary>
public record BinanceEthStakingRedeem
{
    /// <summary>
    /// Successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// ETH Quantity
    /// </summary>
    [JsonProperty("ethAmount")]
    public decimal EthQuantity { get; set; }

    /// <summary>
    /// Conversion Ratio
    /// </summary>
    [JsonProperty("conversionRatio")]
    public decimal ConversionRatio { get; set; }

    /// <summary>
    /// Arrival timestamp
    /// </summary>
    [JsonProperty("arrivalTime")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime ArrivalTime { get; set; }
}
