namespace Binance.Api.Futures;

/// <summary>
/// Binance Futures Index Price Constituents
/// </summary>
public record BinanceFuturesIndexPriceConstituents
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("time")]
    public DateTime Time { get; set; }

    /// <summary>
    /// Constituents
    /// </summary>
    [JsonProperty("constituents")]
    public List<BinanceFuturesIndexPriceConstituent> InitialMargin { get; set; } = [];
    }

/// <summary>
/// Binance Futures Index Price Constituent
/// </summary>
public record BinanceFuturesIndexPriceConstituent
{
    /// <summary>
    /// Exchange
    /// </summary>
    [JsonProperty("exchange")]
    public string Exchange { get; set; } = string.Empty;

    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Price
    /// </summary>
    [JsonProperty("price")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Weight
    /// </summary>
    [JsonProperty("weight")]
    public decimal? Weight { get; set; }
}
