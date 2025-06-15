namespace Binance.Api.DualInvestment;

/// <summary>
/// Binance Dual Investment Position Auto Compound Plan
/// </summary>
public record BinanceDualInvestmentPositionPlan
{
    /// <summary>
    /// Position Id
    /// </summary>
    [JsonProperty("positionId")]
    public long PositionId { get; set; }

    /// <summary>
    /// Auto Compound Plan
    /// </summary>
    [JsonProperty("autoCompoundPlan")]
    public BinanceDualInvestmentAutoCompoundPlan AutoCompoundPlan { get; set; }
}