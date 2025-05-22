namespace Binance.Api.AutoInvest;

/// <summary>
/// Invest plan info
/// </summary>
public record BinanceAutoInvestPlan
{
    /// <summary>
    /// Plan value in USD
    /// </summary>
    [JsonProperty("planValueInUSD")]
    public decimal PlanValueInUsd { get; set; }

    /// <summary>
    /// Plan value in BTC
    /// </summary>
    [JsonProperty("planValueInBTC")]
    public decimal PlanValueInBtc { get; set; }

    /// <summary>
    /// Profit and loss in USD
    /// </summary>
    [JsonProperty("pnlInUSD")]
    public decimal PnlInUsd { get; set; }

    /// <summary>
    /// Roi
    /// </summary>
    [JsonProperty("roi")]
    public decimal Roi { get; set; }

    /// <summary>
    /// Plans
    /// </summary>
    [JsonProperty("plans")]
    public List<BinanceAutoInvestPlanDetails> Plans { get; set; } = [];
}

/// <summary>
/// Plan info
/// </summary>
public record BinanceAutoInvestPlanDetails
{
    /// <summary>
    /// Plan id
    /// </summary>
    [JsonProperty("planId")]
    public long PlanId { get; set; }

    /// <summary>
    /// Plan type
    /// </summary>
    [JsonProperty("planType")]
    public BinanceAutoInvestPlanType PlanType { get; set; }

    /// <summary>
    /// Edit allowed
    /// </summary>
    [JsonProperty("editAllowed")]
    public bool EditAllowed { get; set; }

    /// <summary>
    /// Creation date time
    /// </summary>
    [JsonProperty("creationDateTime")]
    public DateTime? CreateTime { get; set; }

    /// <summary>
    /// First execution date time
    /// </summary>
    [JsonProperty("firstExecutionDateTime")]
    public DateTime? FirstExecutionTime { get; set; }

    /// <summary>
    /// Next execution date time
    /// </summary>
    [JsonProperty("nextExecutionDateTime")]
    public DateTime? NextExecutionTime { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public BinanceAutoInvestPlanStatus? Status { get; set; }

    /// <summary>
    /// Last updated date time
    /// </summary>
    [JsonProperty("lastUpdatedDateTime")]
    public DateTime? LastUpdateTime { get; set; }

    /// <summary>
    /// Target asset
    /// </summary>
    [JsonProperty("targetAsset")]
    public string TargetAsset { get; set; } = string.Empty;

    /// <summary>
    /// Total target quantity
    /// </summary>
    [JsonProperty("totalTargetAmount")]
    public decimal TotalTargetQuantity { get; set; }

    /// <summary>
    /// Source asset
    /// </summary>
    [JsonProperty("sourceAsset")]
    public string SourceAsset { get; set; } = string.Empty;

    /// <summary>
    /// Total invested in USD
    /// </summary>
    [JsonProperty("totalInvestedInUSD")]
    public decimal TotalInvestedInUsd { get; set; }

    /// <summary>
    /// Subscription quantity
    /// </summary>
    [JsonProperty("subscriptionAmount")]
    public decimal SubscriptionQuantity { get; set; }

    /// <summary>
    /// Subscription cycle
    /// </summary>
    [JsonProperty("subscriptionCycle")]
    public AutoInvestSubscriptionCycle SubscriptionCycle { get; set; }

    /// <summary>
    /// Subscription start day
    /// </summary>
    [JsonProperty("subscriptionStartDay")]
    public int? SubscriptionStartDay { get; set; }

    /// <summary>
    /// Subscription start weekday
    /// </summary>
    [JsonProperty("subscriptionStartWeekday")]
    public string SubscriptionStartWeekday { get; set; } = string.Empty;

    /// <summary>
    /// Subscription start time
    /// </summary>
    [JsonProperty("subscriptionStartTime")]
    public int? SubscriptionStartTime { get; set; }

    /// <summary>
    /// Source wallet
    /// </summary>
    [JsonProperty("sourceWallet")]
    public string SourceWallet { get; set; } = string.Empty;

    /// <summary>
    /// Flexible allowed to use
    /// </summary>
    [JsonProperty("flexibleAllowedToUse")]
    public bool FlexibleAllowedToUse { get; set; }

    /// <summary>
    /// Plan value in USD
    /// </summary>
    [JsonProperty("planValueInUSD")]
    public decimal? PlanValueInUsd { get; set; }

    /// <summary>
    /// Pnl in USD
    /// </summary>
    [JsonProperty("pnlInUSD")]
    public decimal? PnlInUsd { get; set; }

    /// <summary>
    /// Roi
    /// </summary>
    [JsonProperty("roi")]
    public decimal? Roi { get; set; }
}
