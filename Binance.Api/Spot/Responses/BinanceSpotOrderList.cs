namespace Binance.Api.Spot;

/// <summary>
/// Current status of a Spot order list.
/// </summary>
public record BinanceSpotOrderList
{
    /// <summary>The order-list identifier.</summary>
    public long OrderListId { get; set; }

    /// <summary>The contingency type, such as OCO, OTO, OTOCO, OPO, or OPOCO.</summary>
    public string ContingencyType { get; set; } = string.Empty;

    /// <summary>The list execution status.</summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceListStatusType ListStatusType { get; set; }

    /// <summary>The aggregate order status.</summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceListOrderStatus ListOrderStatus { get; set; }

    /// <summary>The client order-list identifier.</summary>
    public string ListClientOrderId { get; set; } = string.Empty;

    /// <summary>The last list transaction time.</summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime TransactionTime { get; set; }

    /// <summary>The symbol.</summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>The orders belonging to the list.</summary>
    public List<BinanceOrderId> Orders { get; set; } = [];
}
