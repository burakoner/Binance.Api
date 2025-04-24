namespace Binance.Api.Futures;

/// <summary>
/// Information about an account asset
/// </summary>
public record BinanceFuturesAccountAsset
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Initial Margin
    /// </summary>
    [JsonProperty("initialMargin")]
    public decimal InitialMargin { get; set; }

    /// <summary>
    /// Maintenance Margin
    /// </summary>
    [JsonProperty("maintMargin")]
    public decimal MaintMargin { get; set; }

    /// <summary>
    /// Margin Balance
    /// </summary>
    [JsonProperty("marginBalance")]
    public decimal MarginBalance { get; set; }

    /// <summary>
    /// Maximum Withdraw Quantity
    /// </summary>
    [JsonProperty("maxWithdrawAmount")]
    public decimal MaxWithdrawQuantity { get; set; }

    /// <summary>
    /// Open Order Initial Margin
    /// </summary>
    [JsonProperty("openOrderInitialMargin")]
    public decimal OpenOrderInitialMargin { get; set; }

    /// <summary>
    /// Position Initial Margin
    /// </summary>
    [JsonProperty("positionInitialMargin")]
    public decimal PositionInitialMargin { get; set; }

    /// <summary>
    /// Unrealized Profit
    /// </summary>
    [JsonProperty("unrealizedProfit")]
    public decimal UnrealizedPnl { get; set; }

    /// <summary>
    /// Wallet Balance
    /// </summary>
    [JsonProperty("walletBalance")]
    public decimal WalletBalance { get; set; }

    /// <summary>
    /// Crossed Wallet Balance
    /// </summary>
    [JsonProperty("crossWalletBalance")]
    public decimal CrossWalletBalance { get; set; }

    /// <summary>
    /// Unrealized profit of crossed positions
    /// </summary>
    [JsonProperty("crossUnPnl")]
    public decimal CrossUnrealizedPnl { get; set; }

    /// <summary>
    /// Available balance
    /// </summary>
    [JsonProperty("availableBalance")]
    public decimal AvailableBalance { get; set; }

    /// <summary>
    /// Whether the asset can be used as margin in Multi-Assets mode
    /// </summary>
    [JsonProperty("marginAvailable")]
    public bool? MarginAvailable { get; set; }
    /// <summary>
    /// Last update time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }
}
