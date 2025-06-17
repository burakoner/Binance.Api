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
    /// Binance Futures Data Rest API Address
    /// </summary>
    public string FuturesDataRestApiAddress { get; set; } = "";

    #region European Options
    /// <summary>
    /// Binance Options Rest API Address
    /// </summary>
    public string OptionsRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance Options WebSocket API Stream Address
    /// </summary>
    public string OptionsSocketApiStreamAddress { get; set; } = "";
    #endregion

    #region Binance Link
    /// <summary>
    /// Exchange Link Rest API Address
    /// </summary>
    public string ExchangeLinkRestApiAddress { get; set; } = "";

    /// <summary>
    /// Link and Trade (Fast API) Rest API Address
    /// </summary>
    public string LinkAndTradeFastApiRestApiAddress { get; set; } = "";

    /// <summary>
    /// Link and Trade (Futures) Rest API Address
    /// </summary>
    public string LinkAndTradeFuturesRestApiAddress { get; set; } = "";

    /// <summary>
    /// Link and Trade (Spot) Rest API Address
    /// </summary>
    public string LinkAndTradeSpotRestApiAddress { get; set; } = "";
    #endregion

    /// <summary>
    /// Binance Auto Invest Rest API Address
    /// </summary>
    public string AutoInvestRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance Convert Rest API Address
    /// </summary>
    public string ConvertRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance Institutional Loan Rest API Address
    /// </summary>
    public string InstitutionalLoanRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance Rebate Rest API Address
    /// </summary>
    public string RebateRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance Pay History Rest API Address
    /// </summary>
    public string PayHistoryRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance Pay Merchant Rest API Address
    /// </summary>
    public string PayMerchantRestApiAddress { get; set; } = "";

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
    /// Binance Mining Rest API Address
    /// </summary>
    public string MiningRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance NFT Rest API Address
    /// </summary>
    public string NFTRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance Staking Rest API Address
    /// </summary>
    public string StakingRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance Dual Investment Rest API Address
    /// </summary>
    public string DualInvestmentRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance Simple Earn Rest API Address
    /// </summary>
    public string SimpleEarnRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance Algo Trading Rest API Address
    /// </summary>
    public string AlgoTradingRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance Copy Trading Rest API Address
    /// </summary>
    public string CopyTradingRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance Crypto Loan Rest API Address
    /// </summary>
    public string CryptoLoanRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance VIP Loan Rest API Address
    /// </summary>
    public string VipLoanRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance C2C Rest API Address
    /// </summary>
    public string C2CRestApiAddress { get; set; } = "";

    /// <summary>
    /// Binance Fiat Rest API Address
    /// </summary>
    public string FiatRestApiAddress { get; set; } = "";

    /// <summary>
    /// The default addresses to connect to the binance.com API
    /// </summary>
    public static BinanceAddress Default { get; } = new BinanceAddress
    {
        SpotRestApiAddress = "https://api.binance.com",
        SpotSocketApiQueryAddress = "wss://ws-api.binance.com:443",
        SpotSocketApiStreamAddress = "wss://stream.binance.com:9443",

        CoinFuturesRestApiAddress = "https://dapi.binance.com",
        CoinFuturesSocketApiQueryAddress = "wss://ws-dapi.binance.com",
        CoinFuturesSocketApiStreamAddress = "wss://dstream.binance.com",

        UsdFuturesRestApiAddress = "https://fapi.binance.com",
        UsdFuturesSocketApiQueryAddress = "wss://ws-fapi.binance.com",
        UsdFuturesSocketApiStreamAddress = "wss://fstream.binance.com",

        FuturesDataRestApiAddress = "https://api.binance.com",

        OptionsRestApiAddress = "https://eapi.binance.com",
        OptionsSocketApiStreamAddress = "wss://nbstream.binance.com/eoptions",

        ExchangeLinkRestApiAddress = "https://api.binance.com",
        LinkAndTradeFastApiRestApiAddress = "https://api.binance.com",
        LinkAndTradeFuturesRestApiAddress = "https://fapi.binance.com",
        LinkAndTradeSpotRestApiAddress = "https://api.binance.com",
        AutoInvestRestApiAddress = "https://api.binance.com",
        ConvertRestApiAddress = "https://api.binance.com",
        InstitutionalLoanRestApiAddress = "https://api.binance.com",
        MarginRestApiAddress = "https://api.binance.com",
        WalletRestApiAddress = "https://api.binance.com",
        SubAccountRestApiAddress = "https://api.binance.com",
        MiningRestApiAddress = "https://api.binance.com",
        NFTRestApiAddress = "https://api.binance.com",
        StakingRestApiAddress = "https://api.binance.com",
        DualInvestmentRestApiAddress = "https://api.binance.com",
        SimpleEarnRestApiAddress = "https://api.binance.com",
        RebateRestApiAddress = "https://api.binance.com",
        PayHistoryRestApiAddress = "https://api.binance.com",
        PayMerchantRestApiAddress = "https://api.binance.com",
        AlgoTradingRestApiAddress = "https://api.binance.com",
        CopyTradingRestApiAddress = "https://api.binance.com",
        CryptoLoanRestApiAddress = "https://api.binance.com",
        VipLoanRestApiAddress = "https://api.binance.com",
        C2CRestApiAddress = "https://api.binance.com",
        FiatRestApiAddress = "https://api.binance.com",
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

        OptionsRestApiAddress = "",
        OptionsSocketApiStreamAddress = "",

        ExchangeLinkRestApiAddress = "https://testnet.binance.vision",
        LinkAndTradeFastApiRestApiAddress = "https://testnet.binance.vision",
        LinkAndTradeFuturesRestApiAddress = "https://testnet.binance.vision",
        LinkAndTradeSpotRestApiAddress = "https://testnet.binance.vision",
        AutoInvestRestApiAddress = "https://testnet.binance.vision",
        ConvertRestApiAddress = "https://testnet.binance.vision",
        InstitutionalLoanRestApiAddress = "https://testnet.binance.vision",
        MarginRestApiAddress = "https://testnet.binance.vision",
        WalletRestApiAddress = "https://testnet.binance.vision",
        SubAccountRestApiAddress = "https://testnet.binance.vision",
        MiningRestApiAddress = "https://testnet.binance.vision",
        NFTRestApiAddress = "https://testnet.binance.vision",
        StakingRestApiAddress = "https://testnet.binance.vision",
        DualInvestmentRestApiAddress = "https://testnet.binance.vision",
        SimpleEarnRestApiAddress = "https://testnet.binance.vision",
        RebateRestApiAddress = "https://testnet.binance.vision",
        PayHistoryRestApiAddress = "https://testnet.binance.vision",
        PayMerchantRestApiAddress = "https://testnet.binance.vision",
        AlgoTradingRestApiAddress = "https://testnet.binance.vision",
        CopyTradingRestApiAddress = "https://testnet.binance.vision",
        CryptoLoanRestApiAddress = "https://testnet.binance.vision",
        VipLoanRestApiAddress = "https://testnet.binance.vision",
        C2CRestApiAddress = "https://testnet.binance.vision",
        FiatRestApiAddress = "https://testnet.binance.vision",
    };

    /// <summary>
    /// The addresses to connect to binance.us. (binance.us futures not are not available)
    /// </summary>
    public static BinanceAddress US { get; } = new BinanceAddress
    {
        SpotRestApiAddress = "https://api.binance.us",
    };
}
