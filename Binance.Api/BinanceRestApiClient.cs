namespace Binance.Api;

/// <summary>
/// Binance Rest API Client
/// </summary>
public sealed class BinanceRestApiClient
{
    // Internal
    internal ILogger Logger { get; }
    internal BinanceRestApiClientOptions ClientOptions { get; }

    /// <summary>
    /// Binance Spot Rest API Client
    /// </summary>
    public BinanceSpotRestApi Spot { get; }

    /// <summary>
    /// Binance Margin Rest API Client
    /// </summary>
    public BinanceRestApiMarginClient Margin { get; }

    /// <summary>
    /// Binance Coin Futures Rest API Client
    /// </summary>
    public BinanceRestApiCoinFuturesClient CoinFutures { get; }

    /// <summary>
    /// Binance USDT Futures Rest API Client
    /// </summary>
    public BinanceRestApiUsdtFuturesClient UsdtFutures { get; }

    // General
    private BinanceRestApiGeneralClient General { get; }

    // General Futures
    internal BinanceRestApiFuturesAlgoClient FuturesAlgo { get => General.FuturesAlgo; }
    internal BinanceRestApiFuturesLoanClient FuturesLoan { get => General.FuturesLoan; }
    internal BinanceRestApiFuturesTransferClient FuturesTransfer { get => General.FuturesTransfer; }

    // Other Clients
    public BinanceRestApiSubAccountClient SubAccount { get => General.SubAccount; }
    public BinanceRestApiBrokerageClient Brokerage { get => General.Brokerage; }
    public BinanceRestApiSavingsClient Savings { get => General.Savings; }
    public BinanceRestApiStakingClient Staking { get => General.Staking; }
    public BinanceRestApiMiningClient Mining { get => General.Mining; }
    public BinanceRestApiBlvtClient BLVT { get => General.BLVT; }
    public BinanceRestApiBSwapClient BSwap { get => General.BSwap; }
    public BinanceRestApiFiatClient Fiat { get => General.Fiat; }
    public BinanceRestApiC2cClient C2C { get => General.C2C; }

    // More Clients
    public BinanceRestApiCryptoLoansClient CryptoLoans { get => General.CryptoLoans; }
    public BinanceRestApiGiftCardClient GiftCards { get => General.GiftCards; }
    public BinanceRestApiVipLoansClient VipLoans { get => General.VipLoans; }
    public BinanceRestApiConvertClient Convert { get => General.Convert; }
    public BinanceRestApiRebateClient Rebate { get => General.Rebate; }
    public BinanceRestApiNftClient NFT { get => General.NFT; }
    public BinanceRestApiPayClient Pay { get => General.Pay; }

    /// <summary>
    /// Default Constructor
    /// </summary>
    public BinanceRestApiClient() : this(null, new BinanceRestApiClientOptions())
    {
    }

    /// <summary>
    /// Constructor with logger
    /// </summary>
    /// <param name="logger">Logger</param>
    public BinanceRestApiClient(ILogger logger) : this(logger, new BinanceRestApiClientOptions())
    {
    }

    /// <summary>
    /// Constructor with options
    /// </summary>
    /// <param name="options">Binance Rest API Client Options</param>
    public BinanceRestApiClient(BinanceRestApiClientOptions options) : this(null, options)
    {
    }

    /// <summary>
    /// Constructor with logger and options
    /// </summary>
    /// <param name="logger">Logger</param>
    /// <param name="options">Binance Rest API Client Options</param>
    public BinanceRestApiClient(ILogger? logger, BinanceRestApiClientOptions options)
    {
        Logger = logger ?? LoggerFactory.Create(c => { }).CreateLogger(typeof(BinanceRestApiClient));
        ClientOptions = options;

        Spot = new BinanceSpotRestApi(this);
        Margin = new BinanceRestApiMarginClient(this);
        General = new BinanceRestApiGeneralClient(this);
        CoinFutures = new BinanceRestApiCoinFuturesClient(this);
        UsdtFutures = new BinanceRestApiUsdtFuturesClient(this);
    }

}
