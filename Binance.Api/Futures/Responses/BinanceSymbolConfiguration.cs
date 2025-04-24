namespace Binance.Api.Futures;

/// <summary>
/// Symbol configuration
/// </summary>
public record BinanceSymbolConfiguration
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Margin type
    /// </summary>
    [JsonProperty("marginType")]
    public BinanceFuturesMarginType? MarginType { get; set; }

    /// <summary>
    /// Is auto add margin
    /// </summary>
    [JsonProperty("isAutoAddMargin")]
    public bool IsAutoAddMargin { get; set; }

    /// <summary>
    /// Leverage
    /// </summary>
    [JsonProperty("leverage")]
    public decimal Leverage { get; set; }

    /// <summary>
    /// Max notional value
    /// </summary>
    [JsonProperty("maxNotionalValue")]
    public decimal MaxNotionalValue { get; set; }
}
