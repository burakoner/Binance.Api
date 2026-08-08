namespace Binance.Api.Margin;

/// <summary>
/// Cross Margin risk-level status update.
/// </summary>
public record BinanceMarginRiskLevelUpdate : BinanceSocketStreamEvent
{
    /// <summary>
    /// Current margin level.
    /// </summary>
    [JsonProperty("l")]
    public decimal MarginLevel { get; set; }

    /// <summary>
    /// Current margin-call status.
    /// </summary>
    [JsonProperty("s")]
    public string Status { get; set; } = string.Empty;
}
