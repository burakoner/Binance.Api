namespace Binance.Api.Spot;

/// <summary>
/// Positions update
/// </summary>
public record BinanceSpotStreamPositionsUpdate : BinanceSpotStreamEvent
{
    /// <summary>
    /// Time of last account update
    /// </summary>
    [JsonProperty("u"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Balances
    /// </summary>
    [JsonProperty("B")]
    public List<BinanceSpotStreamBalance> Balances { get; set; } = [];

    /// <summary>
    /// The listen key the update was for
    /// </summary>
    [JsonIgnore]
    public string ListenKey { get; set; } = string.Empty;
}