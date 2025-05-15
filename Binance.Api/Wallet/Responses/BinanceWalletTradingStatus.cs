namespace Binance.Api.Wallet;

/// <summary>
/// Trade status
/// </summary>
public record BinanceWalletTradingStatus
{
    /// <summary>
    /// Is locked
    /// </summary>
    public bool IsLocked { get; set; }

    /// <summary>
    /// Planned time of recovery
    /// </summary>
    public int PlannedRecoverTime { get; set; }

    /// <summary>
    /// Conditions
    /// </summary>
    [JsonProperty("triggerCondition")]
    public Dictionary<string, int> TriggerConditions { get; set; } = new Dictionary<string, int>();

    /// <summary>
    /// Dictionary of indicator lists for symbols
    /// </summary>
    public Dictionary<string, List<BinanceWalletIndicator>> Indicators { get; set; } = [];

    /// <summary>
    /// Last update time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateTime { get; set; }
}