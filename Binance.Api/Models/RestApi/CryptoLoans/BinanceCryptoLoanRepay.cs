namespace Binance.Api.Models.RestApi.CryptoLoans;

/// <summary>
/// Repay info
/// </summary>
public record BinanceCryptoLoanRepay
{
    /// <summary>
    /// The loaning asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; } = "";
    /// <summary>
    /// The collateral asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public string CollateralAsset { get; set; } = "";
    /// <summary>
    /// Current LTV
    /// </summary>
    public decimal? CurrentLTV { get; set; }
    /// <summary>
    /// Remaining principal
    /// </summary>
    public decimal? RemainingPrincipal { get; set; }
    /// <summary>
    /// Repay status
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BorrowStatus RepayStatus { get; set; }
    /// <summary>
    /// Remaining collateral
    /// </summary>
    public decimal? RemainingCollateral { get; set; }
    /// <summary>
    /// Remaining interest
    /// </summary>
    public decimal? RemainingInterest { get; set; }
}
