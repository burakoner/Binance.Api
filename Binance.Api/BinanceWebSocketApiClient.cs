using Binance.Api.Clients.StreamApi;
using Binance.Api.Clients.StreamApi.General;

namespace Binance.Api;

public class BinanceWebSocketApiClient
{
    // Logger
    internal ILogger Logger { get; }

    // Options
    public BinanceWebSocketApiClientOptions ClientOptions { get; }

    // Master Clients
    public BinanceStreamSpotClient Spot { get; }
    private BinanceStreamGeneralClient General { get; }
    public BinanceStreamCoinFuturesClient CoinFutures { get; }
    public BinanceStreamUsdtFuturesClient UsdtFutures { get; }

    // Other Clients
    public BinanceStreamBlvtClient BLVT { get => General.BLVT; }

    public BinanceWebSocketApiClient() : this(null, new BinanceWebSocketApiClientOptions())
    {
    }

    public BinanceWebSocketApiClient(BinanceWebSocketApiClientOptions options) : this(null, options)
    {
    }

    public BinanceWebSocketApiClient(ILogger logger, BinanceWebSocketApiClientOptions options)
    {
        Logger = logger;
        ClientOptions = options;

        Spot = new BinanceStreamSpotClient(this);
        General = new BinanceStreamGeneralClient(this);
        CoinFutures = new BinanceStreamCoinFuturesClient(this);
        UsdtFutures = new BinanceStreamUsdtFuturesClient(this);
    }
}
