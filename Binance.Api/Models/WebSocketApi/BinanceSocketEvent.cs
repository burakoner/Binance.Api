namespace Binance.Api.Models.WebSocketApi;

/// <summary>
/// A event received by a Binance websocket
/// </summary>
public class BinanceSocketEvent
{
    /// <summary>
    /// The type of the event
    /// </summary>
    [JsonProperty("e")]
    public string Event { get; set; }
    /// <summary>
    /// The time the event happened
    /// </summary>
    [JsonProperty("E"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime EventTime { get; set; }
}
