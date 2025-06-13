namespace Binance.Api.Link;

/// <summary>
/// Futures Asset Info
/// </summary>
public record BinanceLinkFuturesAssetInfo
{
    /// <summary>
    /// Data
    /// </summary>
    [JsonProperty("data")]
    public List<BinanceBrokerageSubAccountFuturesAssetInfo> Data { get; set; } = [];

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Account Futures Asset Info
/// </summary>
public record BinanceBrokerageSubAccountFuturesAssetInfo
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    [JsonProperty("subaccountId")]
    public string SubAccountId { get; set; } = string.Empty;

    /// <summary>
    /// Futures enable
    /// </summary>
    [JsonProperty("futuresEnable")]
    public bool IsFuturesEnable { get; set; }

    /// <summary>
    /// Total Initial Margin Of Usdt
    /// </summary>
    [JsonProperty("totalInitialMarginOfUsdt")]
    public decimal TotalInitialMarginOfUsdt { get; set; }

    /// <summary>
    /// Total Maintenance Margin Of Usdt
    /// </summary>
    [JsonProperty("totalMaintenanceMarginOfUsdt")]
    public decimal TotalMaintenanceMarginOfUsdt { get; set; }

    /// <summary>
    /// Total Wallet Balance Of Usdt
    /// </summary>
    [JsonProperty("totalWalletBalanceOfUsdt")]
    public decimal TotalWalletBalanceOfUsdt { get; set; }

    /// <summary>
    /// Total Unrealized Profit Of Usdt
    /// </summary>
    [JsonProperty("totalUnrealizedProfitOfUsdt")]
    public decimal TotalUnrealizedProfitOfUsdt { get; set; }

    /// <summary>
    /// Total Margin Balance Of Usdt
    /// </summary>
    [JsonProperty("totalMarginBalanceOfUsdt")]
    public decimal TotalMarginBalanceOfUsdt { get; set; }

    /// <summary>
    /// Total Position Initial Margin Of Usdt
    /// </summary>
    [JsonProperty("totalPositionInitialMarginOfUsdt")]
    public decimal TotalPositionInitialMarginOfUsdt { get; set; }

    /// <summary>
    /// Total Open Order Initial Margin Of Usdt
    /// </summary>
    [JsonProperty("totalOpenOrderInitialMarginOfUsdt")]
    public decimal TotalOpenOrderInitialMarginOfUsdt { get; set; }
}