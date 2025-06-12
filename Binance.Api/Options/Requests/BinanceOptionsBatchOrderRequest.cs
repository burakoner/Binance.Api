namespace Binance.Api.Options;

/// <summary>
/// Parameters for a new options batch order
/// </summary>
public record BinanceOptionsBatchOrderRequest
{
    /// <summary>
    /// Symbol of the order
    /// </summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Side of the order
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// Order Type
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceOptionsOrderType Type { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// Price
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Time in force
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceTimeInForce? TimeInForce { get; set; }

    /// <summary>
    /// A unique id among open orders. Automatically generated if not sent.
    /// </summary>
    public string ClientOrderId { get; set; } = string.Empty;

    /// <summary>
    /// Reduce only, default false
    /// </summary>
    public bool? ReduceOnly { get; set; }

    /// <summary>
    /// Post only
    /// </summary>
    public bool? PostOnly { get; set; }

    /// <summary>
    /// Is MMP Order
    /// </summary>
    public bool? MMP { get; set; }
}
