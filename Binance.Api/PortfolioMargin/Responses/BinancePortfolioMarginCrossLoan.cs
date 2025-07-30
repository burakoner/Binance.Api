namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Cross Loan
/// </summary>
public record BinancePortfolioMarginCrossLoan
{
    /// <summary>
    /// Transaction ID
    /// </summary>
    [JsonProperty("txId")]
    public long TransactionId { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = "";

    /// <summary>
    /// Principal
    /// </summary>
    [JsonProperty("principal")]
    public decimal Principal { get; set; }

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public BinancePortfolioMarginLoanStatus Status { get; set; }
}