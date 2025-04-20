namespace Binance.Api.Models.RestApi.Account;

/// <summary>
/// Indicator info
/// </summary>
public record BinanceIndicator
{
    /// <summary>
    /// Indicator name
    /// </summary>
    [JsonProperty("i")]
    public IndicatorType IndicatorType { get; set; }

    /// <summary>
    /// Count
    /// </summary>
    [JsonProperty("c")]
    public int Count { get; set; }
    /// <summary>
    /// Current value
    /// </summary>
    [JsonProperty("v")]
    public decimal CurrentValue { get; set; }
    /// <summary>
    /// Trigger value
    /// </summary>
    [JsonProperty("t")]
    public decimal TriggerValue { get; set; }
}
