namespace Binance.Api.PortfolioMargin;

public  record BinancePortfolioMarginAutoRepay
{
    [JsonProperty("autoRepay")]
    public bool AutoRepay { get; set; }
}