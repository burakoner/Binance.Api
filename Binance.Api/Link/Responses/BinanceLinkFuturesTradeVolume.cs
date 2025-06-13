namespace Binance.Api.Link;

/// <summary>
/// Link -> Futures -> Trade Volume
/// </summary>
public record BinanceLinkFuturesTradeVolume
{
    /// <summary>
    /// Volume Unit
    /// </summary>
    [JsonProperty("unit")]
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// Volume
    /// </summary>
    [JsonProperty("tradeVol")]
    public decimal Volume { get; set; }

    /// <summary>
    /// Time of the trade volume record
    /// </summary>
    [JsonProperty("time")]
    public DateTime Time { get; set; }
}
