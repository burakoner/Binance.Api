namespace Binance.Api.CryptoLoan;

/// <summary>
/// Borrow info
/// </summary>
public record BinanceCryptoLoanFlexibleBorrow
{
    /// <summary>
    /// The loaning asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; } = string.Empty;

    /// <summary>
    /// The loan quantity
    /// </summary>
    [JsonProperty("loanAmount")]
    public decimal LoanQuantity { get; set; }

    /// <summary>
    /// The collateral asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public string CollateralAsset { get; set; } = string.Empty;

    /// <summary>
    /// The collateral quantity
    /// </summary>
    [JsonProperty("collateralAmount")]
    public decimal CollateralQuantity { get; set; }

    /// <summary>
    /// Status of the borrow order
    /// </summary>
    [JsonProperty("orderId")]
    public BinanceCryptoLoanFlexibleBorrowStatus Status { get; set; }
}
