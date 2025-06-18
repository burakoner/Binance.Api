namespace Binance.Api.PortfolioMargin;

public  record BinancePortfolioMarginNegativeBalanceInterest
{
    [JsonProperty("asset")]
    public string Asset { get; set; }

    [JsonProperty("interest")]
    public decimal Interest { get; set; }

    [JsonProperty("interestAccuredTime")]
    public DateTime InterestAccuredTime { get; set; }

    [JsonProperty("interestRate")]
    public decimal InterestRate { get; set; }

    [JsonProperty("principal")]
    public decimal Principal { get; set; }
}