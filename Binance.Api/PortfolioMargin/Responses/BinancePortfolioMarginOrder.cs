namespace Binance.Api.PortfolioMargin;

public  record BinancePortfolioMarginOrder
{
    [JsonProperty("clientOrderId")]
    public string ClientOrderId { get; set; } = "";

    [JsonProperty("cumQty")]
    public decimal CumulativeQuantity { get; set; }

    [JsonProperty("executedQty")]
    public decimal ExecutedQuantity { get; set; }

    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    [JsonProperty("avgPrice")]
    public decimal AveragePrice { get; set; }

    [JsonProperty("origQty")]
    public decimal OriginalQuantity { get; set; }

    [JsonProperty("price")]
    public decimal? Price { get; set; }

    [JsonProperty("reduceOnly")]
    public bool ReduceOnly { get; set; }

    [JsonProperty("side")]
    public BinanceOrderSide Side { get; set; }

    [JsonProperty("positionSide")]
    public BinancePositionSide PositionSide { get; set; }

    [JsonProperty("status")]
    public BinancePortfolioMarginOrderStatus Status { get; set; }

    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    [JsonProperty("timeInForce")]
    public BinanceTimeInForce TimeInForce { get; set; }

    [JsonProperty("type")]
    public BinancePortfolioMarginOrderType Type { get; set; }

    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }
}

public record BinancePortfolioMarginOrderCM : BinancePortfolioMarginOrder
{
    [JsonProperty("cumBase")]
    public decimal CumulativeBaseQuantity { get; set; }
}

public record BinancePortfolioMarginOrderUM : BinancePortfolioMarginOrder
{
    [JsonProperty("cumQuote")]
    public decimal CumulativeQuoteQuantity { get; set; }

    [JsonProperty("selfTradePreventionMode")]
    public BinanceSelfTradePreventionMode SelfTradePreventionMode { get; set; }

    [JsonProperty("goodTillDate")]
    public DateTime? GoodTillDate { get; set; }

    [JsonProperty("priceMatch")]
    public string PriceMatch { get; set; } // TODO: Enum
}