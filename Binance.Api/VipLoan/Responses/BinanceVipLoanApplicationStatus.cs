namespace Binance.Api.VipLoan;

/// <summary>
/// VIP Loan Application Status
/// </summary>
public record BinanceVipLoanApplicationStatus
{
    /// <summary>
    /// Loan Account ID
    /// </summary>
    [JsonProperty("loanAccountId")]
    public long LoanAccountId { get; set; }

    /// <summary>
    /// Order ID
    /// </summary>
    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    /// <summary>
    /// Request ID
    /// </summary>
    [JsonProperty("requestId")]
    public long RequestId { get; set; }

    /// <summary>
    /// Loan Asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; } = string.Empty;

    /// <summary>
    /// Loan Quantity
    /// </summary>
    [JsonProperty("loanAmount")]
    public decimal LoanQuantity { get; set; }

    /// <summary>
    /// Collateral Account ID
    /// </summary>
    [JsonProperty("collateralAccountId")]
    public string CollateralAccountId { get; set; } = string.Empty;

    /// <summary>
    /// Collateral Asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public string CollateralAsset { get; set; } = string.Empty;

    /// <summary>
    /// Loan Term in days
    /// </summary>
    [JsonProperty("loanTerm")]
    public string LoanTerm { get; set; } = string.Empty;

    /// <summary>
    /// Status of the loan application
    /// </summary>
    [JsonProperty("status")]
    public BinanceVipLoanStatus Status { get; set; }

    /// <summary>
    /// Loan Date
    /// </summary>
    [JsonProperty("loanDate")]
    public DateTime LoanDate { get; set; }
}
