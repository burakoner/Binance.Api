using Binance.Api.Spot;

namespace Binance.Api.Margin;

/// <summary>
/// One result returned when all open Margin orders on a symbol are canceled. Binance can return
/// either an individual order or an order-list summary in this array.
/// </summary>
public record BinanceMarginCanceledOrder : BinanceSpotOrderBase
{
    /// <summary>Order-list contingency type when the result represents a list.</summary>
    public string? ContingencyType { get; set; }

    /// <summary>Order-list execution status when the result represents a list.</summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceListStatusType? ListStatusType { get; set; }

    /// <summary>Aggregate order-list status when the result represents a list.</summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceListOrderStatus? ListOrderStatus { get; set; }

    /// <summary>Client order-list identifier.</summary>
    public string? ListClientOrderId { get; set; }

    /// <summary>Order-list transaction time.</summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? TransactionTime { get; set; }

    /// <summary>Orders belonging to the canceled list.</summary>
    public List<BinanceOrderId> Orders { get; set; } = [];

    /// <summary>Detailed canceled-order reports belonging to the list.</summary>
    public List<BinancePlacedOcoOrder> OrderReports { get; set; } = [];
}
