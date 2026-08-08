using Binance.Api.Spot;

namespace Binance.Api.Margin;

/// <summary>
/// Common parameters for a Margin OTO or OTOCO order list.
/// </summary>
public abstract record BinanceMarginOrderListRequest
{
    /// <summary>Creates the common working-order state.</summary>
    protected BinanceMarginOrderListRequest(
        string symbol,
        BinanceSpotOrderType workingType,
        BinanceOrderSide workingSide,
        decimal workingPrice,
        decimal workingQuantity)
    {
        Symbol = symbol;
        WorkingType = workingType;
        WorkingSide = workingSide;
        WorkingPrice = workingPrice;
        WorkingQuantity = workingQuantity;
    }

    /// <summary>The symbol the order list is for.</summary>
    public string Symbol { get; set; }

    /// <summary>The working-order type. Binance accepts LIMIT and LIMIT_MAKER.</summary>
    public BinanceSpotOrderType WorkingType { get; set; }

    /// <summary>The working-order side.</summary>
    public BinanceOrderSide WorkingSide { get; set; }

    /// <summary>The working-order price.</summary>
    public decimal WorkingPrice { get; set; }

    /// <summary>The working-order quantity.</summary>
    public decimal WorkingQuantity { get; set; }

    /// <summary>Whether the order list is for isolated margin.</summary>
    public bool? IsIsolated { get; set; }

    /// <summary>Side effect applied by the order list. OTO and OTOCO accept only NO_SIDE_EFFECT and MARGIN_BUY.</summary>
    public BinanceMarginSideEffectType? SideEffectType { get; set; }

    /// <summary>Whether a debt created by MARGIN_BUY should be repaid when the order is canceled.</summary>
    public bool? AutoRepayAtCancel { get; set; }

    /// <summary>Unique identifier for the order list.</summary>
    public string? ListClientOrderId { get; set; }

    /// <summary>Response detail level.</summary>
    public BinanceOrderResponseType? OrderResponseType { get; set; }

    /// <summary>Self-trade-prevention mode configured for the symbol.</summary>
    public BinanceSelfTradePreventionMode? SelfTradePreventionMode { get; set; }

    /// <summary>Unique identifier for the working order.</summary>
    public string? WorkingClientOrderId { get; set; }

    /// <summary>Working-order time in force. Required when <see cref="WorkingType"/> is LIMIT.</summary>
    public BinanceTimeInForce? WorkingTimeInForce { get; set; }
}

/// <summary>Parameters for a Margin one-triggers-the-other order list.</summary>
public record BinanceMarginOtoOrderListRequest : BinanceMarginOrderListRequest
{
    /// <summary>Creates a Margin OTO request from the fields marked required by the current endpoint schema.</summary>
    public BinanceMarginOtoOrderListRequest(
        string symbol,
        BinanceSpotOrderType workingType,
        BinanceOrderSide workingSide,
        decimal workingPrice,
        decimal workingQuantity,
        decimal workingIcebergQuantity,
        BinanceSpotOrderType pendingType,
        BinanceOrderSide pendingSide,
        decimal pendingQuantity)
        : base(symbol, workingType, workingSide, workingPrice, workingQuantity)
    {
        WorkingIcebergQuantity = workingIcebergQuantity;
        PendingType = pendingType;
        PendingSide = pendingSide;
        PendingQuantity = pendingQuantity;
    }

    /// <summary>
    /// Working-order iceberg quantity. The current Margin OTO schema marks this field as required.
    /// </summary>
    public decimal WorkingIcebergQuantity { get; set; }

    /// <summary>The pending-order type.</summary>
    public BinanceSpotOrderType PendingType { get; set; }

    /// <summary>The pending-order side.</summary>
    public BinanceOrderSide PendingSide { get; set; }

    /// <summary>The pending-order quantity.</summary>
    public decimal PendingQuantity { get; set; }

    /// <summary>Unique identifier for the pending order.</summary>
    public string? PendingClientOrderId { get; set; }

    /// <summary>Pending-order price.</summary>
    public decimal? PendingPrice { get; set; }

    /// <summary>Pending-order stop price.</summary>
    public decimal? PendingStopPrice { get; set; }

    /// <summary>Pending-order trailing delta.</summary>
    public decimal? PendingTrailingDelta { get; set; }

    /// <summary>Pending-order iceberg quantity.</summary>
    public decimal? PendingIcebergQuantity { get; set; }

    /// <summary>Pending-order time in force.</summary>
    public BinanceTimeInForce? PendingTimeInForce { get; set; }
}

/// <summary>Parameters for a Margin one-triggers-one-cancels-the-other order list.</summary>
public record BinanceMarginOtocoOrderListRequest : BinanceMarginOrderListRequest
{
    /// <summary>Creates a Margin OTOCO request from the fields marked required by the current endpoint schema.</summary>
    public BinanceMarginOtocoOrderListRequest(
        string symbol,
        BinanceSpotOrderType workingType,
        BinanceOrderSide workingSide,
        decimal workingPrice,
        decimal workingQuantity,
        BinanceOrderSide pendingSide,
        decimal pendingQuantity,
        BinanceSpotOrderType pendingAboveType)
        : base(symbol, workingType, workingSide, workingPrice, workingQuantity)
    {
        PendingSide = pendingSide;
        PendingQuantity = pendingQuantity;
        PendingAboveType = pendingAboveType;
    }

    /// <summary>Working-order iceberg quantity.</summary>
    public decimal? WorkingIcebergQuantity { get; set; }

    /// <summary>The side shared by the two pending orders.</summary>
    public BinanceOrderSide PendingSide { get; set; }

    /// <summary>The quantity shared by the two pending orders.</summary>
    public decimal PendingQuantity { get; set; }

    /// <summary>The required pending-above order type.</summary>
    public BinanceSpotOrderType PendingAboveType { get; set; }

    /// <summary>Unique identifier for the pending-above order.</summary>
    public string? PendingAboveClientOrderId { get; set; }

    /// <summary>Pending-above price.</summary>
    public decimal? PendingAbovePrice { get; set; }

    /// <summary>Pending-above stop price.</summary>
    public decimal? PendingAboveStopPrice { get; set; }

    /// <summary>Pending-above trailing delta.</summary>
    public decimal? PendingAboveTrailingDelta { get; set; }

    /// <summary>Pending-above iceberg quantity.</summary>
    public decimal? PendingAboveIcebergQuantity { get; set; }

    /// <summary>Pending-above time in force.</summary>
    public BinanceTimeInForce? PendingAboveTimeInForce { get; set; }

    /// <summary>Pending-below order type. The current formal schema makes the complete below leg optional.</summary>
    public BinanceSpotOrderType? PendingBelowType { get; set; }

    /// <summary>Unique identifier for the pending-below order.</summary>
    public string? PendingBelowClientOrderId { get; set; }

    /// <summary>Pending-below price.</summary>
    public decimal? PendingBelowPrice { get; set; }

    /// <summary>Pending-below stop price.</summary>
    public decimal? PendingBelowStopPrice { get; set; }

    /// <summary>Pending-below trailing delta.</summary>
    public decimal? PendingBelowTrailingDelta { get; set; }

    /// <summary>Pending-below iceberg quantity.</summary>
    public decimal? PendingBelowIcebergQuantity { get; set; }

    /// <summary>Pending-below time in force.</summary>
    public BinanceTimeInForce? PendingBelowTimeInForce { get; set; }
}
