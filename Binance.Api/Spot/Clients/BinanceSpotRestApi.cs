namespace Binance.Api.Spot;

/// <summary>
/// Binance Spot Rest API Client
/// </summary>
public class BinanceSpotRestApi : RestApiClient
{
    // Api
    private const string v1 = "1";
    private const string v3 = "3";
    private const string api = "api";
    private const string sapi = "sapi";

    // Server
    private const string systemStatusEndpoint = "system/status";

    // Wallet
    private const string userCoinsEndpoint = "capital/config/getall";
    private const string accountSnapshotEndpoint = "accountSnapshot";
    private const string disableFastWithdrawSwitchEndpoint = "account/disableFastWithdrawSwitch";
    private const string enableFastWithdrawSwitchEndpoint = "account/enableFastWithdrawSwitch";
    private const string withdrawEndpoint = "capital/withdraw/apply";
    private const string depositHistoryEndpoint = "capital/deposit/hisrec";
    private const string withdrawHistoryEndpoint = "capital/withdraw/history";
    private const string depositAddressEndpoint = "capital/deposit/address";
    private const string accountStatusEndpoint = "account/status";
    private const string tradingStatusEndpoint = "account/apiTradingStatus";
    private const string dustLogEndpoint = "asset/dribblet";
    private const string dustElligableEndpoint = "asset/dust-btc";
    private const string dustTransferEndpoint = "asset/dust";
    private const string dividendRecordsEndpoint = "asset/assetDividend";
    private const string assetDetailsEndpoint = "asset/assetDetail";
    private const string tradeFeeEndpoint = "asset/tradeFee";
    private const string universalTransferEndpoint = "asset/transfer";
    private const string fundingWalletEndpoint = "asset/get-funding-asset";
    private const string balancesEndpoint = "asset/getUserAsset";

    // Account
    private const string convertTransferEndpoint = "asset/convert-transfer";
    private const string convertTransferHistoryEndpoint = "asset/convert-transfer/queryByPage";
    private const string apiRestrictionsEndpoint = "account/apiRestrictions";
    // TODO: Get Cloud-Mining payment and refund history (USER_DATA)
    // TODO: Query auto-converting stable coins (USER_DATA)
    // TODO: Switch on/off BUSD and stable coins conversion (USER_DATA)

    /// <summary>
    /// Event triggered when an order is placed via this client. Only available for Spot orders
    /// </summary>
    public event Action<long>? OnOrderPlaced;

    /// <summary>
    /// Event triggered when an order is canceled via this client. 
    /// Note that this does not trigger when using CancelAllOrdersAsync. Only available for Spot orders
    /// </summary>
    public event Action<long>? OnOrderCanceled;

    // Parent
    internal BinanceRestApiClient _ { get; }

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceRestApiClientOptions Options => (BinanceRestApiClientOptions)ClientOptions;
    internal BinanceExchangeInfo? ExchangeInfo;
    internal DateTime? LastExchangeInfoUpdate;
    internal TimeSyncState TimeSyncState = new("Binance Spot");

    /// <summary>
    /// General Client
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints" /></para>
    /// </summary>
    public BinanceSpotRestApiGeneral General { get; set; }

    /// <summary>
    /// Market Data Client
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/market-data-endpoints" /></para>
    /// </summary>
    public BinanceSpotRestApiMarketData MarketData { get; set; }

    /// <summary>
    /// Trading Client
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/trading-endpoints" /></para>
    /// </summary>
    public BinanceSpotRestApiTrading Trading { get; set; }

    /// <summary>
    /// Account Client
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/account-endpoints" /></para>
    /// </summary>
    public BinanceSpotRestApiAccount Account { get; set; }

    /// <summary>
    /// Binance Spot Rest API Client
    /// </summary>
    /// <param name="root">Parent</param>
    internal BinanceSpotRestApi(BinanceRestApiClient root) : base(root.Logger, root.ClientOptions)
    {
        _ = root;

        General = new BinanceSpotRestApiGeneral(this);
        MarketData = new BinanceSpotRestApiMarketData(this);
        Trading = new BinanceSpotRestApiTrading(this);
        Account = new BinanceSpotRestApiAccount(this);

        RequestBodyFormat = RestRequestBodyFormat.FormData;
        ArraySerialization = ArraySerialization.MultipleValues;
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
        => General.GetTimeAsync();

    /// <inheritdoc/>
    protected override TimeSyncInfo GetTimeSyncInfo()
        => new(Logger, Options.AutoTimestamp, Options.TimestampRecalculationInterval, TimeSyncState);

    /// <inheritdoc/>
    protected override TimeSpan GetTimeOffset()
        => TimeSyncState.TimeOffset;
    #endregion

    #region Internal Methods
    internal void InvokeOrderPlaced(long id)
        => OnOrderPlaced?.Invoke(id);

    internal void InvokeOrderCanceled(long id)
        => OnOrderCanceled?.Invoke(id);

    internal string GetSymbolName(string baseAsset, string quoteAsset) =>
        (baseAsset + quoteAsset).ToUpper(CultureInfo.InvariantCulture);

    internal Uri GetUrl(string api, string version, string endpoint)
    {
        var url = Options.BaseAddress.AppendPath(api);
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }

    internal int? ReceiveWindow(int? receiveWindow)
    {
        return receiveWindow ?? (Options.ReceiveWindow != null ? Convert.ToInt32(Options.ReceiveWindow?.TotalMilliseconds) : null);
    }

    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object>? queryParameters = null, Dictionary<string, object>? bodyParameters = null, Dictionary<string, string>? headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer? deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
    {
        var result = await SendRequestAsync<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight).ConfigureAwait(false);
        if (!result && result.Error!.Code == -1021 && Options.AutoTimestamp)
        {
            Logger.Log(LogLevel.Debug, "Received Invalid Timestamp error, triggering new time sync");
            TimeSyncState.LastSyncTime = DateTime.MinValue;
        }
        return result;
    }

    internal async Task<RestCallResult<BinanceSpotOrder>> PlaceOrderInternal(
        Uri uri,
        string symbol,
        BinanceSpotOrderSide side,
        BinanceSpotOrderType type,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        decimal? price = null,
        decimal? stopPrice = null,
        decimal? icebergQuantity = null,
        string? newClientOrderId = null,
        BinanceSpotTimeInForce? timeInForce = null,
        BinanceSpotOrderResponseType? orderResponseType = null,
        BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
        long? trailingDelta = null,
        long? strategyId = null,
        int? strategyType = null,
        int? receiveWindow = null,
        bool? isIsolated = null,
        bool? autoRepayAtCancel = null,
        BinanceSideEffectType? sideEffectType = null,
        int weight = 1,
        CancellationToken ct = default)
    {
        if (quoteQuantity != null && type != BinanceSpotOrderType.Market)
            throw new ArgumentException("quoteQuantity is only valid for market orders");

        if (quantity == null && quoteQuantity == null || quantity != null && quoteQuantity != null)
            throw new ArgumentException("1 of either should be specified, quantity or quoteOrderQuantity");

        var rulesCheck = await CheckTradeRules(symbol, quantity, quoteQuantity, price, stopPrice, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger?.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceSpotOrder>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;
        quoteQuantity = rulesCheck.QuoteQuantity;
        var clientOrderId = BinanceHelpers.ApplyBrokerId(newClientOrderId, BinanceConstants.ClientOrderIdSpot, 36, Options.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
        };
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddOptional("quantity", quantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("quoteOrderQty", quoteQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("newClientOrderId", clientOrderId);
        parameters.AddOptional("price", price?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptional("stopPrice", stopPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("icebergQty", icebergQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalEnum("sideEffectType", sideEffectType);
        parameters.AddOptional("isIsolated", isIsolated);
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptional("trailingDelta", trailingDelta);
        parameters.AddOptional("strategyId", strategyId);
        parameters.AddOptional("strategyType", strategyType);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptional("autoRepayAtCancel", autoRepayAtCancel);
        parameters.AddOptional("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceSpotOrder>(uri, HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: weight).ConfigureAwait(false);
    }

    internal async Task<BinanceTradeRuleResult> CheckTradeRules(string symbol, decimal? quantity, decimal? quoteQuantity, decimal? price, decimal? stopPrice, BinanceSpotOrderType? type, CancellationToken ct)
    {
        if (Options.SpotOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.None)
            return BinanceTradeRuleResult.CreatePassed(quantity, quoteQuantity, price, stopPrice);

        if (ExchangeInfo == null || LastExchangeInfoUpdate == null || (DateTime.UtcNow - LastExchangeInfoUpdate.Value).TotalMinutes > Options.SpotOptions.TradeRulesUpdateInterval.TotalMinutes)
            await General.GetExchangeInfoAsync(ct).ConfigureAwait(false);

        if (ExchangeInfo == null)
            return BinanceTradeRuleResult.CreateFailed("Unable to retrieve trading rules, validation failed");

        return BinanceHelpers.ValidateTradeRules(Logger, Options.SpotOptions.TradeRulesBehavior, ExchangeInfo, symbol, quantity, quoteQuantity, price, stopPrice, type);
    }
    #endregion

    #region System Status
    public async Task<RestCallResult<BinanceSystemStatus>> GetSystemStatusAsync(CancellationToken ct = default)
    {
        return await SendRequestInternal<BinanceSystemStatus>(GetUrl(sapi, v1, systemStatusEndpoint), HttpMethod.Get, ct).ConfigureAwait(false);
    }
    #endregion

    #region Get Products
    public async Task<RestCallResult<IEnumerable<BinanceProduct>>> GetProductsAsync(CancellationToken ct = default)
    {
        var url = Options.BaseAddress.Replace("api.", "www.").AppendPath("exchange-api/v2/public/asset-service/product/get-products");

        var data = await SendRequestInternal<BinanceExchangeApiWrapper<IEnumerable<BinanceProduct>>>(new Uri(url), HttpMethod.Get, ct).ConfigureAwait(false);
        if (!data)
            return data.As<IEnumerable<BinanceProduct>>(null);

        if (!data.Data.Success)
            return data.AsError<IEnumerable<BinanceProduct>>(new ServerError(data.Data.Code, data.Data.Message + " - " + data.Data.MessageDetail));

        return data.As(data.Data.Data);
    }
    #endregion

    #region Account Methods

    #region All Coins' Information
    public async Task<RestCallResult<IEnumerable<BinanceUserAsset>>> GetUserAssetsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));


        return await SendRequestInternal<IEnumerable<BinanceUserAsset>>(GetUrl(sapi, v1, userCoinsEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Daily Account Snapshots
    public async Task<RestCallResult<IEnumerable<BinanceSpotAccountSnapshot>>> GetDailySpotAccountSnapshotAsync(
        DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null,
        CancellationToken ct = default) =>
        await GetDailyAccountSnapshot<IEnumerable<BinanceSpotAccountSnapshot>>(BinancePermissionType.Spot, startTime, endTime, limit, receiveWindow, ct).ConfigureAwait(false);

    public async Task<RestCallResult<IEnumerable<BinanceMarginAccountSnapshot>>> GetDailyMarginAccountSnapshotAsync(
        DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null,
        CancellationToken ct = default) =>
        await GetDailyAccountSnapshot<IEnumerable<BinanceMarginAccountSnapshot>>(BinancePermissionType.Margin, startTime, endTime, limit, receiveWindow, ct).ConfigureAwait(false);

    public async Task<RestCallResult<IEnumerable<BinanceFuturesAccountSnapshot>>> GetDailyFutureAccountSnapshotAsync(
        DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null,
        CancellationToken ct = default) =>
        await GetDailyAccountSnapshot<IEnumerable<BinanceFuturesAccountSnapshot>>(BinancePermissionType.Futures, startTime, endTime, limit, receiveWindow, ct).ConfigureAwait(false);


    private async Task<RestCallResult<T>> GetDailyAccountSnapshot<T>(BinancePermissionType accountType, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null,
        CancellationToken ct = default) where T : class
    {
        limit?.ValidateIntBetween(nameof(limit), 5, 30);

        var parameters = new Dictionary<string, object>
            {
                { "type", MapConverter.GetString(accountType) }
            };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceSnapshotWrapper<T>>(GetUrl(sapi, v1, accountSnapshotEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 2400).ConfigureAwait(false);
        if (!result.Success)
            return result.As<T>(default);

        if (result.Data.Code != 200)
            return result.AsError<T>(new ServerError(result.Data.Code, result.Data.Message));

        return result.As(result.Data.SnapshotData);
    }
    #endregion

    #region Disable Fast Withdraw Switch
    public async Task<RestCallResult<object>> DisableFastWithdrawSwitchAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<object>(GetUrl(sapi, v1, disableFastWithdrawSwitchEndpoint), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Enable Fast Withdraw Switch
    public async Task<RestCallResult<object>> EnableFastWithdrawSwitchAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<object>(GetUrl(sapi, v1, enableFastWithdrawSwitchEndpoint), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Withdraw
    public async Task<RestCallResult<BinanceWithdrawalPlaced>> WithdrawAsync(string asset, string address, decimal quantity, string withdrawOrderId = null, string network = null, string addressTag = null, string name = null, bool? transactionFeeFlag = null, WalletType? walletType = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));
        address.ValidateNotNull(nameof(address));

        var parameters = new Dictionary<string, object>
            {
                { "coin", asset },
                { "address", address },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
        parameters.AddOptionalParameter("name", name);
        parameters.AddOptionalParameter("withdrawOrderId", withdrawOrderId);
        parameters.AddOptionalParameter("network", network);
        parameters.AddOptionalParameter("transactionFeeFlag", transactionFeeFlag);
        parameters.AddOptionalParameter("addressTag", addressTag);
        parameters.AddOptionalParameter("walletType", walletType != null ? JsonConvert.SerializeObject(walletType, new WalletTypeConverter(false)) : null);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceWithdrawalPlaced>(GetUrl(sapi, v1, withdrawEndpoint), HttpMethod.Post, ct, true, queryParameters: parameters).ConfigureAwait(false);
        return result;
    }
    #endregion

    #region Withdraw History (Supporting Network)
    public async Task<RestCallResult<IEnumerable<BinanceWithdrawal>>> GetWithdrawalHistoryAsync(string asset = null, string withdrawOrderId = null, WithdrawalStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, int? limit = null, int? offset = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("coin", asset);
        parameters.AddOptionalParameter("withdrawOrderId", withdrawOrderId);
        parameters.AddOptionalParameter("status", status != null ? JsonConvert.SerializeObject(status, new WithdrawalStatusConverter(false)) : null);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit);
        parameters.AddOptionalParameter("offset", offset);

        var result = await SendRequestInternal<IEnumerable<BinanceWithdrawal>>(GetUrl(sapi, v1, withdrawHistoryEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        return result;
    }
    #endregion

    #region Deposit History (Supporting Network)
    public async Task<RestCallResult<IEnumerable<BinanceDeposit>>> GetDepositHistoryAsync(string asset = null, DepositStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? offset = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("coin", asset);
        parameters.AddOptionalParameter("offset", offset?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("status", status != null ? JsonConvert.SerializeObject(status, new DepositStatusConverter(false)) : null);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceDeposit>>(
                GetUrl(sapi, v1, depositHistoryEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters)
            .ConfigureAwait(false);
    }
    #endregion

    #region Deposit Address (Supporting Network)
    public async Task<RestCallResult<BinanceDepositAddress>> GetDepositAddressAsync(string asset, string network = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));

        var parameters = new Dictionary<string, object>
            {
                { "coin", asset }
            };
        parameters.AddOptionalParameter("network", network);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceDepositAddress>(GetUrl(sapi, v1, depositAddressEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Account Status
    public async Task<RestCallResult<BinanceAccountStatus>> GetAccountStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceAccountStatus>(GetUrl(sapi, v1, accountStatusEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        return result;
    }
    #endregion

    #region Account API Trading Status
    public async Task<RestCallResult<BinanceTradingStatus>> GetTradingStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceResult<BinanceTradingStatus>>(GetUrl(sapi, v1, tradingStatusEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        if (!result)
            return result.As<BinanceTradingStatus>(default);

        return !string.IsNullOrEmpty(result.Data.Message) ? result.AsError<BinanceTradingStatus>(new ServerError(result.Data.Message!)) : result.As(result.Data.Data);
    }
    #endregion

    #region DustLog
    public async Task<RestCallResult<BinanceDustLogList>> GetDustLogAsync(DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        var result = await SendRequestInternal<BinanceDustLogList>(GetUrl(sapi, v1, dustLogEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        return result;
    }
    #endregion

    #region Get Assets That Can Be Converted Into BNB
    public async Task<RestCallResult<BinanceElligableDusts>> GetAssetsForDustTransferAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceElligableDusts>(GetUrl(sapi, v1, dustElligableEndpoint), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Dust Transfer
    public async Task<RestCallResult<BinanceDustTransferResult>> DustTransferAsync(IEnumerable<string> assets, int? receiveWindow = null, CancellationToken ct = default)
    {
        var assetsArray = assets.ToArray();

        assetsArray.ValidateNotNull(nameof(assets));
        foreach (var asset in assetsArray)
            asset.ValidateNotNull(nameof(asset));

        var parameters = new Dictionary<string, object>
            {
                { "asset", assetsArray }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceDustTransferResult>(GetUrl(sapi, v1, dustTransferEndpoint), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Asset Dividend Record
    public async Task<RestCallResult<BinanceQueryRecords<BinanceDividendRecord>>> GetAssetDividendRecordsAsync(string asset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("asset", asset);
        parameters.AddOptionalParameter("limit", limit);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceDividendRecord>>(GetUrl(sapi, v1, dividendRecordsEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Asset Detail
    public async Task<RestCallResult<Dictionary<string, BinanceAssetDetails>>> GetAssetDetailsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<Dictionary<string, BinanceAssetDetails>>(GetUrl(sapi, v1, assetDetailsEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        return result;
    }
    #endregion

    #region Trade Fee
    public async Task<RestCallResult<IEnumerable<BinanceTradeFee>>> GetTradeFeeAsync(string symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol?.ValidateBinanceSymbol();
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<IEnumerable<BinanceTradeFee>>(GetUrl(sapi, v1, tradeFeeEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        return result;
    }
    #endregion

    #region User Universal Transfer
    public async Task<RestCallResult<BinanceTransaction>> TransferAsync(UniversalTransferType type, string asset, decimal quantity, string fromSymbol = null, string toSymbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "type", JsonConvert.SerializeObject(type, new UniversalTransferTypeConverter(false)) },
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };

        parameters.AddOptionalParameter("fromSymbol", fromSymbol);
        parameters.AddOptionalParameter("toSymbol", toSymbol);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceTransaction>(GetUrl(sapi, v1, universalTransferEndpoint), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Query User Universal Transfer History
    public async Task<RestCallResult<BinanceQueryRecords<BinanceTransfer>>> GetTransfersAsync(UniversalTransferType type, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "type", JsonConvert.SerializeObject(type, new UniversalTransferTypeConverter(false)) }
            };
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("current", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("size", pageSize?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceTransfer>>(GetUrl(sapi, v1, universalTransferEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Funding Wallet
    public async Task<RestCallResult<IEnumerable<BinanceFundingAsset>>> GetFundingWalletAsync(string asset = null, bool? needBtcValuation = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("asset", asset);
        parameters.AddOptionalParameter("needBtcValuation", needBtcValuation?.ToString());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFundingAsset>>(GetUrl(sapi, v1, fundingWalletEndpoint), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region User Asset
    public async Task<RestCallResult<IEnumerable<BinanceUserBalance>>> GetBalancesAsync(string asset = null, bool? needBtcValuation = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("asset", asset);
        parameters.AddOptionalParameter("needBtcValuation", needBtcValuation);
        return await SendRequestInternal<IEnumerable<BinanceUserBalance>>(GetUrl(sapi, v3, balancesEndpoint), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5).ConfigureAwait(false);
    }
    #endregion

    #region BUSD Convert
    public async Task<RestCallResult<BinanceConvertTransferResult>> ConvertTransferAsync(string clientTransferId, string asset, decimal quantity, string targetAsset, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
            {
                { "clientTranId", clientTransferId },
                { "asset", asset },
                { "amount", quantity },
                { "targetAsset", targetAsset }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceConvertTransferResult>(GetUrl(sapi, v1, convertTransferEndpoint), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5).ConfigureAwait(false);
    }
    #endregion

    #region BUSD Convert History
    public async Task<RestCallResult<BinanceQueryRecords<BinanceConvertTransferRecord>>> GetConvertTransferHistoryAsync(DateTime startTime, DateTime endTime, long? transferId = null, string asset = null, int? page = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
            {
                { "startTime", startTime.ConvertToMilliseconds() },
                { "endTime", endTime.ConvertToMilliseconds() },
            };
        parameters.AddOptionalParameter("tranId", transferId);
        parameters.AddOptionalParameter("asset", asset);
        parameters.AddOptionalParameter("current", page);
        parameters.AddOptionalParameter("size", limit);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceConvertTransferRecord>>(GetUrl(sapi, v1, convertTransferHistoryEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
    }
    #endregion

    #region Get API Key Permission
    public async Task<RestCallResult<BinanceAPIKeyPermissions>> GetAPIKeyPermissionsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceAPIKeyPermissions>(GetUrl(sapi, v1, apiRestrictionsEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #endregion

}