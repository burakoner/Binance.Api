namespace Binance.Api.Futures;

/// <summary>
/// Quantile estimation
/// </summary>
public record BinanceFuturesQuantileEstimation
{
    /// <summary>
    /// The symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Quantile
    /// </summary>
    [JsonProperty("adlQuantile")]
    public BinanceFuturesAdlQuantile? AdlQuantile { get; set; }
}

/// <summary>
/// Quantile info
/// </summary>
public record BinanceFuturesAdlQuantile
{
    /// <summary>
    /// Long position
    /// </summary>
    [JsonProperty("LONG")]
    public int Long { get; set; }

    /// <summary>
    /// Short position
    /// </summary>
    [JsonProperty("SHORT")]
    public int Short { get; set; }

    /// <summary>
    /// Hedge
    /// </summary>
    [JsonProperty("HEDGE")]
    public int Hedge { get; set; }

    /// <summary>
    /// Hedge
    /// </summary>
    [JsonProperty("BOTH")]
    public int Both { get; set; }
}
