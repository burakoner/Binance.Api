namespace Binance.Api.Models.WebSocketApi.Futures;

/// <summary>
/// Index price update
/// </summary>
public class BinanceFuturesStreamIndexPrice : BinanceSocketEvent
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
