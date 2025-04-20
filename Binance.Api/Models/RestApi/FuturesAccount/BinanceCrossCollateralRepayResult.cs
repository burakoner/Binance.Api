namespace Binance.Api.Models.RestApi.Futures;

/// <summary>
/// Repay result
/// </summary>
public record BinanceCrossCollateralRepayResult
{
    /// <summary>
    /// Id
    /// </summary>
    public string RepayId { get; set; } = "";
    /// <summary>
    /// The asset borrowed
    /// </summary>
    [JsonProperty("coin")]
    public string Asset { get; set; } = "";
    /// <summary>
    /// The asset used for collateral
    /// </summary>
    [JsonProperty("collateralCoin")]
    public string CollateralAsset { get; set; } = "";
    /// <summary>
    /// The quantity borrowed
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
}
