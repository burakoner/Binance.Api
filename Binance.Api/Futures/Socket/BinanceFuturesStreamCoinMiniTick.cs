using Binance.Api.Spot;

namespace Binance.Api.Futures;

/// <summary>
/// Stream mini tick
/// </summary>
public record BinanceFuturesStreamCoinMiniTick : BinanceSpotStreamMiniTickBase
{
    /// <inheritdoc/>
    [JsonProperty("q")]
    public override decimal Volume { get; set; }

    /// <inheritdoc/>
    [JsonProperty("v")]
    public override decimal QuoteVolume { get; set; }

    /// <summary>
    /// The pair
    /// </summary>
    [JsonProperty("ps")]
    public string Pair { get; set; } = string.Empty;
}