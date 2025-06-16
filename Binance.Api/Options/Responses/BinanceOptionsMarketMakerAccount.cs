namespace Binance.Api.Options;

/// <summary>
/// Binance Options Market Maker Account
/// </summary>
public record BinanceOptionsMarketMakerAccount
{
    /// <summary>
    /// Asset information for the account
    /// </summary>
    [JsonProperty("asset")]
    public List<BinanceOptionsAccountAsset> Asset { get; set; } = [];

    /// <summary>
    /// Greek information for the account
    /// </summary>
    [JsonProperty("greek")]
    public List<BinanceOptionsMarketMakerAccountGreek> Greek { get; set; } = [];

    /// <summary>
    /// Time when the account information was last updated
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }
}

/// <summary>
/// Binance Options Market Maker Account Asset
/// </summary>
public record BinanceOptionsMarketMakerAccountAsset
{
    /// <summary>
    /// Asset type
    /// </summary>
    public string Asset { get; set; } = "";

    /// <summary>
    /// Account balance
    /// </summary>
    public decimal MarginBalance { get; set; }

    /// <summary>
    /// Account equity
    /// </summary>
    public decimal Equity { get; set; }

    /// <summary>
    /// Available funds
    /// </summary>
    public decimal Available { get; set; }

    /// <summary>
    /// Initial Margin
    /// </summary>
    [JsonProperty("initialMargin")]
    public decimal InitialMargin { get; set; }

    /// <summary>
    /// Maintenance Margin
    /// </summary>
    [JsonProperty("maintMargin")]
    public decimal MaintenanceMargin { get; set; }

    /// <summary>
    /// Unrealized profit/loss
    /// </summary>
    [JsonProperty("unrealizedPNL")]
    public decimal UnrealizedPNL { get; set; }

    /// <summary>
    /// Unrealized profit for long position 
    /// </summary>
    [JsonProperty("lpProfit")]
    public decimal LongPositionProfit { get; set; }
}

/// <summary>
/// Binance Options Market Maker Account Greek
/// </summary>
public record BinanceOptionsMarketMakerAccountGreek
{
    /// <summary>
    /// Underlying asset for the Greek information
    /// </summary>
    public string Underlying { get; set; } = "";

    /// <summary>
    /// Account Delta
    /// </summary>
    public decimal Delta { get; set; }

    /// <summary>
    /// Account Gamma
    /// </summary>
    public decimal Gamma { get; set; }

    /// <summary>
    /// Account Theta
    /// </summary>
    public decimal Theta { get; set; }

    /// <summary>
    /// Account Vega
    /// </summary>
    public decimal Vega { get; set; }
}
