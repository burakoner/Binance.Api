namespace Binance.Api.Futures;

/// <summary>
/// TradFi perpetual market trading schedules
/// </summary>
public record BinanceFuturesTradingSchedule
{
    /// <summary>
    /// Raw schedule update time published by Binance
    /// </summary>
    public long UpdateTime { get; set; }

    /// <summary>
    /// Schedules grouped by market
    /// </summary>
    public BinanceFuturesMarketSchedules MarketSchedules { get; set; } = new();
}

/// <summary>
/// TradFi perpetual schedules by market
/// </summary>
public record BinanceFuturesMarketSchedules
{
    /// <summary>
    /// U.S. equity market sessions
    /// </summary>
    [JsonProperty("EQUITY")]
    public BinanceFuturesTradingSessions? Equity { get; set; }

    /// <summary>
    /// Commodity market sessions
    /// </summary>
    [JsonProperty("COMMODITY")]
    public BinanceFuturesTradingSessions? Commodity { get; set; }

    /// <summary>
    /// Korean equity market sessions
    /// </summary>
    [JsonProperty("KR_EQUITY")]
    public BinanceFuturesTradingSessions? KoreanEquity { get; set; }

    /// <summary>
    /// Hong Kong equity market sessions
    /// </summary>
    [JsonProperty("HK_EQUITY")]
    public BinanceFuturesTradingSessions? HongKongEquity { get; set; }
}

/// <summary>
/// Trading sessions for a market
/// </summary>
public record BinanceFuturesTradingSessions
{
    /// <summary>
    /// Trading sessions
    /// </summary>
    public List<BinanceFuturesTradingSession> Sessions { get; set; } = [];
}

/// <summary>
/// A market trading session
/// </summary>
public record BinanceFuturesTradingSession
{
    /// <summary>
    /// Raw session start time published by Binance
    /// </summary>
    public long StartTime { get; set; }

    /// <summary>
    /// Raw session end time published by Binance
    /// </summary>
    public long EndTime { get; set; }

    /// <summary>
    /// Session type
    /// </summary>
    public string Type { get; set; } = string.Empty;
}
