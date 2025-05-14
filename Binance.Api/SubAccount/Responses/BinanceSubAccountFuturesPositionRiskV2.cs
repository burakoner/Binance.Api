namespace Binance.Api.SubAccount;

/// <summary>
/// Sub account position risk
/// </summary>
public record BinanceSubAccountFuturesPositionRiskV2
{
    /// <summary>
    /// Futures account response (USDT margined)
    /// </summary>
    [JsonProperty("futurePositionRiskVos")]
    public BinanceSubAccountFuturesPositionRisk[] UsdtMarginedFutures { get; set; } = [];

    /// <summary>
    /// Delivery account response (COIN margined)
    /// </summary>
    [JsonProperty("deliveryPositionRiskVos")]
    public BinanceSubAccountFuturesPositionRiskCoin[] CoinMarginedFutures { get; set; } = [];
}

/// <summary>
/// Sub account position risk
/// </summary>
public record BinanceSubAccountFuturesPositionRiskCoin
{
    /// <summary>
    /// The entry price
    /// </summary>
    [JsonProperty("entryPrice")]
    public decimal EntryPrice { get; set; }

    /// <summary>
    /// Mark price
    /// </summary>
    [JsonProperty("markPrice")]
    public decimal MarkPrice { get; set; }

    /// <summary>
    /// Leverage
    /// </summary>
    [JsonProperty("leverage")]
    public decimal Leverage { get; set; }

    /// <summary>
    /// Isolated
    /// </summary>
    [JsonProperty("isolated")]
    public bool Isolated { get; set; }

    /// <summary>
    /// Isolated wallet
    /// </summary>
    [JsonProperty("isolatedWallet")]
    public decimal IsolatedWallet { get; set; }

    /// <summary>
    /// Isolated margin
    /// </summary>
    [JsonProperty("isolatedMargin")]
    public decimal IsolatedMargin { get; set; }

    /// <summary>
    /// Is auto add margin
    /// </summary>
    [JsonProperty("isAutoAddMargin")]
    public bool IsAutoAddMargin { get; set; }

    /// <summary>
    /// Position side
    /// </summary>
    [JsonProperty("positionSide")]
    public BinancePositionSide PositionSide { get; set; }

    /// <summary>
    /// Position amount
    /// </summary>
    [JsonProperty("positionAmount")]
    public decimal PositionQuantity { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Unrealized profit
    /// </summary>
    [JsonProperty("unrealizedProfit")]
    public decimal UnrealizedProfit { get; set; }
}
