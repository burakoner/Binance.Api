namespace Binance.Api.PortfolioMargin;

public  record BinancePortfolioMarginForceOrder
{
    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("status")]
    public BinancePortfolioMarginOrderStatus Status { get; set; }

    [JsonProperty("clientOrderId")]
    public string ClientOrderId { get; set; } = "";

    [JsonProperty("price")]
    public decimal? Price { get; set; }

    [JsonProperty("avgPrice")]
    public decimal? AveragePrice { get; set; }

    [JsonProperty("origQty")]
    public decimal OriginalQuantity { get; set; }

    [JsonProperty("executedQty")]
    public decimal ExecutedQuantity { get; set; }

    [JsonProperty("timeInForce")]
    public BinanceTimeInForce TimeInForce { get; set; }

    [JsonProperty("type")]
    public BinancePortfolioMarginOrderType Type { get; set; }

    [JsonProperty("reduceOnly")]
    public bool ReduceOnly { get; set; }

    [JsonProperty("side")]
    public BinanceOrderSide Side { get; set; }

    [JsonProperty("positionSide")]
    public BinancePositionSide PositionSide { get; set; }

    [JsonProperty("origType")]
    public BinancePortfolioMarginOrderType OriginalType { get; set; }

    [JsonProperty("time")]
    public DateTime Time { get; set; }

    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }
}

public  record BinancePortfolioMarginForceOrderCM: BinancePortfolioMarginForceOrder
{
    [JsonProperty("pair")]
    public string Pair { get; set; } = "";

    [JsonProperty("cumBase")]
    public decimal CummulativeBaseQuantity { get; set; }

}

public  record BinancePortfolioMarginForceOrderUM : BinancePortfolioMarginForceOrder
{
    [JsonProperty("cumQuote")]
    public decimal CummulativeQuoteQuantity { get; set; }
}