namespace Binance.Api.AutoInvest;

/// <summary>
/// Rebalance info
/// </summary>
public record BinanceAutoInvestRebalance
{
    /// <summary>
    /// Index id
    /// </summary>
    [JsonProperty("indexId")]
    public long IndexId { get; set; }

    /// <summary>
    /// Index name
    /// </summary>
    [JsonProperty("indexName")]
    public string IndexName { get; set; } = string.Empty;

    /// <summary>
    /// Rebalance id
    /// </summary>
    [JsonProperty("rebalanceId")]
    public long RebalanceId { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Rebalance fee
    /// </summary>
    [JsonProperty("rebalanceFee")]
    public decimal RebalanceFee { get; set; }

    /// <summary>
    /// Rebalance fee unit
    /// </summary>
    [JsonProperty("rebalanceFeeUnit")]
    public string RebalanceFeeAsset { get; set; } = string.Empty;

    /// <summary>
    /// Transaction details
    /// </summary>
    [JsonProperty("transactionDetails")]
    public List<BinanceAutoInvestRebalanceDetails> TransactionDetails { get; set; } = [];
}

/// <summary>
/// Rebalance info details
/// </summary>
public record BinanceAutoInvestRebalanceDetails
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Transaction date time
    /// </summary>
    [JsonProperty("transactionDateTime")]
    public DateTime TransactionTime { get; set; }

    /// <summary>
    /// Rebalance direction
    /// </summary>
    [JsonProperty("rebalanceDirection")]
    public string RebalanceDirection { get; set; } = string.Empty;

    /// <summary>
    /// Rebalance quantity
    /// </summary>
    [JsonProperty("rebalanceAmount")]
    public decimal RebalanceQuantity { get; set; }
}
