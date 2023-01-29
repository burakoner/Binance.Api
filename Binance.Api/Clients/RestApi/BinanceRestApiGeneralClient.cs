using Binance.ApiClient.Models.RestApi;

namespace Binance.ApiClient.Clients.RestApi;

public class BinanceRestApiGeneralClient : RestApiClient
{
    // General Futures
    internal BinanceRestApiFuturesAlgoClient FuturesAlgo { get; }
    internal BinanceRestApiFuturesLoanClient FuturesLoan { get; }
    internal BinanceRestApiFuturesTransferClient FuturesTransfer { get; }

    // Other Clients
    public BinanceRestApiSubAccountClient SubAccount { get; }
    public BinanceRestApiBrokerageClient Brokerage { get; }
    public BinanceRestApiSavingsClient Savings { get; }
    public BinanceRestApiStakingClient Staking { get; }
    public BinanceRestApiMiningClient Mining { get; }
    public BinanceRestApiBlvtClient BLVT { get; }
    public BinanceRestApiBSwapClient BSwap { get; }
    public BinanceRestApiFiatClient Fiat { get; }
    public BinanceRestApiC2cClient C2C { get; }

    // More Clients
    public BinanceRestApiCryptoLoansClient CryptoLoans { get; }
    public BinanceRestApiGiftCardClient GiftCards { get; }
    public BinanceRestApiVipLoansClient VipLoans { get; }
    public BinanceRestApiConvertClient Convert { get; }
    public BinanceRestApiRebateClient Rebate { get; }
    public BinanceRestApiNftClient NFT { get; }
    public BinanceRestApiPayClient Pay { get; }

    // Internal
    internal Log Log { get => this.log; }
    internal TimeSyncState TimeSyncState = new("Binance RestApi");

    // Root Client
    internal BinanceRestApiClient RootClient { get; }

    // Options
    public new BinanceRestApiClientOptions Options { get { return (BinanceRestApiClientOptions)base.Options; } }

    internal BinanceRestApiGeneralClient(BinanceRestApiClient root) : this(root, new BinanceRestApiClientOptions())
    {
    }

    internal BinanceRestApiGeneralClient(BinanceRestApiClient root, BinanceRestApiClientOptions options) : base("Binance RestApi", options)
    {
        RootClient = root;

        RequestBodyEmptyContent = "";
        RequestBodyFormat = RequestBodyFormat.FormData;
        ArraySerialization = ArraySerialization.MultipleValues;

        this.FuturesAlgo = new BinanceRestApiFuturesAlgoClient(root, this);
        this.FuturesLoan = new BinanceRestApiFuturesLoanClient(root, this);
        this.FuturesTransfer = new BinanceRestApiFuturesTransferClient(root, this);

        this.SubAccount = new BinanceRestApiSubAccountClient(root, this);
        this.Brokerage = new BinanceRestApiBrokerageClient(root, this);
        this.Savings = new BinanceRestApiSavingsClient(root, this);
        this.Staking = new BinanceRestApiStakingClient(root, this);
        this.Mining = new BinanceRestApiMiningClient(root, this);
        this.BLVT = new BinanceRestApiBlvtClient(root, this);
        this.BSwap = new BinanceRestApiBSwapClient(root, this);
        this.Fiat = new BinanceRestApiFiatClient(root, this);
        this.C2C = new BinanceRestApiC2cClient(root, this);

        this.CryptoLoans = new BinanceRestApiCryptoLoansClient(root, this);
        this.GiftCards = new BinanceRestApiGiftCardClient(root, this);
        this.VipLoans = new BinanceRestApiVipLoansClient(root, this);
        this.Convert = new BinanceRestApiConvertClient(root, this);
        this.Rebate = new BinanceRestApiRebateClient(root, this);
        this.NFT = new BinanceRestApiNftClient(root, this);
        this.Pay = new BinanceRestApiPayClient(root, this);
    }

    protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
        => new BinanceAuthenticationProvider(credentials);

    protected override CallError ParseErrorResponse(JToken error)
    {
        if (!error.HasValues)
            return new ServerError(error.ToString());

        if (error["msg"] == null && error["code"] == null)
            return new ServerError(error.ToString());

        if (error["msg"] != null && error["code"] == null)
            return new ServerError((string)error["msg"]!);

        return new ServerError((int)error["code"]!, (string)error["msg"]!);
    }

    protected override Task<RestCallResult<DateTime>> GetServerTimestampAsync()
        => RootClient.Spot.Server.GetServerTimeAsync();

    protected override TimeSyncInfo GetTimeSyncInfo()
        => new(log, Options.AutoTimestamp, Options.TimestampRecalculationInterval, TimeSyncState);

    public override TimeSpan GetTimeOffset()
        => TimeSyncState.TimeOffset;

    protected string GetSymbolName(string baseAsset, string quoteAsset) =>
        (baseAsset + quoteAsset).ToUpper(CultureInfo.InvariantCulture);

    internal Uri GetUrl(string endpoint, string api, string version = null)
    {
        var result = Options.BaseAddress.AppendPath(api);

        if (!string.IsNullOrEmpty(version))
            result = result.AppendPath($"v{version}");

        return new Uri(result.AppendPath(endpoint));
    }

    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri,
        HttpMethod method,
        CancellationToken cancellationToken,
        Dictionary<string, object> parameters = null,
        bool signed = false,
        HttpMethodParameterPosition? postPosition = null,
        ArraySerialization? arraySerialization = null,
        int weight = 1,
        bool ignoreRateLimit = false) where T : class
    {
        var result = await SendRequestAsync<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRatelimit: ignoreRateLimit).ConfigureAwait(false);
        if (!result && result.Error!.Code == -1021 && Options.AutoTimestamp)
        {
            log.Write(LogLevel.Debug, "Received Invalid Timestamp error, triggering new time sync");
            TimeSyncState.LastSyncTime = DateTime.MinValue;
        }

        return result;
    }

}
