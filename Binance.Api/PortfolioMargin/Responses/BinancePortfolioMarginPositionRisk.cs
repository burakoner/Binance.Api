namespace Binance.Api.PortfolioMargin;

public record BinancePortfolioMarginPositionRisk
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("positionAmt")]
    public decimal PositionQuantity { get; set; }

    [JsonProperty("entryPrice")]
    public decimal EntryPrice { get; set; }

    [JsonProperty("markPrice")]
    public decimal MarkPrice { get; set; }

    [JsonProperty("unRealizedProfit")]
    public decimal UnRealizedProfit { get; set; }

    [JsonProperty("liquidationPrice")]
    public decimal LiquidationPrice { get; set; }

    [JsonProperty("leverage")]
    public decimal Leverage { get; set; }

    [JsonProperty("positionSide")]
    public BinancePositionSide PositionSide { get; set; }

    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }

}

public record BinancePortfolioMarginPositionRiskCM : BinancePortfolioMarginPositionRisk
{
    [JsonProperty("maxQty")]
    public decimal MaximumQuantity { get; set; }

    [JsonProperty("notionalValue")]
    public decimal Notional { get; set; }
}

public record BinancePortfolioMarginPositionRiskUM : BinancePortfolioMarginPositionRisk
{
    [JsonProperty("maxNotionalValue")]
    public decimal MaximumNotional { get; set; }

    [JsonProperty("notional")]
    public decimal Notional { get; set; }
}