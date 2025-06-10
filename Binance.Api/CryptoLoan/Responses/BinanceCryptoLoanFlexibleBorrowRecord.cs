namespace Binance.Api.CryptoLoan;

/// <summary>
/// Borrow History
/// </summary>
public record BinanceCryptoLoanFlexibleBorrowHistory
{
    /// <summary>
    /// The loaning asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; } = string.Empty;

    /// <summary>
    /// Initial Loan Quantity
    /// </summary>
    [JsonProperty("initialLoanAmount")]
    public decimal InitialLoanQuantity { get; set; }

    /// <summary>
    /// The collateral asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public string CollateralAsset { get; set; } = string.Empty;

    /// <summary>
    /// Initial Collateral Quantity
    /// </summary>
    [JsonProperty("initialCollateralAmount")]
    public decimal InitialCollateralQuantity { get; set; }

    /// <summary>
    /// Borrow Time
    /// </summary>
    [JsonProperty("borrowTime")]
    public DateTime Time { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public BinanceCryptoLoanFlexibleBorrowStatus Status { get; set; }
}
