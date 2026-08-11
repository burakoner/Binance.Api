namespace Binance.Api.Options;

/// <summary>
/// Best bid and ask update for an Options symbol
/// </summary>
public record BinanceOptionsStreamBookTicker : BinanceSocketStreamEvent
{
    /// <summary>
    /// Order book update ID
    /// </summary>
    [JsonProperty("u")]
    public long UpdateId { get; set; }

    /// <summary>
    /// Options symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Best bid price
    /// </summary>
    [JsonProperty("b")]
    public decimal BestBidPrice { get; set; }

    /// <summary>
    /// Best bid quantity
    /// </summary>
    [JsonProperty("B")]
    public decimal BestBidQuantity { get; set; }

    /// <summary>
    /// Best ask price
    /// </summary>
    [JsonProperty("a")]
    public decimal BestAskPrice { get; set; }

    /// <summary>
    /// Best ask quantity
    /// </summary>
    [JsonProperty("A")]
    public decimal BestAskQuantity { get; set; }

    /// <summary>
    /// Transaction time
    /// </summary>
    [JsonProperty("T")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime TransactionTime { get; set; }
}
