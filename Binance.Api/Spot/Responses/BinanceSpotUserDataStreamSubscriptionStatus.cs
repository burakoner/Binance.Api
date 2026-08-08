namespace Binance.Api.Spot;

/// <summary>
/// An active Spot WebSocket API user data stream subscription.
/// </summary>
public record BinanceSpotUserDataStreamSubscriptionStatus
{
    /// <summary>
    /// Server-assigned subscription identifier.
    /// </summary>
    [JsonProperty("subscriptionId")]
    public long SubscriptionId { get; set; }
}
