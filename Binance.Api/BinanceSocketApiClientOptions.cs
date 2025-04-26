namespace Binance.Api;

public class BinanceSocketApiClientOptions : WebSocketApiClientOptions
{
    // BLVT
    public string BlvtStreamAddress { get; set; }

    public BinanceSocketApiClientOptions()
    {
        this.BaseAddress = "wss://stream.binance.com:9443/";
        this.BlvtStreamAddress = BinanceAddress.Default.BlvtSocketClientAddress;
    }
}
