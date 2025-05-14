namespace Binance.Api.CopyTrading;

/// <summary>
/// Copy trading user status
/// </summary>
public record BinanceCopyTradingFuturesLeadTraderStatus
{
    /// <summary>
    /// Is lead trader
    /// </summary>
    [JsonProperty("isLeadTrader")]
    public bool IsLeadTrader { get; set; }

    /// <summary>
    /// Time
    /// </summary>
    [JsonProperty("time")]
    public long Timestamp { get; set; }
}
