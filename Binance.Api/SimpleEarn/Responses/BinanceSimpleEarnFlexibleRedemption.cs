namespace Binance.Api.SimpleEarn;

/// <summary>
/// Redemption
/// </summary>
public record BinanceSimpleEarnFlexibleRedemption
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
