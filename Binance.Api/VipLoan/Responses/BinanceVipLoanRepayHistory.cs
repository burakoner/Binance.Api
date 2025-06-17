namespace Binance.Api.VipLoan;

/// <summary>
/// VIP Loan Repay History Record
/// </summary>
public record BinanceVipLoanRepayHistory
{
    /// <summary>
    /// Loan Asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; } = string.Empty;

    /// <summary>
    /// Repay Quantity
    /// </summary>
    [JsonProperty("repayAmount")]
    public decimal RepayQuantity { get; set; }

    /// <summary>
    /// Collateral Asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public long CollateralAsset { get; set; }

    /// <summary>
    /// Repay Status
    /// </summary>
    [JsonProperty("repayStatus")]
    public BinanceVipLoanStatus RepayStatus { get; set; }

    /// <summary>
    /// Loan Date
    /// </summary>
    [JsonProperty("loanDate")]
    public DateTime LoanDate { get; set; }

    /// <summary>
    /// Repay Time
    /// </summary>
    [JsonProperty("repayTime")]
    public DateTime RepayTime { get; set; }

    /// <summary>
    /// Order ID
    /// </summary>
    [JsonProperty("orderId")]
    public long OrderId { get; set; }
}
