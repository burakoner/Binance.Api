namespace Binance.Api.Wallet;

/// <summary>
/// VIP level and futures/margin enabled status
/// </summary>
public record BinanceVipLevelAndStatus
{
    /// <summary>
    /// VIP level
    /// </summary>
    [JsonProperty("vipLevel")]
    public int VipLevel { get; set; }

    /// <summary>
    /// Is margin enabled
    /// </summary>
    [JsonProperty("isMarginEnabled")]
    public bool IsMarginEnabled { get; set; }

    /// <summary>
    /// Is futures enabled
    /// </summary>
    [JsonProperty("isFutureEnabled")]
    public bool IsFuturesEnabled { get; set; }

    /// <summary>
    /// Is options enabled
    /// </summary>
    [JsonProperty("isOptionsEnabled")]
    public bool IsOptionsEnabled { get; set; }

    /// <summary>
    /// Is portfolio margin retail enabled
    /// </summary>
    [JsonProperty("isPortfolioMarginRetailEnabled")]
    public bool IsPortfolioMarginRetailEnabled { get; set; }
}
