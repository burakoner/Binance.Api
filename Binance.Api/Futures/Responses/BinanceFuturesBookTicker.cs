namespace Binance.Api.Futures;

/// <summary>
/// Information about the best price/quantity available for a symbol
/// </summary>
public record BinanceFuturesBookTicker
{
    /// <summary>
    /// The symbol the information is about
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The highest bid price for the symbol
    /// </summary>
    [JsonProperty("bidPrice")]
    public decimal BestBidPrice { get; set; }

    /// <summary>
    /// The quantity of the highest bid price currently in the order book
    /// </summary>
    [JsonProperty("bidQty")]
    public decimal BestBidQuantity { get; set; }

    /// <summary>
    /// The lowest ask price for the symbol
    /// </summary>
    [JsonProperty("askPrice")]
    public decimal BestAskPrice { get; set; }

    /// <summary>
    /// The quantity of the lowest ask price currently in the order book
    /// </summary>
    [JsonProperty("askQty")]
    public decimal BestAskQuantity { get; set; }

    /// <summary>
    /// Transaction time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }
}

/// <summary>
/// Information about the best price/quantity available for a Coin-M Futures symbol
/// </summary>
public record BinanceFuturesCoinBookTicker: BinanceFuturesBookTicker
{
    /// <summary>
    /// Last update id
    /// </summary>
    public long LastUpdateId { get; set; }

    /// <summary>
    /// Name of the pair
    /// </summary>
    public string Pair { get; set; } = "";
}