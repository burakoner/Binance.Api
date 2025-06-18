namespace Binance.Api.PortfolioMargin;

public  record BinancePortfolioMarginCrossLoan
{
    [JsonProperty("txId")]
    public long TransactionId { get; set; }

    [JsonProperty("asset")]
    public string asset { get; set; } = "";

    [JsonProperty("principal")]
    public decimal Principal { get; set; }

    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonProperty("status")]
    public BinancePortfolioMarginLoanStatus Status { get; set; }
}