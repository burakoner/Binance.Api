namespace Binance.Api.Wallet;

/// <summary>
/// Cloud mining payment/refund history
/// </summary>
public record BinanceCloudMiningHistory
{
    /// <summary>
    /// Creation time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Transaction id
    /// </summary>
    [JsonProperty("tranId")]
    public long TransactionId { get; set; }

    /// <summary>
    /// Type
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceCloudMiningPaymentStatus Type { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public string Status { get; set; } = string.Empty;
}
