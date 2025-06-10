namespace Binance.Api.CryptoLoan;

/// <summary>
/// Ltv adjustment info
/// </summary>
public record BinanceCryptoLoanStableAdjustmentRecord
{
    /// <summary>
    /// The loaning asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; } = string.Empty;

    /// <summary>
    /// The collateral asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public string CollateralAsset { get; set; } = string.Empty;

    /// <summary>
    /// Direction
    /// </summary>
    [JsonProperty("direction")]
    public string Direction { get; set; } = string.Empty;

    /// <summary>
    /// Amount
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Pre adjust ltv
    /// </summary>
    [JsonProperty("preLTV")]
    public decimal PreLtv { get; set; }

    /// <summary>
    /// Post adjust ltv
    /// </summary>
    [JsonProperty("afterLTV")]
    public decimal AfterLtv { get; set; }

    /// <summary>
    /// Adjust time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("adjustTime")]
    public DateTime AdjustTime { get; set; }

    /// <summary>
    /// Order id
    /// </summary>
    [JsonProperty("orderId")]
    public long OrderId { get; set; }
}
