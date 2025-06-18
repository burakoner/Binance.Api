namespace Binance.Api.PortfolioMargin;

public  record BinancePortfolioMarginCrossBorrowable
{
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    [JsonProperty("borrowLimit")]
    public decimal Limit { get; set; }
}