namespace Binance.Api.Models.RestApi.MarketData;

/// <summary>
/// The order book for a asset
/// </summary>
public record BinanceOrderBook : IBinanceOrderBook
{
    /// <summary>
    /// The symbol of the order book 
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The ID of the last update
    /// </summary>
    [JsonProperty("lastUpdateId")]
    public long LastUpdateId { get; set; }

    /// <summary>
    /// The list of bids
    /// </summary>
    public IEnumerable<BinanceOrderBookEntry> Bids { get; set; } = [];

    /// <summary>
    /// The list of asks
    /// </summary>
    public IEnumerable<BinanceOrderBookEntry> Asks { get; set; } = [];
}
