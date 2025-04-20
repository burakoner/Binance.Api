namespace Binance.Api.Spot.Responses;

/// <summary>
/// Price statistics of the last 24 hours
/// </summary>
public class BinanceFullTicker : BinanceMiniTicker
{
    /// <summary>
    /// The actual price change in the last 24 hours
    /// </summary>
    public decimal PriceChange { get; set; }

    /// <summary>
    /// The price change in percentage in the last 24 hours
    /// </summary>
    public decimal PriceChangePercent { get; set; }

    /// <summary>
    /// The weighted average price in the last 24 hours
    /// </summary>
    [JsonProperty("weightedAvgPrice")]
    public decimal WeightedAveragePrice { get; set; }

    /// <summary>
    /// The close price 24 hours ago
    /// </summary>
    [JsonProperty("prevClosePrice")]
    public decimal PrevDayClosePrice { get; set; }

    /// <summary>
    /// The most recent trade quantity
    /// </summary>
    [JsonProperty("lastQty")]
    public decimal LastQuantity { get; set; }

    /// <summary>
    /// The best bid price in the order book
    /// </summary>
    [JsonProperty("bidPrice")]
    public decimal BestBidPrice { get; set; }

    /// <summary>
    /// The quantity of the best bid price in the order book
    /// </summary>
    [JsonProperty("bidQty")]
    public decimal BestBidQuantity { get; set; }

    /// <summary>
    /// The best ask price in the order book
    /// </summary>
    [JsonProperty("askPrice")]
    public decimal BestAskPrice { get; set; }

    /// <summary>
    /// The quantity of the best ask price in the order book
    /// </summary>
    [JsonProperty("AskQty")]
    public decimal BestAskQuantity { get; set; }
}
