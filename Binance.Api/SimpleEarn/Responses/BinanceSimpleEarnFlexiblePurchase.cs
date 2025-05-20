namespace Binance.Api.SimpleEarn;

/// <summary>
/// Purchase id
/// </summary>
public record BinanceSimpleEarnFlexiblePurchase
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
}
