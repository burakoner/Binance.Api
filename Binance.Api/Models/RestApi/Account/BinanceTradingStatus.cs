namespace Binance.Api.Models.RestApi.Account;

/// <summary>
/// Trade status
/// </summary>
public record BinanceTradingStatus
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
    public Dictionary<string, IEnumerable<BinanceIndicator>> Indicators { get; set; } = new Dictionary<string, IEnumerable<BinanceIndicator>>();
    /// <summary>
    /// Last update time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateTime { get; set; }
}