namespace Binance.Api.Spot;

/// <summary>
/// Price change stats for the current trading day
/// </summary>
public record BinanceTradingDayFullTicker: BinanceTradingDayMiniTicker
{
    /// <summary>
    /// The actual price change in the last 24 hours
    /// </summary>
    public decimal PriceChange { get; set; }

    /// <summary>
    /// The price change in percentage in the last 24 hours
    /// </summary>
    public decimal PriceChangePercent { get; set; }

    /// <summary>
    /// The weighted average price in the last 24 hours
    /// </summary>
    [JsonProperty("weightedAvgPrice")]
    public decimal WeightedAveragePrice { get; set; }
}
