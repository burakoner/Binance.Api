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
    public DateTime TransactionTime { get; set; }

    /// <summary>
    /// The symbol of the order book 
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The ID of the last update
    /// </summary>
    [JsonProperty("u")]
    public long UpdateId { get; set; }

    /// <summary>
    /// The list of bids
    /// </summary>
    [JsonProperty("b")]
    public List<BinanceSpotOrderBookEntry> Bids { get; set; } = [];

    /// <summary>
    /// The list of asks
    /// </summary>
    [JsonProperty("a")]
    public List<BinanceSpotOrderBookEntry> Asks { get; set; } = [];
}