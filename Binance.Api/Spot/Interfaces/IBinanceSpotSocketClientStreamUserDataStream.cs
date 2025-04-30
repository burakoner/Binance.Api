namespace Binance.Api.Spot;

public interface IBinanceSpotSocketClientStreamUserDataStream
{
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToUserDataStreamAsync(
        string listenKey,
        Action<WebSocketDataEvent<BinanceSpotStreamOrderUpdate>>? onOrderUpdateMessage = null,
        Action<WebSocketDataEvent<BinanceSpotStreamOrderListUpdate>>? onOcoOrderUpdateMessage = null,
        Action<WebSocketDataEvent<BinanceSpotStreamPositionsUpdate>>? onAccountPositionMessage = null,
        Action<WebSocketDataEvent<BinanceSpotStreamBalanceUpdate>>? onAccountBalanceUpdate = null,
        Action<WebSocketDataEvent<BinanceSpotStreamBalanceUpdate>>? onBalanceLockUpdate = null,
        Action<WebSocketDataEvent<BinanceSpotStreamEvent>>? onUserDataStreamTerminated = null,
        Action<WebSocketDataEvent<BinanceSpotStreamEvent>>? onListenKeyExpired = null,
        CancellationToken ct = default);
}