namespace Binance.Net.Objects.Models.Futures;

/// <summary>
/// Book price
/// </summary>
public record BinanceFuturesBookPrice//: BinanceBookPrice
{
    /// <summary>
    /// Pair
    /// </summary>
    [JsonProperty("pair")]
    public string Pair { get; set; } = string.Empty;
}
