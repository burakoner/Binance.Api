namespace Binance.Api.Futures;

/// <summary>
/// Interface for the Binance Futures USD Socket API client.
/// </summary>
public interface IBinanceFuturesSocketClientUsd:
    IBinanceFuturesSocketClientUsdQueryAccount,
    IBinanceFuturesSocketClientUsdQueryAuthentication,
    IBinanceFuturesSocketClientUsdQueryGeneral,
    IBinanceFuturesSocketClientUsdQueryMarketData,
    IBinanceFuturesSocketClientUsdQueryTrade,
    IBinanceFuturesSocketClientUsdQueryUserDataStream,
    IBinanceFuturesSocketClientUsdStreamMarketData,
    IBinanceFuturesSocketClientUsdStreamUserData;
