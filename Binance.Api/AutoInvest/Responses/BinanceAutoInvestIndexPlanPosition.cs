namespace Binance.Api.AutoInvest;

/// <summary>
/// Plan position info
/// </summary>
public record BinanceAutoInvestIndexPlanPosition
{
    /// <summary>
    /// Index id
    /// </summary>
    [JsonProperty("indexId")]
    public long IndexId { get; set; }

    /// <summary>
    /// Total invested in USD
    /// </summary>
    [JsonProperty("totalInvestedInUSD")]
    public decimal TotalInvestedInUsd { get; set; }

    /// <summary>
    /// Current invested in USD
    /// </summary>
    [JsonProperty("currentInvestedInUSD")]
    public decimal CurrentInvestedInUsd { get; set; }

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
    /// Asset allocation
    /// </summary>
    [JsonProperty("assetAllocation")]
    public List<BinanceAutoInvestIndexPlanPositionAllocation> AssetAllocation { get; set; } = [];

    /// <summary>
    /// Details
    /// </summary>
    [JsonProperty("details")]
    public List<BinanceAutoInvestIndexPlanPositionDetails> Details { get; set; } = [];
}

/// <summary>
/// Asset allocation info
/// </summary>
public record BinanceAutoInvestIndexPlanPositionAllocation
{
    /// <summary>
    /// Target asset
    /// </summary>
    [JsonProperty("targetAsset")]
    public string TargetAsset { get; set; } = string.Empty;

    /// <summary>
    /// Allocation percentage
    /// </summary>
    [JsonProperty("allocation")]
    public decimal Allocation { get; set; }
}

/// <summary>
/// Position details
/// </summary>
public record BinanceAutoInvestIndexPlanPositionDetails
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
    /// Current invested in USD
    /// </summary>
    [JsonProperty("currentInvestedInUSD")]
    public decimal CurrentInvestedInUsd { get; set; }

    /// <summary>
    /// Purchased quantity
    /// </summary>
    [JsonProperty("purchasedAmount")]
    public decimal PurchasedQuantity { get; set; }

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
    /// Available quantity
    /// </summary>
    [JsonProperty("availableAmount")]
    public decimal AvailableQuantity { get; set; }

    /// <summary>
    /// Redeemed quantity
    /// </summary>
    [JsonProperty("redeemedAmount")]
    public decimal RedeemedQuantity { get; set; }

    /// <summary>
    /// Asset value in USD
    /// </summary>
    [JsonProperty("assetValueInUSD")]
    public decimal AssetValueInUsd { get; set; }
}
