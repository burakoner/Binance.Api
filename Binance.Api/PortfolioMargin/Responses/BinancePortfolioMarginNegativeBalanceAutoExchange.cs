namespace Binance.Api.PortfolioMargin;

public record BinancePortfolioMarginNegativeBalanceAutoExchange
{
    [JsonProperty("startTime")]
    public DateTime StartTime { get; set; }

    [JsonProperty("endTime")]
    public DateTime EndTime { get; set; }

    [JsonProperty("details")]
    public List<BinancePortfolioMarginNegativeBalanceAutoExchangeData> Details { get; set; }
}

public record BinancePortfolioMarginNegativeBalanceAutoExchangeData
{
    [JsonProperty("asset")]
    public string Asset { get; set; }

    [JsonProperty("negativeBalance")]
    public decimal NegativeBalance { get; set; }

    [JsonProperty("negativeMaxThreshold")]
    public decimal NegativeMaximumThreshold { get; set; }
}