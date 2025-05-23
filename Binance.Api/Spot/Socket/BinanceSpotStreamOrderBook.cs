﻿namespace Binance.Api.Spot;

/// <summary>
/// Stream order book
/// </summary>
public record BinanceSpotStreamOrderBook : BinanceSpotOrderBook
{
    /// <summary>
    /// The id of this update, can be synced with BinanceClient.Spot.GetOrderBook to update the order book
    /// </summary>
    [JsonProperty("U")]
    public long? FirstUpdateId { get; set; }

    /// <summary>
    /// Setter for last update id, need for Json.Net
    /// </summary>
    [JsonProperty("u")]
    internal long LastUpdateIdStream { set => LastUpdateId = value; }

    /// <summary>
    /// Event type
    /// </summary>
    [JsonProperty("e")]
    internal string EventType { get; set; } = "";

    /// <summary>
    /// Event time of the update
    /// </summary>
    [JsonProperty("E"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime EventTime { get; set; }

    /// <summary>
    /// Setter for bids (needed forJson.Net)
    /// </summary>
    [JsonProperty("b")]
    internal List<BinanceSpotOrderBookEntry> BidsStream { set => Bids = value; }

    /// <summary>
    /// Setter for asks (needed forJson.Net)
    /// </summary>
    [JsonProperty("a")]
    internal List<BinanceSpotOrderBookEntry> AsksStream { set => Asks = value; }
}
