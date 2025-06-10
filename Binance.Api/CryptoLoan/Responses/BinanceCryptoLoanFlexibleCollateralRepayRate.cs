namespace Binance.Api.CryptoLoan;

/// <summary>
/// Flexible Collateral Repay Rate
/// </summary>
public record BinanceCryptoLoanFlexibleCollateralRepayRate
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
    /// Rate
    /// </summary>
    [JsonProperty("rate")]
    public decimal Rate { get; set; }
}
