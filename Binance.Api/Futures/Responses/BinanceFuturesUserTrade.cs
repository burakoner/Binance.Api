namespace Binance.Api.Futures;

/// <summary>
/// Trade info
/// </summary>
public record BinanceFuturesUserTrade
{
    /// <summary>
    /// The symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Is buyer
    /// </summary>
    [JsonProperty("buyer")]
    public bool Buyer { get; set; }

    /// <summary>
    /// Paid fee
    /// </summary>
    [JsonProperty("commission")]
    public decimal Fee { get; set; }

    /// <summary>
    /// Asset the fee is paid in
    /// </summary>
    [JsonProperty("commissionAsset")]
    public string FeeAsset { get; set; } = string.Empty;

    /// <summary>
    /// Trade id
    /// </summary>
    [JsonProperty("id")]
    public long Id { get; set; }

    /// <summary>
    /// Is maker
    /// </summary>
    [JsonProperty("maker")]
    public bool Maker { get; set; }

    /// <summary>
    /// Order id
    /// </summary>
    [JsonProperty("orderId")]
    public long OrderId { get; set; }

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
    /// Realized pnl
    /// </summary>
    [JsonProperty("realizedPnl")]
    public decimal RealizedPnl { get; set; }

    /// <summary>
    /// Order side
    /// </summary>
    [JsonProperty("side")]
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// Position side
    /// </summary>
    [JsonProperty("positionSide")]
    public BinancePositionSide PositionSide { get; set; }

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Trade details
/// </summary>
public record BinanceFuturesUsdUserTrade: BinanceFuturesUserTrade
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
public record BinanceFuturesCoinUserTrade : BinanceFuturesUserTrade
{
    /// <summary>
    /// The pair
    /// </summary>
    [JsonProperty("pair")]
    public string Pair { get; set; } = string.Empty;

    /// <summary>
    /// The margin asset
    /// </summary>
    [JsonProperty("marginAsset")]
    public string MarginAsset { get; set; } = string.Empty;

    /// <summary>
    /// Base quantity
    /// </summary>
    [JsonProperty("baseQty")]
    public decimal BaseQuantity { get; set; }
}
