namespace Binance.Api.Futures;

/// <summary>
/// Result of the requested margin amount change
/// </summary>
public record BinanceFuturesPositionMarginResult
{
    /// <summary>
    /// New margin amount
    /// </summary>
    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Request response code
    /// </summary>
    [JsonProperty("code")]
    public int Code { get; set; }

    /// <summary>
    /// Message
    /// </summary>
    [JsonProperty("msg")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Maximum margin value
    /// NOTE: string type, because the value van be 'inf' (infinite)
    /// </summary>
    [JsonProperty("maxNotionalValue")]
    public string MaxNotionalValue { get; set; } = string.Empty;

    /// <summary>
    /// Direction of the requested margin change
    /// </summary>
    [JsonProperty("type")]
    public BinanceFuturesMarginChangeDirectionType Type { get; set; }
}
