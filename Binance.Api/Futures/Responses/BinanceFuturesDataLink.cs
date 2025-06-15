namespace Binance.Api.Futures;

/// <summary>
/// Future TickLevel Orderbook Historical Data Download Link
/// </summary>
public record BinanceFuturesDataLink
{
    /// <summary>
    /// Day
    /// </summary>
    [JsonProperty("day")]
    public string Day { get; set; } = string.Empty;

    /// <summary>
    /// Url
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;
}
