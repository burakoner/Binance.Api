﻿namespace Binance.Api.Futures;

/// <summary>
/// Interface for the Binance Futures Coin Socket API client.
/// </summary>
public interface IBinanceFuturesSocketClientCoin:
    IBinanceFuturesSocketClientCoinQueryAccount,
    IBinanceFuturesSocketClientCoinQueryGeneral,
    IBinanceFuturesSocketClientCoinQueryMarketData,
    IBinanceFuturesSocketClientCoinQueryTrade,
    IBinanceFuturesSocketClientCoinStreamMarketData,
    IBinanceFuturesSocketClientCoinStreamUserData;