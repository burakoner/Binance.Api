﻿namespace Binance.Api.Clients.RestApi;

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

    internal BinanceRestApiGeneralClient(BinanceRestApiClient root) : base("Binance RestApi", root.Options)
    {
        RootClient = root;

        RequestBodyEmptyContent = "";
        RequestBodyFormat = RestRequestBodyFormat.FormData;
        ArraySerialization = ArraySerialization.MultipleValues;

        this.FuturesAlgo = new BinanceRestApiFuturesAlgoClient(this);
        this.FuturesLoan = new BinanceRestApiFuturesLoanClient(this);
        this.FuturesTransfer = new BinanceRestApiFuturesTransferClient(this);

        this.SubAccount = new BinanceRestApiSubAccountClient(this);
        this.Brokerage = new BinanceRestApiBrokerageClient(this);
        this.Savings = new BinanceRestApiSavingsClient(this);
        this.Staking = new BinanceRestApiStakingClient(this);
        this.Mining = new BinanceRestApiMiningClient(this);
        this.BLVT = new BinanceRestApiBlvtClient(this);
        this.BSwap = new BinanceRestApiBSwapClient(this);
        this.Fiat = new BinanceRestApiFiatClient(this);
        this.C2C = new BinanceRestApiC2cClient(this);

        this.CryptoLoans = new BinanceRestApiCryptoLoansClient(this);
        this.GiftCards = new BinanceRestApiGiftCardClient(this);
        this.VipLoans = new BinanceRestApiVipLoansClient(this);
        this.Convert = new BinanceRestApiConvertClient(this);
        this.Rebate = new BinanceRestApiRebateClient(this);
        this.NFT = new BinanceRestApiNftClient(this);
        this.Pay = new BinanceRestApiPayClient(this);
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
        RestParameterPosition? postPosition = null,
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
