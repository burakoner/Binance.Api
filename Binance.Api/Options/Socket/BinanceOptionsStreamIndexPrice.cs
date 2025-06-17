namespace Binance.Api.Options;

/// <summary>
/// Binance Options Web Socket Stream Index Price
/// </summary>
public record BinanceOptionsStreamIndexPrice : BinanceSocketStreamEvent
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Index Price
    /// </summary>
    [JsonProperty("p")]
    public decimal IndexPrice { get; set; }
}
