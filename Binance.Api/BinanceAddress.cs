namespace Binance.Api;

/// <summary>
/// BinanceAddress
/// </summary>
public class BinanceAddress
{
    /// <summary>
    /// The address used by the BinanceClient for the Spot API
    /// </summary>
    public string SpotRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance WebSocket API Query Address
    /// </summary>
    public string SpotSocketApiQueryAddress { get; set; } = "";

    /// <summary>
    /// Binance WebSocket API Stream Address
    /// </summary>
    public string SpotSocketApiStreamAddress { get; set; } = "";

    /// <summary>
    /// The address used by the BinanceSocketClient for connecting to the BLVT streams
    /// </summary>
    public string BlvtSocketClientAddress { get; set; } = "";

    /// <summary>
    /// The address used by the BinanceClient for the USD futures API
    /// </summary>
    public string UsdFuturesRestClientAddress { get; set; } = "";

    /// <summary>
    /// The address used by the BinanceSocketClient for the USD futures API
    /// </summary>
    public string UsdFuturesSocketClientAddress { get; set; } = "";

    /// <summary>
    /// The address used by the BinanceClient for the COIN futures API
    /// </summary>
    public string CoinFuturesRestClientAddress { get; set; } = "";

    /// <summary>
    /// The address used by the BinanceSocketClient for the Coin futures API
    /// </summary>
    public string CoinFuturesSocketClientAddress { get; set; } = "";

    /// <summary>
    /// The default addresses to connect to the binance.com API
    /// </summary>
    public static BinanceAddress Default = new()
    {
        SpotRestApiAddress = "https://api.binance.com",
        SpotSocketApiQueryAddress = "wss://ws-api.binance.com:443/",
        SpotSocketApiStreamAddress = "wss://stream.binance.com:9443/",

        BlvtSocketClientAddress = "wss://nbstream.binance.com/lvt-p",
        UsdFuturesRestClientAddress = "https://fapi.binance.com",
        UsdFuturesSocketClientAddress = "wss://fstream.binance.com/",
        CoinFuturesRestClientAddress = "https://dapi.binance.com",
        CoinFuturesSocketClientAddress = "wss://dstream.binance.com/",
    };

    /// <summary>
    /// The addresses to connect to the binance testnet
    /// </summary>
    public static BinanceAddress TestNet = new()
    {
        SpotRestApiAddress = "https://testnet.binance.vision",
        SpotSocketApiQueryAddress = "wss://ws-api.testnet.binance.vision/",
        SpotSocketApiStreamAddress = "wss://testnet.binance.vision",

        BlvtSocketClientAddress = "wss://fstream.binancefuture.com",
        UsdFuturesRestClientAddress = "https://testnet.binancefuture.com",
        UsdFuturesSocketClientAddress = "wss://fstream.binancefuture.com",
        CoinFuturesRestClientAddress = "https://testnet.binancefuture.com",
        CoinFuturesSocketClientAddress = "wss://dstream.binancefuture.com",
    };

    /// <summary>
    /// The addresses to connect to binance.us. (binance.us futures not are not available)
    /// </summary>
    public static BinanceAddress Us = new()
    {
        SpotRestApiAddress = "https://api.binance.us",
    };
}
