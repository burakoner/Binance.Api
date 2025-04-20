using Binance.Api.Models.RestApi.Account;
using Binance.Api.Models.RestApi.SubAccount;

namespace Binance.Api.Clients.RestApi.General;

public class BinanceRestApiSubAccountClient
{
    // Sub-Account
    private const string subAccountCreateVirtualEndpoint = "sub-account/virtualSubAccount";
    private const string subAccountListEndpoint = "sub-account/list";
    private const string subAccountTransferHistoryEndpoint = "sub-account/sub/transfer/history";
    // TODO: Query Sub-Account Futures Asset Transfer History (For Master Account)
    // TODO: Sub-Account Futures Asset Transfer (For Master Account)
    private const string subAccountAssetsEndpoint = "sub-account/assets";
    private const string subAccountSpotSummaryEndpoint = "sub-account/spotSummary";
    private const string subAccountDepositAddressEndpoint = "capital/deposit/subAddress";
    private const string subAccountDepositHistoryEndpoint = "capital/deposit/subHisrec";
    private const string subAccountStatusEndpoint = "sub-account/status";
    private const string subAccountEnableMarginEndpoint = "sub-account/margin/enable";
    private const string subAccountMarginDetailsEndpoint = "sub-account/margin/account";
    private const string subAccountMarginSummaryEndpoint = "sub-account/margin/accountSummary";
    private const string subAccountEnableFuturesEndpoint = "sub-account/futures/enable";
    private const string subAccountFuturesDetailsEndpoint = "sub-account/futures/account";
    private const string subAccountFuturesSummaryEndpoint = "sub-account/futures/accountSummary";
    private const string subAccountFuturesPositionRiskEndpoint = "sub-account/futures/positionRisk";
    private const string subAccountTransferFuturesSpotEndpoint = "sub-account/futures/transfer";
    private const string subAccountTransferMarginSpotEndpoint = "sub-account/margin/transfer";
    private const string subAccountTransferToSubEndpoint = "sub-account/transfer/subToSub";
    private const string subAccountTransferToMasterEndpoint = "sub-account/transfer/subToMaster";
    private const string subAccountTransferHistorySubAccountEndpoint = "sub-account/transfer/subUserHistory";
    private const string transferSubAccountEndpoint = "sub-account/universalTransfer";
    private const string queryUniversalTransferHistoryEndpoint = "sub-account/universalTransfer";
    private const string subAccountFuturesDetailsV2Endpoint = "sub-account/futures/account";
    private const string subAccountFuturesSummaryV2Endpoint = "sub-account/futures/accountSummary";
    private const string subAccountFuturesPositionRiskV2Endpoint = "sub-account/futures/positionRisk";
    private const string subAccountEnableBlvtEndpoint = "sub-account/blvt/enable";

    // IP Restrictions
    private const string subAccountIpRestrictionEndpoint = "sub-account/subAccountApi/ipRestriction";
    private const string subAccountIpRestrictionDeleteEndpoint = "sub-account/subAccountApi/ipRestriction/ipList";
    private const string subAccountIpRestrictionUpdateEndpoint = "sub-account/subAccountApi/ipRestriction";

    // Managed Sub-Account
    // TODO: Deposit Assets Into The Managed Sub-Account（For Investor Master Account）
    // TODO: Query Managed Sub-Account Asset Details（For Investor Master Account）
    // TODO: Withdrawl Assets From The Managed Sub-Account（For Investor Master Account）
    // TODO: Query Managed Sub-Account Snapshot（For Investor Master Account）
    // TODO: Query Managed Sub Account Transfer Log (Investor) (USER_DATA)
    // TODO: Query Managed Sub-Account Futures Asset Details（For Investor Master Account）(USER_DATA)
    // TODO: Query Managed Sub-Account Margin Asset Details (For Investor Master Account) (USER_DATA)
    // TODO: Query Managed Sub Account Transfer Log(Trading Team) (USER_DATA)

    // Internal References
    internal BinanceRestApiGeneralClient MainClient { get; }
    internal BinanceRestApiClientOptions ClientOptions { get => MainClient.RootClient.ClientOptions; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal BinanceRestApiSubAccountClient(BinanceRestApiGeneralClient main)
    {
        MainClient = main;
    }

    #region Create a Virtual Sub-Account (For Master Account)
    public async Task<RestCallResult<BinanceSubAccountEmail>> CreateVirtualSubAccountAsync(string subAccountString, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "subAccountString", subAccountString }
            };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceSubAccountEmail>(GetUrl(subAccountCreateVirtualEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Query Sub-Account List(For Master Account)
    public async Task<RestCallResult<IEnumerable<BinanceSubAccount>>> GetSubAccountsAsync(string email = null, int? page = null, int? limit = null, int? receiveWindow = null, bool? isFreeze = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("email", email);
        parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("isFreeze", isFreeze);

        var result = await SendRequestInternal<BinanceSubAccountWrapper>(GetUrl(subAccountListEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        return result ? result.As(result.Data.SubAccounts) : result.As<IEnumerable<BinanceSubAccount>>(default);
    }
    #endregion

    #region Query Sub-Account Spot Asset Transfer History (For Master Account)
    public async Task<RestCallResult<IEnumerable<BinanceSubAccountTransfer>>> GetSubAccountTransferHistoryForMasterAsync(string fromEmail = null, string toEmail = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("fromEmail", fromEmail);
        parameters.AddOptionalParameter("toEmail", toEmail);
        parameters.AddOptionalParameter("startTime", startTime?.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime?.ConvertToMilliseconds());
        parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<IEnumerable<BinanceSubAccountTransfer>>(GetUrl(subAccountTransferHistoryEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        return result;
    }

    #endregion

    #region Query Sub-Account Assets (For Master Account)
    public async Task<RestCallResult<IEnumerable<BinanceBalance>>> GetSubAccountAssetsAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));

        var parameters = new Dictionary<string, object>
            {
                { "email", email }
            };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceSubAccountAsset>(GetUrl(subAccountAssetsEndpoint, "sapi", "3"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        if (!result.Success)
            return result.As<IEnumerable<BinanceBalance>>(default);

        if (!result.Data.Success)
            return result.AsError<IEnumerable<BinanceBalance>>(new ServerError(result.Data!.Message));

        return result.As(result.Data.Balances);
    }
    #endregion

    #region Query Sub-Account Spot Assets Summary (For Master Account)
    public async Task<RestCallResult<BinanceSubAccountSpotAssetsSummary>> GetSubAccountBtcValuesAsync(string email = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("email", email);
        parameters.AddOptionalParameter("page", page);
        parameters.AddOptionalParameter("limit", limit);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceSubAccountSpotAssetsSummary>(GetUrl(subAccountSpotSummaryEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get Sub-Account Deposit Address (For Master Account)
    public async Task<RestCallResult<BinanceSubAccountDepositAddress>> GetSubAccountDepositAddressAsync(string email, string asset, string network = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));
        asset.ValidateNotNull(nameof(asset));

        var parameters = new Dictionary<string, object>
            {
                { "email", email },
                { "coin", asset }
            };

        parameters.AddOptionalParameter("network", network);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceSubAccountDepositAddress>(GetUrl(subAccountDepositAddressEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get Sub-Account Deposit History (For Master Account)
    public async Task<RestCallResult<IEnumerable<BinanceSubAccountDeposit>>> GetSubAccountDepositHistoryAsync(string email, string asset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? offset = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));

        var parameters = new Dictionary<string, object>
        {
            { "email", email }
        };
        parameters.AddOptionalParameter("coin", asset);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("offset", offset?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceSubAccountDeposit>>(GetUrl(subAccountDepositHistoryEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get Sub-Account's Status on Margin/Futures(For Master Account)
    public async Task<RestCallResult<IEnumerable<BinanceSubAccountStatus>>> GetSubAccountStatusAsync(string email = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("email", email);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceSubAccountStatus>>(GetUrl(subAccountStatusEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }

    #endregion

    #region Enable Margin for Sub-Account (For Master Account)
    public async Task<RestCallResult<BinanceSubAccountMarginEnabled>> EnableMarginForSubAccountAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));

        var parameters = new Dictionary<string, object>
        {
            { "email", email }
        };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceSubAccountMarginEnabled>(GetUrl(subAccountEnableMarginEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get Detail on Sub-Account's Margin Account (For Master Account)
    public async Task<RestCallResult<BinanceSubAccountMarginDetails>> GetSubAccountMarginDetailsAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));

        var parameters = new Dictionary<string, object>
            {
                { "email", email }
            };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceSubAccountMarginDetails>(GetUrl(subAccountMarginDetailsEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Get Summary of Sub-Account's Margin Account (For Master Account)
    public async Task<RestCallResult<BinanceSubAccountMarginSummary>> GetSubAccountsMarginSummaryAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceSubAccountMarginSummary>(GetUrl(subAccountMarginSummaryEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Enable Futures for Sub-Account (For Master Account) 
    public async Task<RestCallResult<BinanceSubAccountFuturesEnabled>> EnableFuturesForSubAccountAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));
        var parameters = new Dictionary<string, object>
            {
                { "email", email }
            };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceSubAccountFuturesEnabled>(GetUrl(subAccountEnableFuturesEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get Detail on Sub-Account's Futures Account (For Master Account) 
    public async Task<RestCallResult<BinanceSubAccountFuturesDetails>> GetSubAccountFuturesDetailsAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));

        var parameters = new Dictionary<string, object>
            {
                { "email", email }
            };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceSubAccountFuturesDetails>(GetUrl(subAccountFuturesDetailsEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Get Summary of Sub-Account's Futures Account (For Master Account)
    public async Task<RestCallResult<BinanceSubAccountFuturesSummary>> GetSubAccountsFuturesSummaryAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceSubAccountFuturesSummary>(GetUrl(subAccountFuturesSummaryEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get Futures Postion-Risk of Sub-Account (For Master Account)
    public async Task<RestCallResult<IEnumerable<BinanceSubAccountFuturesPositionRisk>>> GetSubAccountsFuturesPositionRiskAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "email", email }
            };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceSubAccountFuturesPositionRisk>>(GetUrl(subAccountFuturesPositionRiskEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Futures Transfer for Sub-Account (For Master Account)
    public async Task<RestCallResult<BinanceSubAccountTransaction>> TransferSubAccountFuturesAsync(string email, string asset, decimal quantity, SubAccountFuturesTransferType type, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));
        asset.ValidateNotNull(nameof(asset));

        var parameters = new Dictionary<string, object>
            {
                { "email", email },
                { "asset", asset },
                { "type", JsonConvert.SerializeObject(type, new SubAccountFuturesTransferTypeConverter(false)) },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceSubAccountTransaction>(GetUrl(subAccountTransferFuturesSpotEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Margin Transfer for Sub-Account (For Master Account)
    public async Task<RestCallResult<BinanceSubAccountTransaction>> TransferSubAccountMarginAsync(string email, string asset, decimal quantity, SubAccountMarginTransferType type, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));
        asset.ValidateNotNull(nameof(asset));

        var parameters = new Dictionary<string, object>
            {
                { "email", email },
                { "asset", asset },
                { "type", JsonConvert.SerializeObject(type, new SubAccountMarginTransferTypeConverter(false)) },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceSubAccountTransaction>(GetUrl(subAccountTransferMarginSpotEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Transfer to Sub-Account of Same Master (For Sub-Account)
    public async Task<RestCallResult<BinanceSubAccountTransaction>> TransferSubAccountToSubAccountAsync(string email, string asset, decimal quantity, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));
        asset.ValidateNotNull(nameof(asset));

        var parameters = new Dictionary<string, object>
            {
                { "toEmail", email },
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceSubAccountTransaction>(GetUrl(subAccountTransferToSubEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Transfer to Master (For Sub-Account)
    public async Task<RestCallResult<BinanceSubAccountTransaction>> TransferSubAccountToMasterAsync(string asset, decimal quantity, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));

        var parameters = new Dictionary<string, object>
            {
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceSubAccountTransaction>(GetUrl(subAccountTransferToMasterEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Sub-Account Transfer History (For Sub-Account)
    public async Task<RestCallResult<IEnumerable<BinanceSubAccountTransferSubAccount>>> GetSubAccountTransferHistoryForSubAccountAsync(string asset = null, SubAccountTransferSubAccountType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("asset", asset);
        parameters.AddOptionalParameter("type", type == null ? null : JsonConvert.SerializeObject(type, new SubAccountTransferSubAccountTypeConverter(false)));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceSubAccountTransferSubAccount>>(GetUrl(subAccountTransferHistorySubAccountEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Universal Transfer (For Master Account)
    public async Task<RestCallResult<BinanceTransaction>> TransferSubAccountAsync(TransferAccountType fromAccountType, TransferAccountType toAccountType, string asset, decimal quantity, string fromEmail = null, string toEmail = null, string symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(fromEmail) && string.IsNullOrEmpty(toEmail))
            throw new ArgumentException("fromEmail and/or toEmail should be provided");
        asset.ValidateNotNull(nameof(asset));

        var parameters = new Dictionary<string, object>
            {
                { "fromAccountType", JsonConvert.SerializeObject(fromAccountType, new TransferAccountTypeConverter(false)) },
                { "toAccountType", JsonConvert.SerializeObject(toAccountType, new TransferAccountTypeConverter(false)) },
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("fromEmail", fromEmail);
        parameters.AddOptionalParameter("toEmail", toEmail);

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceTransaction>(GetUrl(transferSubAccountEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Query Universal Transfer History (For Master Account)
    public async Task<RestCallResult<IEnumerable<BinanceSubAccountUniversalTransferTransaction>>> GetUniversalTransferHistoryAsync(string fromEmail = null, string toEmail = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("fromEmail", fromEmail);
        parameters.AddOptionalParameter("toEmail", toEmail);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceSubAccountUniversalTransfersList>(GetUrl(queryUniversalTransferHistoryEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        return result ? result.As(result.Data.Transactions) : result.As<IEnumerable<BinanceSubAccountUniversalTransferTransaction>>(default);
    }
    #endregion

    #region Get Detail on Sub-Account's Futures Account V2 (For Master Account)
    public async Task<RestCallResult<BinanceSubAccountFuturesDetailsV2>> GetSubAccountFuturesDetailsAsync(FuturesAccountType futuresAccountType, string email, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));

        var parameters = new Dictionary<string, object>
            {
                { "email", email },
                { "futuresType", JsonConvert.SerializeObject(futuresAccountType, new FuturesAccountTypeConverter(false)) }
            };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceSubAccountFuturesDetailsV2>(GetUrl(subAccountFuturesDetailsV2Endpoint, "sapi", "2"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1).ConfigureAwait(false);
    }
    #endregion

    #region Get Summary of Sub-Account's Futures Account V2 (For Master Account)
    public async Task<RestCallResult<BinanceSubAccountFuturesSummaryV2>> GetSubAccountsFuturesSummaryAsync(FuturesAccountType futuresAccountType, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "futuresType", JsonConvert.SerializeObject(futuresAccountType, new FuturesAccountTypeConverter(false)) }
        };
        parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceSubAccountFuturesSummaryV2>(GetUrl(subAccountFuturesSummaryV2Endpoint, "sapi", "2"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get Futures Position-Risk of Sub-Account V2 (For Master Account)
    public async Task<RestCallResult<BinanceSubAccountFuturesPositionRiskV2>> GetSubAccountsFuturesPositionRiskAsync(FuturesAccountType futuresAccountType, string email, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "email", email },
                { "futuresType", JsonConvert.SerializeObject(futuresAccountType, new FuturesAccountTypeConverter(false)) }
            };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceSubAccountFuturesPositionRiskV2>(GetUrl(subAccountFuturesPositionRiskV2Endpoint, "sapi", "2"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Enable Leverage Token for Sub-Account (For Master Account)
    public async Task<RestCallResult<BinanceSubAccountBlvt>> EnableBlvtForSubAccountAsync(string email, bool enable, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "email", email },
                { "enableBlvt", enable }
            };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceSubAccountBlvt>(GetUrl(subAccountEnableBlvtEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get IP Restriction for a Sub-Account API Key (For Master Account)
    public async Task<RestCallResult<BinanceIpRestriction>> GetSubAccountIpRestrictionAsync(string email, string subAccountApiKey, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "email", email },
                { "subAccountApiKey", subAccountApiKey }
            };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceIpRestriction>(GetUrl(subAccountIpRestrictionEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 3000).ConfigureAwait(false);
    }
    #endregion

    #region Delete IP List For a Sub-Account API Key (For Master Account)
    public async Task<RestCallResult<BinanceIpRestriction>> RemoveSubAccountIpRestrictionAsync(string email, string subAccountApiKey, IEnumerable<string> ipAddresses, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "email", email },
                { "subAccountApiKey", subAccountApiKey },
                { "ipAddress", string.Join(",", ipAddresses) },
            };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceIpRestriction>(GetUrl(subAccountIpRestrictionDeleteEndpoint, "sapi", "1"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 3000).ConfigureAwait(false);
    }
    #endregion

    #region Update IP Restriction for Sub-Account API key (For Master Account)
    public async Task<RestCallResult<BinanceIpRestrictionUpdate>> UpdateSubAccountIpRestrictionAsync(string email, string subAccountApiKey, IpRestrictionStatus status, IEnumerable<string> ipAddresses, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "email", email },
                { "subAccountApiKey", subAccountApiKey },
                { "status", JsonConvert.SerializeObject(status, new IpRestrictionStatusConverter(false)) },
                { "ipAddress", string.Join(",", ipAddresses) }
            };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceIpRestrictionUpdate>(GetUrl(subAccountIpRestrictionUpdateEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 3000).ConfigureAwait(false);
    }
    #endregion

}