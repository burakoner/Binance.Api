namespace Binance.Api.SubAccount;

/// <summary>
/// Sub account futures details
/// </summary>
public record BinanceSubAccountFuturesDetailsV2
{
    /// <summary>
    /// Futures account response (USDT margined)
    /// </summary>
    [JsonProperty("futureAccountResp")]
    public BinanceSubAccountFuturesDetailV2Usdt UsdtMarginedFutures { get; set; } = default!;

    /// <summary>
    /// Delivery account response (COIN margined)
    /// </summary>
    [JsonProperty("deliveryAccountResp")]
    public BinanceSubAccountFuturesDetailV2 CoinMarginedFutures { get; set; } = default!;
}

/// <summary>
/// Sub account futures details
/// </summary>
public record BinanceSubAccountFuturesDetailV2
{
    /// <summary>
    /// Email of the sub account
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// List of asset details
    /// </summary>
    [JsonProperty("assets")]
    public BinanceSubAccountFuturesAsset[] Assets { get; set; } = [];

    /// <summary>
    /// Can deposit
    /// </summary>
    [JsonProperty("canDeposit")]
    public bool CanDeposit { get; set; }

    /// <summary>
    /// Can trade
    /// </summary>
    [JsonProperty("canTrade")]
    public bool CanTrade { get; set; }

    /// <summary>
    /// Can withdraw
    /// </summary>
    [JsonProperty("canWithdraw")]
    public bool CanWithdraw { get; set; }

    /// <summary>
    /// Fee tier
    /// </summary>
    [JsonProperty("feeTier")]
    public int FeeTier { get; set; }

    /// <summary>
    /// Time of the data
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("updateTime")]
    public DateTime UpdateTime { get; set; }
}

/// <summary>
/// Sub account futures details
/// </summary>
public record BinanceSubAccountFuturesDetailV2Usdt : BinanceSubAccountFuturesDetailV2
{
    /// <summary>
    /// Max quantity which can be withdrawn
    /// </summary>
    [JsonProperty("maxWithdrawAmount")]
    public decimal MaxWithdrawQuantity { get; set; }

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
}
