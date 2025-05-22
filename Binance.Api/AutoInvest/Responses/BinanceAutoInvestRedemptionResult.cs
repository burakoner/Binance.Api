namespace Binance.Api.AutoInvest;

/// <summary>
/// Redemption result
/// </summary>
public record BinanceAutoInvestRedemptionResult
{
    /// <summary>
    /// Redemption id
    /// </summary>
    [JsonProperty("redemptionId")]
    public long RedemptionId { get; set; }
}
