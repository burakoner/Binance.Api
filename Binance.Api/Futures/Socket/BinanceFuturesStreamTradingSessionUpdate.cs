namespace Binance.Api.Futures;

/// <summary>
/// TradFi perpetual market trading session update
/// </summary>
public record BinanceFuturesStreamTradingSessionUpdate : BinanceFuturesStreamEvent
{
    /// <summary>
    /// Session start time
    /// </summary>
    [JsonProperty("t"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime SessionStartTime { get; set; }

    /// <summary>
    /// Session end time
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime SessionEndTime { get; set; }

    /// <summary>
    /// Session type. U.S. equity updates can contain PRE_MARKET, REGULAR,
    /// AFTER_MARKET, OVERNIGHT, or NO_TRADING. Commodity, Korean equity,
    /// and Hong Kong equity updates contain REGULAR or NO_TRADING.
    /// </summary>
    [JsonProperty("S")]
    public string SessionType { get; set; } = string.Empty;
}
