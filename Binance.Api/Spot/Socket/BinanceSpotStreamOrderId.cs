namespace Binance.Api.Spot;

/// <summary>
/// Order reference
/// </summary>
public record BinanceSpotStreamOrderId
{
    /// <summary>
    /// The symbol of the order
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// The id of the order
    /// </summary>
    [JsonProperty("i")]
    public long OrderId { get; set; }

    /// <summary>
    /// The client order id
    /// </summary>
    [JsonProperty("c")]
    public string ClientOrderId { get; set; } = string.Empty;
}
