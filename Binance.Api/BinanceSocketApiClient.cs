using Binance.Api.Spot;

namespace Binance.Api;

/// <summary>
/// Binance WebSocket API Client
/// </summary>
public class BinanceSocketApiClient
{
    // Internal
    internal ILogger Logger { get; }
    internal TimeSyncState TimeSyncState { get; } = new("Binance");
    internal BinanceSocketApiClientOptions SocketOptions { get; }

    /// <summary>
    /// Spot WebSocket API Client
    /// </summary>
    public IBinanceSpotSocketClient Spot { get; }

    //public BinanceStreamCoinFuturesClient CoinFutures { get; }
    //public BinanceStreamUsdtFuturesClient UsdtFutures { get; }
    //private BinanceStreamGeneralClient General { get; }

    /// <summary>
    /// Binance WebSocket API Client Constructor
    /// </summary>
    public BinanceSocketApiClient() : this(null, new BinanceSocketApiClientOptions())
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
        Logger = logger ?? BaseClient.LoggerFactory.CreateLogger(typeof(BinanceSocketApiClient));
        SocketOptions = options;

        Spot = new BinanceSpotSocketClient(this);
        //General = new BinanceStreamGeneralClient(this);
        //CoinFutures = new BinanceStreamCoinFuturesClient(this);
        //UsdtFutures = new BinanceStreamUsdtFuturesClient(this);
    }
}
