namespace Binance.Api.Margin;

/// <summary>
/// Order reference in a Margin order-list event.
/// </summary>
public record BinanceMarginStreamOrderId
{
    /// <summary>Symbol.</summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>Order identifier.</summary>
    [JsonProperty("i")]
    public long OrderId { get; set; }

    /// <summary>Client order identifier.</summary>
    [JsonProperty("c")]
    public string ClientOrderId { get; set; } = string.Empty;
}
