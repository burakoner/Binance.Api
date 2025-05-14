namespace Binance.Api.Shared;

/// <summary>
/// BNB burn for fee reduction status
/// </summary>
public record BinanceBnbBurnStatus
{
    /// <summary>
    /// Fee burn status
    /// </summary>
    [JsonProperty("feeBurn")]
    public bool FeeBurn { get; set; }
}
