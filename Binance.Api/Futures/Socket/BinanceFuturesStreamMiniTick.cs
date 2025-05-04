using Binance.Api.Spot;

namespace Binance.Api.Futures;

/// <summary>
/// Stream mini tick
/// </summary>
public record BinanceFuturesStreamMiniTick : BinanceSpotStreamMiniTickBase
{
    /// <inheritdoc/>
    [JsonProperty("v")]
    public override decimal Volume { get; set; }

    /// <inheritdoc/>
    [JsonProperty("q")]
    public override decimal QuoteVolume { get; set; }
}