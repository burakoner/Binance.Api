using Binance.Api.Spot;

namespace Binance.Api.Margin;

/// <summary>
/// Interface for the Binance Margin REST API Client Trade Methods
/// </summary>
public interface IBinanceMarginRestApiClientTrade
{
    event Action<long>? OnOrderPlaced;
    event Action<long>? OnOrderCanceled;

    Task<RestCallResult<BinanceQueryRecords<BinanceForcedLiquidation>>> GetMarginForcedLiquidationHistoryAsync(int? page = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, string? isolatedSymbol = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceSmallLiabilityAsset>>> GetCrossMarginSmallLiabilityExchangeAssetsAsync(int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<BinanceQueryRecords<BinanceSmallLiabilityHistory>>> GetCrossMarginSmallLiabilityExchangeHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceOrderBase>>> CancelAllMarginOrdersAsync(string symbol, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<BinanceMarginOrderOcoList>> CancelMarginOcoOrderAsync(string symbol, bool? isIsolated = null, long? orderListId = null, string? listClientOrderId = null, string? newClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<BinanceOrderBase>> CancelMarginOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, string? newClientOrderId = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<BinanceMarginOrderOcoList>> PlaceMarginOCOOrderAsync(string symbol, BinanceOrderSide side, decimal price, decimal stopPrice, decimal quantity, decimal? stopLimitPrice = null, BinanceTimeInForce? stopLimitTimeInForce = null, decimal? stopIcebergQuantity = null, decimal? limitIcebergQuantity = null, BinanceSideEffectType? sideEffectType = null, bool? isIsolated = null, string? listClientOrderId = null, string? limitClientOrderId = null, string? stopClientOrderId = null, BinanceOrderResponseType? orderResponseType = null, BinanceSelfTradePreventionMode? selfTradePreventionMode = null, bool? autoRepayAtCancel = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<BinancePlacedOrder>> PlaceMarginOrderAsync(string symbol, BinanceOrderSide side, BinanceSpotOrderType type, decimal? quantity = null, decimal? quoteQuantity = null, string? newClientOrderId = null, decimal? price = null, BinanceTimeInForce? timeInForce = null, decimal? stopPrice = null, decimal? icebergQuantity = null, BinanceSideEffectType? sideEffectType = null, bool? isIsolated = null, BinanceOrderResponseType? orderResponseType = null, BinanceSelfTradePreventionMode? selfTradePreventionMode = null, bool? autoRepayAtCancel = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceCurrentRateLimit>>> GetMarginOrderRateLimitStatusAsync(int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceMarginOrderOcoList>>> GetMarginOcoOrdersAsync(string? symbol = null, bool? isIsolated = null, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceMarginOrder>>> GetMarginOrdersAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<BinanceMarginOrderOcoList>> GetMarginOcoOrderAsync(string? symbol = null, bool? isIsolated = null, long? orderListId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceMarginOrderOcoList>>> GetMarginOpenOcoOrdersAsync(string? symbol = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceMarginOrder>>> GetOpenMarginOrdersAsync(string? symbol = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<BinanceMarginOrder>> GetMarginOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceMarginTrade>>> GetMarginUserTradesAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<bool>> CrossMarginSmallLiabilityExchangeAsync(IEnumerable<string> assets, int? receiveWindow = null, CancellationToken ct = default);

    // TODO: Margin Manual Liquidation(MARGIN)
    // TODO: Margin Account New OTO(TRADE)
    // TODO: Margin Account New OTOCO (TRADE)
    // TODO: Create Special Key(Low-Latency Trading)(TRADE)
    // TODO: Delete Special Key(Low-Latency Trading)(TRADE)
    // TODO: Edit ip for Special Key(Low-Latency Trading)(TRADE)
    // TODO: Query Special key List(Low Latency Trading)(TRADE)
    // TODO: Query Special key(Low Latency Trading)(TRADE)
}