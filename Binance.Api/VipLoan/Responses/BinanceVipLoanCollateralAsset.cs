namespace Binance.Api.VipLoan;

/// <summary>
/// Collateral asset info
/// </summary>
public record BinanceVipLoanCollateralAsset
{
    /// <summary>
    /// Collateral asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public string CollateralAsset { get; set; } = string.Empty;

    /// <summary>
    /// 1st collateral ratio
    /// </summary>
    [JsonProperty("_1stCollateralRatio")]
    public string _1stCollateralRatio { get; set; } = "";

    /// <summary>
    /// 1st collateral range
    /// </summary>
    [JsonProperty("_1stCollateralRange")]
    public string _1stCollateralRange { get; set; } = "";

    /// <summary>
    /// 2nd collateral ratio
    /// </summary>
    [JsonProperty("_2ndCollateralRatio")]
    public string _2ndCollateralRatio { get; set; } = "";

    /// <summary>
    /// 2nd collateral range
    /// </summary>
    [JsonProperty("_2ndCollateralRange")]
    public string _2ndCollateralRange { get; set; } = "";

    /// <summary>
    /// 3rd collateral ratio
    /// </summary>
    [JsonProperty("_3stCollateralRatio")]
    public string _3stCollateralRatio { get; set; } = "";

    /// <summary>
    /// 3rd collateral range
    /// </summary>
    [JsonProperty("_3stCollateralRange")]
    public string _3stCollateralRange { get; set; } = "";

    /// <summary>
    /// 4th collateral ratio
    /// </summary>
    [JsonProperty("_4ndCollateralRatio")]
    public string _4ndCollateralRatio { get; set; } = "";

    /// <summary>
    /// 4th collateral range
    /// </summary>
    [JsonProperty("_4ndCollateralRange")]
    public string _4ndCollateralRange { get; set; } = "";

}
