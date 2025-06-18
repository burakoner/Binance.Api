namespace Binance.Api.PortfolioMargin;

public record BinancePortfolioMarginCrossOco
{
    [JsonProperty("orderListId")]
    public long OrderListId { get; set; }

    [JsonProperty("contingencyType")]
    public string ContingencyType { get; set; } // TODO: Enum

    [JsonProperty("listStatusType")]
    public string ListStatusType { get; set; } // TODO: Enum

    [JsonProperty("listOrderStatus")]
    public string ListOrderStatus { get; set; } // TODO: Enum

    [JsonProperty("listClientOrderId")]
    public string ListClientOrderId { get; set; }

    [JsonProperty("transactionTime")]
    public DateTime TransactionTime { get; set; }

    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("orders")]
    public List<BinancePortfolioMarginCrossOcoOrder> Orders { get; set; } = [];

    [JsonProperty("orderReports")]
    public List<BinancePortfolioMarginCrossOcoReport>? Reports { get; set; } = [];
}

public record BinancePortfolioMarginCrossOcoOrder
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    [JsonProperty("clientOrderId")]
    public string ClientOrderId { get; set; }
}

public record BinancePortfolioMarginCrossOcoReport
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    [JsonProperty("orderListId")]
    public long OrderListId { get; set; }

    [JsonProperty("clientOrderId")]
    public string ClientOrderId { get; set; }

    [JsonProperty("transactTime")]
    public DateTime TransactTime { get; set; }

    [JsonProperty("price")]
    public decimal? Price { get; set; }

    [JsonProperty("origQty")]
    public decimal OriginalQuantity { get; set; }

    [JsonProperty("executedQty")]
    public decimal ExecutedQuantity { get; set; }

    [JsonProperty("cummulativeQuoteQty")]
    public decimal CummulativeQuoteQuantity { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; } // TODO: Enum

    [JsonProperty("timeInForce")]
    public BinanceTimeInForce TimeInForce { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; } // TODO: Enum

    [JsonProperty("side")]
    public BinanceOrderSide Side { get; set; }

    [JsonProperty("stopPrice")]
    public decimal? StopPrice { get; set; }
} 