namespace Binance.Api.SimpleEarn;

/// <summary>
/// Simple Earn flexible product redemption record
/// </summary>
public record BinanceSimpleEarnFlexibleRedemptionRecord
{
    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Project id
    /// </summary>
    [JsonProperty("projectId")]
    public string ProjectId { get; set; } = string.Empty;

    /// <summary>
    /// Redeem id
    /// </summary>
    [JsonProperty("redeemId")]
    public long RedeemId { get; set; }

    /// <summary>
    /// Destination account
    /// </summary>
    [JsonProperty("destAccount")]
    public BinanceSimpleEarnSourceAccount DestinationAccount { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;
}
