namespace Binance.Api.Options;

/// <summary>
/// Options Risk Level Update
/// </summary>
public record BinanceOptionsStreamRiskLevel : BinanceSocketStreamEvent
{
    /// <summary>
    /// The listen key the update was for
    /// </summary>
    [JsonIgnore]
    public string ListenKey { get; set; } = string.Empty;

    /// <summary>
    /// Risk Level
    /// </summary>
    [JsonProperty("s")]
    public string RiskLevel { get; set; } = "";

    /// <summary>
    /// Margin Balance
    /// </summary>
    [JsonProperty("mb")]
    public decimal MarginBalance { get; set; }

    /// <summary>
    /// Maintenance Margin
    /// </summary>
    [JsonProperty("mm")]
    public decimal MaintenanceMargin { get; set; }
}
