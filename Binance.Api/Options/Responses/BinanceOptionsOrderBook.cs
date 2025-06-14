﻿namespace Binance.Api.Options;

/// <summary>
/// The order book for a asset
/// </summary>
public record BinanceOptionsOrderBook
{
    /// <summary>
    /// The symbol of the order book 
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Transaction Time
    /// </summary>
    [JsonProperty("T")]
    public DateTime TransactionTime { get; set; }

    /// <summary>
    /// The ID of the last update
    /// </summary>
    [JsonProperty("u")]
    public long UpdateId { get; set; }

    /// <summary>
    /// The list of bids
    /// </summary>
    public List<BinanceSpotOrderBookEntry> Bids { get; set; } = [];

    /// <summary>
    /// The list of asks
    /// </summary>
    public List<BinanceSpotOrderBookEntry> Asks { get; set; } = [];
}

/// <summary>
/// An entry in the order book
/// </summary>
[JsonConverter(typeof(ArrayConverter))]
public record BinanceSpotOrderBookEntry
{
    /// <summary>
    /// The price of this order book entry
    /// </summary>
    [ArrayProperty(0)]
    public decimal Price { get; set; }

    /// <summary>
    /// The quantity of this price in the order book
    /// </summary>
    [ArrayProperty(1)]
    public decimal Quantity { get; set; }
}
