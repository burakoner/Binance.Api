namespace Binance.Api.Options;

/// <summary>
/// Options Order
/// </summary>
public record BinanceOptionsOrder
{
    /// <summary>
    /// Order Id
    /// </summary>
    [JsonProperty("orderId")]
    public long Id { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Price
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Executed Quantity
    /// </summary>
    [JsonProperty("executedQty")]
    public decimal? ExecutedQuantity { get; set; }

    /// <summary>
    /// Fee
    /// </summary>
    public decimal? Fee { get; set; }

    /// <summary>
    /// Order Side
    /// </summary>
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// Order Type
    /// </summary>
    public BinanceOptionsOrderType Type { get; set; }

    /// <summary>
    /// Order Time In Force
    /// </summary>
    public BinanceTimeInForce? TimeInForce { get; set; }

    /// <summary>
    /// Reduce Only
    /// </summary>
    public bool ReduceOnly { get; set; }

    /// <summary>
    /// Post Only
    /// </summary>
    public bool PostOnly { get; set; }

    /// <summary>
    /// Creation Time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? CreateTime { get; set; }

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// Order Status
    /// </summary>
    public string Status { get; set; } = "";

    /// <summary>
    /// Average Price
    /// </summary>
    [JsonProperty("avgPrice")]
    public decimal? AveragePrice { get; set; }

    /// <summary>
    /// Client Order Id
    /// </summary>
    public string ClientOrderId { get; set; } = "";

    /// <summary>
    /// Price Scale
    /// </summary>
    public int? PriceScale { get; set; }

    /// <summary>
    /// Quantity Scale
    /// </summary>
    public int? QuantityScale { get; set; }

    /// <summary>
    /// Option Side
    /// </summary>
    [JsonProperty("optionSide")]
    public BinanceOptionsSide? OptionSide { get; set; }

    /// <summary>
    /// Quote Asset
    /// </summary>
    public string QuoteAsset { get; set; } = "";

    /// <summary>
    /// Is Maker Maker Protection Order
    /// </summary>
    public bool? MMP { get; set; }
}
