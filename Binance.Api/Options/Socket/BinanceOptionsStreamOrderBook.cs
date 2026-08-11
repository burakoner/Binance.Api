namespace Binance.Api.Options;

/// <summary>
/// The order book for a asset
/// </summary>
public record BinanceOptionsStreamOrderBook : BinanceSocketStreamEvent
{
    /// <summary>
    /// Transaction Time
    /// </summary>
    [JsonProperty("T")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime TransactionTime { get; set; }

    /// <summary>
    /// The symbol of the order book 
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// First update ID in this event
    /// </summary>
    [JsonProperty("U")]
    public long FirstUpdateId { get; set; }

    /// <summary>
    /// Final update ID in this event
    /// </summary>
    [JsonProperty("u")]
    public long LastUpdateId { get; set; }

    /// <summary>
    /// Final update ID in the previous event
    /// </summary>
    [JsonProperty("pu")]
    public long PreviousLastUpdateId { get; set; }

    /// <summary>
    /// The list of bids
    /// </summary>
    [JsonProperty("b")]
    public List<BinanceOptionsOrderBookEntry> Bids { get; set; } = [];

    /// <summary>
    /// The list of asks
    /// </summary>
    [JsonProperty("a")]
    public List<BinanceOptionsOrderBookEntry> Asks { get; set; } = [];
}
