namespace Binance.Api;

public class BinanceStreamClientOptions : StreamApiClientOptions
{
    // BLVT
    public string BlvtStreamAddress { get; set; }

    public BinanceStreamClientOptions()
    {
        this.BaseAddress = "wss://stream.binance.com:9443/";
        this.BlvtStreamAddress = BinanceApiAddresses.Default.BlvtSocketClientAddress;
    }
}
