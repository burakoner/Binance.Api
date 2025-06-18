namespace Binance.Api.PortfolioMargin;

public record BinancePortfolioMarginCommissionRate
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    [JsonProperty("makerCommissionRate")]
    public decimal MakerCommissionRate { get; set; }

    [JsonProperty("takerCommissionRate")]
    public decimal TakerCommissionRate { get; set; }
}

public record BinancePortfolioMarginCommissionRateCM : BinancePortfolioMarginCommissionRate
{
}

public record BinancePortfolioMarginCommissionRateUM : BinancePortfolioMarginCommissionRate
{
}