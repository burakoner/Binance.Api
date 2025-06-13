namespace Binance.Api.Link;

/// <summary>
/// Link -> Futures -> Trader Number
/// </summary>
public record BinanceLinkFuturesTraderNumber
{
    /// <summary>
    /// New Trader Count
    /// </summary>
    [JsonProperty("newTrader")]
    public int NewTrader { get; set; }

    /// <summary>
    /// Old Trader Count
    /// </summary>
    [JsonProperty("oldTrader")]
    public int OldTrader { get; set; }

    /// <summary>
    /// Time of the trader number record
    /// </summary>
    [JsonProperty("time")]
    public DateTime Time { get; set; }
}
