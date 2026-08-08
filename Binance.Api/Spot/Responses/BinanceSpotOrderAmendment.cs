namespace Binance.Api.Spot;

/// <summary>
/// A keep-priority quantity amendment made to an order.
/// </summary>
public record BinanceSpotOrderAmendment
{
    /// <summary>The symbol.</summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>The order identifier.</summary>
    public long OrderId { get; set; }

    /// <summary>The execution identifier assigned to the amendment.</summary>
    public long ExecutionId { get; set; }

    /// <summary>The client order identifier before the amendment.</summary>
    [JsonProperty("origClientOrderId")]
    public string OriginalClientOrderId { get; set; } = string.Empty;

    /// <summary>The client order identifier after the amendment.</summary>
    public string NewClientOrderId { get; set; } = string.Empty;

    /// <summary>The quantity before the amendment.</summary>
    [JsonProperty("origQty")]
    public decimal OriginalQuantity { get; set; }

    /// <summary>The quantity after the amendment.</summary>
    [JsonProperty("newQty")]
    public decimal NewQuantity { get; set; }

    /// <summary>The amendment time.</summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }
}
