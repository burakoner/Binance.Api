namespace Binance.Api.PortfolioMargin;

public  record BinancePortfolioMarginCrossRepayRecord
{
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    [JsonProperty("asset")]
    public string Asset { get; set; } = "";

    [JsonProperty("interest")]
    public decimal Interest { get; set; }

    [JsonProperty("principal")]
    public decimal Principal { get; set; }

    [JsonProperty("status")]
    public BinancePortfolioMarginLoanStatus Status { get; set; }

    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonProperty("txId")]
    public long TransactionId { get; set; }
}