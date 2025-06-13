namespace Binance.Api.Rebate;

/// <summary>
/// Binance Rebate Spot Data
/// </summary>
public record BinanceRebateSpotRecord
{
    /// <summary>
    /// Asset
    /// </summary>
    public string Asset { get; set; } = "";

    /// <summary>
    /// Type
    /// </summary>
    public BinanceRebateType Type { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateTime { get; set; }
}
