namespace Binance.Api.Staking;

/// <summary>
/// Wrap history
/// </summary>
public record BinanceEthStakingWbEthWrap
{
    /// <summary>
    /// Exchange rate
    /// </summary>
    public decimal ExchangeRate { get; set; }

    /// <summary>
    /// Output quantity
    /// </summary>
    [JsonProperty("toAmount")]
    public decimal ToQuantity { get; set; }

    /// <summary>
    /// Input quantity
    /// </summary>
    [JsonProperty("fromAmount")]
    public decimal FromQuantity { get; set; }

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("time")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// From asset
    /// </summary>
    public string FromAsset { get; set; } = string.Empty;

    /// <summary>
    /// To asset
    /// </summary>
    public string ToAsset { get; set; } = string.Empty;

    /// <summary>
    /// Status
    /// </summary>
    public string Status { get; set; } = string.Empty;
}
