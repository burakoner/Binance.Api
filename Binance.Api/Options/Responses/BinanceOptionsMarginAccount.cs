namespace Binance.Api.Options;

/// <summary>
/// Binance Options Margin Account
/// </summary>
public record BinanceOptionsMarginAccount
{
    /// <summary>
    /// Asset information for the account
    /// </summary>
    [JsonProperty("asset")]
    public List<BinanceOptionsMarginAccountBalance> Balances { get; set; } = [];

    /// <summary>
    /// Greek information for the account
    /// </summary>
    [JsonProperty("greek")]
    public List<BinanceOptionsMarginAccountGreek> Greek { get; set; } = [];

    /// <summary>
    /// Time when the account information was last updated
    /// </summary>
    [JsonProperty("time")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }

    /// <summary>
    /// Whether the account can trade
    /// </summary>
    [JsonProperty("canTrade")]
    public bool CanTrade { get; set; }

    /// <summary>
    /// Whether the account can deposit
    /// </summary>
    [JsonProperty("canDeposit")]
    public bool CanDeposit { get; set; }

    /// <summary>
    /// Whether the account can withdraw
    /// </summary>
    [JsonProperty("canWithdraw")]
    public bool CanWithdraw { get; set; }

    /// <summary>
    /// Whether the account is restricted to reducing positions
    /// </summary>
    [JsonProperty("reduceOnly")]
    public bool ReduceOnly { get; set; }

    /// <summary>
    /// Self-trade prevention trade group identifier
    /// </summary>
    [JsonProperty("tradeGroupId")]
    public long TradeGroupId { get; set; }
}

/// <summary>
/// Binance Options Margin Account Asset
/// </summary>
public record BinanceOptionsMarginAccountBalance
{
    /// <summary>
    /// Asset type
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Account balance
    /// </summary>
    [JsonProperty("marginBalance")]
    public decimal MarginBalance { get; set; }

    /// <summary>
    /// Account equity
    /// </summary>
    [JsonProperty("equity")]
    public decimal Equity { get; set; }

    /// <summary>
    /// Available funds
    /// </summary>
    [JsonProperty("available")]
    public decimal Available { get; set; }

    /// <summary>
    /// Initial margin
    /// </summary>
    [JsonProperty("initialMargin")]
    public decimal InitialMargin { get; set; }

    /// <summary>
    /// Maintenance margin
    /// </summary>
    [JsonProperty("maintMargin")]
    public decimal MaintenanceMargin { get; set; }

    /// <summary>
    /// Unrealized profit and loss
    /// </summary>
    [JsonProperty("unrealizedPNL")]
    public decimal UnrealizedPNL { get; set; }

    /// <summary>
    /// Adjusted account equity
    /// </summary>
    [JsonProperty("adjustedEquity")]
    public decimal AdjustedEquity { get; set; }
}

/// <summary>
/// Binance Options Margin Account Greek
/// </summary>
public record BinanceOptionsMarginAccountGreek
{
    /// <summary>
    /// Underlying asset for the Greek information
    /// </summary>
    [JsonProperty("underlying")]
    public string Underlying { get; set; } = string.Empty;

    /// <summary>
    /// Account delta
    /// </summary>
    [JsonProperty("delta")]
    public decimal Delta { get; set; }

    /// <summary>
    /// Account gamma
    /// </summary>
    [JsonProperty("gamma")]
    public decimal Gamma { get; set; }

    /// <summary>
    /// Account theta
    /// </summary>
    [JsonProperty("theta")]
    public decimal Theta { get; set; }

    /// <summary>
    /// Account vega
    /// </summary>
    [JsonProperty("vega")]
    public decimal Vega { get; set; }
}
