namespace Binance.Api.PortfolioMargin;

public record BinancePortfolioMarginBnbBurnStatus
{
    [JsonProperty("feeBurn")]
    public bool FeeBurn { get; set; }
}

public record BinancePortfolioMarginBnbBurnStatusCM : BinancePortfolioMarginBnbBurnStatus
{
}

public record BinancePortfolioMarginBnbBurnStatusUM : BinancePortfolioMarginBnbBurnStatus
{
}