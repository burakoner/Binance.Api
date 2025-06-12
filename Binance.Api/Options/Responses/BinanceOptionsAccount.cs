namespace Binance.Api.Options;

/// <summary>
/// Information about an account
/// </summary>
public record BinanceOptionsAccount
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
    public List<BinanceOptionsAccountGreek> Greek { get; set; } = [];

    /// <summary>
    /// Time when the account information was last updated
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }

    /// <summary>
    /// Risk level of the account
    /// </summary>
    public string RiskLevel { get; set; } = "";
}

/// <summary>
/// Options account asset information
/// </summary>
public record BinanceOptionsAccountAsset
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
    /// locked balance for order and position 
    /// </summary>
    public decimal Locked { get; set; }

    /// <summary>
    /// Unrealized profit/loss  
    /// </summary>
    public decimal UnrealizedPNL { get; set; }
}

/// <summary>
/// Options account Greek information
/// </summary>
public record BinanceOptionsAccountGreek
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
