using Binance.Api.Models.RestApi.Account;

namespace Binance.Api.Clients.RestApi.Spot;

public class BinanceRestApiSpotAccountClient
{
    // Api
    protected const string marginApi = "sapi";
    protected const string marginVersion = "1";

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

    // Convert Transfer
    private const string convertTransferEndpoint = "asset/convert-transfer";
    private const string convertTransferHistoryEndpoint = "asset/convert-transfer/queryByPage";

    // Api Keys
    private const string apiRestrictionsEndpoint = "account/apiRestrictions";

    // TODO: Get Cloud-Mining payment and refund history (USER_DATA)
    // TODO: Query auto-converting stable coins (USER_DATA)
    // TODO: Switch on/off BUSD and stable coins conversion (USER_DATA)

    // Internal References
    internal BinanceRestApiSpotClient MainClient { get; }
    internal BinanceRestApiClientOptions Options { get => MainClient.RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal BinanceRestApiSpotAccountClient(BinanceRestApiSpotClient main)
    {
        MainClient = main;
    }

    #region All Coins' Information
    public async Task<RestCallResult<IEnumerable<BinanceUserAsset>>> GetUserAssetsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));


        return await SendRequestInternal<IEnumerable<BinanceUserAsset>>(GetUrl(userCoinsEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Daily Account Snapshots
    public async Task<RestCallResult<IEnumerable<BinanceSpotAccountSnapshot>>> GetDailySpotAccountSnapshotAsync(
        DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null,
        CancellationToken ct = default) =>
        await GetDailyAccountSnapshot<IEnumerable<BinanceSpotAccountSnapshot>>(AccountType.Spot, startTime, endTime, limit, receiveWindow, ct).ConfigureAwait(false);

    public async Task<RestCallResult<IEnumerable<BinanceMarginAccountSnapshot>>> GetDailyMarginAccountSnapshotAsync(
        DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null,
        CancellationToken ct = default) =>
        await GetDailyAccountSnapshot<IEnumerable<BinanceMarginAccountSnapshot>>(AccountType.Margin, startTime, endTime, limit, receiveWindow, ct).ConfigureAwait(false);

    public async Task<RestCallResult<IEnumerable<BinanceFuturesAccountSnapshot>>> GetDailyFutureAccountSnapshotAsync(
        DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null,
        CancellationToken ct = default) =>
        await GetDailyAccountSnapshot<IEnumerable<BinanceFuturesAccountSnapshot>>(AccountType.Futures, startTime, endTime, limit, receiveWindow, ct).ConfigureAwait(false);


    private async Task<RestCallResult<T>> GetDailyAccountSnapshot<T>(AccountType accountType, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null,
        CancellationToken ct = default) where T : class
    {
        limit?.ValidateIntBetween(nameof(limit), 5, 30);

        var parameters = new Dictionary<string, object>
            {
                { "type", EnumConverter.GetString(accountType) }
            };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceSnapshotWrapper<T>>(GetUrl(accountSnapshotEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 2400).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<object>(GetUrl(disableFastWithdrawSwitchEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Enable Fast Withdraw Switch
    public async Task<RestCallResult<object>> EnableFastWithdrawSwitchAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<object>(GetUrl(enableFastWithdrawSwitchEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceWithdrawalPlaced>(GetUrl(withdrawEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, queryParameters: parameters).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit);
        parameters.AddOptionalParameter("offset", offset);

        var result = await SendRequestInternal<IEnumerable<BinanceWithdrawal>>(GetUrl(withdrawHistoryEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceDeposit>>(
                GetUrl(depositHistoryEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters)
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceDepositAddress>(GetUrl(depositAddressEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Account Status
    public async Task<RestCallResult<BinanceAccountStatus>> GetAccountStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceAccountStatus>(GetUrl(accountStatusEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        return result;
    }
    #endregion

    #region Account API Trading Status
    public async Task<RestCallResult<BinanceTradingStatus>> GetTradingStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceResult<BinanceTradingStatus>>(GetUrl(tradingStatusEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        if (!result)
            return result.As<BinanceTradingStatus>(default);

        return !string.IsNullOrEmpty(result.Data.Message) ? result.AsError<BinanceTradingStatus>(new ServerError(result.Data.Message!)) : result.As(result.Data.Data);
    }
    #endregion

    #region DustLog
    public async Task<RestCallResult<BinanceDustLogList>> GetDustLogAsync(DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        var result = await SendRequestInternal<BinanceDustLogList>(GetUrl(dustLogEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        return result;
    }
    #endregion

    #region Get Assets That Can Be Converted Into BNB
    public async Task<RestCallResult<BinanceElligableDusts>> GetAssetsForDustTransferAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceElligableDusts>(GetUrl(dustElligableEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 10).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceDustTransferResult>(GetUrl(dustTransferEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 10).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceDividendRecord>>(GetUrl(dividendRecordsEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Asset Detail
    public async Task<RestCallResult<Dictionary<string, BinanceAssetDetails>>> GetAssetDetailsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<Dictionary<string, BinanceAssetDetails>>(GetUrl(assetDetailsEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        return result;
    }
    #endregion

    #region Trade Fee
    public async Task<RestCallResult<IEnumerable<BinanceTradeFee>>> GetTradeFeeAsync(string symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol?.ValidateBinanceSymbol();
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<IEnumerable<BinanceTradeFee>>(GetUrl(tradeFeeEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceTransaction>(GetUrl(universalTransferEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceTransfer>>(GetUrl(universalTransferEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Funding Wallet
    public async Task<RestCallResult<IEnumerable<BinanceFundingAsset>>> GetFundingWalletAsync(string asset = null, bool? needBtcValuation = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("asset", asset);
        parameters.AddOptionalParameter("needBtcValuation", needBtcValuation?.ToString());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFundingAsset>>(GetUrl(fundingWalletEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region User Asset
    public async Task<RestCallResult<IEnumerable<BinanceUserBalance>>> GetBalancesAsync(string asset = null, bool? needBtcValuation = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("asset", asset);
        parameters.AddOptionalParameter("needBtcValuation", needBtcValuation);
        return await SendRequestInternal<IEnumerable<BinanceUserBalance>>(GetUrl(balancesEndpoint, "sapi", "3"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceConvertTransferResult>(GetUrl(convertTransferEndpoint, marginApi, marginVersion), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceConvertTransferRecord>>(GetUrl(convertTransferHistoryEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
    }
    #endregion

    #region Get API Key Permission
    public async Task<RestCallResult<BinanceAPIKeyPermissions>> GetAPIKeyPermissionsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceAPIKeyPermissions>(GetUrl(apiRestrictionsEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion
}