namespace Binance.Api.Futures;

/// <summary>
/// Account configuration
/// </summary>
public record BinanceFuturesAccountConfiguration
{
    /// <summary>
    /// Fee tier
    /// </summary>
    [JsonProperty("feeTier")]
    public int FeeTier { get; set; }

    /// <summary>
    /// Can trade
    /// </summary>
    [JsonProperty("canTrade")]
    public bool CanTrade { get; set; }

    /// <summary>
    /// Can deposit
    /// </summary>
    [JsonProperty("canDeposit")]
    public bool CanDeposit { get; set; }

    /// <summary>
    /// Can withdraw
    /// </summary>
    [JsonProperty("canWithdraw")]
    public bool CanWithdraw { get; set; }

    /// <summary>
    /// Dual side position
    /// </summary>
    [JsonProperty("dualSidePosition")]
    public bool DualSidePosition { get; set; }

    /// <summary>
    /// Update time
    /// </summary>
    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// Multi assets margin
    /// </summary>
    [JsonProperty("multiAssetsMargin")]
    public bool MultiAssetsMargin { get; set; }

    /// <summary>
    /// Trade group id
    /// </summary>
    [JsonProperty("tradeGroupId")]
    public long TradeGroupId { get; set; }
}
