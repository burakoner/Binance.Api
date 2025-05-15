﻿namespace Binance.Api.Futures;

/// <summary>
/// The order book for a asset
/// </summary>
public record BinanceFuturesStreamOrderBookDepth : BinanceFuturesStreamEvent
{
    /// <summary>
    /// The symbol of the order book (only filled from stream updates)
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// The time the event happened
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime TransactionTime { get; set; }

    /// <summary>
    /// The ID of the first update
    /// </summary>
    [JsonProperty("U")]
    public long? FirstUpdateId { get; set; }

    /// <summary>
    /// The ID of the last update
    /// </summary>
    [JsonProperty("u")]
    public long LastUpdateId { get; set; }

    /// <summary>
    /// The ID of the last update Id in last stream
    /// </summary>
    [JsonProperty("pu")]
    public long LastUpdateIdStream { get; set; }

    /// <summary>
    /// The list of diff bids
    /// </summary>
    [JsonProperty("b")]
    public List<BinanceFuturesOrderBookEntry> Bids { get; set; } = [];

    /// <summary>
    /// The list of diff asks
    /// </summary>
    [JsonProperty("a")]
    public List<BinanceFuturesOrderBookEntry> Asks { get; set; } = [];
}
