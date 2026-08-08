namespace Binance.Api.Spot;

/// <summary>
/// Full rolling-window price-change statistics.
/// </summary>
public record BinanceSpotRollingWindowTicker : BinanceSpotMiniTicker
{
    /// <summary>
    /// Absolute price change during the window.
    /// </summary>
    public decimal PriceChange { get; set; }

    /// <summary>
    /// Relative price change during the window, in percent.
    /// </summary>
    public decimal PriceChangePercent { get; set; }

    /// <summary>
    /// Quote volume divided by base volume during the window.
    /// </summary>
    [JsonProperty("weightedAvgPrice")]
    public decimal WeightedAveragePrice { get; set; }
}
