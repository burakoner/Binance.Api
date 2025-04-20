﻿namespace Binance.Api.Models.RestApi.Staking;

/// <summary>
/// Quota left
/// </summary>
public record BinanceStakingPersonalQuota
{
    /// <summary>
    /// Quota left
    /// </summary>
    [JsonProperty("leftPersonalQuota")]
    public decimal PersonalQuotaLeft { get; set; }
}
