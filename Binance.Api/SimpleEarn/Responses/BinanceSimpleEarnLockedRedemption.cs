namespace Binance.Api.SimpleEarn;

/// <summary>
/// Redemption
/// </summary>
public record BinanceSimpleEarnLockedRedemption
{
    /// <summary>
    /// Success
    /// </summary>
    [JsonProperty("success")]
    public bool Success { get; set; }

    /// <summary>
    /// Redeem id
    /// </summary>
    [JsonProperty("redeemId")]
    public long RedeemId { get; set; }
}
