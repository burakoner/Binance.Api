namespace Binance.Api.AutoInvest;

/// <summary>
/// Auto invest assets
/// </summary>
public record BinanceAutoInvestAssets
{
    /// <summary>
    /// Target assets
    /// </summary>
    [JsonProperty("targetAssets")]
    public List<string> TargetAssets { get; set; } = [];

    /// <summary>
    /// Source assets
    /// </summary>
    [JsonProperty("sourceAssets")]
    public List<string> SourceAssets { get; set; } = [];
}
