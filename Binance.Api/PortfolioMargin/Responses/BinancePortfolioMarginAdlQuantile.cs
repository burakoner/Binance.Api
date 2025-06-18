namespace Binance.Api.PortfolioMargin;

public record BinancePortfolioMarginAdlQuantile
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("adlQuantile")]
    public Dictionary<BinancePositionSide,int> AdlQuantile { get; set; }
}

public record BinancePortfolioMarginAdlQuantileCM: BinancePortfolioMarginAdlQuantile
{
}

public record BinancePortfolioMarginAdlQuantileUM : BinancePortfolioMarginAdlQuantile
{
}