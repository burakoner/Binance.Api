namespace Binance.Api.Futures;

/// <summary>
/// Aggregated information about trades for a symbol
/// </summary>
public record BinanceFuturesStreamTrade : BinanceFuturesStreamEvent
{
    /// <summary>
    /// The symbol the trade was for
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The id of this trade
    /// </summary>
    [JsonProperty("t")]
    public long Id { get; set; }

    /// <summary>
    /// The price of the trades
    /// </summary>
    [JsonProperty("p")]
    public decimal Price { get; set; }

    /// <summary>
    /// The quantity of the trade
    /// </summary>
    [JsonProperty("q")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// The buyer order id
    /// </summary>
    [JsonProperty("X")]
    public string Type { get; set; } = "";

    /// <summary>
    /// Whether the buyer was the maker
    /// </summary>
    [JsonProperty("m")]
    public bool BuyerIsMaker { get; set; }
}
