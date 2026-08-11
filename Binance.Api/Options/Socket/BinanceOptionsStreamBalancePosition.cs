namespace Binance.Api.Options;

/// <summary>
/// Options balance and position update
/// </summary>
public record BinanceOptionsStreamBalancePosition : BinanceSocketStreamEvent
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
    /// Event reason
    /// </summary>
    [JsonProperty("m")]
    public BinanceOptionsBalancePositionUpdateReason? Reason { get; set; }

    /// <summary>
    /// Changed balances
    /// </summary>
    [JsonProperty("B")]
    public List<BinanceOptionsStreamBalancePositionBalance> Balances { get; set; } = [];

    /// <summary>
    /// Changed positions
    /// </summary>
    [JsonProperty("P")]
    public List<BinanceOptionsStreamBalancePositionPosition> Positions { get; set; } = [];
}

/// <summary>
/// Changed Options balance
/// </summary>
public record BinanceOptionsStreamBalancePositionBalance
{
    /// <summary>
    /// Margin asset
    /// </summary>
    [JsonProperty("a")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Account balance
    /// </summary>
    [JsonProperty("b")]
    public decimal? Balance { get; set; }

    /// <summary>
    /// Balance change excluding profit and loss and commission
    /// </summary>
    [JsonProperty("bc")]
    public decimal? BalanceChange { get; set; }
}

/// <summary>
/// Changed Options position
/// </summary>
public record BinanceOptionsStreamBalancePositionPosition
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Position quantity
    /// </summary>
    [JsonProperty("c")]
    public decimal? Quantity { get; set; }

    /// <summary>
    /// Position value
    /// </summary>
    [JsonProperty("p")]
    public decimal? PositionValue { get; set; }

    /// <summary>
    /// Average entry price
    /// </summary>
    [JsonProperty("a")]
    public decimal? AverageEntryPrice { get; set; }
}
