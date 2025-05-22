namespace Binance.Api.AutoInvest;

/// <summary>
/// Redemption info
/// </summary>
public record BinanceAutoInvestRedemption
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
    /// Redemption id
    /// </summary>
    [JsonProperty("redemptionId")]
    public long RedemptionId { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public AutoInvestRedemptionStatus Status { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Redemption date time
    /// </summary>
    [JsonProperty("redemptionDateTime")]
    public DateTime RedeemTime { get; set; }

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
}
