namespace Binance.Api.SimpleEarn;

/// <summary>
/// Locked product position info
/// </summary>
public record BinanceSimpleEarnLockedPosition
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Position id
    /// </summary>
    [JsonProperty("positionId")]
    public string PositionId { get; set; } = string.Empty;

    /// <summary>
    /// Project id
    /// </summary>
    [JsonProperty("projectId")]
    public string ProjectId { get; set; } = string.Empty;

    /// <summary>
    /// Position quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Purchase time
    /// </summary>
    [JsonProperty("purchaseTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime? PurchaseTime { get; set; }

    /// <summary>
    /// Duration in days
    /// </summary>
    [JsonProperty("duration")]
    public int Duration { get; set; }

    /// <summary>
    /// Accrual days
    /// </summary>
    [JsonProperty("accrualDays")]
    public int AccrualDays { get; set; }

    /// <summary>
    /// Reward asset
    /// </summary>
    [JsonProperty("rewardAsset")]
    public string RewardAsset { get; set; } = string.Empty;

    /// <summary>
    /// APY
    /// </summary>
    [JsonProperty("APY")]
    public decimal APY { get; set; }

    /// <summary>
    /// Is renewable
    /// </summary>
    [JsonProperty("isRenewable")]
    public bool IsRenewable { get; set; }

    /// <summary>
    /// Is auto renew enabled
    /// </summary>
    [JsonProperty("isAutoRenew")]
    public bool IsAutoRenew { get; set; }

    /// <summary>
    /// Redeem date
    /// </summary>
    [JsonProperty("redeemDate"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime? RedeemDate { get; set; }

    /// <summary>
    /// Reward quantity
    /// </summary>
    [JsonProperty("rewardAmt")]
    public decimal RewardQuantity { get; set; }

    /// <summary>
    /// Extra reward asset
    /// </summary>
    [JsonProperty("extraRewardAsset")]
    public string ExtraRewardAsset { get; set; } = string.Empty;

    /// <summary>
    /// Extra reward APR
    /// </summary>
    [JsonProperty("extraRewardAPR")]
    public decimal ExtraRewardApr { get; set; }

    /// <summary>
    /// Estimated extra reward quantity
    /// </summary>
    [JsonProperty("estExtraRewardAmt")]
    public decimal EstimatedExtraRewardQuantity { get; set; }

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
    /// Total boost reward quantity
    /// </summary>
    [JsonProperty("totalBoostRewardAmt")]
    public decimal? TotalBoostRewardQuantity { get; set; }

    /// <summary>
    /// Estimated quantity of next payment
    /// </summary>
    [JsonProperty("nextPay")]
    public decimal? EstimatedNextPayQuantity { get; set; }

    /// <summary>
    /// Next pay time
    /// </summary>
    [JsonProperty("nextPayDate")]
    public DateTime? NextPayTime { get; set; }

    /// <summary>
    /// Payment cycle
    /// </summary>
    [JsonProperty("payPeriod")]
    public string? PaymentPeriod { get; set; }

    /// <summary>
    /// Early redemption quantity
    /// </summary>
    [JsonProperty("redeemAmountEarly")]
    public decimal? EarlyRedemptionQuantity { get; set; }

    /// <summary>
    /// Rewards accrual end time
    /// </summary>
    [JsonProperty("rewardsEndDate")]
    public DateTime? RewardsEndTime { get; set; }

    /// <summary>
    /// Redemption arrival time
    /// </summary>
    [JsonProperty("deliverDate")]
    public DateTime? DeliverTime { get; set; }

    /// <summary>
    /// Redeem period
    /// </summary>
    [JsonProperty("redeemPeriod")]
    public string? RedeemPeriod { get; set; }

    /// <summary>
    /// Quantity under redemption
    /// </summary>
    [JsonProperty("redeemingAmt")]
    public decimal? RedemptionQuantity { get; set; }

    /// <summary>
    /// Redeem target account
    /// </summary>
    [JsonProperty("redeemTo")]
    public string? RedeemTo { get; set; }

    /// <summary>
    /// Arrival time of partial redemption amount of order
    /// </summary>
    [JsonProperty("partialAmtDeliverDate")]
    public DateTime? PartialRedemptionDeliverTime { get; set; }

    /// <summary>
    /// Can redeem early
    /// </summary>
    [JsonProperty("canRedeemEarly")]
    public bool? CanRedeemEarly { get; set; }

    /// <summary>
    /// Can fast redeem
    /// </summary>
    [JsonProperty("canFastRedemption")]
    public bool? CanFastRedeem { get; set; }

    /// <summary>
    /// Auto subscribe is enabled
    /// </summary>
    [JsonProperty("autoSubscribe")]
    public bool? AutoSubscribe { get; set; }

    /// <summary>
    /// Auto subscribe or normal
    /// </summary>
    [JsonProperty("type")]
    public string? OrderType { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public string? Status { get; set; }

    /// <summary>
    /// Can restake
    /// </summary>
    [JsonProperty("canReStake")]
    public bool? CanRestake { get; set; }
}
