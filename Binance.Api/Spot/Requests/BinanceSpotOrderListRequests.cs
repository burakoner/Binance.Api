namespace Binance.Api.Spot;

/// <summary>
/// Common parameters for placing a Spot order list.
/// </summary>
public abstract record BinanceSpotOrderListRequest
{
    /// <summary>Creates the common order-list request state.</summary>
    /// <param name="symbol">The symbol the order list is for.</param>
    protected BinanceSpotOrderListRequest(string symbol)
    {
        Symbol = symbol;
    }

    /// <summary>The symbol the order list is for.</summary>
    public string Symbol { get; set; }

    /// <summary>Unique identifier for the order list. Binance generates one when omitted.</summary>
    public string? ListClientOrderId { get; set; }

    /// <summary>Response detail level.</summary>
    public BinanceOrderResponseType? OrderResponseType { get; set; }

    /// <summary>Self-trade-prevention mode configured for the symbol.</summary>
    public BinanceSelfTradePreventionMode? SelfTradePreventionMode { get; set; }

    /// <summary>Request validity window in milliseconds, with up to three decimal places.</summary>
    public decimal? ReceiveWindow { get; set; }
}

/// <summary>
/// Working order used by OPO, OPOCO, OTO, and OTOCO lists.
/// </summary>
public record BinanceSpotOrderListWorkingOrderRequest
{
    /// <summary>Creates a working order.</summary>
    public BinanceSpotOrderListWorkingOrderRequest(BinanceSpotOrderType type, BinanceOrderSide side, decimal price, decimal quantity)
    {
        Type = type;
        Side = side;
        Price = price;
        Quantity = quantity;
    }

    /// <summary>Order type. Binance supports LIMIT and LIMIT_MAKER for working orders.</summary>
    public BinanceSpotOrderType Type { get; set; }

    /// <summary>Order side.</summary>
    public BinanceOrderSide Side { get; set; }

    /// <summary>Order price.</summary>
    public decimal Price { get; set; }

    /// <summary>Order quantity.</summary>
    public decimal Quantity { get; set; }

    /// <summary>Unique identifier for the working order.</summary>
    public string? ClientOrderId { get; set; }

    /// <summary>Iceberg quantity.</summary>
    public decimal? IcebergQuantity { get; set; }

    /// <summary>Time-in-force instruction. Required for LIMIT working orders.</summary>
    public BinanceTimeInForce? TimeInForce { get; set; }

    /// <summary>Strategy identifier.</summary>
    public long? StrategyId { get; set; }

    /// <summary>Strategy type; values below 1000000 are reserved.</summary>
    public long? StrategyType { get; set; }

    /// <summary>Pegged-price reference.</summary>
    public BinanceSpotPegPriceType? PegPriceType { get; set; }

    /// <summary>Pegged-price offset unit.</summary>
    public BinanceSpotPegOffsetType? PegOffsetType { get; set; }

    /// <summary>Pegged-price level offset, up to 100.</summary>
    public int? PegOffsetValue { get; set; }
}

/// <summary>
/// Pending order used by OPO and OTO lists. OPO determines its payable quantity from the working order;
/// OTO supplies the pending quantity at the list level.
/// </summary>
public record BinanceSpotOrderListPendingOrderRequest
{
    /// <summary>Creates a pending order.</summary>
    public BinanceSpotOrderListPendingOrderRequest(BinanceSpotOrderType type, BinanceOrderSide side)
    {
        Type = type;
        Side = side;
    }

    /// <summary>Pending order type.</summary>
    public BinanceSpotOrderType Type { get; set; }

    /// <summary>Pending order side.</summary>
    public BinanceOrderSide Side { get; set; }

    /// <summary>Unique identifier for the pending order.</summary>
    public string? ClientOrderId { get; set; }

    /// <summary>Limit price.</summary>
    public decimal? Price { get; set; }

    /// <summary>Stop price.</summary>
    public decimal? StopPrice { get; set; }

    /// <summary>Trailing delta in BIPS.</summary>
    public decimal? TrailingDelta { get; set; }

    /// <summary>Iceberg quantity.</summary>
    public decimal? IcebergQuantity { get; set; }

    /// <summary>Time-in-force instruction.</summary>
    public BinanceTimeInForce? TimeInForce { get; set; }

    /// <summary>Strategy identifier.</summary>
    public long? StrategyId { get; set; }

    /// <summary>Strategy type; values below 1000000 are reserved.</summary>
    public long? StrategyType { get; set; }

    /// <summary>Pegged-price reference.</summary>
    public BinanceSpotPegPriceType? PegPriceType { get; set; }

    /// <summary>Pegged-price offset unit.</summary>
    public BinanceSpotPegOffsetType? PegOffsetType { get; set; }

    /// <summary>Pegged-price level offset, up to 100.</summary>
    public int? PegOffsetValue { get; set; }
}

/// <summary>
/// One above or below order in an OCO pair.
/// </summary>
public record BinanceSpotOrderListOcoLegRequest
{
    /// <summary>Creates an OCO leg.</summary>
    public BinanceSpotOrderListOcoLegRequest(BinanceSpotOrderType type)
    {
        Type = type;
    }

    /// <summary>Leg order type.</summary>
    public BinanceSpotOrderType Type { get; set; }

    /// <summary>Unique identifier for this order.</summary>
    public string? ClientOrderId { get; set; }

    /// <summary>Iceberg quantity.</summary>
    public decimal? IcebergQuantity { get; set; }

    /// <summary>Limit price.</summary>
    public decimal? Price { get; set; }

    /// <summary>Stop price.</summary>
    public decimal? StopPrice { get; set; }

    /// <summary>Trailing delta in BIPS.</summary>
    public decimal? TrailingDelta { get; set; }

    /// <summary>Time-in-force instruction.</summary>
    public BinanceTimeInForce? TimeInForce { get; set; }

    /// <summary>Strategy identifier.</summary>
    public long? StrategyId { get; set; }

    /// <summary>Strategy type; values below 1000000 are reserved.</summary>
    public long? StrategyType { get; set; }

    /// <summary>Pegged-price reference.</summary>
    public BinanceSpotPegPriceType? PegPriceType { get; set; }

    /// <summary>Pegged-price offset unit.</summary>
    public BinanceSpotPegOffsetType? PegOffsetType { get; set; }

    /// <summary>Pegged-price level offset, up to 100.</summary>
    public int? PegOffsetValue { get; set; }
}

/// <summary>Parameters for a one-cancels-the-other order list.</summary>
public record BinanceSpotOcoOrderListRequest : BinanceSpotOrderListRequest
{
    /// <summary>Creates an OCO order-list request.</summary>
    public BinanceSpotOcoOrderListRequest(string symbol, BinanceOrderSide side, decimal quantity, BinanceSpotOrderListOcoLegRequest aboveOrder, BinanceSpotOrderListOcoLegRequest belowOrder)
        : base(symbol)
    {
        Side = side;
        Quantity = quantity;
        AboveOrder = aboveOrder;
        BelowOrder = belowOrder;
    }

    /// <summary>Side shared by both orders.</summary>
    public BinanceOrderSide Side { get; set; }

    /// <summary>Quantity shared by both orders.</summary>
    public decimal Quantity { get; set; }

    /// <summary>Above order.</summary>
    public BinanceSpotOrderListOcoLegRequest AboveOrder { get; set; }

    /// <summary>Below order.</summary>
    public BinanceSpotOrderListOcoLegRequest BelowOrder { get; set; }
}

/// <summary>Parameters for a one-pays-the-other order list.</summary>
public record BinanceSpotOpoOrderListRequest : BinanceSpotOrderListRequest
{
    /// <summary>Creates an OPO order-list request.</summary>
    public BinanceSpotOpoOrderListRequest(string symbol, BinanceSpotOrderListWorkingOrderRequest workingOrder, BinanceSpotOrderListPendingOrderRequest pendingOrder)
        : base(symbol)
    {
        WorkingOrder = workingOrder;
        PendingOrder = pendingOrder;
    }

    /// <summary>Working order.</summary>
    public BinanceSpotOrderListWorkingOrderRequest WorkingOrder { get; set; }

    /// <summary>Pending order paid from the working order's fills.</summary>
    public BinanceSpotOrderListPendingOrderRequest PendingOrder { get; set; }
}

/// <summary>Parameters for a one-pays-one-cancels-the-other order list.</summary>
public record BinanceSpotOpocoOrderListRequest : BinanceSpotOrderListRequest
{
    /// <summary>Creates an OPOCO order-list request.</summary>
    public BinanceSpotOpocoOrderListRequest(string symbol, BinanceSpotOrderListWorkingOrderRequest workingOrder, BinanceOrderSide pendingSide, BinanceSpotOrderListOcoLegRequest pendingAboveOrder)
        : base(symbol)
    {
        WorkingOrder = workingOrder;
        PendingSide = pendingSide;
        PendingAboveOrder = pendingAboveOrder;
    }

    /// <summary>Working order.</summary>
    public BinanceSpotOrderListWorkingOrderRequest WorkingOrder { get; set; }

    /// <summary>Side shared by the pending OCO pair.</summary>
    public BinanceOrderSide PendingSide { get; set; }

    /// <summary>Pending above order.</summary>
    public BinanceSpotOrderListOcoLegRequest PendingAboveOrder { get; set; }

    /// <summary>Pending below order. Binance treats this field set as optional in the current contract.</summary>
    public BinanceSpotOrderListOcoLegRequest? PendingBelowOrder { get; set; }
}

/// <summary>Parameters for a one-triggers-the-other order list.</summary>
public record BinanceSpotOtoOrderListRequest : BinanceSpotOrderListRequest
{
    /// <summary>Creates an OTO order-list request.</summary>
    public BinanceSpotOtoOrderListRequest(string symbol, BinanceSpotOrderListWorkingOrderRequest workingOrder, BinanceSpotOrderListPendingOrderRequest pendingOrder, decimal pendingQuantity)
        : base(symbol)
    {
        WorkingOrder = workingOrder;
        PendingOrder = pendingOrder;
        PendingQuantity = pendingQuantity;
    }

    /// <summary>Working order.</summary>
    public BinanceSpotOrderListWorkingOrderRequest WorkingOrder { get; set; }

    /// <summary>Pending order.</summary>
    public BinanceSpotOrderListPendingOrderRequest PendingOrder { get; set; }

    /// <summary>Pending order quantity.</summary>
    public decimal PendingQuantity { get; set; }
}

/// <summary>Parameters for a one-triggers-one-cancels-the-other order list.</summary>
public record BinanceSpotOtocoOrderListRequest : BinanceSpotOrderListRequest
{
    /// <summary>Creates an OTOCO order-list request.</summary>
    public BinanceSpotOtocoOrderListRequest(string symbol, BinanceSpotOrderListWorkingOrderRequest workingOrder, BinanceOrderSide pendingSide, decimal pendingQuantity, BinanceSpotOrderListOcoLegRequest pendingAboveOrder)
        : base(symbol)
    {
        WorkingOrder = workingOrder;
        PendingSide = pendingSide;
        PendingQuantity = pendingQuantity;
        PendingAboveOrder = pendingAboveOrder;
    }

    /// <summary>Working order.</summary>
    public BinanceSpotOrderListWorkingOrderRequest WorkingOrder { get; set; }

    /// <summary>Side shared by the pending OCO pair.</summary>
    public BinanceOrderSide PendingSide { get; set; }

    /// <summary>Quantity shared by the pending OCO pair.</summary>
    public decimal PendingQuantity { get; set; }

    /// <summary>Pending above order.</summary>
    public BinanceSpotOrderListOcoLegRequest PendingAboveOrder { get; set; }

    /// <summary>Pending below order. Binance treats this field set as optional in the current contract.</summary>
    public BinanceSpotOrderListOcoLegRequest? PendingBelowOrder { get; set; }
}
