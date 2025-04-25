namespace Binance.Api.Futures;

/// <summary>
/// Account info
/// </summary>
public record BinanceFuturesCoinAccountInfo
{
    /// <summary>
    /// Can deposit
    /// </summary>
    [JsonProperty("canDeposit")]
    public bool CanDeposit { get; set; }

    /// <summary>
    /// Can trade
    /// </summary>
    [JsonProperty("canTrade")]
    public bool CanTrade { get; set; }

    /// <summary>
    /// Can withdraw
    /// </summary>
    [JsonProperty("canWithdraw")]
    public bool CanWithdraw { get; set; }

    /// <summary>
    /// Fee tier
    /// </summary>
    [JsonProperty("feeTier")]
    public int FeeTier { get; set; }

    /// <summary>
    /// Update tier
    /// </summary>
    [JsonProperty("updateTier")]
    public int UpdateTier { get; set; }

    /// <summary>
    /// Account assets
    /// </summary>
    [JsonProperty("assets")]
    public IEnumerable<BinanceFuturesAccountAsset> Assets { get; set; } = [];

    /// <summary>
    /// Account positions
    /// </summary>
    [JsonProperty("positions")]
    public IEnumerable<BinanceFuturesCoinPositionInfo> Positions { get; set; } = [];

    /// <summary>
    /// Update time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("updateTime")]
    public DateTime UpdateTime { get; set; }
}
