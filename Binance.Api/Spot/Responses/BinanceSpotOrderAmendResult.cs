namespace Binance.Api.Spot;

/// <summary>
/// Result of reducing an order's quantity while keeping its priority.
/// </summary>
public record BinanceSpotOrderAmendResult
{
    /// <summary>Transaction time.</summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime TransactionTime { get; set; }

    /// <summary>Execution identifier for the amendment.</summary>
    public long ExecutionId { get; set; }

    /// <summary>The amended order.</summary>
    public BinanceSpotAmendedOrder AmendedOrder { get; set; } = new();

    /// <summary>Updated list status when the order belongs to an order list.</summary>
    public BinanceSpotAmendedOrderListStatus? ListStatus { get; set; }
}

/// <summary>
/// Order state returned by an amend-keep-priority request.
/// </summary>
public record BinanceSpotAmendedOrder
{
    /// <summary>Symbol.</summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>Order identifier.</summary>
    [JsonProperty("orderId")]
    public long Id { get; set; }

    /// <summary>Order-list identifier, or -1 when the order is not in a list.</summary>
    public long OrderListId { get; set; }

    /// <summary>Client order identifier before the amendment.</summary>
    [JsonProperty("origClientOrderId")]
    public string OriginalClientOrderId { get; set; } = string.Empty;

    /// <summary>Client order identifier after the amendment.</summary>
    public string ClientOrderId { get; set; } = string.Empty;

    /// <summary>Order price.</summary>
    public decimal Price { get; set; }

    /// <summary>Quantity after the amendment.</summary>
    [JsonProperty("qty")]
    public decimal Quantity { get; set; }

    /// <summary>Executed quantity.</summary>
    [JsonProperty("executedQty")]
    public decimal QuantityFilled { get; set; }

    /// <summary>Quantity prevented from executing.</summary>
    [JsonProperty("preventedQty")]
    public decimal PreventedQuantity { get; set; }

    /// <summary>Original quote order quantity.</summary>
    [JsonProperty("quoteOrderQty")]
    public decimal QuoteQuantity { get; set; }

    /// <summary>Executed quote quantity.</summary>
    [JsonProperty("cumulativeQuoteQty")]
    public decimal QuoteQuantityFilled { get; set; }

    /// <summary>Order status.</summary>
    public BinanceOrderStatus Status { get; set; }

    /// <summary>Time-in-force instruction.</summary>
    public BinanceTimeInForce TimeInForce { get; set; }

    /// <summary>Order type.</summary>
    public BinanceSpotOrderType Type { get; set; }

    /// <summary>Order side.</summary>
    public BinanceOrderSide Side { get; set; }

    /// <summary>Time at which the order started working.</summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? WorkingTime { get; set; }

    /// <summary>Self-trade-prevention mode.</summary>
    public BinanceSelfTradePreventionMode SelfTradePreventionMode { get; set; }

    /// <summary>Iceberg quantity.</summary>
    [JsonProperty("icebergQty")]
    public decimal? IcebergQuantity { get; set; }

    /// <summary>Prevented match identifier.</summary>
    public long? PreventedMatchId { get; set; }

    /// <summary>Quantity prevented by self-trade prevention after the amendment.</summary>
    [JsonProperty("preventedQuantity")]
    public decimal? PreventedQuantityAfterAmend { get; set; }

    /// <summary>Stop price.</summary>
    public decimal? StopPrice { get; set; }

    /// <summary>Strategy identifier.</summary>
    public long? StrategyId { get; set; }

    /// <summary>Strategy type.</summary>
    public long? StrategyType { get; set; }

    /// <summary>Trailing delta in basis points.</summary>
    public long? TrailingDelta { get; set; }

    /// <summary>Time at which a trailing order became active.</summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? TrailingTime { get; set; }

    /// <summary>Whether Smart Order Routing was used.</summary>
    [JsonProperty("usedSor")]
    public bool? UsedSmartOrderRouting { get; set; }

    /// <summary>Floor on which the order is working.</summary>
    public string? WorkingFloor { get; set; }

    /// <summary>Pegged-price type.</summary>
    public string? PegPriceType { get; set; }

    /// <summary>Pegged-price offset type.</summary>
    public string? PegOffsetType { get; set; }

    /// <summary>Pegged-price offset value.</summary>
    public int? PegOffsetValue { get; set; }

    /// <summary>Current pegged price.</summary>
    public decimal? PeggedPrice { get; set; }

    /// <summary>Order expiry reason.</summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceSpotOrderExpiryReason? ExpiryReason { get; set; }
}

/// <summary>
/// Order-list state returned after amending a member order.
/// </summary>
public record BinanceSpotAmendedOrderListStatus
{
    /// <summary>Order-list identifier.</summary>
    public long OrderListId { get; set; }

    /// <summary>Order-list contingency type.</summary>
    public string ContingencyType { get; set; } = string.Empty;

    /// <summary>Aggregate order-list status.</summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceListOrderStatus ListOrderStatus { get; set; }

    /// <summary>Client order-list identifier.</summary>
    public string ListClientOrderId { get; set; } = string.Empty;

    /// <summary>Symbol.</summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>Orders in the list.</summary>
    public List<BinanceOrderId> Orders { get; set; } = [];
}
