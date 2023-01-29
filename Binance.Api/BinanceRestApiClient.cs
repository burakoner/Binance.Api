namespace Binance.ApiClient;

public sealed class BinanceRestApiClient
{
    // Options
    public BinanceRestApiClientOptions Options { get; }

    // Master Clients
    public BinanceRestApiSpotClient Spot { get; }
    public BinanceRestApiMarginClient Margin { get; }
    private BinanceRestApiGeneralClient GeneralApi { get; }
    public BinanceRestApiCoinFuturesClient CoinFutures { get; }
    public BinanceRestApiUsdtFuturesClient UsdtFutures { get; }

    // General Futures
    internal BinanceRestApiFuturesAlgoClient FuturesAlgo { get => GeneralApi.FuturesAlgo; }
    internal BinanceRestApiFuturesLoanClient FuturesLoan { get => GeneralApi.FuturesLoan; }
    internal BinanceRestApiFuturesTransferClient FuturesTransfer { get => GeneralApi.FuturesTransfer; }

    // Other Clients
    public BinanceRestApiSubAccountClient SubAccount { get => GeneralApi.SubAccount; }
    public BinanceRestApiBrokerageClient Brokerage { get => GeneralApi.Brokerage; }
    public BinanceRestApiSavingsClient Savings { get => GeneralApi.Savings; }
    public BinanceRestApiStakingClient Staking { get => GeneralApi.Staking; }
    public BinanceRestApiMiningClient Mining { get => GeneralApi.Mining; }
    public BinanceRestApiBlvtClient BLVT { get => GeneralApi.BLVT; }
    public BinanceRestApiBSwapClient BSwap { get => GeneralApi.BSwap; }
    public BinanceRestApiFiatClient Fiat { get => GeneralApi.Fiat; }
    public BinanceRestApiC2cClient C2C { get => GeneralApi.C2C; }

    // More Clients
    public BinanceRestApiCryptoLoansClient CryptoLoans { get => GeneralApi.CryptoLoans; }
    public BinanceRestApiGiftCardClient GiftCards { get => GeneralApi.GiftCards; }
    public BinanceRestApiVipLoansClient VipLoans { get => GeneralApi.VipLoans; }
    public BinanceRestApiConvertClient Convert { get => GeneralApi.Convert; }
    public BinanceRestApiRebateClient Rebate { get => GeneralApi.Rebate; }
    public BinanceRestApiNftClient NFT { get => GeneralApi.NFT; }
    public BinanceRestApiPayClient Pay { get => GeneralApi.Pay; }

    public BinanceRestApiClient() : this(new BinanceRestApiClientOptions())
    {
    }

    public BinanceRestApiClient(BinanceRestApiClientOptions options)
    {
        this.Options = options;

        this.Spot = new BinanceRestApiSpotClient(this, options);
        this.Margin = new BinanceRestApiMarginClient(this, options);
        this.GeneralApi = new BinanceRestApiGeneralClient(this, options);
        this.CoinFutures = new BinanceRestApiCoinFuturesClient(this, options);
        this.UsdtFutures = new BinanceRestApiUsdtFuturesClient(this, options);

        // this.FuturesAccount = new BinanceRestApiFuturesAccountClient(this, options);
        // this.FuturesAlgo = new BinanceRestApiFuturesAlgoClient(this, options);
    }
}