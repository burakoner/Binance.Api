namespace Binance.Api.Options;

/// <summary>
/// Options Order Update
/// </summary>
public record BinanceOptionsStreamOrder : BinanceSocketStreamEvent
{
    /// <summary>
    /// The listen key the update was for
    /// </summary>
    [JsonIgnore]
    public string ListenKey { get; set; } = string.Empty;

    /// <summary>
    /// Creation Time
    /// </summary>
    [JsonProperty("T")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? CreateTime { get; set; }

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonProperty("t")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Client Order Id
    /// </summary>
    [JsonProperty("c")]
    public string ClientOrderId { get; set; } = "";

    /// <summary>
    /// Order Id
    /// </summary>
    [JsonProperty("oid")]
    public long Id { get; set; }

    /// <summary>
    /// Price
    /// </summary>
    [JsonProperty("p")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("q")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Reduce Only
    /// </summary>
    [JsonProperty("r")]
    public bool ReduceOnly { get; set; }

    /// <summary>
    /// Post Only
    /// </summary>
    [JsonProperty("po")]
    public bool PostOnly { get; set; }

    /// <summary>
    /// Order Status
    /// </summary>
    [JsonProperty("S")]
    public string Status { get; set; } = "";

    /// <summary>
    /// Executed Quantity
    /// </summary>
    [JsonProperty("e")]
    public decimal? ExecutedQuantity { get; set; }

    /// <summary>
    /// Executed Quantity
    /// </summary>
    [JsonProperty("ec")]
    public decimal? ExecutedEquity { get; set; }

    /// <summary>
    /// Fee
    /// </summary>
    [JsonProperty("f")]
    public decimal? Fee { get; set; }

    /// <summary>
    /// Order Time In Force
    /// </summary>
    [JsonProperty("tif")]
    public BinanceTimeInForce? TimeInForce { get; set; }

    /// <summary>
    /// Order Type
    /// </summary>
    [JsonProperty("oty")]
    public BinanceOptionsOrderType Type { get; set; }

    /// <summary>
    /// Fills
    /// </summary>
    [JsonProperty("oty")]
    public List<BinanceOptionsStreamOrderFill> Fills { get; set; } = [];
}

/// <summary>
/// Options Order Update Fills
/// </summary>
public record BinanceOptionsStreamOrderFill
{
    /// <summary>
    /// Trade Id
    /// </summary>
    [JsonProperty("t")]
    public long Id { get; set; }

    /// <summary>
    /// Trade Price
    /// </summary>
    [JsonProperty("p")]
    public decimal Price { get; set; }

    /// <summary>
    /// Trade Quantity
    /// </summary>
    [JsonProperty("q")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Trade Time
    /// </summary>
    [JsonProperty("T")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }

    /// <summary>
    /// Trade Liquidity
    /// </summary>
    [JsonProperty("m")]
    public BinanceOptionsLiquidity Liquidity { get; set; }

    /// <summary>
    /// Fee
    /// </summary>
    [JsonProperty("f")]
    public decimal Fee { get; set; }
}
