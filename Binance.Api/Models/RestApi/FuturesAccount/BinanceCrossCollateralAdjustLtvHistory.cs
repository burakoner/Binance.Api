namespace Binance.ApiClient.Models.RestApi.Futures;

/// <summary>
/// Adjust history
/// </summary>
public class BinanceCrossCollateralAdjustLtvHistory
{
    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Collateral asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public string CollateralAsset { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("coin")]
    public string Asset { get; set; }
    /// <summary>
    /// Pre adjustment rate
    /// </summary>
    public decimal PreCollateralRate { get; set; }
    /// <summary>
    /// After adjustment rate
    /// </summary>
    public decimal AfterCollateralRate { get; set; }
    /// <summary>
    /// Direction
    /// </summary>
    [JsonConverter(typeof(AdjustRateDirectionConverter))]
    public AdjustRateDirection Direction { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public string Status { get; set; }
    /// <summary>
    /// Time of adjustment
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime AdjustTime { get; set; }
}
