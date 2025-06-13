namespace Binance.Api.Link;

/// <summary>
/// Link -> Futures -> Rebate Volume
/// </summary>
public record BinanceLinkFuturesRebateVolume
{
    /// <summary>
    /// Rebate Unit
    /// </summary>
    [JsonProperty("unit")]
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// Volume
    /// </summary>
    [JsonProperty("rebateVol")]
    public decimal Volume { get; set; }

    /// <summary>
    /// Time of the trade volume record
    /// </summary>
    [JsonProperty("time")]
    public DateTime Time { get; set; }
}
