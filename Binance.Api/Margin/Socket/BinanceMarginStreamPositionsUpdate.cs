namespace Binance.Api.Margin;

/// <summary>
/// Margin account-position update.
/// </summary>
public record BinanceMarginStreamPositionsUpdate : BinanceMarginStreamUpdate
{
    /// <summary>Time of the last account update.</summary>
    [JsonProperty("u"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    /// <summary>Balances changed by the update.</summary>
    [JsonProperty("B")]
    public List<BinanceMarginStreamBalance> Balances { get; set; } = [];
}
