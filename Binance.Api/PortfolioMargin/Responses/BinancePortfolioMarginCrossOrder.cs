namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Cross Order
/// </summary>
public record BinancePortfolioMarginCrossOrder
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Order ID
    /// </summary>
    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    /// <summary>
    /// Client Order ID
    /// </summary>
    [JsonProperty("clientOrderId")]
    public string ClientOrderId { get; set; } = "";

    /// <summary>
    /// Price
    /// </summary>
    [JsonProperty("price")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Original Quantity
    /// </summary>
    [JsonProperty("origQty")]
    public decimal OriginalQuantity { get; set; }

    /// <summary>
    /// Executed Quantity
    /// </summary>
    [JsonProperty("executedQty")]
    public decimal ExecutedQuantity { get; set; }

    /// <summary>
    /// Cummulative Quote Quantity
    /// </summary>
    [JsonProperty("cummulativeQuoteQty")]
    public decimal CummulativeQuoteQuantity { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public BinancePortfolioMarginOrderStatus Status { get; set; }

    /// <summary>
    /// Time in Force
    /// </summary>
    [JsonProperty("timeInForce")]
    public BinanceTimeInForce TimeInForce { get; set; }

    /// <summary>
    /// Type
    /// </summary>
    [JsonProperty("type")]
    public BinancePortfolioMarginOrderType Type { get; set; }

    /// <summary>
    /// Side
    /// </summary>
    [JsonProperty("side")]
    public BinanceOrderSide Side { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Cross Order Fill
/// </summary>
public record BinancePortfolioMarginCrossOrderFill
{
    /// <summary>
    /// Price
    /// </summary>
    [JsonProperty("price")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("qty")]
    public decimal? Quantity { get; set; }

    /// <summary>
    /// Commission
    /// </summary>
    [JsonProperty("commission")]
    public decimal? Commission { get; set; }

    /// <summary>
    /// Commission Asset
    /// </summary>
    [JsonProperty("commissionAsset")]
    public string CommissionAsset { get; set; } = "";
}

/// <summary>
/// Binance Portfolio Margin Cross Order Placed
/// </summary>
public record BinancePortfolioMarginCrossOrderPlaced : BinancePortfolioMarginCrossOrder
{
    /// <summary>
    /// Transaction Time
    /// </summary>
    [JsonProperty("transactTime")]
    public DateTime TransactionTime { get; set; }

    /// <summary>
    /// Margin Buy Borrow Quantity
    /// </summary>
    [JsonProperty("marginBuyBorrowAmount")]
    public decimal MarginBuyBorrowQuantity { get; set; }

    /// <summary>
    /// Margin Buy Borrow Asset
    /// </summary>
    [JsonProperty("marginBuyBorrowAsset")]
    public string MarginBuyBorrowAsset { get; set; } = "";

    /// <summary>
    /// Is Isolated
    /// </summary>
    [JsonProperty("isIsolated")]
    public bool IsIsolated { get; set; }

    /// <summary>
    /// Fills
    /// </summary>
    [JsonProperty("fills")]
    public List<BinancePortfolioMarginCrossOrderFill> Fills { get; set; } = [];
}

/// <summary>
/// Binance Portfolio Margin Cross Order Canceled
/// </summary>
public record BinancePortfolioMarginCrossOrderCanceled : BinancePortfolioMarginCrossOrder
{
    /// <summary>
    /// Original Client Order ID
    /// </summary>
    [JsonProperty("origClientOrderId")]
    public string OriginalClientOrderId { get; set; } = "";

}

/// <summary>
/// Binance Portfolio Margin Cross Order Query
/// </summary>
public record BinancePortfolioMarginCrossOrderQuery : BinancePortfolioMarginCrossOrder
{
    /// <summary>
    /// Iceberg Quantity
    /// </summary>
    [JsonProperty("icebergQty")]
    public decimal? IcebergQuantity { get; set; }

    /// <summary>
    /// Is Working
    /// </summary>
    [JsonProperty("isWorking")]
    public bool IsWorking { get; set; }

    /// <summary>
    /// Stop Price
    /// </summary>
    [JsonProperty("stopPrice")]
    public decimal? StopPrice { get; set; }

    /// <summary>
    /// Time
    /// </summary>
    [JsonProperty("time")]
    public DateTime Time { get; set; }

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// Account ID
    /// </summary>
    [JsonProperty("accountId")]
    public long AccountId { get; set; }

    /// <summary>
    /// Self Trade Prevention Mode
    /// </summary>
    [JsonProperty("selfTradePreventionMode")]
    public BinanceSelfTradePreventionMode SelfTradePreventionMode { get; set; }

    /// <summary>
    /// Prevented Match ID
    /// </summary>
    [JsonProperty("preventedMatchId")]
    public long? PreventedMatchId { get; set; }

    /// <summary>
    /// Prevented Quantity
    /// </summary>
    [JsonProperty("preventedQuantity")]
    public decimal? PreventedQuantity { get; set; }
}