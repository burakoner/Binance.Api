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
    public long SubscriptionId { get; internal set; }
}
