namespace Binance.Api.Spot;

/// <summary>
/// Block-trade stream update.
/// </summary>
public record BinanceSpotStreamBlockTrade : BinanceSocketStreamEvent
{
    /// <summary>
    /// Symbol.
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Block trade ID.
    /// </summary>
    [JsonProperty("t")]
    public long TradeId { get; set; }

    /// <summary>
    /// Trade price.
    /// </summary>
    [JsonProperty("p")]
    public decimal Price { get; set; }

    /// <summary>
    /// Base asset quantity.
    /// </summary>
    [JsonProperty("q")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Trade time.
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime TradeTime { get; set; }

    /// <summary>
    /// Whether the buyer was the maker.
    /// </summary>
    [JsonProperty("m")]
    public bool BuyerIsMaker { get; set; }
}
