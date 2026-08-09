namespace Binance.Api.Futures;

/// <summary>
/// Notional and Leverage Brackets
/// </summary>
public record BinanceFuturesSymbolBracket
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// user symbol bracket multiplier, only appears when user's symbol bracket is adjusted 
    /// </summary>
    [JsonProperty("notionalCoef")]
    public decimal? NotionalCoef { get; set; }

    /// <summary>
    /// Brackets
    /// </summary>
    [JsonProperty("brackets")]
    public List<BinanceFuturesBracket> Brackets { get; set; } = [];

}

/// <summary>
/// Bracket
/// </summary>
public record BinanceFuturesBracket
{
    /// <summary>
    /// Bracket
    /// </summary>
    [JsonProperty("bracket")]
    public long Bracket { get; set; }

    /// <summary>
    /// Max initial leverage for this bracket
    /// </summary>
    [JsonProperty("initialLeverage")]
    public long InitialLeverage { get; set; }

    /// <summary>
    /// Cap of this bracket
    /// </summary>
    [JsonProperty("notionalCap")]
    public long Cap { get; set; }

    [JsonProperty("qtyCap")]
    private long QuantityCap
    {
        set => Cap = value;
    }

    /// <summary>
    /// Floor of this bracket
    /// </summary>
    [JsonProperty("notionalFloor")]
    public long Floor { get; set; }

    [JsonProperty("qtylFloor")]
    private long QuantityFloor
    {
        set => Floor = value;
    }

    /// <summary>
    /// Maintenance ratio for this bracket
    /// </summary>
    [JsonProperty("maintMarginRatio")]
    public decimal MaintenanceMarginRatio { get; set; }

    /// <summary>
    /// Auxiliary number for quick calculation 
    /// </summary>
    [JsonProperty("cum")]
    public decimal MaintAmount { get; set; }
}
