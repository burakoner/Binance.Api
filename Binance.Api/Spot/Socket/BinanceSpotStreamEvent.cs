namespace Binance.Api.Spot;

/// <summary>
/// A event received by a Binance websocket
/// </summary>
public record BinanceSpotStreamEvent
{
    /// <summary>
    /// The type of the event
    /// </summary>
    [JsonProperty("e")]
    public string Event { get; set; } = string.Empty;

    /// <summary>
    /// The time the event happened
    /// </summary>
    [JsonProperty("E"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime EventTime { get; set; }

    /// <summary>
    /// The listen key the update was for
    /// </summary>
    [JsonIgnore]
    public string ListenKey { get; set; } = string.Empty;
}
