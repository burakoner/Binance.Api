namespace Binance.Api.CryptoLoan;

/// <summary>
/// Flexible Loan Ongoing Order
/// </summary>
public record BinanceCryptoLoanFlexibleOpenOrder
{
    /// <summary>
    /// The loaning asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; } = string.Empty;

    /// <summary>
    /// Total Debt
    /// </summary>
    [JsonProperty("totalDebt")]
    public decimal TotalDebt { get; set; }

    /// <summary>
    /// The collateral asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public string CollateralAsset { get; set; } = string.Empty;

    /// <summary>
    /// Collateral Quantity
    /// </summary>
    [JsonProperty("collateralAmount")]
    public decimal CollateralQuantity { get; set; }

    /// <summary>
    /// Current LTV
    /// </summary>
    [JsonProperty("currentLTV")]
    public decimal CurrentLTV { get; set; }
}
