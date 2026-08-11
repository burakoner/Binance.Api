namespace Binance.Api.Options;

/// <summary>
/// Options account update
/// </summary>
public record BinanceOptionsStreamAccount : BinanceSocketStreamEvent
{
    /// <summary>
    /// The listen key the update was received for
    /// </summary>
    [JsonIgnore]
    public string ListenKey { get; set; } = string.Empty;

    /// <summary>
    /// Transaction time
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime? TransactionTime { get; set; }

    /// <summary>
    /// Account equity in USDT
    /// </summary>
    [JsonProperty("eq")]
    public decimal? Equity { get; set; }

    /// <summary>
    /// Adjusted account equity in USDT
    /// </summary>
    [JsonProperty("aeq")]
    public decimal? AdjustedEquity { get; set; }

    /// <summary>
    /// Wallet balance in USDT
    /// </summary>
    [JsonProperty("b")]
    public decimal? WalletBalance { get; set; }

    /// <summary>
    /// Position value
    /// </summary>
    [JsonProperty("m")]
    public decimal? PositionValue { get; set; }

    /// <summary>
    /// Unrealized profit and loss
    /// </summary>
    [JsonProperty("u")]
    public decimal? UnrealizedPnl { get; set; }

    /// <summary>
    /// Initial margin in USDT
    /// </summary>
    [JsonProperty("i")]
    public decimal? InitialMargin { get; set; }

    /// <summary>
    /// Maintenance margin in USDT
    /// </summary>
    [JsonProperty("M")]
    public decimal? MaintenanceMargin { get; set; }
}
