namespace Binance.Api.AutoInvest;

/// <summary>
/// Edit result
/// </summary>
public record BinanceAutoInvestEditResult
{
    /// <summary>
    /// Plan id
    /// </summary>
    [JsonProperty("planId")]
    public long PlanId { get; set; }

    /// <summary>
    /// Next execution date time
    /// </summary>
    [JsonProperty("nextExecutionDateTime")]
    public DateTime? NextExecutionTime { get; set; }
}
