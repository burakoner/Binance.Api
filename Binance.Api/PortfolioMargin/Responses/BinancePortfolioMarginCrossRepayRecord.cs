namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Cross Repay Record
/// </summary>
public record BinancePortfolioMarginCrossRepayRecord
{
    /// <summary>
    /// Quantity of the asset being repaid
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Asset being repaid
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = "";

    /// <summary>
    /// Interest accrued on the loan
    /// </summary>
    [JsonProperty("interest")]
    public decimal Interest { get; set; }

    /// <summary>
    /// Principal amount of the loan being repaid
    /// </summary>
    [JsonProperty("principal")]
    public decimal Principal { get; set; }

    /// <summary>
    /// Status of the repayment
    /// </summary>
    [JsonProperty("status")]
    public BinancePortfolioMarginLoanStatus Status { get; set; }

    /// <summary>
    /// Timestamp of the repayment
    /// </summary>
    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Transaction ID of the repayment
    /// </summary>
    [JsonProperty("txId")]
    public long TransactionId { get; set; }
}