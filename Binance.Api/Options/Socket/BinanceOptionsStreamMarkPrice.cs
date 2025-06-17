namespace Binance.Api.Options;

/// <summary>
/// Binance Options Web Socket Stream Mark Price
/// </summary>
public record BinanceOptionsStreamMarkPrice : BinanceSocketStreamEvent
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Mark Price
    /// </summary>
    [JsonProperty("mp")]
    public decimal MarkPrice { get; set; }
}
