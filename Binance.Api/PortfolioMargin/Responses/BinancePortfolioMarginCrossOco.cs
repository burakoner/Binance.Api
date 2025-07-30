namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Cross OCO (One Cancels Other) response
/// </summary>
public record BinancePortfolioMarginCrossOco
{
    /// <summary>
    /// Order List ID
    /// </summary>
    [JsonProperty("orderListId")]
    public long OrderListId { get; set; }

    /// <summary>
    /// Contingency Type
    /// </summary>
    [JsonProperty("contingencyType")]
    public string ContingencyType { get; set; } = ""; // TODO: Enum

    /// <summary>
    /// List Status Type
    /// </summary>
    [JsonProperty("listStatusType")]
    public string ListStatusType { get; set; } = ""; // TODO: Enum

    /// <summary>
    /// List Order Status
    /// </summary>
    [JsonProperty("listOrderStatus")]
    public string ListOrderStatus { get; set; } = ""; // TODO: Enum

    /// <summary>
    /// List Client Order ID
    /// </summary>
    [JsonProperty("listClientOrderId")]
    public string ListClientOrderId { get; set; } = "";

    /// <summary>
    /// Transaction Time
    /// </summary>
    [JsonProperty("transactionTime")]
    public DateTime TransactionTime { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Oorders
    /// </summary>
    [JsonProperty("orders")]
    public List<BinancePortfolioMarginCrossOcoOrder> Orders { get; set; } = [];

    /// <summary>
    /// Reports
    /// </summary>
    [JsonProperty("orderReports")]
    public List<BinancePortfolioMarginCrossOcoReport>? Reports { get; set; } = [];
}

/// <summary>
/// Binance Portfolio Margin Cross OCO Order
/// </summary>
public record BinancePortfolioMarginCrossOcoOrder
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
}

/// <summary>
/// Binance Portfolio Margin Cross OCO Report
/// </summary>
public record BinancePortfolioMarginCrossOcoReport
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
    /// Order List ID
    /// </summary>
    [JsonProperty("orderListId")]
    public long OrderListId { get; set; }

    /// <summary>
    /// Client Order ID
    /// </summary>
    [JsonProperty("clientOrderId")]
    public string ClientOrderId { get; set; } = "";

    /// <summary>
    /// Transaction Time
    /// </summary>
    [JsonProperty("transactTime")]
    public DateTime TransactTime { get; set; }

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
    public string Status { get; set; } = ""; // TODO: Enum

    /// <summary>
    /// Time in Force
    /// </summary>
    [JsonProperty("timeInForce")]
    public BinanceTimeInForce TimeInForce { get; set; }

    /// <summary>
    /// Type
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; } = ""; // TODO: Enum

    /// <summary>
    /// Side
    /// </summary>
    [JsonProperty("side")]
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// Stop Price
    /// </summary>
    [JsonProperty("stopPrice")]
    public decimal? StopPrice { get; set; }
}