namespace Binance.Api.Options;

/// <summary>
/// Binance Options Market Maker Block Trade Leg
/// </summary>
public record BinanceOptionsMarketMakerBlockTradeLeg
{
    /// <summary>
    /// Create Time
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Update Time
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Order ID
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// Order Price
    /// </summary>
    public decimal OrderPrice { get; set; }

    /// <summary>
    /// Order Quantity
    /// </summary>
    public decimal OrderQuantity { get; set; }

    /// <summary>
    /// Order Status
    /// </summary>
    public string OrderStatus { get; set; } = "";

    /// <summary>
    /// Executed Quantity
    /// </summary>
    [JsonProperty("executedQty")]
    public decimal ExecutedQuantity { get; set; }

    /// <summary>
    /// Executed Amount (Executed Quantity in Quote Asset)
    /// </summary>
    [JsonProperty("executedAmount")]
    public decimal ExecutedAmount { get; set; }

    /// <summary>
    /// Fee
    /// </summary>
    public decimal Fee { get; set; }

    /// <summary>
    /// Order Type
    /// </summary>
    public string OrderType { get; set; } = "";

    /// <summary>
    /// Order Side
    /// </summary>
    public BinanceOrderSide OrderSide { get; set; } 

    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Trade Id
    /// </summary>
    public long TradeId { get; set; }

    /// <summary>
    /// Trade Price
    /// </summary>
    public decimal tradePrice { get; set; }

    /// <summary>
    /// Trade Quantity
    /// </summary>
    [JsonProperty("tradeQty")]
    public decimal TradeQuantity { get; set; }

    /// <summary>
    /// Trade Time
    /// </summary>
    public DateTime TradeTime { get; set; }

    /// <summary>
    /// Liquidity
    /// </summary>
    public BinanceOptionsLiquidity Liquidity { get; set; }

    /// <summary>
    /// Commission
    /// </summary>
    public decimal Commission { get; set; }
}