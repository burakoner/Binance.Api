namespace Binance.Api.PortfolioMargin;

public record BinancePortfolioMarginPositionMode
{
    [JsonProperty("dualSidePosition")]
    public bool DualSidePosition { get; set; }
}

public record BinancePortfolioMarginPositionModeCM : BinancePortfolioMarginPositionMode
{
}

public record BinancePortfolioMarginPositionModeUM : BinancePortfolioMarginPositionMode
{
}