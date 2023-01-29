namespace Binance.ApiClient.Models.RestApi.Staking;

/// <summary>
/// Quota left
/// </summary>
public class BinanceStakingPersonalQuota
{
    /// <summary>
    /// Quota left
    /// </summary>
    [JsonProperty("leftPersonalQuota")]
    public decimal PersonalQuotaLeft { get; set; }
}
