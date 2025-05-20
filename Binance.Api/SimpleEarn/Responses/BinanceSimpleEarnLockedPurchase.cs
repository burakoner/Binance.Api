namespace Binance.Api.SimpleEarn;

/// <summary>
/// Purchase id
/// </summary>
public record BinanceSimpleEarnLockedPurchase
{
    /// <summary>
    /// Success
    /// </summary>
    [JsonProperty("success")]
    public bool Success { get; set; }

    /// <summary>
    /// Purchase id
    /// </summary>
    [JsonProperty("purchaseId")]
    public long PurchaseId { get; set; }

    /// <summary>
    /// Position id
    /// </summary>
    [JsonProperty("positionId")]
    public string PositionId { get; set; } = "";
}
