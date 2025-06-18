namespace Binance.Api.PortfolioMargin;

public record BinancePortfolioMarginInitialLeverage
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("leverage")]
    public decimal Leverage { get; set; }
}

public record BinancePortfolioMarginInitialLeverageCM : BinancePortfolioMarginPositionRisk
{
    [JsonProperty("maxQty")]
    public decimal MaximumQuantity { get; set; }
}

public record BinancePortfolioMarginInitialLeverageUM : BinancePortfolioMarginPositionRisk
{
    [JsonProperty("maxNotionalValue")]
    public decimal MaximumNotional { get; set; }
}