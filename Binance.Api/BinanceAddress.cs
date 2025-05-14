namespace Binance.Api;

/// <summary>
/// BinanceAddress
/// </summary>
public class BinanceAddress
{
    #region Spot
    /// <summary>
    /// Binance Spot Rest API Address
    /// </summary>
    public string SpotRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance Spot WebSocket API Query Address
    /// </summary>
    public string SpotSocketApiQueryAddress { get; set; } = "";

    /// <summary>
    /// Binance Spot WebSocket API Stream Address
    /// </summary>
    public string SpotSocketApiStreamAddress { get; set; } = "";
    #endregion

    #region Coin-M Futures
    /// <summary>
    /// Binance Coin-M Futures Rest API Address
    /// </summary>
    public string CoinFuturesRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance Coin-M Futures WebSocket API Query Address
    /// </summary>
    public string CoinFuturesSocketApiQueryAddress { get; set; } = "";

    /// <summary>
    /// Binance Coin-M Futures WebSocket API Stream Address
    /// </summary>
    public string CoinFuturesSocketApiStreamAddress { get; set; } = "";
    #endregion

    #region USDⓈ-M Futures
    /// <summary>
    /// Binance USDⓈ-M Futures Rest API Address
    /// </summary>
    public string UsdFuturesRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance USDⓈ-M Futures WebSocket API Query Address
    /// </summary>
    public string UsdFuturesSocketApiQueryAddress { get; set; } = "";

    /// <summary>
    /// Binance USDⓈ-M Futures WebSocket API Stream Address
    /// </summary>
    public string UsdFuturesSocketApiStreamAddress { get; set; } = "";
    #endregion

    /// <summary>
    /// Binance Convert Rest API Address
    /// </summary>
    public string ConvertRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance Margin Rest API Address
    /// </summary>
    public string MarginRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance Wallet Rest API Address
    /// </summary>
    public string WalletRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance SubAccount Rest API Address
    /// </summary>
    public string SubAccountRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance Algo Trading Rest API Address
    /// </summary>
    public string AlgoTradingRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance Copy Trading Rest API Address
    /// </summary>
    public string CopyTradingRestApiAddress { get; set; } = "";

    /// <summary>
    /// The default addresses to connect to the binance.com API
    /// </summary>
    public static BinanceAddress Default { get; } = new BinanceAddress
    {
        SpotRestApiAddress = "https://api.binance.com",
        SpotSocketApiQueryAddress = "wss://ws-api.binance.com:443/",
        SpotSocketApiStreamAddress = "wss://stream.binance.com:9443/",

        CoinFuturesRestApiAddress = "https://dapi.binance.com",
        CoinFuturesSocketApiQueryAddress = "wss://ws-dapi.binance.com/",
        CoinFuturesSocketApiStreamAddress = "wss://dstream.binance.com/",

        UsdFuturesRestApiAddress = "https://fapi.binance.com",
        UsdFuturesSocketApiQueryAddress = "wss://ws-fapi.binance.com/",
        UsdFuturesSocketApiStreamAddress = "wss://fstream.binance.com/",

        ConvertRestApiAddress = "https://api.binance.com",
        MarginRestApiAddress = "https://api.binance.com",
        WalletRestApiAddress = "https://api.binance.com",
        SubAccountRestApiAddress = "https://api.binance.com",

        AlgoTradingRestApiAddress = "https://api.binance.com",
        CopyTradingRestApiAddress = "https://api.binance.com",
    };

    /// <summary>
    /// The addresses to connect to the binance testnet
    /// </summary>
    public static BinanceAddress TestNet { get; } = new BinanceAddress
    {
        SpotRestApiAddress = "https://testnet.binance.vision",
        SpotSocketApiQueryAddress = "wss://ws-api.testnet.binance.vision/",
        SpotSocketApiStreamAddress = "wss://testnet.binance.vision",

        CoinFuturesRestApiAddress = "https://testnet.binancefuture.com",
        CoinFuturesSocketApiQueryAddress = "wss://testnet.binancefuture.com/",
        CoinFuturesSocketApiStreamAddress = "wss://dstream.binancefuture.com",

        UsdFuturesRestApiAddress = "https://testnet.binancefuture.com",
        UsdFuturesSocketApiQueryAddress = "wss://testnet.binancefuture.com/",
        UsdFuturesSocketApiStreamAddress = "wss://fstream.binancefuture.com",

        ConvertRestApiAddress = "https://testnet.binance.vision",
        MarginRestApiAddress = "https://testnet.binance.vision",
        WalletRestApiAddress = "https://testnet.binance.vision",
        SubAccountRestApiAddress = "https://testnet.binance.vision",

        AlgoTradingRestApiAddress = "https://testnet.binance.vision",
        CopyTradingRestApiAddress = "https://testnet.binance.vision",
    };

    /// <summary>
    /// The addresses to connect to binance.us. (binance.us futures not are not available)
    /// </summary>
    public static BinanceAddress US { get; } = new BinanceAddress
    {
        SpotRestApiAddress = "https://api.binance.us",
    };
}
