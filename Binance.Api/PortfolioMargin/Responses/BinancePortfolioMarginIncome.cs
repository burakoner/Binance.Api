namespace Binance.Api.PortfolioMargin;

public record BinancePortfolioMarginIncome
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    [JsonProperty("incomeType")]
    public string IncomeType { get; set; } = "";

    [JsonProperty("income")]
    public decimal Income { get; set; }

    [JsonProperty("asset")]
    public string Asset { get; set; } = "";

    [JsonProperty("info")]
    public string Info { get; set; } = "";

    [JsonProperty("time")]
    public DateTime Time { get; set; }

    [JsonProperty("tranId")]
    public long TransactionId { get; set; }

    [JsonProperty("tranId")]
    public long? TradeId { get; set; }
}

public record BinancePortfolioMarginIncomeCM : BinancePortfolioMarginIncome
{
}

public record BinancePortfolioMarginIncomeUM : BinancePortfolioMarginIncome
{
}