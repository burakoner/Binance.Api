namespace Binance.Api.CryptoLoan;

/// <summary>
/// Repay History
/// </summary>
public record BinanceCryptoLoanFlexibleRepayRecord
{
    /// <summary>
    /// The loaning asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; } = string.Empty;

    /// <summary>
    /// Repay Quantity
    /// </summary>
    [JsonProperty("repayAmount")]
    public decimal RepayQuantity { get; set; }

    /// <summary>
    /// The collateral asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public string CollateralAsset { get; set; } = string.Empty;

    /// <summary>
    /// Collateral Return
    /// </summary>
    [JsonProperty("collateralReturn")]
    public decimal CollateralReturn { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public BinanceCryptoLoanFlexibleRepayStatus Status { get; set; }

    /// <summary>
    /// REpay Time
    /// </summary>
    [JsonProperty("repayTime")]
    public DateTime Time { get; set; }
}
