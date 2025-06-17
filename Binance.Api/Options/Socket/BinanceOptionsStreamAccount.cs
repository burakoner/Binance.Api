namespace Binance.Api.Options;

/// <summary>
/// Information about an account
/// </summary>
public record BinanceOptionsStreamAccount : BinanceSocketStreamEvent
{
    /// <summary>
    /// The listen key the update was for
    /// </summary>
    [JsonIgnore]
    public string ListenKey { get; set; } = string.Empty;

    /// <summary>
    /// Asset information for the account
    /// </summary>
    [JsonProperty("B")]
    public List<BinanceOptionsStreamAccountBalance> Balances { get; set; } = [];

    /// <summary>
    /// Greek information for the account
    /// </summary>
    [JsonProperty("G")]
    public List<BinanceOptionsStreamAccountGreek> Greek { get; set; } = [];

    /// <summary>
    /// Position information for the account
    /// </summary>
    [JsonProperty("P")]
    public List<BinanceOptionsStreamAccountPosition> Positions { get; set; } = [];

    /// <summary>
    /// User ID
    /// </summary>
    [JsonProperty("uid")]
    public string UID { get; set; } = "";
}

/// <summary>
/// Binance Options Stream Account Balance
/// </summary>
public record BinanceOptionsStreamAccountBalance
{
    /// <summary>
    /// Account Balance
    /// </summary>
    [JsonProperty("b")]
    public decimal AccountBalance { get; set; }

    /// <summary>
    /// Position Equity
    /// </summary>
    [JsonProperty("m")]
    public decimal PositionValue { get; set; }

    /// <summary>
    /// Unrealized profit/loss  
    /// </summary>
    [JsonProperty("u")]
    public decimal UnrealizedPNL { get; set; }

    /// <summary>
    /// Positive Unrealized profit/loss  
    /// </summary>
    [JsonProperty("U")]
    public decimal PositiveUnrealizedPNL { get; set; }

    /// <summary>
    /// Maintenance Margin
    /// </summary>
    [JsonProperty("M")]
    public decimal MaintenanceMargin { get; set; }

    /// <summary>
    /// Initial Margin
    /// </summary>
    [JsonProperty("i")]
    public decimal InitialMargin { get; set; }

    /// <summary>
    /// Asset type
    /// </summary>
    [JsonProperty("a")]
    public string Asset { get; set; } = "";
}

/// <summary>
/// Binance Options Stream Account Greek
/// </summary>
public record BinanceOptionsStreamAccountGreek
{
    /// <summary>
    /// Underlying asset for the Greek information
    /// </summary>
    [JsonProperty("ui")]
    public string Underlying { get; set; } = "";

    /// <summary>
    /// Account Delta
    /// </summary>
    [JsonProperty("d")]
    public decimal Delta { get; set; }

    /// <summary>
    /// Account Theta
    /// </summary>
    [JsonProperty("t")]
    public decimal Theta { get; set; }

    /// <summary>
    /// Account Gamma
    /// </summary>
    [JsonProperty("g")]
    public decimal Gamma { get; set; }

    /// <summary>
    /// Account Vega
    /// </summary>
    [JsonProperty("v")]
    public decimal Vega { get; set; }
}

/// <summary>
/// Binance Options Stream Account Position
/// </summary>
public record BinanceOptionsStreamAccountPosition
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Number of current positions   
    /// </summary>
    [JsonProperty("c")]
    public int CurrentPositions { get; set; }

    /// <summary>
    /// Number of positions that can be reduced   
    /// </summary>
    [JsonProperty("r")]
    public int ReduciblePositions { get; set; }

    /// <summary>
    /// Position Value
    /// </summary>
    [JsonProperty("p")]
    public decimal PositionValue { get; set; }

    /// <summary>
    /// Average Entry Price
    /// </summary>
    [JsonProperty("a")]
    public decimal AverageEntryPrice { get; set; }
}
