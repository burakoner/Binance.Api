namespace Binance.Api.Futures;

/// <summary>
/// Index price update
/// </summary>
public record BinanceFuturesStreamIndexPrice: BinanceFuturesStreamEvent
{
    /// <summary>
    /// The pair
    /// </summary>
    [JsonProperty("i")]
    public string Pair { get; set; } = string.Empty;

    /// <summary>
    /// The index price
    /// </summary>
    [JsonProperty("p")]
    public decimal IndexPrice { get; set; }
}
