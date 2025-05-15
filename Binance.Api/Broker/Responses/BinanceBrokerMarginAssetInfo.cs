namespace Binance.Api.Broker;

/// <summary>
/// Margin Asset Info
/// </summary>
public record BinanceBrokerMarginAssetInfo
{
    /// <summary>
    /// Data
    /// </summary>
    [JsonProperty("data")]
    public List<BinanceBrokerageSubAccountMarginAssetInfo> Data { get; set; } = [];

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Account Margin Asset Info
/// </summary>
public record BinanceBrokerageSubAccountMarginAssetInfo
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    [JsonProperty("subaccountId")]
    public string SubAccountId { get; set; } = string.Empty;

    /// <summary>
    /// Margin enable
    /// </summary>
    [JsonProperty("marginEnable")]
    public bool IsMarginEnable { get; set; }

    /// <summary>
    /// Total Asset Of Btc
    /// </summary>
    [JsonProperty("totalAssetOfBtc")]
    public decimal TotalAssetOfBtc { get; set; }

    /// <summary>
    /// Total Liability Of Btc
    /// </summary>
    [JsonProperty("totalLiabilityBtc")]
    public decimal TotalLiabilityOfBtc { get; set; }

    /// <summary>
    /// Total Net Asset Of Btc
    /// </summary>
    [JsonProperty("totalNetAssetOfBtc")]
    public decimal TotalNetAssetOfBtc { get; set; }

    /// <summary>
    /// Margin level
    /// </summary>
    [JsonProperty("marginLevel")]
    public decimal MarginLevel { get; set; }
}
