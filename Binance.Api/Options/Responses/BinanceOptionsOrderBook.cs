namespace Binance.Api.Options;

/// <summary>
/// The order book for a asset
/// </summary>
public record BinanceOptionsOrderBook
{
    /// <summary>
    /// Transaction Time
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime TransactionTime { get; set; }

    /// <summary>
    /// The ID of the last update
    /// </summary>
    [JsonProperty("lastUpdateId")]
    public long LastUpdateId { get; set; }

    /// <summary>
    /// The list of bids
    /// </summary>
    public List<BinanceOptionsOrderBookEntry> Bids { get; set; } = [];

    /// <summary>
    /// The list of asks
    /// </summary>
    public List<BinanceOptionsOrderBookEntry> Asks { get; set; } = [];
}

/// <summary>
/// An entry in the order book
/// </summary>
[JsonConverter(typeof(ArrayConverter))]
public record BinanceOptionsOrderBookEntry
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
