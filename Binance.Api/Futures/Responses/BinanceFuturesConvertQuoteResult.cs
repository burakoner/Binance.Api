namespace Binance.Api.Futures;

/// <summary>
/// Convert quote result
/// </summary>
public record BinanceFuturesConvertQuoteResult
{
    /// <summary>
    /// Order id
    /// </summary>
    [JsonProperty("orderId")]
    public string OrderId { get; set; } = string.Empty;

    /// <summary>
    /// Create time
    /// </summary>
    [JsonProperty("createTime")]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Order status
    /// </summary>
    [JsonProperty("orderStatus")]
    public BinanceFuturesConvertOrderStatus Status { get; set; }
}
