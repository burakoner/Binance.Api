namespace Binance.Api.Spot;

/// <summary>
/// Recent trade with quote quantity
/// </summary>
public record BinanceSpotTrade
{
    /// <summary>
    /// The id of the trade
    /// </summary>
    [JsonProperty("id")]
    public long TradeId { get; set; }

    /// <summary>
    /// The price of the trade
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// The base quantity of the trade
    /// </summary>
    [JsonProperty("qty")]
    public decimal BaseQuantity { get; set; }

    /// <summary>
    /// The quote quantity of the trade
    /// </summary>
    [JsonProperty("quoteQty")]
    public decimal QuoteQuantity { get; set; }

    /// <summary>
    /// The timestamp of the trade
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime TradeTime { get; set; }

    /// <summary>
    /// Whether the buyer is maker
    /// </summary>
    public bool IsBuyerMaker { get; set; }

    /// <summary>
    /// Whether the trade was made at the best match
    /// </summary>
    public bool IsBestMatch { get; set; }
}