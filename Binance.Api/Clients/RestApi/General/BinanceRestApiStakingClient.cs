using Binance.ApiClient.Models.RestApi.Staking;

namespace Binance.ApiClient.Clients.RestApi.General;

public class BinanceRestApiStakingClient
{
    // Api
    private const string marginApi = "sapi";
    private const string marginVersion = "1";

    // Staking
    private const string stakingProductListEndpoint = "staking/productList";
    private const string stakingPurchaseEndpoint = "staking/purchase";
    private const string stakingRedeemEndpoint = "staking/redeem";
    private const string stakingPositionEndpoint = "staking/position";
    private const string stakingHistoryEndpoint = "staking/stakingRecord";
    private const string setAutoStakingEndpoint = "staking/setAutoStaking";
    private const string stakingQuotaLeftEndpoint = "staking/personalLeftQuota";

    // Internal References
    internal BinanceRestApiClient RootClient { get; }
    internal BinanceRestApiGeneralClient GeneralClient { get; }
    internal BinanceRestApiClientOptions Options { get => RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => GeneralClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await GeneralClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiStakingClient(BinanceRestApiClient root, BinanceRestApiGeneralClient general)
    {
        RootClient = root;
        GeneralClient = general;
    }

    #region Get Staking Product List
    public async Task<RestCallResult<IEnumerable<BinanceStakingProduct>>> GetStakingProductsAsync(StakingProductType product, string asset = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
            {
                { "product", EnumConverter.GetString(product) }
            };
        parameters.AddOptionalParameter("asset", asset);
        parameters.AddOptionalParameter("current", page);
        parameters.AddOptionalParameter("size", limit);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceStakingProduct>>(GetUrl(stakingProductListEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Purchase Staking Product
    public async Task<RestCallResult<BinanceStakingPositionResult>> PurchaseStakingProductAsync(StakingProductType product, string productId, decimal quantity, bool? renewable = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
            {
                { "product", EnumConverter.GetString(product) },
                { "productId", productId },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
            };
        parameters.AddOptionalParameter("renewable", renewable);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceStakingPositionResult>(GetUrl(stakingPurchaseEndpoint, marginApi, marginVersion), HttpMethod.Post, ct, parameters, true, weight: 1).ConfigureAwait(false);
    }
    #endregion

    #region Redeem Staking Product
    public async Task<RestCallResult<BinanceStakingResult>> RedeemStakingProductAsync(StakingProductType product, string productId, string positionId = null, decimal? quantity = null, bool? renewable = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
            {
                { "product", EnumConverter.GetString(product) },
                { "productId", productId },
            };
        parameters.AddOptionalParameter("positionId", positionId);
        parameters.AddOptionalParameter("amount", quantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("renewable", renewable);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceStakingResult>(GetUrl(stakingRedeemEndpoint, marginApi, marginVersion), HttpMethod.Post, ct, parameters, true, weight: 1).ConfigureAwait(false);
    }
    #endregion

    #region Get Staking Product Position
    public async Task<RestCallResult<IEnumerable<BinanceStakingPosition>>> GetStakingPositionsAsync(StakingProductType product, string productId = null, int? page = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
            {
                { "product", EnumConverter.GetString(product) }
            };
        parameters.AddOptionalParameter("productId", productId);
        parameters.AddOptionalParameter("current", page);
        parameters.AddOptionalParameter("size", limit);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceStakingPosition>>(GetUrl(stakingPositionEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 1).ConfigureAwait(false);
    }
    #endregion

    #region Get Staking History
    public async Task<RestCallResult<IEnumerable<BinanceStakingHistory>>> GetStakingHistoryAsync(StakingProductType product, StakingTransactionType transactionType, string asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
            {
                { "product", EnumConverter.GetString(product) },
                { "txnType", EnumConverter.GetString(transactionType) }
            };
        parameters.AddOptionalParameter("asset", asset);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("current", page);
        parameters.AddOptionalParameter("size", limit);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceStakingHistory>>(GetUrl(stakingHistoryEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 1).ConfigureAwait(false);
    }
    #endregion

    #region Set Auto Staking
    public async Task<RestCallResult<BinanceStakingResult>> SetAutoStakingAsync(StakingProductType product, string positionId, bool renewable, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
            {
                { "product", EnumConverter.GetString(product) },
                { "positionId", positionId },
                { "renewable", renewable },
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceStakingResult>(GetUrl(setAutoStakingEndpoint, marginApi, marginVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Get Personal Left Quota of Staking Product
    public async Task<RestCallResult<BinanceStakingPersonalQuota>> GetStakingPersonalQuotaAsync(StakingProductType product, string productId, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
            {
                { "product", EnumConverter.GetString(product) },
                { "productId", productId }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceStakingPersonalQuota>(GetUrl(stakingQuotaLeftEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

}