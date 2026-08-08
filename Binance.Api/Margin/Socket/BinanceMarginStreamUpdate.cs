namespace Binance.Api.Margin;

/// <summary>
/// Margin user data stream lifecycle event.
/// </summary>
public record BinanceMarginStreamUpdate : BinanceSocketStreamEvent
{
    /// <summary>
    /// Server-assigned subscription identifier.
    /// </summary>
    [JsonIgnore]
    public int SubscriptionId { get; internal set; }
}
