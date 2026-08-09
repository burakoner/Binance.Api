namespace Binance.Api.Convert;

/// <summary>
/// Convert Limit Order
/// </summary>
public record BinanceConvertLimitOrder
{
    /// <summary>
    /// Order id
    /// </summary>
    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    /// <summary>
    /// Order status
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;
}
