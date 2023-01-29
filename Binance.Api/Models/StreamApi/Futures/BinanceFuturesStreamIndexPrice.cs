namespace Binance.Api.Models.StreamApi.Futures;

/// <summary>
/// Index price update
/// </summary>
public class BinanceFuturesStreamIndexPrice : BinanceStreamEvent
{
    /// <summary>
    /// The pair
    /// </summary>
    [JsonProperty("i")]
    public string Pair { get; set; }
    /// <summary>
    /// The index price
    /// </summary>
    [JsonProperty("p")]
    public decimal IndexPrice { get; set; }
}
