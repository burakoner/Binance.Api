namespace Binance.Api.Models.WebSocketApi.MarketData;

/// <summary>
/// Positions update
/// </summary>
public record BinanceStreamPositionsUpdate : BinanceSocketEvent
{
    /// <summary>
    /// Time of last account update
    /// </summary>
    [JsonProperty("u"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// The listen key the update was for
    /// </summary>
    public string ListenKey { get; set; } = "";
    /// <summary>
    /// Balances
    /// </summary>
    [JsonProperty("B")]
    public IEnumerable<BinanceStreamBalance> Balances { get; set; } = [];
}

/// <summary>
/// Information about an asset balance
/// </summary>
public record BinanceStreamBalance
{
    /// <summary>
    /// The asset this balance is for
    /// </summary>
    [JsonProperty("a")]
    public string Asset { get; set; } = "";
    /// <summary>
    /// The quantity that isn't locked in a trade
    /// </summary>
    [JsonProperty("f")]
    public decimal Available { get; set; }
    /// <summary>
    /// The quantity that is currently locked in a trade
    /// </summary>
    [JsonProperty("l")]
    public decimal Locked { get; set; }
    /// <summary>
    /// The total balance of this asset (Free + Locked)
    /// </summary>
    public decimal Total => Available + Locked;
}
