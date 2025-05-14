namespace Binance.Api.Convert;

/// <summary>
/// Convert Limit Order Status
/// </summary>
public record BinanceConvertLimitOrderStatus
{
    /// <summary>
    /// Order ID
    /// </summary>
    [JsonProperty("orderId")]
    public string? OrderId { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public string? Status { get; set; }
}
