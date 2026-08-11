namespace Binance.Api.Options;

/// <summary>
/// Binance Options Market Maker Underlyings
/// </summary>
public record BinanceOptionsMarketMakerUnderlyings
{
    /// <summary>
    /// Underlyings
    /// </summary>
    [JsonProperty("underlyings")]
    public List<string> Underlyings { get; set; } = [];
}
