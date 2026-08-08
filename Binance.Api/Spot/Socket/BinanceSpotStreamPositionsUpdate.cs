namespace Binance.Api.Spot;

/// <summary>
/// Positions update
/// </summary>
public record BinanceSpotStreamPositionsUpdate : BinanceSpotUserDataStreamEvent
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

}
