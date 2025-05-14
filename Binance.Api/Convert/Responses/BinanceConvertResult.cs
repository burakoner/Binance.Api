namespace Binance.Api.Convert;

/// <summary>
/// Convert Quote
/// </summary>
public record BinanceConvertResult
{
    /// <summary>
    /// Order id
    /// </summary>
    [JsonProperty("orderId")]
    public string OrderId { get; set; } = string.Empty;

    /// <summary>
    /// Creation time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("createTime")]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Order status
    /// </summary>
    [JsonProperty("orderStatus")]
    public BinanceConvertOrderStatus Status { get; set; }
}
