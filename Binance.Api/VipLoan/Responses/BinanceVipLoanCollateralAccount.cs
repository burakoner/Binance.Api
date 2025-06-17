namespace Binance.Api.VipLoan;

/// <summary>
/// Collateral Account Info
/// </summary>
public record BinanceVipLoanCollateralAccount
{
    /// <summary>
    /// Collateral Account ID
    /// </summary>
    [JsonProperty("collateralAccountId")]
    public string CollateralAccountId { get; set; } = string.Empty;

    /// <summary>
    /// Collateral Asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public string CollateralAsset { get; set; } = "";
}
