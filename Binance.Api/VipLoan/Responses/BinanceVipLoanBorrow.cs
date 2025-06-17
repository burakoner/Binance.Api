namespace Binance.Api.VipLoan;

/// <summary>
/// VIP Loan Borrow
/// </summary>
public record BinanceVipLoanBorrow
{
    /// <summary>
    /// Loan Account ID
    /// </summary>
    public long LoanAccountId { get; set; }

    /// <summary>
    /// Request ID
    /// </summary>
    public long RequestId { get; set; }

    /// <summary>
    /// Loan Asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; } = string.Empty;

    /// <summary>
    /// Is Flexible Rate
    /// </summary>
    public bool IsFlexibleRate { get; set; }

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
    public long CollateralAsset { get; set; }

    /// <summary>
    /// Loan Term in days
    /// </summary>
    public int? LoanTerm { get; set; }
}
