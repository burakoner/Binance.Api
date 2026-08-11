namespace Binance.Api.Options;

/// <summary>
/// Options Position
/// </summary>
public record BinanceOptionsPosition
{
    /// <summary>
    /// Average Entry Price
    /// </summary>
    [JsonProperty("entryPrice")]
    public decimal AverageEntryPrice { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Position Side
    /// </summary>
    public BinancePositionSide Side { get; set; }

    /// <summary>
    /// Number of positions (positive numbers represent long positions, negative number represent short positions)
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Current market value
    /// </summary>
    [JsonProperty("markValue")]
    public decimal MarkValue { get; set; }

    /// <summary>
    /// Unrealized profit/loss
    /// </summary>
    [JsonProperty("unrealizedPNL")]
    public decimal UnrealizedPNL { get; set; }

    /// <summary>
    /// Mark price
    /// </summary>
    [JsonProperty("markPrice")]
    public decimal MarkPrice { get; set; }

    /// <summary>
    /// Strike Price
    /// </summary>
    [JsonProperty("strikePrice")]
    public decimal StrikePrice { get; set; }

    /// <summary>
    /// Expiry Date
    /// </summary>
    [JsonProperty("expiryDate")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime ExpiryDate { get; set; }

    /// <summary>
    /// Price Scale
    /// </summary>
    public long PriceScale { get; set; }

    /// <summary>
    /// Quantity Scale
    /// </summary>
    public long QuantityScale { get; set; }

    /// <summary>
    /// Option Side
    /// </summary>
    [JsonProperty("optionSide")]
    public BinanceOptionsSide OptionsSide { get; set; }

    /// <summary>
    /// Quote Asset
    /// </summary>
    public string QuoteAsset { get; set; } = "";

    /// <summary>
    /// Last update time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }

    /// <summary>
    /// Buy-order quantity
    /// </summary>
    public decimal BidQuantity { get; set; }

    /// <summary>
    /// Sell-order quantity
    /// </summary>
    public decimal AskQuantity { get; set; }
}
