using Binance.Api.Spot;

namespace Binance.Api.Margin;

/// <summary>
/// Margin order-list update.
/// </summary>
public record BinanceMarginStreamOrderListUpdate : BinanceMarginStreamUpdate
{
    /// <summary>Symbol.</summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>Order-list identifier.</summary>
    [JsonProperty("g")]
    public long Id { get; set; }

    /// <summary>Contingency type.</summary>
    [JsonProperty("c")]
    public string ContingencyType { get; set; } = string.Empty;

    /// <summary>List status type.</summary>
    [JsonProperty("l")]
    public BinanceListStatusType ListStatusType { get; set; }

    /// <summary>List order status.</summary>
    [JsonProperty("L")]
    public BinanceListOrderStatus ListOrderStatus { get; set; }

    /// <summary>List rejection reason.</summary>
    [JsonProperty("r")]
    public string? RejectReason { get; set; }

    /// <summary>Client order-list identifier.</summary>
    [JsonProperty("C")]
    public string ClientOrderId { get; set; } = string.Empty;

    /// <summary>Transaction time.</summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime TransactionTime { get; set; }

    /// <summary>Orders in the list.</summary>
    [JsonProperty("O")]
    public List<BinanceMarginStreamOrderId> Orders { get; set; } = [];
}
