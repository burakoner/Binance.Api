namespace Binance.Api.Spot;

/// <summary>
/// Historical Spot block trade.
/// </summary>
public record BinanceSpotBlockTrade
{
    /// <summary>
    /// Block trade ID.
    /// </summary>
    [JsonProperty("id")]
    public long TradeId { get; set; }

    /// <summary>
    /// Trade price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Base asset quantity.
    /// </summary>
    [JsonProperty("qty")]
    public decimal BaseQuantity { get; set; }

    /// <summary>
    /// Quote asset quantity.
    /// </summary>
    [JsonProperty("quoteQty")]
    public decimal QuoteQuantity { get; set; }

    /// <summary>
    /// Trade time.
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime TradeTime { get; set; }

    /// <summary>
    /// Whether the buyer was the maker.
    /// </summary>
    public bool IsBuyerMaker { get; set; }
}
