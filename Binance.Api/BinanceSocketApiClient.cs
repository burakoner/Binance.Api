﻿using Binance.Api.Futures;
using Binance.Api.Options;
using Binance.Api.Spot;

namespace Binance.Api;

/// <summary>
/// Binance WebSocket API Client
/// </summary>
public class BinanceSocketApiClient
{
    // Internal
    internal ILogger Logger { get; }
    internal IBinanceFuturesSocketClient Futures { get; }
    internal TimeSyncState TimeSyncState { get; } = new("Binance");
    internal BinanceSocketApiClientOptions ApiOptions { get; }
    internal BinanceRestApiClient RestApiClient { get; }

    /// <summary>
    /// Binance Spot WebSocket API Client
    /// </summary>
    public IBinanceSpotSocketClient Spot { get; }

    /// <summary>
    /// Binance USDⓈ Futures Socket API Client
    /// </summary>
    public IBinanceFuturesSocketClientUsd UsdFutures { get => Futures.USD; }

    /// <summary>
    /// Binance Coin Futures Rest API Client
    /// </summary>
    public IBinanceFuturesSocketClientCoin CoinFutures { get => Futures.Coin; }

    /// <summary>
    /// Binance European Options WebSocket API Client
    /// </summary>
    public IBinanceOptionsSocketClient Options { get; }

    /// <summary>
    /// Binance WebSocket API Client Constructor
    /// </summary>
    public BinanceSocketApiClient() : this(null, new())
    {
    }

    /// <summary>
    /// Constructor with logger
    /// </summary>
    /// <param name="logger">Logger</param>
    public BinanceSocketApiClient(ILogger logger) : this(logger, new())
    {
    }

    /// <summary>
    /// Binance WebSocket API Client Constructor
    /// </summary>
    /// <param name="options"></param>
    public BinanceSocketApiClient(BinanceSocketApiClientOptions options) : this(null, options)
    {
    }

    /// <summary>
    /// Binance WebSocket API Client Constructor
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="options">Web Socket API Options</param>
    public BinanceSocketApiClient(ILogger? logger, BinanceSocketApiClientOptions options)
    {
        ApiOptions = options;
        Logger = logger ?? BaseClient.LoggerFactory.CreateLogger<BinanceSocketApiClient>();
        RestApiClient = new(Logger, new());

        Spot = new BinanceSpotSocketClient(this);
        Futures = new BinanceFuturesSocketClient(this);
        Options = new BinanceOptionsSocketClient(this);
    }

    internal int? ReceiveWindow(int? receiveWindow) => receiveWindow ?? (ApiOptions.ReceiveWindow != null ? System.Convert.ToInt32(ApiOptions.ReceiveWindow?.TotalMilliseconds) : null);

    /// <summary>
    /// Sets API Credentials
    /// </summary>
    /// <param name="apikey"></param>
    /// <param name="secret"></param>
    public void SetApiCredentials(string apikey, string secret)
    {
        ((BinanceSpotSocketClient)Spot).SetApiCredentials(new ApiCredentials(apikey, secret));
        ((BinanceFuturesSocketClientUsd)Futures.USD).SetApiCredentials(new ApiCredentials(apikey, secret));
        ((BinanceFuturesSocketClientCoin)Futures.Coin).SetApiCredentials(new ApiCredentials(apikey, secret));
    }

}
