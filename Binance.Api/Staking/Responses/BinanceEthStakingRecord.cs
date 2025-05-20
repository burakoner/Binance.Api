namespace Binance.Api.Staking;

/// <summary>
/// Staking history
/// </summary>
public record BinanceEthStakingRecord
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
    /// Timestamp
    /// </summary>
    [JsonProperty("time")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Distribute asset
    /// </summary>
    [JsonProperty("distributeAsset")]
    public string DistributeAsset { get; set; } = string.Empty;

    /// <summary>
    /// Quantity distributed
    /// </summary>
    [JsonProperty("distributeAmount")]
    public decimal DistributeQuantity { get; set; }

    /// <summary>
    /// Conversion ratio
    /// </summary>
    [JsonProperty("conversionRatio")]
    public decimal ConversionRatio { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public BinanceStakingStatus Status { get; set; }
}
