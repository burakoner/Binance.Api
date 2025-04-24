namespace Binance.Api.Futures;

/// <summary>
/// Buy Sell Volume Ratio Info
/// </summary>
public record BinanceFuturesBuySellVolumeRatio
{
    /// <summary>
    /// buy/sell ratio
    /// </summary>
    [JsonProperty("buySellRatio")]
    public decimal BuySellRatio { get; set; }

    /// <summary>
    /// buy volume
    /// </summary>
    [JsonProperty("buyVol")]
    public decimal BuyVolume { get; set; }

    /// <summary>
    /// sell volume
    /// </summary>
    [JsonProperty("sellVol")]
    public decimal SellVolume { get; set; }

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("timestamp"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime? Timestamp { get; set; }
}