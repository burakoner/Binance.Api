namespace Binance.Api.Spot;

/// <summary>
/// User data stream event
/// </summary>
public record BinanceSpotStreamUpdate : BinanceSocketStreamEvent
{
    /// <summary>
    /// The listen key the update was for
    /// </summary>
    [JsonIgnore]
    public string ListenKey { get; set; } = string.Empty;
}
