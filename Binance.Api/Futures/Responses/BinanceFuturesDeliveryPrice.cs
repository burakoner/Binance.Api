namespace Binance.Api.Futures;

/// <summary>
/// Binance Futures Delivery Price
/// </summary>
public record BinanceFuturesDeliveryPrice
{
    /// <summary>
    /// Delivery time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("deliveryTime")]
    public DateTime DeliveryTime { get; set; }

    /// <summary>
    /// Delivery Price
    /// </summary>
    [JsonProperty("deliveryPrice")]
    public decimal DeliveryPrice { get; set; }
}