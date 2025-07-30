namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Cross Repay
/// </summary>
public record BinancePortfolioMarginCrossRepay
{
    /// <summary>
    /// Quantity of the asset being repaid
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Asset being repaid
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = "";

    /// <summary>
    /// Repay Assets
    /// </summary>
    [JsonProperty("specifyRepayAssets")]
    public List<string> RepayAssets { get; set; } = [];

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// Success
    /// </summary>
    [JsonProperty("success")]
    public bool Success { get; set; }
}