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
    /// Hedge-mode marker for cross-margined positions
    /// </summary>
    [JsonProperty("HEDGE")]
    public int Hedge { get; set; }

    /// <summary>
    /// Position quantile in one-way mode
    /// </summary>
    [JsonProperty("BOTH")]
    public int Both { get; set; }
}
