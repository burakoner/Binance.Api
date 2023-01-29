using Binance.Api.Models.RestApi.Brokerage;

namespace Binance.Api.Clients.RestApi.General;

public class BinanceRestApiBrokerageClient
{
    // Api
    private const string brokerageApi = "sapi";
    private const string brokerageVersion = "1";

    // Sub Account
    private const string subAccountCreateEndpoint = "broker/subAccount";
    private const string subAccountQueryEndpoint = "broker/subAccount";

    // Permissions
    private const string enableMarginForSubAccountEndpoint = "broker/subAccount/margin";
    private const string enableFuturesForSubAccountEndpoint = "broker/subAccount/futures";
    private const string enableLeverageTokenForSubAccountEndpoint = "broker/subAccount/blvt";
    // TODO: Enable Universal Transfer Permission For Sub Account Api Key
    // TODO: Enable Vanilla Options Permission For Sub Account Api Key

    // Api Keys
    private const string apiKeyCreateEndpoint = "broker/subAccountApi";
    private const string apiKeyDeleteEndpoint = "broker/subAccountApi";
    private const string apiKeyQueryEndpoint = "broker/subAccountApi";
    private const string apiKeyPermissionEndpoint = "broker/subAccountApi/permission";
    private const string apiKeyIpRestrictionEndpoint = "broker/subAccountApi/ipRestriction";
    private const string apiKeyIpRestrictionAddEndpoint = "broker/subAccountApi/ipRestriction/ipList";
    private const string apiKeyIpRestrictionListEndpoint = "broker/subAccountApi/ipRestriction/ipList";
    private const string apiKeyIpRestrictionUpdateEndpoint = "broker/subAccountApi/ipRestriction";

    // Commission
    private const string apiKeyCommissionEndpoint = "broker/subAccountApi/commission";
    private const string apiKeyCommissionFuturesChangeEndpoint = "broker/subAccountApi/commission/futures";
    private const string apiKeyCommissionFuturesQueryEndpoint = "broker/subAccountApi/commission/futures";
    private const string apiKeyCommissionCoinFuturesChangeEndpoint = "broker/subAccountApi/commission/coinFutures";
    private const string apiKeyCommissionCoinFuturesQueryEndpoint = "broker/subAccountApi/commission/coinFutures";

    // Broker Info
    private const string brokerAccountInfoEndpoint = "broker/info";

    // Transfer
    private const string transferEndpoint = "broker/transfer";
    private const string transferQueryEndpoint = "broker/transfer";
    private const string transferFuturesEndpoint = "broker/transfer/futures";
    private const string transferFuturesQueryEndpoint = "broker/transfer/futures";

    // Rebate
    private const string rebatesRecentEndpoint = "broker/rebate/recentRecord";
    private const string rebatesFuturesHistoryEndpoint = "broker/rebate/futures/recentRecord";

    // BNB Burn
    private const string changeBnbBurnForSubAccountSpotAndMarginEndpoint = "broker/subAccount/bnbBurn/spot";
    private const string changeBnbBurnForSubAccountMarginInterestEndpoint = "broker/subAccount/bnbBurn/marginInterest";
    private const string bnbBurnForSubAccountStatusEndpoint = "broker/subAccount/bnbBurn/status";

    // Deposit
    private const string depositHistoryEndpoint = "broker/subAccount/depositHist";

    // Summary
    private const string spotSummaryEndpoint = "broker/subAccount/spotSummary";
    private const string marginSummaryEndpoint = "broker/subAccount/marginSummary";
    private const string futuresSummaryEndpoint = "broker/subAccount/futuresSummary";

    // Universal Transfer
    private const string transferUniversalEndpoint = "broker/universalTransfer";
    private const string transferUniversalQueryEndpoint = "broker/universalTransfer";

    // Internal References
    internal BinanceRestApiClient RootClient { get; }
    internal BinanceRestApiGeneralClient GeneralClient { get; }
    internal BinanceRestApiClientOptions Options { get => RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => GeneralClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await GeneralClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiBrokerageClient(BinanceRestApiClient root, BinanceRestApiGeneralClient general)
    {
        RootClient = root;
        GeneralClient = general;
    }

    #region Create a Sub Account
    public async Task<RestCallResult<BinanceBrokerageSubAccountCreateResult>> CreateSubAccountAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageSubAccountCreateResult>(GetUrl(subAccountCreateEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Query Sub Account
    public async Task<RestCallResult<IEnumerable<BinanceBrokerageSubAccount>>> GetSubAccountsAsync(string subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("subAccountId", subAccountId);
        parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("size", size?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceBrokerageSubAccount>>(GetUrl(subAccountQueryEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Enable Margin for Sub Account
    public async Task<RestCallResult<BinanceBrokerageEnableMarginResult>> EnableMarginForSubAccountAsync(string subAccountId, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"margin", true},  // only true for now
                             };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageEnableMarginResult>(GetUrl(enableMarginForSubAccountEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Enable Futures for Sub Account
    public async Task<RestCallResult<BinanceBrokerageEnableFuturesResult>> EnableFuturesForSubAccountAsync(string subAccountId, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));
        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"futures", true},  // only true for now
                             };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageEnableFuturesResult>(GetUrl(enableFuturesForSubAccountEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Enable Leverage Token for Sub Account
    public async Task<RestCallResult<BinanceBrokerageEnableLeverageTokenResult>> EnableLeverageTokenForSubAccountAsync(string subAccountId, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"blvt", true},  // only true for now
                             };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageEnableLeverageTokenResult>(GetUrl(enableLeverageTokenForSubAccountEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Create Api Key for Sub Account
    public async Task<RestCallResult<BinanceBrokerageApiKeyCreateResult>> CreateApiKeyForSubAccountAsync(string subAccountId, bool isSpotTradingEnabled,
    bool? isMarginTradingEnabled = null, bool? isFuturesTradingEnabled = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"canTrade", isSpotTradingEnabled}
                             };
        parameters.AddOptionalParameter("marginTrade", isMarginTradingEnabled.ToString().ToLower());
        parameters.AddOptionalParameter("futuresTrade", isFuturesTradingEnabled.ToString().ToLower());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageApiKeyCreateResult>(GetUrl(apiKeyCreateEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Delete Sub Account Api Key
    public async Task<object> DeleteSubAccountApiKeyAsync(string subAccountId, string apiKey, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));
        apiKey.ValidateNotNull(nameof(apiKey));

        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"subAccountApiKey", apiKey}
                             };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<object>(GetUrl(apiKeyDeleteEndpoint, brokerageApi, brokerageVersion), HttpMethod.Delete, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Query Sub Account Api Key
    public async Task<RestCallResult<BinanceBrokerageSubAccountApiKey>> GetSubAccountApiKeyAsync(string subAccountId, string apiKey = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                             };
        parameters.AddOptionalParameter("subAccountApiKey", apiKey);
        parameters.AddOptionalParameter("page", page);
        parameters.AddOptionalParameter("size", size);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageSubAccountApiKey>(GetUrl(apiKeyQueryEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Change Sub Account Api Permission
    public async Task<RestCallResult<BinanceBrokerageSubAccountApiKey>> ChangeSubAccountApiKeyPermissionAsync(string subAccountId, string apiKey,
    bool isSpotTradingEnabled, bool isMarginTradingEnabled, bool isFuturesTradingEnabled, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));
        apiKey.ValidateNotNull(nameof(apiKey));

        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"subAccountApiKey", apiKey},
                                 {"canTrade", isSpotTradingEnabled.ToString().ToLower()},
                                 {"marginTrade", isMarginTradingEnabled.ToString().ToLower()},
                                 {"futuresTrade", isFuturesTradingEnabled.ToString().ToLower()}
                             };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageSubAccountApiKey>(GetUrl(apiKeyPermissionEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Add IP Restriction for Sub Account Api Key
    public async Task<RestCallResult<BinanceBrokerageAddIpRestrictionResult>> AddIpRestrictionForSubAccountApiKeyAsync(string subAccountId,
        string apiKey, string ipAddress, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));
        apiKey.ValidateNotNull(nameof(apiKey));
        ipAddress.ValidateNotNull(nameof(ipAddress));

        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"subAccountApiKey", apiKey},
                                 {"ipAddress", ipAddress}
                             };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageAddIpRestrictionResult>(GetUrl(apiKeyIpRestrictionAddEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Get IP Restriction for Sub Account Api Key
    public async Task<RestCallResult<BinanceBrokerageIpRestriction>> GetIpRestrictionForSubAccountApiKeyAsync(string subAccountId,
        string apiKey, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));
        apiKey.ValidateNotNull(nameof(apiKey));

        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"subAccountApiKey", apiKey}
                             };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageIpRestriction>(GetUrl(apiKeyIpRestrictionEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Delete IP Restriction for Sub Account Api Key
    public async Task<RestCallResult<BinanceBrokerageIpRestrictionBase>> DeleteIpRestrictionForSubAccountApiKeyAsync(string subAccountId,
        string apiKey, string ipAddress, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));
        apiKey.ValidateNotNull(nameof(apiKey));
        ipAddress.ValidateNotNull(nameof(ipAddress));

        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"subAccountApiKey", apiKey},
                                 {"ipAddress", ipAddress}
                             };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageIpRestrictionBase>(GetUrl(apiKeyIpRestrictionListEndpoint, brokerageApi, brokerageVersion), HttpMethod.Delete, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Update IP Restriction for Sub-Account API key (For Master Account)
    public async Task<RestCallResult<BinanceBrokerageIpRestriction>> ChangeIpRestrictionForSubAccountApiKeyAsync(string subAccountId,
           string apiKey, bool ipRestrict, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));
        apiKey.ValidateNotNull(nameof(apiKey));

        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"subAccountApiKey", apiKey},
                                 {"ipRestrict", ipRestrict.ToString().ToLower()}
                             };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageIpRestriction>(GetUrl(apiKeyIpRestrictionUpdateEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Change Sub Account Commission
    public async Task<RestCallResult<BinanceBrokerageSubAccountCommission>> ChangeSubAccountCommissionAsync(string subAccountId,
        decimal makerCommission, decimal takerCommission, decimal? marginMakerCommission = null, decimal? marginTakerCommission = null,
        int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"makerCommission", makerCommission.ToString(CultureInfo.InvariantCulture)},
                                 {"takerCommission", takerCommission.ToString(CultureInfo.InvariantCulture)}
                             };
        parameters.AddOptionalParameter("marginMakerCommission", marginMakerCommission?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("marginTakerCommission", marginTakerCommission?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageSubAccountCommission>(GetUrl(apiKeyCommissionEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Change Sub Account USDT-Ⓜ Futures Commission Adjustment
    public async Task<RestCallResult<BinanceBrokerageSubAccountFuturesCommission>> ChangeSubAccountFuturesCommissionAdjustmentAsync(string subAccountId, string symbol,
        int makerAdjustment, int takerAdjustment, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));
        symbol.ValidateNotNull(nameof(symbol));

        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"symbol", symbol},
                                 {"makerAdjustment", makerAdjustment.ToString(CultureInfo.InvariantCulture)},
                                 {"takerAdjustment", takerAdjustment.ToString(CultureInfo.InvariantCulture)}
                             };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageSubAccountFuturesCommission>(GetUrl(apiKeyCommissionFuturesChangeEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Query Sub Account USDT-Ⓜ Futures Commission Adjustment
    public async Task<RestCallResult<IEnumerable<BinanceBrokerageSubAccountFuturesCommission>>> GetSubAccountFuturesCommissionAdjustmentAsync(string subAccountId,
        string symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId}
                             };
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceBrokerageSubAccountFuturesCommission>>(GetUrl(apiKeyCommissionFuturesQueryEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Change Sub Account COIN-Ⓜ Futures Commission Adjustment
    public async Task<RestCallResult<BinanceBrokerageSubAccountCoinFuturesCommission>> ChangeSubAccountCoinFuturesCommissionAdjustmentAsync(string subAccountId,
         string pair, int makerAdjustment, int takerAdjustment, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));
        pair.ValidateNotNull(nameof(pair));

        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"pair", pair},
                                 {"makerAdjustment", makerAdjustment.ToString(CultureInfo.InvariantCulture)},
                                 {"takerAdjustment", takerAdjustment.ToString(CultureInfo.InvariantCulture)}
                             };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageSubAccountCoinFuturesCommission>(GetUrl(apiKeyCommissionCoinFuturesChangeEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Query Sub Account COIN-Ⓜ Futures Commission Adjustment
    public async Task<RestCallResult<IEnumerable<BinanceBrokerageSubAccountFuturesCommission>>> GetSubAccountCoinFuturesCommissionAdjustmentAsync(string subAccountId,
        string pair = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId}
                             };
        parameters.AddOptionalParameter("pair", pair);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceBrokerageSubAccountFuturesCommission>>(GetUrl(apiKeyCommissionCoinFuturesQueryEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Broker Account Information
    public async Task<RestCallResult<BinanceBrokerageAccountInfo>> GetBrokerAccountInfoAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageAccountInfo>(GetUrl(brokerAccountInfoEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Sub Account Transfer (SPOT)
    public async Task<RestCallResult<BinanceBrokerageTransferResult>> TransferAsync(string asset, decimal quantity,
        string fromId, string toId, string clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));

        var parameters = new Dictionary<string, object>
                             {
                                 {"asset", asset},
                                 {"amount", quantity.ToString(CultureInfo.InvariantCulture)},
                             };
        parameters.AddOptionalParameter("fromId", fromId);
        parameters.AddOptionalParameter("toId", toId);
        parameters.AddOptionalParameter("clientTranId", clientTransferId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageTransferResult>(GetUrl(transferEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Query Sub Account Transfer History (SPOT)
    public async Task<RestCallResult<IEnumerable<BinanceBrokerageTransferTransaction>>> GetTransferHistoryAsync(string fromId = null, string toId = null,
        string clientTransferId = null, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? limit = null, bool showAllStatus = false,
        int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
                             {
                                 {"showAllStatus", showAllStatus.ToString().ToLower()},
                             };
        parameters.AddOptionalParameter("fromId", fromId);
        parameters.AddOptionalParameter("toId", toId);
        parameters.AddOptionalParameter("clientTranId", clientTransferId);
        parameters.AddOptionalParameter("startTime", startDate.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endDate.ConvertToMilliseconds());
        parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceBrokerageTransferTransaction>>(GetUrl(transferQueryEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Sub Account Transfer (FUTURES)
    public async Task<RestCallResult<BinanceBrokerageTransferFuturesResult>> TransferFuturesAsync(string asset, decimal quantity, BinanceBrokerageFuturesType futuresType,
        string fromId, string toId, string clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));

        var parameters = new Dictionary<string, object>
                             {
                                 {"asset", asset},
                                 {"amount", quantity.ToString(CultureInfo.InvariantCulture)},
                                 {"futuresType", ((int)futuresType).ToString(CultureInfo.InvariantCulture)}
                             };
        parameters.AddOptionalParameter("fromId", fromId);
        parameters.AddOptionalParameter("toId", toId);
        parameters.AddOptionalParameter("clientTranId", clientTransferId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageTransferFuturesResult>(GetUrl(transferFuturesEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Query Sub Account Transfer History (FUTURES)
    public async Task<RestCallResult<BinanceBrokerageTransferFuturesTransactions>> GetTransferFuturesHistoryAsync(string subAccountId,
        BinanceBrokerageFuturesType futuresType, DateTime? startDate = null, DateTime? endDate = null,
        int? page = null, int? limit = null, string clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"futuresType", ((int)futuresType).ToString(CultureInfo.InvariantCulture)}
                             };
        parameters.AddOptionalParameter("startTime", startDate.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endDate.ConvertToMilliseconds());
        parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("clientTranId", clientTransferId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageTransferFuturesTransactions>(GetUrl(transferFuturesQueryEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Query Broker Commission Rebate Recent Record (Spot)
    public async Task<RestCallResult<IEnumerable<BinanceBrokerageRebate>>> GetBrokerCommissionRebatesRecentAsync(string subAccountId,
        DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                             };
        parameters.AddOptionalParameter("startTime", startDate.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endDate.ConvertToMilliseconds());
        parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("size", size?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceBrokerageRebate>>(GetUrl(rebatesRecentEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Query Broker Commission Rebate Recent Record (Futures)
    public async Task<RestCallResult<IEnumerable<BinanceBrokerageFuturesRebate>>> GetBrokerFuturesCommissionRebatesHistoryAsync(BinanceBrokerageFuturesType futuresType,
        DateTime startDate, DateTime endDate, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
                             {
                                 {"futuresType", ((int)futuresType).ToString(CultureInfo.InvariantCulture)},
                                 {"startTime", startDate.ConvertToMilliseconds()!},
                                 {"endTime",  endDate.ConvertToMilliseconds()!}
                             };
        parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("size", size?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceBrokerageFuturesRebate>>(GetUrl(rebatesFuturesHistoryEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }

    #endregion

    #region Enable Or Disable BNB Burn for Sub Account SPOT and MARGIN
    public async Task<RestCallResult<BinanceBrokerageChangeBnbBurnSpotAndMarginResult>> ChangeBnbBurnForSubAccountSpotAndMarginAsync(string subAccountId, bool spotBnbBurn,
        int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"spotBNBBurn", spotBnbBurn.ToString().ToLower()}
                             };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageChangeBnbBurnSpotAndMarginResult>(GetUrl(changeBnbBurnForSubAccountSpotAndMarginEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Enable Or Disable BNB Burn for Sub Account Margin Interest
    public async Task<RestCallResult<BinanceBrokerageChangeBnbBurnMarginInterestResult>> ChangeBnbBurnForSubAccountMarginInterestAsync(string subAccountId, bool interestBnbBurn,
        int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId},
                                 {"interestBNBBurn", interestBnbBurn.ToString().ToLower()}
                             };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageChangeBnbBurnMarginInterestResult>(GetUrl(changeBnbBurnForSubAccountMarginInterestEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Get BNB Burn Status for Sub Account
    public async Task<RestCallResult<BinanceBrokerageBnbBurnStatus>> GetBnbBurnStatusForSubAccountAsync(string subAccountId, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new Dictionary<string, object>
                             {
                                 {"subAccountId", subAccountId}
                             };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageBnbBurnStatus>(GetUrl(bnbBurnForSubAccountStatusEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Get Sub Account Deposit History
    public async Task<RestCallResult<IEnumerable<BinanceBrokerageSubAccountDepositTransaction>>> GetSubAccountDepositHistoryAsync(string subAccountId = null,
        string asset = null, BinanceBrokerageSubAccountDepositStatus? status = null, DateTime? startDate = null, DateTime? endDate = null,
        int? limit = null, int? offset = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("subAccountId", subAccountId);
        parameters.AddOptionalParameter("coin", asset);
        parameters.AddOptionalParameter("status", status.HasValue ? ((int)status).ToString(CultureInfo.InvariantCulture) : null);
        parameters.AddOptionalParameter("startTime", startDate.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endDate.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("offset", offset?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceBrokerageSubAccountDepositTransaction>>(GetUrl(depositHistoryEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Query Sub Account Spot Asset info
    public async Task<RestCallResult<BinanceBrokerageSpotAssetInfo>> GetSubAccountSpotAssetInfoAsync(
        string subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("subAccountId", subAccountId);
        parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("size", size?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageSpotAssetInfo>(GetUrl(spotSummaryEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Query Subaccount Margin Asset info
    public async Task<RestCallResult<BinanceBrokerageMarginAssetInfo>> GetSubAccountMarginAssetInfoAsync(
        string subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("subAccountId", subAccountId);
        parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("size", size?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageMarginAssetInfo>(GetUrl(marginSummaryEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Query Subaccount Futures Asset info
    public async Task<RestCallResult<BinanceBrokerageFuturesAssetInfo>> GetSubAccountFuturesAssetInfoAsync(BinanceBrokerageFuturesType futuresType,
        string subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
                             {
                                 {"futuresType", ((int)futuresType).ToString(CultureInfo.InvariantCulture)}
                             };
        parameters.AddOptionalParameter("subAccountId", subAccountId);
        parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("size", size?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageFuturesAssetInfo>(GetUrl(futuresSummaryEndpoint, brokerageApi, "2"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Universal Transfer
    public async Task<RestCallResult<BinanceBrokerageTransferResult>> TransferUniversalAsync(string asset, decimal quantity,
        string fromId, BrokerageAccountType fromAccountType, string toId, BrokerageAccountType toAccountType,
        string clientTransferId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));

        var parameters = new Dictionary<string, object>
                             {
                                 {"asset", asset},
                                 {"amount", quantity.ToString(CultureInfo.InvariantCulture)},
                                 {"fromAccountType", JsonConvert.SerializeObject(fromAccountType, new BrokerageAccountTypeConverter(false))},
                                 {"toAccountType", JsonConvert.SerializeObject(toAccountType, new BrokerageAccountTypeConverter(false))}
                             };
        parameters.AddOptionalParameter("fromId", fromId);
        parameters.AddOptionalParameter("toId", toId);
        parameters.AddOptionalParameter("clientTranId", clientTransferId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBrokerageTransferResult>(GetUrl(transferUniversalEndpoint, brokerageApi, brokerageVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Query Universal Transfer History
    public async Task<RestCallResult<IEnumerable<BinanceBrokerageTransferTransactionUniversal>>> GetTransferHistoryUniversalAsync(
        string fromId = null, string toId = null, string clientTransferId = null, DateTime? startDate = null, DateTime? endDate = null,
        int? page = null, int? limit = null, bool showAllStatus = false, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
                             {
                                 {"showAllStatus", showAllStatus.ToString().ToLower()},
                             };
        parameters.AddOptionalParameter("fromId", fromId);
        parameters.AddOptionalParameter("toId", toId);
        parameters.AddOptionalParameter("clientTranId", clientTransferId);
        parameters.AddOptionalParameter("startTime", startDate.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endDate.ConvertToMilliseconds());
        parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceBrokerageTransferTransactionUniversal>>(GetUrl(transferUniversalQueryEndpoint, brokerageApi, brokerageVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

}