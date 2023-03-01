namespace Binance.Api;

public class BinanceStreamClient
{
    // Options
    public BinanceStreamClientOptions ClientOptions { get; }

    // Master Clients
    public BinanceStreamSpotClient Spot { get; }
    private BinanceStreamGeneralClient General { get; }
    public BinanceStreamCoinFuturesClient CoinFutures { get; }
    public BinanceStreamUsdtFuturesClient UsdtFutures { get; }

    // Other Clients
    public BinanceStreamBlvtClient BLVT { get => General.BLVT; }

    public BinanceStreamClient() : this(new BinanceStreamClientOptions())
    {
    }

    public BinanceStreamClient(BinanceStreamClientOptions options)
    {
        ClientOptions = options;

        Spot = new BinanceStreamSpotClient(this);
        General = new BinanceStreamGeneralClient(this);
        CoinFutures = new BinanceStreamCoinFuturesClient(this);
        UsdtFutures = new BinanceStreamUsdtFuturesClient(this);
    }
}
