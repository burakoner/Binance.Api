namespace Binance.Api;

public class BinanceWebSocketApiClientOptions : WebSocketApiClientOptions
{
    // BLVT
    public string BlvtStreamAddress { get; set; }

    public BinanceWebSocketApiClientOptions()
    {
        this.BaseAddress = "wss://stream.binance.com:9443/";
        this.BlvtStreamAddress = BinanceAddress.Default.BlvtSocketClientAddress;
    }
}
