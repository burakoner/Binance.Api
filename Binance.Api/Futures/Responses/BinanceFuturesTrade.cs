namespace Binance.Api.Futures;

/// <summary>
/// Trade info
/// </summary>
public record BinanceFuturesTrade
{
    /// <summary>
    /// Trade id
    /// </summary>
    [JsonProperty("id")]
    public long Id { get; set; }

    /// <summary>
    /// Price
    /// </summary>
    [JsonProperty("price")]
    public decimal Price { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("qty")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Whether the buyer is maker
    /// </summary>
    [JsonProperty("isBuyerMaker")]
    public bool IsBuyerMaker { get; set; }
}

/// <summary>
/// Trade details
/// </summary>
public record BinanceFuturesUsdTrade: BinanceFuturesTrade
{
    /// <summary>
    /// Quote quantity
    /// </summary>
    [JsonProperty("quoteQty")]
    public decimal QuoteQuantity { get; set; }
}

/// <summary>
/// Trade details
/// </summary>
public record BinanceFuturesCoinTrade : BinanceFuturesTrade
{
    /// <summary>
    /// Base quantity
    /// </summary>
    [JsonProperty("baseQty")]
    public decimal BaseQuantity { get; set; }
}
