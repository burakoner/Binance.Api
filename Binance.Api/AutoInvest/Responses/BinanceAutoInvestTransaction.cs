namespace Binance.Api.AutoInvest;

/// <summary>
/// Plan transaction info
/// </summary>
public record BinanceAutoInvestTransaction
{
    /// <summary>
    /// Id
    /// </summary>
    [JsonProperty("id")]
    public long Id { get; set; }

    /// <summary>
    /// Target asset
    /// </summary>
    [JsonProperty("targetAsset")]
    public string TargetAsset { get; set; } = string.Empty;

    /// <summary>
    /// Plan type
    /// </summary>
    [JsonProperty("planType")]
    public BinanceAutoInvestPlanType PlanType { get; set; }

    /// <summary>
    /// Plan name
    /// </summary>
    [JsonProperty("planName")]
    public string PlanName { get; set; } = string.Empty;

    /// <summary>
    /// Plan id
    /// </summary>
    [JsonProperty("planId")]
    public long PlanId { get; set; }

    /// <summary>
    /// Transaction time
    /// </summary>
    [JsonProperty("transactionDateTime")]
    public DateTime TransactionTime { get; set; }

    /// <summary>
    /// Transaction status
    /// </summary>
    [JsonProperty("transactionStatus")]
    public AutoInvestTransactionStatus TransactionStatus { get; set; }

    /// <summary>
    /// Failed type
    /// </summary>
    [JsonProperty("failedType")]
    public string? FailedType { get; set; }

    /// <summary>
    /// Source asset
    /// </summary>
    [JsonProperty("sourceAsset")]
    public string SourceAsset { get; set; } = string.Empty;

    /// <summary>
    /// Source asset quantity
    /// </summary>
    [JsonProperty("sourceAssetAmount")]
    public decimal SourceAssetQuantity { get; set; }

    /// <summary>
    /// Target asset quantity
    /// </summary>
    [JsonProperty("targetAssetAmount")]
    public decimal TargetAssetQuantity { get; set; }

    /// <summary>
    /// Source wallet
    /// </summary>
    [JsonProperty("sourceWallet")]
    public string SourceWallet { get; set; } = string.Empty;

    /// <summary>
    /// Flexible used
    /// </summary>
    [JsonProperty("flexibleUsed")]
    public bool FlexibleUsed { get; set; }

    /// <summary>
    /// Transaction fee
    /// </summary>
    [JsonProperty("transactionFee")]
    public decimal Fee { get; set; }

    /// <summary>
    /// Transaction fee unit
    /// </summary>
    [JsonProperty("transactionFeeUnit")]
    public string FeeAsset { get; set; } = string.Empty;

    /// <summary>
    /// Execution price
    /// </summary>
    [JsonProperty("executionPrice")]
    public decimal ExecutionPrice { get; set; }

    /// <summary>
    /// Execution type
    /// </summary>
    [JsonProperty("executionType")]
    public BinanceAutoInvestExecutionType ExecutionType { get; set; }

    /// <summary>
    /// Subscription cycle
    /// </summary>
    [JsonProperty("subscriptionCycle")]
    public string SubscriptionCycle { get; set; } = string.Empty;
}
