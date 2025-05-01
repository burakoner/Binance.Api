using Binance.Api.Spot;

namespace Binance.Api.Futures;

/// <summary>
/// Stream tick
/// </summary>
public record BinanceFuturesStreamCoinTick : BinanceStreamTickBase
{
    /// <summary>
    /// Total traded volume in the base asset
    /// </summary>
    [JsonProperty("q")]
    public override decimal Volume { get; set; }

    /// <summary>
    /// Total traded volume in the quote asset
    /// </summary>
    [JsonProperty("v")]
    public override decimal QuoteVolume { get; set; }
}
