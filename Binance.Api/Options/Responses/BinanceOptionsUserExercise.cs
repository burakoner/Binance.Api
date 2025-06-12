namespace Binance.Api.Options;

/// <summary>
/// Options User Exercise Record
/// </summary>
public record BinanceOptionsUserExercise
{
    /// <summary>
    /// Unique Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Currency
    /// </summary>
    public string Currency { get; set; } = "";

    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Exercise Price
    /// </summary>
    [JsonProperty("exercisePrice")]
    public decimal ExercisePrice { get; set; }

    /// <summary>
    /// Mark price
    /// </summary>
    [JsonProperty("markPrice")]
    public decimal MarkPrice { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Fee
    /// </summary>
    public decimal Fee { get; set; }

    /// <summary>
    /// Create Date
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateDate { get; set; }

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
    /// Position Side
    /// </summary>
    public BinancePositionSide Side { get; set; }

    /// <summary>
    /// Quote Asset
    /// </summary>
    public string QuoteAsset { get; set; } = "";
}
