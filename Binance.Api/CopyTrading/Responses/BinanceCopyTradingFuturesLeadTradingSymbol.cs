namespace Binance.Api.CopyTrading;

/// <summary>
/// Copy trading lead symbol
/// </summary>
public record BinanceCopyTradingFuturesLeadTradingSymbol
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Base asset
    /// </summary>
    [JsonProperty("baseAsset")]
    public string BaseAsset { get; set; } = string.Empty;

    /// <summary>
    /// Quote asset
    /// </summary>
    [JsonProperty("quoteAsset")]
    public string QuoteAsset { get; set; } = string.Empty;
}
