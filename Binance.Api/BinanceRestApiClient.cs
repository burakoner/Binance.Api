﻿using Binance.Api.Algo;
using Binance.Api.AutoInvest;
using Binance.Api.Broker;
using Binance.Api.Convert;
using Binance.Api.CopyTrading;
using Binance.Api.Futures;
using Binance.Api.Margin;
using Binance.Api.Mining;
using Binance.Api.NFT;
using Binance.Api.SimpleEarn;
using Binance.Api.Spot;
using Binance.Api.Staking;
using Binance.Api.SubAccount;
using Binance.Api.Wallet;

namespace Binance.Api;

/// <summary>
/// Binance Rest API Client
/// </summary>
public sealed class BinanceRestApiClient : RestApiClient
{
    // Internal
    internal ILogger Logger => this._logger;
    internal TimeSyncState TimeSyncState { get; } = new("Binance");
    internal BinanceRestApiClientOptions RestOptions => (BinanceRestApiClientOptions)ClientOptions;

    /// <summary>
    /// Binance Spot Rest API Client
    /// </summary>
    public IBinanceSpotRestClient Spot { get; }

    /// <summary>
    /// Binance Futures Rest API Client
    /// </summary>
    internal IBinanceFuturesRestClient Futures { get; }

    /// <summary>
    /// Binance USDⓈ Futures Rest API Client
    /// </summary>
    public IBinanceFuturesRestClientUsd UsdFutures { get => Futures.USD; }

    /// <summary>
    /// Binance Coin Futures Rest API Client
    /// </summary>
    public IBinanceFuturesRestClientCoin CoinFutures { get => Futures.Coin; }

    // TODO: European Options
    // TODO: Portfolio Margin
    // TODO: Portfolio Margin Pro

    /// <summary>
    /// Binance Margin Rest API Client
    /// </summary>
    public IBinanceMarginRestClient Margin { get; }

    /// <summary>
    /// Binance Algo Rest API Client
    /// </summary>
    public IBinanceAlgoRestClient Algo { get; }

    /// <summary>
    /// Binance Wallet Rest API Client
    /// </summary>
    public IBinanceWalletRestClient Wallet { get; }

    /// <summary>
    /// Binance Copy Trading Rest API Client
    /// </summary>
    public IBinanceCopyTradingRestClient CopyTrading { get; }

    /// <summary>
    /// Binance Auto Invest Rest API Client
    /// </summary>
    public IBinanceAutoInvestRestClient AutoInvest { get; }

    /// <summary>
    /// Binance Convert Rest API Client
    /// </summary>
    public IBinanceConvertRestClient Convert { get; }

    /// <summary>
    /// Binance Sub-Account Rest API Client
    /// </summary>
    public IBinanceSubAccountRestClient SubAccount { get; }

    /// <summary>
    /// Binance Broker (Binance Link) Rest API Client
    /// </summary>
    public IBinanceBrokerRestClient Broker { get; }

    /// <summary>
    /// Binance Mining Rest API Client
    /// </summary>
    public IBinanceMiningRestClient Mining { get; }

    /// <summary>
    /// Binance NFT Rest API Client
    /// </summary>
    public IBinanceNftRestClient NFT { get; }

    /// <summary>
    /// Binance Staking Rest API Client
    /// </summary>
    public IBinanceStakingRestClient Staking { get; }

    /// <summary>
    /// Binance Simple Earn Rest API Client
    /// </summary>
    public IBinanceSimpleEarnRestClient SimpleEarn { get; }

    // TODO: Dual Investment
    // TODO: Crypto Loan
    // TODO: VIP Loan
    // TODO: C2C
    // TODO: Fiat
    // TODO: Gift Card
    // TODO: BAB Token
    // TODO: Rebate
    // TODO: Binance Pay History
    // TODO: Web3 DApp
    // TODO: Binance Web3 Connect
    // TODO: Task Verification
    // TODO: Open Platform
    // TODO: Mini Program
    // TODO: Binance Open API
    // TODO: Binance Fiat Widget
    // TODO: Binance Pay Merchant
    // TODO: Binance Connect

    /// <summary>
    /// Default Constructor
    /// </summary>
    public BinanceRestApiClient() : this(null, new())
    {
    }

    /// <summary>
    /// Constructor with logger
    /// </summary>
    /// <param name="logger">Logger</param>
    public BinanceRestApiClient(ILogger logger) : this(logger, new())
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
    public BinanceRestApiClient(ILogger? logger, BinanceRestApiClientOptions options) : base(logger ?? BaseClient.LoggerFactory.CreateLogger(typeof(BinanceRestApiClient)), options)
    {
        RequestBodyFormat = RestRequestBodyFormat.FormData;
        ArraySerialization = ArraySerialization.MultipleValues;

        Spot = new BinanceSpotRestClient(this);
        Futures = new BinanceFuturesRestClient(this);
        Margin = new BinanceMarginRestClient(this);
        Algo = new BinanceAlgoRestClient(this);
        Wallet = new BinanceWalletRestClient(this);
        CopyTrading = new BinanceCopyTradingRestClient(this);
        AutoInvest = new BinanceAutoInvestRestClient(this);
        Convert = new BinanceConvertRestClient(this);
        SubAccount = new BinanceSubAccountRestClient(this);
        Broker = new BinanceBrokerRestClient(this);
        Mining = new BinanceMiningRestClient(this);
        NFT = new BinanceNftRestClient(this);
        Staking = new BinanceStakingRestClient(this);
        SimpleEarn = new BinanceSimpleEarnRestClient(this);
    }

    #region Public Nethods
    /// <summary>
    /// Sets API Credentials
    /// </summary>
    /// <param name="apikey">API Key</param>
    /// <param name="secret">API Secret</param>
    public void SetApiCredentials(string apikey, string secret) => SetApiCredentials(new ApiCredentials(apikey, secret));
    #endregion

    #region Overrided Methods
    /// <inheritdoc/>
    protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials) => new BinanceAuthentication(credentials);

    /// <inheritdoc/>
    protected override Task<RestCallResult<DateTime>> GetServerTimestampAsync() => Spot.GetTimeAsync();

    /// <inheritdoc/>
    protected override TimeSyncInfo GetTimeSyncInfo() => new(Logger, RestOptions.AutoTimestamp, RestOptions.TimestampRecalculationInterval, TimeSyncState);

    /// <inheritdoc/>
    protected override TimeSpan GetTimeOffset() => TimeSyncState.TimeOffset;

    /// <inheritdoc/>
    protected override Error ParseErrorResponse(JToken error)
    {
        if (!error.HasValues)
            return new ServerError(error.ToString());

        if (error["msg"] == null && error["code"] == null)
            return new ServerError(error.ToString());

        if (error["msg"] != null && error["code"] == null)
            return new ServerError((string)error["msg"]!);

        return new ServerError((int)error["code"]!, (string)error["msg"]!);
    }
    #endregion

    #region Internal Methods
    internal int? ReceiveWindow(int? receiveWindow) => receiveWindow ?? (RestOptions.ReceiveWindow != null ? System.Convert.ToInt32(RestOptions.ReceiveWindow?.TotalMilliseconds) : null);

    internal async Task<RestCallResult<T>> RequestAsync<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object>? queryParameters = null, Dictionary<string, object>? bodyParameters = null, Dictionary<string, string>? headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer? deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
    {
        var result = await SendRequestAsync<T>(uri, method, cancellationToken, signed, queryParameters ?? [], bodyParameters ?? [], headerParameters ?? [], serialization, deserializer, ignoreRatelimit, requestWeight).ConfigureAwait(false);
        if (!result && result.Error!.Code == -1021 && RestOptions.AutoTimestamp)
        {
            Logger.Log(LogLevel.Debug, "Received Invalid Timestamp error, triggering new time sync");
            TimeSyncState.LastSyncTime = DateTime.MinValue;
        }
        return result;
    }
    #endregion
}
