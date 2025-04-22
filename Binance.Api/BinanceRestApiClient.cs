using Binance.Api.Algo;
using Binance.Api.Margin;
using Binance.Api.Spot;
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
    public IBinanceSpotRestApiClient Spot { get; }

    /// <summary>
    /// Binance Margin Rest API Client
    /// </summary>
    public IBinanceMarginRestApiClient Margin { get; }

    /// <summary>
    /// Binance Algo Rest API Client
    /// </summary>
    public IBinanceAlgoRestApiClient Algo { get; }

    /// <summary>
    /// Binance Wallet Rest API Client
    /// </summary>
    public IBinanceWalletRestApiClient Wallet { get; }














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
    public BinanceRestApiClient(ILogger? logger, BinanceRestApiClientOptions options)
        : base(logger ?? LoggerFactory.CreateLogger(typeof(BinanceRestApiClient)), options)
    {
        RequestBodyFormat = RestRequestBodyFormat.FormData;
        ArraySerialization = ArraySerialization.MultipleValues;

        Spot = new BinanceSpotRestApiClient(this);
        Margin = new BinanceMarginRestApiClient(this);
        Algo = new BinanceAlgoRestApiClient(this);
        Wallet = new BinanceWalletRestApiClient(this);
    }

    #region Overrided Methods
    /// <inheritdoc/>
    protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
        => new BinanceAuthenticationProvider(credentials);

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

    /// <inheritdoc/>
    protected override Task<RestCallResult<DateTime>> GetServerTimestampAsync()
        => Spot.GetTimeAsync();

    /// <inheritdoc/>
    protected override TimeSyncInfo GetTimeSyncInfo()
        => new(Logger, RestOptions.AutoTimestamp, RestOptions.TimestampRecalculationInterval, TimeSyncState);

    /// <inheritdoc/>
    protected override TimeSpan GetTimeOffset()
        => TimeSyncState.TimeOffset;
    #endregion

    #region Internal Methods
    internal string GetSymbolName(string baseAsset, string quoteAsset) => (baseAsset + quoteAsset).ToUpper(CultureInfo.InvariantCulture);

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
