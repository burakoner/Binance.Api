namespace Binance.Api.PortfolioMargin;

public  record BinancePortfolioMarginCrossForceOrder
{
    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("price")]
    public decimal? Price { get; set; }

    [JsonProperty("avgPrice")]
    public decimal? AveragePrice { get; set; }

    [JsonProperty("qty")]
    public decimal Quantity { get; set; }

    [JsonProperty("executedQty")]
    public decimal ExecutedQuantity { get; set; }

    [JsonProperty("timeInForce")]
    public BinanceTimeInForce TimeInForce { get; set; }

    [JsonProperty("side")]
    public BinanceOrderSide Side { get; set; }

    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }
}