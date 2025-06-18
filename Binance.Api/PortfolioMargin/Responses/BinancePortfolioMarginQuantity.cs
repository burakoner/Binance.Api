namespace Binance.Api.PortfolioMargin;

public  record BinancePortfolioMarginQuantity
{
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
}