namespace Binance.ApiClient;

public class BinanceStreamClientOptions : StreamApiClientOptions
{
    public BinanceStreamClientOptions()
    {
        this.BaseAddress = "wss://stream.binance.com:9443/";
    }
}
