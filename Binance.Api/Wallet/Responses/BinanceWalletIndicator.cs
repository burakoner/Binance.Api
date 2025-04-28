namespace Binance.Api.Wallet;

/// <summary>
/// Indicator info
/// </summary>
public record BinanceWalletIndicator
{
    /// <summary>
    /// Indicator name
    /// </summary>
    [JsonProperty("i")]
    public BinanceWalletIndicatorType IndicatorType { get; set; }

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
