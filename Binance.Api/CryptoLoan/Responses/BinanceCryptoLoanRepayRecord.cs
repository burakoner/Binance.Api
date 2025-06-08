namespace Binance.Api.CryptoLoan;

/// <summary>
/// Repay record
/// </summary>
public record BinanceCryptoLoanRepayRecord
{
    /// <summary>
    /// The loaning asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; } = string.Empty;

    /// <summary>
    /// The collateral asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public string CollateralAsset { get; set; } = string.Empty;

    /// <summary>
    /// Borrow order id
    /// </summary>
    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    /// <summary>
    /// Repay timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("repayTime")]
    public DateTime RepayTime { get; set; }

    /// <summary>
    /// Status of the repay
    /// </summary>
    [JsonProperty("repayStatus")]
    public BinanceCryptoLoanBorrowStatus RepayStatus { get; set; }

    /// <summary>
    /// Collateral return
    /// </summary>
    [JsonProperty("collateralReturn")]
    public decimal CollateralReturn { get; set; }

    /// <summary>
    /// Collateral used
    /// </summary>
    [JsonProperty("collateralUsed")]
    public decimal CollateralUsed { get; set; }

    /// <summary>
    /// Repay quantity
    /// </summary>
    [JsonProperty("repayAmount")]
    public decimal RepayQuantity { get; set; }

    /// <summary>
    /// 1 for "repay with borrowed asset", 2 for "repay with collateral"
    /// </summary>
    [JsonProperty("repayType")]
    public int RepayType { get; set; }
}
