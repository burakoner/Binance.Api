namespace Binance.Api.Models.RestApi.Futures;

/// <summary>
/// Adjust result
/// </summary>
public class BinanceCrossCollateralAdjustLtvResult
{
    /// <summary>
    /// Collateral asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public string CollateralAsset { get; set; }
    /// <summary>
    /// Loan asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; }
    /// <summary>
    /// The direction
    /// </summary>
    [JsonConverter(typeof(AdjustRateDirectionConverter))]
    public AdjustRateDirection Direction { get; set; }
    /// <summary>
    /// The quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// The time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("time")]
    public DateTime Timestamp { get; set; }
}
