using Binance.Api.Clients.StreamApi;
using Binance.Api.Clients.StreamApi.General;
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

    // Options
    public BinanceSocketApiClientOptions ClientOptions { get; }

    // Master Clients
    public BinanceSpotSocketClient Spot { get; }
    private BinanceStreamGeneralClient General { get; }
    public BinanceStreamCoinFuturesClient CoinFutures { get; }
    public BinanceStreamUsdtFuturesClient UsdtFutures { get; }

    // Other Clients
    public BinanceStreamBlvtClient BLVT { get => General.BLVT; }

    public BinanceSocketApiClient() : this(null, new BinanceSocketApiClientOptions())
    {
    }

    public BinanceSocketApiClient(BinanceSocketApiClientOptions options) : this(null, options)
    {
    }

    public BinanceSocketApiClient(ILogger logger, BinanceSocketApiClientOptions options)
    {
        Logger = logger;
        ClientOptions = options;

        Spot = new BinanceSpotSocketClient(this);
        General = new BinanceStreamGeneralClient(this);
        CoinFutures = new BinanceStreamCoinFuturesClient(this);
        UsdtFutures = new BinanceStreamUsdtFuturesClient(this);
    }
}
