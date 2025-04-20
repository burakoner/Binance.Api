namespace Binance.Api;

public sealed class BinanceRestApiClient
{
    // Logger
    internal ILogger Logger { get; }

    // Options
    public BinanceRestApiClientOptions ClientOptions { get; }

    // Master Clients
    public BinanceRestApiSpotClient Spot { get; }
    public BinanceRestApiMarginClient Margin { get; }
    private BinanceRestApiGeneralClient General { get; }
    public BinanceRestApiCoinFuturesClient CoinFutures { get; }
    public BinanceRestApiUsdtFuturesClient UsdtFutures { get; }

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

    public BinanceRestApiClient() : this(null, new BinanceRestApiClientOptions())
    {
    }

    public BinanceRestApiClient(BinanceRestApiClientOptions options) : this(null, options)
    {
    }

    public BinanceRestApiClient(ILogger? logger, BinanceRestApiClientOptions options)
    {
        Logger = logger ?? LoggerFactory.Create(c => { }).CreateLogger(typeof(BinanceRestApiClient));
        ClientOptions = options;

        Spot = new BinanceRestApiSpotClient(this);
        Margin = new BinanceRestApiMarginClient(this);
        General = new BinanceRestApiGeneralClient(this);
        CoinFutures = new BinanceRestApiCoinFuturesClient(this);
        UsdtFutures = new BinanceRestApiUsdtFuturesClient(this);
    }

}
