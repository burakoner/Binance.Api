namespace Binance.Api.Staking;

/// <summary>
/// Staking Subscribe Result
/// </summary>
public record BinanceEthStakingStake
{
    /// <summary>
    /// Successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// WBETH Quantity
    /// </summary>
    [JsonProperty("wbethAmount")]
    public decimal WBEthQuantity { get; set; }

    /// <summary>
    /// Conversion Ratio
    /// </summary>
    [JsonProperty("conversionRatio")]
    public decimal ConversionRatio { get; set; }
}
