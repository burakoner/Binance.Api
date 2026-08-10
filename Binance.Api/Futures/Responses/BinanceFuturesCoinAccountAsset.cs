namespace Binance.Api.Futures;

/// <summary>
/// COIN-M account asset information
/// </summary>
public record BinanceFuturesCoinAccountAsset
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Wallet balance
    /// </summary>
    [JsonProperty("walletBalance")]
    public decimal WalletBalance { get; set; }

    /// <summary>
    /// Unrealized profit
    /// </summary>
    [JsonProperty("unrealizedProfit")]
    public decimal UnrealizedPnl { get; set; }

    /// <summary>
    /// Margin balance
    /// </summary>
    [JsonProperty("marginBalance")]
    public decimal MarginBalance { get; set; }

    /// <summary>
    /// Maintenance margin
    /// </summary>
    [JsonProperty("maintMargin")]
    public decimal MaintMargin { get; set; }

    /// <summary>
    /// Initial margin
    /// </summary>
    [JsonProperty("initialMargin")]
    public decimal InitialMargin { get; set; }

    /// <summary>
    /// Position initial margin
    /// </summary>
    [JsonProperty("positionInitialMargin")]
    public decimal PositionInitialMargin { get; set; }

    /// <summary>
    /// Open-order initial margin
    /// </summary>
    [JsonProperty("openOrderInitialMargin")]
    public decimal OpenOrderInitialMargin { get; set; }

    /// <summary>
    /// Maximum withdraw amount
    /// </summary>
    [JsonProperty("maxWithdrawAmount")]
    public decimal MaxWithdrawQuantity { get; set; }

    /// <summary>
    /// Cross wallet balance
    /// </summary>
    [JsonProperty("crossWalletBalance")]
    public decimal CrossWalletBalance { get; set; }

    /// <summary>
    /// Cross-position unrealized profit
    /// </summary>
    [JsonProperty("crossUnPnl")]
    public decimal CrossUnrealizedPnl { get; set; }

    /// <summary>
    /// Available balance
    /// </summary>
    [JsonProperty("availableBalance")]
    public decimal AvailableBalance { get; set; }

    /// <summary>
    /// Update time
    /// </summary>
    [JsonProperty("updateTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateTime { get; set; }
}
