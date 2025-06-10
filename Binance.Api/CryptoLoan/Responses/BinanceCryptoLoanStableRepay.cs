namespace Binance.Api.CryptoLoan;

/// <summary>
/// Repay info
/// </summary>
public record BinanceCryptoLoanStableRepay
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
    /// Current LTV
    /// </summary>
    [JsonProperty("currentLTV")]
    public decimal? CurrentLTV { get; set; }

    /// <summary>
    /// Remaining principal
    /// </summary>
    [JsonProperty("remainingPrincipal")]
    public decimal? RemainingPrincipal { get; set; }

    /// <summary>
    /// Repay status
    /// </summary>
    [JsonProperty("repayStatus")]
    public BinanceCryptoLoanStableBorrowStatus RepayStatus { get; set; }

    /// <summary>
    /// Remaining collateral
    /// </summary>
    [JsonProperty("remainingCollateral")]
    public decimal? RemainingCollateral { get; set; }

    /// <summary>
    /// Remaining interest
    /// </summary>
    [JsonProperty("remainingInterest")]
    public decimal? RemainingInterest { get; set; }
}
