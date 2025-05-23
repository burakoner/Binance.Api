﻿namespace Binance.Api.SimpleEarn;

/// <summary>
/// Simple earn locked product info
/// </summary>
public record BinanceSimpleEarnLockedProduct
{
    /// <summary>
    /// Project id
    /// </summary>
    [JsonProperty("projectId")]
    public string ProjectId { get; set; } = string.Empty;

    /// <summary>
    /// Details
    /// </summary>
    [JsonProperty("detail")]
    public BinanceSimpleEarnLockedProjectDetails Details { get; set; } = null!;

    /// <summary>
    /// Quota
    /// </summary>
    [JsonProperty("quota")]
    public BinanceSimpleEarnLockedProjectQuota Quota { get; set; } = null!;
}

/// <summary>
/// Simple earn locked project details
/// </summary>
public record BinanceSimpleEarnLockedProjectDetails
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Reward asset
    /// </summary>
    [JsonProperty("rewardAsset")]
    public string RewardAsset { get; set; } = string.Empty;

    /// <summary>
    /// Duration
    /// </summary>
    [JsonProperty("duration")]
    public int Duration { get; set; }

    /// <summary>
    /// Renewable
    /// </summary>
    [JsonProperty("renewable")]
    public bool Renewable { get; set; }

    /// <summary>
    /// Is sold out
    /// </summary>
    [JsonProperty("isSoldOut")]
    public bool IsSoldOut { get; set; }

    /// <summary>
    /// Apr
    /// </summary>
    [JsonProperty("apr")]
    public decimal Apr { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Subscription start time
    /// </summary>
    [JsonProperty("subscriptionStartTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime? SubscriptionStartTime { get; set; }

    /// <summary>
    /// Extra reward asset
    /// </summary>
    [JsonProperty("extraRewardAsset")]
    public string ExtraRewardAsset { get; set; } = string.Empty;

    /// <summary>
    /// Extra reward apr
    /// </summary>
    [JsonProperty("extraRewardAPR")]
    public decimal ExtraRewardApr { get; set; }

    /// <summary>
    /// Asset the boost reward is in
    /// </summary>
    [JsonProperty("boostRewardAsset")]
    public string? BoostRewardAsset { get; set; }

    /// <summary>
    /// Boost apr
    /// </summary>
    [JsonProperty("boostApr")]
    public decimal? BoostApr { get; set; }

    /// <summary>
    /// Boost end time
    /// </summary>
    [JsonProperty("boostEndTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime? BoostEndTime { get; set; }
}

/// <summary>
/// Simple earn locked project quota
/// </summary>
public record BinanceSimpleEarnLockedProjectQuota
{
    /// <summary>
    /// Total personal quota
    /// </summary>
    [JsonProperty("totalPersonalQuota")]
    public decimal TotalPersonalQuota { get; set; }

    /// <summary>
    /// Minimum
    /// </summary>
    [JsonProperty("minimum")]
    public decimal Minimum { get; set; }
}
