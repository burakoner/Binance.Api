namespace Binance.Api.SubAccount;

/// <summary>
/// Sub accounts futures summary
/// </summary>
public record BinanceSubAccountFuturesSummary
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Total initial margin
    /// </summary>
    [JsonProperty("totalInitialMargin")]
    public decimal TotalInitialMargin { get; set; }

    /// <summary>
    /// Total maintenance margin
    /// </summary>
    [JsonProperty("totalMaintenanceMargin")]
    public decimal TotalMaintenanceMargin { get; set; }

    /// <summary>
    /// Total margin balance
    /// </summary>
    [JsonProperty("totalMarginBalance")]
    public decimal TotalMarginBalance { get; set; }

    /// <summary>
    /// Total open order initial margin
    /// </summary>
    [JsonProperty("totalOpenOrderInitialMargin")]
    public decimal TotalOpenOrderInitialMargin { get; set; }

    /// <summary>
    /// Total position initial margin
    /// </summary>
    [JsonProperty("totalPositionInitialMargin")]
    public decimal TotalPositionInitialMargin { get; set; }

    /// <summary>
    /// Total unrealized profit
    /// </summary>
    [JsonProperty("totalUnrealizedProfit")]
    public decimal TotalUnrealizedProfit { get; set; }

    /// <summary>
    /// Total wallet balance
    /// </summary>
    [JsonProperty("totalWalletBalance")]
    public decimal TotalWalletBalance { get; set; }

    /// <summary>
    /// Sub accounts info
    /// </summary>
    [JsonProperty("subAccountList")]
    public List<BinanceSubAccountFuturesInfo>  SubAccounts { get; set; } = [];
}

/// <summary>
/// Sub account future details
/// </summary>
public record BinanceSubAccountFuturesInfo
{
    /// <summary>
    /// Email of the sub account
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Total initial margin
    /// </summary>
    [JsonProperty("totalInitialMargin")]
    public decimal TotalInitialMargin { get; set; }

    /// <summary>
    /// Total maintenance margin
    /// </summary>
    [JsonProperty("totalMaintenanceMargin")]
    public decimal TotalMaintenanceMargin { get; set; }

    /// <summary>
    /// Total margin balance
    /// </summary>
    [JsonProperty("totalMarginBalance")]
    public decimal TotalMarginBalance { get; set; }

    /// <summary>
    /// Total open order initial margin
    /// </summary>
    [JsonProperty("totalOpenOrderInitialMargin")]
    public decimal TotalOpenOrderInitialMargin { get; set; }

    /// <summary>
    /// Total position initial margin
    /// </summary>
    [JsonProperty("totalPositionInitialMargin")]
    public decimal TotalPositionInitialMargin { get; set; }

    /// <summary>
    /// Total unrealized profit
    /// </summary>
    [JsonProperty("totalUnrealizedProfit")]
    public decimal TotalUnrealizedProfit { get; set; }

    /// <summary>
    /// Total wallet balance
    /// </summary>
    [JsonProperty("totalWalletBalance")]
    public decimal TotalWalletBalance { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;
}
