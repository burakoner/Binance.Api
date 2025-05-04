namespace Binance.Api.Futures;

/// <summary>
/// 
/// </summary>
public record BinanceFuturesStreamConditionOrderTriggerRejectUpdate : BinanceFuturesStreamEvent
{
    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("T")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Reject info
    /// </summary>
    [JsonProperty("or")]
    public BinanceConditionOrderTriggerReject RejectInfo { get; set; } = null!;

    /// <summary>
    /// The listen key the update was for
    /// </summary>
    [JsonIgnore]
    public string ListenKey { get; set; } = string.Empty;
}

/// <summary>
/// Reject info
/// </summary>
public record BinanceConditionOrderTriggerReject
{
    /// <summary>
    /// The symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Order id
    /// </summary>
    [JsonProperty("i")]
    public long OrderId { get; set; }

    /// <summary>
    /// Reject reason
    /// </summary>
    [JsonProperty("r")]
    public string Reason { get; set; } = string.Empty;
}
