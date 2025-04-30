namespace Binance.Api.Spot;

/// <summary>
/// Interface for the Binance Spot Web Socket API Client
/// </summary>
public interface IBinanceSpotSocketClient :
    IBinanceSpotSocketClientQueryAccount,
    IBinanceSpotSocketClientQueryAuthentication,
    IBinanceSpotSocketClientQueryGeneral,
    IBinanceSpotSocketClientQueryMarketData,
    IBinanceSpotSocketClientQueryTrading,
    IBinanceSpotSocketClientQueryUserDataStream,
    IBinanceSpotSocketClientStreamMarketData,
    IBinanceSpotSocketClientStreamUserDataStream
{
    Task UnsubscribeAsync(WebSocketUpdateSubscription subscription, bool force = false, CancellationToken ct = default);
    Task UnsubscribeAsync(int subscriptionId, CancellationToken ct = default);
    Task UnsubscribeAllAsync(CancellationToken ct = default);
}

