namespace Binance.Api.Spot;

/// <summary>
/// Notification that a Spot WebSocket server is about to shut down.
/// </summary>
public record BinanceSpotServerShutdown : BinanceSocketStreamEvent;
