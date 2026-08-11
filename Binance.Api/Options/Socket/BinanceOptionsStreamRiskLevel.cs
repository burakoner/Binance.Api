namespace Binance.Api.Options;

/// <summary>
/// Options risk-level update
/// </summary>
public record BinanceOptionsStreamRiskLevel : BinanceSocketStreamEvent
{
    /// <summary>
    /// The listen key the update was received for
    /// </summary>
    [JsonIgnore]
    public string ListenKey { get; set; } = string.Empty;

    /// <summary>
    /// Risk level
    /// </summary>
    [JsonProperty("s")]
    public BinanceOptionsRiskLevel? RiskLevel { get; set; }

    /// <summary>
    /// Margin balance
    /// </summary>
    [JsonProperty("mb")]
    public decimal? MarginBalance { get; set; }

    /// <summary>
    /// Maintenance margin
    /// </summary>
    [JsonProperty("mm")]
    public decimal? MaintenanceMargin { get; set; }
}
