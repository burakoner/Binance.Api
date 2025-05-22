namespace Binance.Api.AutoInvest;

/// <summary>
/// Plan holdings
/// </summary>
public record BinanceAutoInvestHoldings
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
    /// Flexible allowed to use
    /// </summary>
    [JsonProperty("flexibleAllowedToUse")]
    public bool FlexibleAllowedToUse { get; set; }

    /// <summary>
    /// Creation date time
    /// </summary>
    [JsonProperty("creationDateTime")]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// First execution date time
    /// </summary>
    [JsonProperty("firstExecutionDateTime")]
    public DateTime FirstExecutionTime { get; set; }

    /// <summary>
    /// Next execution date time
    /// </summary>
    [JsonProperty("nextExecutionDateTime")]
    public DateTime? NextExecutionTime { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public BinanceAutoInvestPlanStatus Status { get; set; }

    /// <summary>
    /// Target asset
    /// </summary>
    [JsonProperty("targetAsset")]
    public string TargetAsset { get; set; } = string.Empty;

    /// <summary>
    /// Source asset
    /// </summary>
    [JsonProperty("sourceAsset")]
    public string SourceAsset { get; set; } = string.Empty;

    /// <summary>
    /// Plan value in USD
    /// </summary>
    [JsonProperty("planValueInUSD")]
    public decimal PlanValueInUsd { get; set; }

    /// <summary>
    /// Pnl in USD
    /// </summary>
    [JsonProperty("pnlInUSD")]
    public decimal PnlInUsd { get; set; }

    /// <summary>
    /// Roi
    /// </summary>
    [JsonProperty("roi")]
    public decimal Roi { get; set; }

    /// <summary>
    /// Total invested in USD
    /// </summary>
    [JsonProperty("totalInvestedInUSD")]
    public decimal TotalInvestedInUsd { get; set; }

    /// <summary>
    /// Details
    /// </summary>
    [JsonProperty("details")]
    public List<BinanceAutoInvestPlanHoldingDetails> Details { get; set; } = [];
}

/// <summary>
/// Holding details
/// </summary>
public record BinanceAutoInvestPlanHoldingDetails
{
    /// <summary>
    /// Target asset
    /// </summary>
    [JsonProperty("targetAsset")]
    public string TargetAsset { get; set; } = string.Empty;

    /// <summary>
    /// Average price in USD
    /// </summary>
    [JsonProperty("averagePriceInUSD")]
    public decimal AveragePriceInUsd { get; set; }

    /// <summary>
    /// Total invested in USD
    /// </summary>
    [JsonProperty("totalInvestedInUSD")]
    public decimal TotalInvestedInUsd { get; set; }

    /// <summary>
    /// Purchased quantity
    /// </summary>
    [JsonProperty("purchasedAmount")]
    public decimal PurchasedQuantity { get; set; }

    /// <summary>
    /// Purchased quantity asset
    /// </summary>
    [JsonProperty("purchasedAmountUnit")]
    public string PurchasedQuantityAsset { get; set; } = string.Empty;

    /// <summary>
    /// Pnl in USD
    /// </summary>
    [JsonProperty("pnlInUSD")]
    public decimal PnlInUsd { get; set; }

    /// <summary>
    /// Roi
    /// </summary>
    [JsonProperty("roi")]
    public decimal Roi { get; set; }

    /// <summary>
    /// Percentage
    /// </summary>
    [JsonProperty("percentage")]
    public decimal Percentage { get; set; }

    /// <summary>
    /// Asset status
    /// </summary>
    [JsonProperty("assetStatus")]
    public string AssetStatus { get; set; } = string.Empty;

    /// <summary>
    /// Available quantity
    /// </summary>
    [JsonProperty("availableAmount")]
    public decimal? AvailableQuantity { get; set; }

    /// <summary>
    /// Available quantity unit
    /// </summary>
    [JsonProperty("availableAmountUnit")]
    public string? AvailableQuantityUnit { get; set; }

    /// <summary>
    /// Redeemed amount
    /// </summary>
    [JsonProperty("redeemedAmout")]
    public decimal? RedeemedAmount { get; set; }

    /// <summary>
    /// Redeemed amount asset
    /// </summary>
    [JsonProperty("redeemedAmoutUnit")]
    public string? RedeemedAmountAsset { get; set; }

    /// <summary>
    /// Asset value in USD
    /// </summary>
    [JsonProperty("assetValueInUSD")]
    public decimal? AssetValueInUsd { get; set; }
}
