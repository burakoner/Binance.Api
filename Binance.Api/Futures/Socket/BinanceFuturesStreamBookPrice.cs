namespace Binance.Api.Futures;

/// <summary>
/// Futures book price
/// </summary>
public record BinanceFuturesStreamBookPrice
{
    /// <summary>
    /// Update id
    /// </summary>
    [JsonProperty("u")]
    public long UpdateId { get; set; }

    /// <summary>
    /// The symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Price of the best bid
    /// </summary>
    [JsonProperty("b")]
    public decimal BestBidPrice { get; set; }

    /// <summary>
    /// Quantity of the best bid
    /// </summary>
    [JsonProperty("B")]
    public decimal BestBidQuantity { get; set; }

    /// <summary>
    /// Price of the best ask
    /// </summary>
    [JsonProperty("a")]
    public decimal BestAskPrice { get; set; }

    /// <summary>
    /// Quantity of the best ask
    /// </summary>
    [JsonProperty("A")]
    public decimal BestAskQuantity { get; set; }

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime? TransactionTime { get; set; }

    /// <summary>
    /// The time the event happened
    /// </summary>
    [JsonProperty("E"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime EventTime { get; set; }

    /// <summary>
    /// The type of the event
    /// </summary>
    [JsonProperty("e")] 
    public string Event { get; set; } = string.Empty;
}
