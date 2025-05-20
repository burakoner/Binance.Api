namespace Binance.Api.SimpleEarn;

/// <summary>
/// Simple Earn personal quota left
/// </summary>
public record BinanceSimpleEarnQuota
{
    /// <summary>
    /// Personal quota left
    /// </summary>
    [JsonProperty("leftPersonalQuota")]
    public decimal PersonalQuotaLeft { get; set; }
}
