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
    /// Number of positions that can be reduced
    /// </summary>
    [JsonProperty("reducibleQty")]
    public decimal ReducibleQuantity { get; set; }

    /// <summary>
    /// Current market value
    /// </summary>
    [JsonProperty("markValue")]
    public decimal MarkValue { get; set; }

    /// <summary>
    /// Rate of return
    /// </summary>
    [JsonProperty("ror")]
    public decimal RateOfReturn { get; set; }

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
    /// Position Cost
    /// </summary>
    [JsonProperty("positionCost")]
    public decimal PositionCost { get; set; }

    /// <summary>
    /// Expiry Date
    /// </summary>
    [JsonProperty("expiryDate")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime ExpiryDate { get; set; }

    /// <summary>
    /// Price Scale
    /// </summary>
    public int PriceScale { get; set; }

    /// <summary>
    /// Quantity Scale
    /// </summary>
    public int QuantityScale { get; set; }

    /// <summary>
    /// Option Side
    /// </summary>
    [JsonProperty("optionSide")]
    public BinanceOptionsSide OptionSide { get; set; }

    /// <summary>
    /// Quote Asset
    /// </summary>
    public string QuoteAsset { get; set; } = "";
}
