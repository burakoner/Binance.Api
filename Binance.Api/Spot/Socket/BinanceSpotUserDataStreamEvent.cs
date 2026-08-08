namespace Binance.Api.Spot;

/// <summary>
/// Base type for Spot user data stream events received through the WebSocket API.
/// </summary>
public record BinanceSpotUserDataStreamEvent : BinanceSocketStreamEvent
{
    /// <summary>
    /// Server-assigned identifier of the user data stream subscription.
    /// </summary>
    [JsonIgnore]
    public long SubscriptionId { get; internal set; }
}
