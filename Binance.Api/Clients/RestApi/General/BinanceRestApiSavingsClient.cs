using Binance.Api.Models.RestApi.Savings;

namespace Binance.Api.Clients.RestApi.General;

public class BinanceRestApiSavingsClient
{
    // Savings
    private const string flexibleProductListEndpoint = "lending/daily/product/list";
    private const string leftDailyPurchaseQuotaEndpoint = "lending/daily/userLeftQuota";
    private const string purchaseFlexibleProductEndpoint = "lending/daily/purchase";
    private const string leftDailyRedemptionQuotaEndpoint = "lending/daily/userRedemptionQuota";
    private const string redeemFlexibleProductEndpoint = "lending/daily/redeem";
    private const string flexiblePositionEndpoint = "lending/daily/token/position";
    private const string fixedAndCustomizedFixedProjectListEndpoint = "lending/project/list";
    private const string purchaseCustomizedFixedProjectEndpoint = "lending/customizedFixed/purchase";
    private const string fixedAndCustomizedProjectPositionEndpoint = "lending/project/position/list";
    private const string lendingAccountEndpoint = "lending/union/account";
    private const string purchaseRecordEndpoint = "lending/union/purchaseRecord";
    private const string redemptionRecordEndpoint = "lending/union/redemptionRecord";
    private const string lendingInterestHistoryEndpoint = "lending/union/interestHistory";
    private const string positionChangedEndpoint = "lending/positionChanged";

    // Internal References
    internal BinanceRestApiGeneralClient MainClient { get; }
    internal BinanceRestApiClientOptions ClientOptions { get => MainClient.RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal BinanceRestApiSavingsClient(BinanceRestApiGeneralClient main)
    {
        MainClient = main;
    }

    #region Get Flexible Product List
    public async Task<RestCallResult<IEnumerable<BinanceSavingsProduct>>> GetFlexibleProductListAsync(ProductStatus? status = null, bool? featured = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("status", status == null ? null : JsonConvert.SerializeObject(status, new ProductStatusConverter(false)));
        parameters.AddOptionalParameter("featured", featured == true ? "TRUE" : "ALL");
        parameters.AddOptionalParameter("current", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("size", pageSize?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceSavingsProduct>>(GetUrl(flexibleProductListEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get Left Daily Purchase Quota of Flexible Product
    public async Task<RestCallResult<BinancePurchaseQuotaLeft>> GetLeftDailyPurchaseQuotaOfFlexableProductAsync(string productId, long? receiveWindow = null, CancellationToken ct = default)
    {
        productId.ValidateNotNull(nameof(productId));

        var parameters = new Dictionary<string, object>
            {
                { "productId", productId }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinancePurchaseQuotaLeft>(GetUrl(leftDailyPurchaseQuotaEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Purchase Flexible Product
    public async Task<RestCallResult<BinanceLendingPurchaseResult>> PurchaseFlexibleProductAsync(string productId, decimal quantity, long? receiveWindow = null, CancellationToken ct = default)
    {
        productId.ValidateNotNull(nameof(productId));

        var parameters = new Dictionary<string, object>
            {
                { "productId", productId },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceLendingPurchaseResult>(GetUrl(purchaseFlexibleProductEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get Left Daily Redemption Quota of Flexible Product
    public async Task<RestCallResult<BinanceRedemptionQuotaLeft>> GetLeftDailyRedemptionQuotaOfFlexibleProductAsync(string productId, RedeemType type, long? receiveWindow = null, CancellationToken ct = default)
    {
        productId.ValidateNotNull(nameof(productId));

        var parameters = new Dictionary<string, object>
            {
                { "productId", productId },
                { "type",  JsonConvert.SerializeObject(type, new RedeemTypeConverter(false)) }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceRedemptionQuotaLeft>(GetUrl(leftDailyRedemptionQuotaEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Redeem Flexible Product
    public async Task<RestCallResult<object>> RedeemFlexibleProductAsync(string productId, decimal quantity, RedeemType type, long? receiveWindow = null, CancellationToken ct = default)
    {
        productId.ValidateNotNull(nameof(productId));

        var parameters = new Dictionary<string, object>
            {
                { "productId", productId },
                { "type", JsonConvert.SerializeObject(type, new RedeemTypeConverter(false)) },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<object>(GetUrl(redeemFlexibleProductEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get Flexible Product Position
    public async Task<RestCallResult<IEnumerable<BinanceFlexibleProductPosition>>> GetFlexibleProductPositionAsync(string asset = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("asset", asset);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFlexibleProductPosition>>(GetUrl(flexiblePositionEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get Fixed And Customized Fixed Project List
    public async Task<RestCallResult<IEnumerable<BinanceProject>>> GetFixedAndCustomizedFixedProjectListAsync(ProjectType type, string asset = null, ProductStatus? status = null, bool? sortAscending = null, string sortBy = null, int? currentPage = null, int? size = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "type", JsonConvert.SerializeObject(type, new ProjectTypeConverter(false)) }
            };
        parameters.AddOptionalParameter("asset", asset);
        parameters.AddOptionalParameter("status", status == null ? null : JsonConvert.SerializeObject(status, new ProductStatusConverter(false)));
        parameters.AddOptionalParameter("isSortAsc", sortAscending.ToString().ToLower());
        parameters.AddOptionalParameter("sortBy", sortBy);
        parameters.AddOptionalParameter("current", currentPage);
        parameters.AddOptionalParameter("size", size);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceProject>>(GetUrl(fixedAndCustomizedFixedProjectListEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Purchase Customized Fixed Project
    public async Task<RestCallResult<BinanceLendingPurchaseResult>> PurchaseCustomizedFixedProjectAsync(string projectId, int lot, long? receiveWindow = null, CancellationToken ct = default)
    {
        projectId.ValidateNotNull(nameof(projectId));

        var parameters = new Dictionary<string, object>
            {
                { "projectId", projectId },
                { "lot", lot.ToString(CultureInfo.InvariantCulture) }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceLendingPurchaseResult>(GetUrl(purchaseCustomizedFixedProjectEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get Customized Fixed Project Position
    public async Task<RestCallResult<IEnumerable<BinanceCustomizedFixedProjectPosition>>> GetCustomizedFixedProjectPositionsAsync(string asset, string projectId = null, ProjectStatus? status = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));

        var parameters = new Dictionary<string, object>
            {
                { "asset", asset }
            };
        parameters.AddOptionalParameter("projectId", projectId);
        parameters.AddOptionalParameter("status", status == null ? null : JsonConvert.SerializeObject(status, new ProjectStatusConverter(false)));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceCustomizedFixedProjectPosition>>(GetUrl(fixedAndCustomizedProjectPositionEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Lending Account
    public async Task<RestCallResult<BinanceLendingAccount>> GetLendingAccountAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceLendingAccount>(GetUrl(lendingAccountEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get Purchase Records
    public async Task<RestCallResult<IEnumerable<BinancePurchaseRecord>>> GetPurchaseRecordsAsync(LendingType lendingType, string asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = 1, int? limit = 10, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "lendingType", JsonConvert.SerializeObject(lendingType, new LendingTypeConverter(false)) }
            };
        parameters.AddOptionalParameter("asset", asset);
        parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("current", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinancePurchaseRecord>>(GetUrl(purchaseRecordEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get Redemption Record
    public async Task<RestCallResult<IEnumerable<BinanceRedemptionRecord>>> GetRedemptionRecordsAsync(LendingType lendingType, string asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = 1, int? limit = 10, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "lendingType", JsonConvert.SerializeObject(lendingType, new LendingTypeConverter(false)) }
            };
        parameters.AddOptionalParameter("asset", asset);
        parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("current", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceRedemptionRecord>>(GetUrl(redemptionRecordEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get Interest History
    public async Task<RestCallResult<IEnumerable<BinanceLendingInterestHistory>>> GetLendingInterestHistoryAsync(LendingType lendingType, string asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = 1, int? limit = 10, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "lendingType", JsonConvert.SerializeObject(lendingType, new LendingTypeConverter(false)) }
            };
        parameters.AddOptionalParameter("asset", asset);
        parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("current", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceLendingInterestHistory>>(GetUrl(lendingInterestHistoryEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region ChangeToDailyPosition
    public async Task<RestCallResult<BinanceLendingChangeToDailyResult>> ChangeToDailyPositionAsync(string projectId, int lot, long? positionId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        projectId.ValidateNotNull(nameof(projectId));

        var parameters = new Dictionary<string, object>
            {
                { "projectId", projectId },
                { "lot", lot.ToString(CultureInfo.InvariantCulture) }
            };
        parameters.AddOptionalParameter("positionId", positionId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceLendingChangeToDailyResult>(GetUrl(positionChangedEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

}