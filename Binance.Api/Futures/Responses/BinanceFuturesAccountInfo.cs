namespace Binance.Api.Futures;

/// <summary>
/// Information about an account
/// </summary>
public record BinanceFuturesAccountInfo
{
    /// <summary>
    /// Information about an account assets
    /// </summary>
    [JsonProperty("assets")]
    public List<BinanceFuturesAccountAsset> Assets { get; set; } = [];

    /// <summary>
    /// Boolean indicating if this account can deposit
    /// </summary>
    [JsonProperty("canDeposit")]
    public bool CanDeposit { get; set; }

    /// <summary>
    /// Boolean indicating if this account can trade
    /// </summary>
    [JsonProperty("canTrade")]
    public bool CanTrade { get; set; }

    /// <summary>
    /// Boolean indicating if this account can withdraw
    /// </summary>
    [JsonProperty("canWithdraw")]
    public bool CanWithdraw { get; set; }

    /// <summary>
    /// Boolean indicating if this account is in multi asset mode
    /// </summary>
    [JsonProperty("multiAssetsMargin")]
    public bool MultiAssetsMargin { get; set; }

    /// <summary>
    /// Trade group id
    /// </summary>
    [JsonProperty("tradeGroupId")]
    public int TradeGroupId { get; set; }

    /// <summary>
    /// Fee tier
    /// </summary>
    [JsonProperty("feeTier")]
    public int FeeTier { get; set; }

    /// <summary>
    /// Maximum withdraw quantity
    /// </summary>
    [JsonProperty("maxWithdrawAmount")]
    public decimal MaxWithdrawQuantity { get; set; }

    /// <summary>
    /// Information about an account positions
    /// </summary>
    [JsonProperty("positions")]
    public List<BinanceFuturesUsdPositionInfo> Positions { get; set; } = [];

    /// <summary>
    /// Total initial margin
    /// </summary>
    [JsonProperty("totalInitialMargin")]
    public decimal TotalInitialMargin { get; set; }

    /// <summary>
    /// Total maintenance margin
    /// </summary>
    [JsonProperty("totalMaintMargin")]
    public decimal TotalMaintMargin { get; set; }

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
    /// Total positional initial margin
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
    /// Total crossed wallet balance
    /// </summary>
    [JsonProperty("totalCrossWalletBalance")]
    public decimal TotalCrossWalletBalance { get; set; }

    /// <summary>
    /// Unrealized profit of crossed positions
    /// </summary>
    [JsonProperty("totalCrossUnPnl")]
    public decimal TotalCrossUnPnl { get; set; }

    /// <summary>
    /// Available balance
    /// </summary>
    [JsonProperty("availableBalance")]
    public decimal AvailableBalance { get; set; }

    /// <summary>
    /// The time of account info was updated
    /// </summary>
    [JsonProperty("updateTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime? UpdateTime { get; set; }
}
