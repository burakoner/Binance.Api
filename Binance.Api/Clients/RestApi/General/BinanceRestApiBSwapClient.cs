using Binance.Api.Models.RestApi.BSwap;

namespace Binance.Api.Clients.RestApi.General;

public class BinanceRestApiBSwapClient
{
    // Api
    private const string bSwapApi = "sapi";
    private const string bSwapVersion = "1";

    // BSwap
    private const string bSwapPoolsEndpoint = "bswap/pools";
    private const string bSwapPoolLiquidityEndpoint = "bswap/liquidity";
    private const string bSwapAddLiquidityEndpoint = "bswap/liquidityAdd";
    private const string bSwapRemoveLiquidityEndpoint = "bswap/liquidityRemove";
    private const string bSwapLiquidityOperationsEndpoint = "bswap/liquidityOps";
    private const string bSwapQuoteEndpoint = "bswap/quote";
    private const string bSwapSwapEndpoint = "bswap/swap";
    private const string bSwapSwapRecordsEndpoint = "bswap/swap";
    private const string bSwapPoolsConfigureEndpoint = "bswap/poolConfigure";
    private const string bSwapAddLiquidityPreviewEndpoint = "bswap/addLiquidityPreview";
    private const string bSwapRemoveLiquidityPreviewEndpoint = "bswap/removeLiquidityPreview";
    // TODO: Get Unclaimed Rewards Record
    // TODO: Claim Rewards
    // TODO: Get Claimed History

    // Internal References
    internal BinanceRestApiClient RootClient { get; }
    internal BinanceRestApiGeneralClient GeneralClient { get; }
    internal BinanceRestApiClientOptions Options { get => RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => GeneralClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await GeneralClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiBSwapClient(BinanceRestApiClient root, BinanceRestApiGeneralClient general)
    {
        RootClient = root;
        GeneralClient = general;
    }

    #region List All Swap Pools
    public async Task<RestCallResult<IEnumerable<BinanceBSwapPool>>> GetLiquidityPoolsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceBSwapPool>>(GetUrl(bSwapPoolsEndpoint, bSwapApi, bSwapVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get liquidity information of a pool
    public async Task<RestCallResult<IEnumerable<BinanceBSwapPoolLiquidity>>> GetLiquidityPoolInfoAsync(int? poolId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("poolId", poolId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceBSwapPoolLiquidity>>(GetUrl(bSwapPoolLiquidityEndpoint, bSwapApi, bSwapVersion), HttpMethod.Get, ct, parameters, true, weight: poolId == null ? 10 : 1).ConfigureAwait(false);
    }
    #endregion

    #region Add Liquidity
    public async Task<RestCallResult<BinanceBSwapOperationResult>> AddToLiquidityPoolAsync(int poolId, string asset, decimal quantity, LiquidityType? type = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                {"poolId", poolId},
                {"asset", asset},
                {"quantity", quantity.ToString(CultureInfo.InvariantCulture)}
            };
        parameters.AddOptionalParameter("type", type == null ? null : JsonConvert.SerializeObject(type.Value, new LiquidityTypeConverter(false)));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBSwapOperationResult>(GetUrl(bSwapAddLiquidityEndpoint, bSwapApi, bSwapVersion), HttpMethod.Post, ct, parameters, true, weight: 1000).ConfigureAwait(false);
    }
    #endregion

    #region Remove Liquidity
    public async Task<RestCallResult<BinanceBSwapOperationResult>> RemoveFromLiquidityPoolAsync(int poolId, string asset, LiquidityType type, decimal shareQuantity, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                {"poolId", poolId},
                {"asset", asset},
                {"type", JsonConvert.SerializeObject(type, new LiquidityTypeConverter(false))},
                {"shareAmount", shareQuantity.ToString(CultureInfo.InvariantCulture)}
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBSwapOperationResult>(GetUrl(bSwapRemoveLiquidityEndpoint, bSwapApi, bSwapVersion), HttpMethod.Post, ct, parameters, true, weight: 1000).ConfigureAwait(false);
    }
    #endregion

    #region Get Liquidity Operation Record
    public async Task<RestCallResult<IEnumerable<BinanceBSwapOperation>>> GetLiquidityPoolOperationRecordsAsync(long? operationId = null, int? poolId = null, BSwapOperation? operation = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 100);

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("operationId", operationId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("poolId", poolId);
        parameters.AddOptionalParameter("operation", operation.HasValue ? JsonConvert.SerializeObject(new BSwapOperationConverter(false)) : null);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceBSwapOperation>>(GetUrl(bSwapLiquidityOperationsEndpoint, bSwapApi, bSwapVersion), HttpMethod.Get, ct, parameters, true, weight: 3000).ConfigureAwait(false);
    }
    #endregion

    #region Request Quote
    public async Task<RestCallResult<BinanceBSwapQuote>> GetLiquidityPoolSwapQuoteAsync(string quoteAsset, string baseAsset, decimal quoteQuantity, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                {"quoteAsset", quoteAsset},
                {"baseAsset", baseAsset},
                {"quoteQty", quoteQuantity.ToString(CultureInfo.InvariantCulture)}
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBSwapQuote>(GetUrl(bSwapQuoteEndpoint, bSwapApi, bSwapVersion), HttpMethod.Get, ct, parameters, true, weight: 150).ConfigureAwait(false);
    }
    #endregion

    #region Swap
    public async Task<RestCallResult<BinanceBSwapResult>> LiquidityPoolSwapAsync(string quoteAsset, string baseAsset, decimal quoteQuantity, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                {"quoteAsset", quoteAsset},
                {"baseAsset", baseAsset},
                {"quoteQty",quoteQuantity.ToString(CultureInfo.InvariantCulture)}
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBSwapResult>(GetUrl(bSwapSwapEndpoint, bSwapApi, bSwapVersion), HttpMethod.Post, ct, parameters, true, weight: 1000).ConfigureAwait(false);
    }
    #endregion

    #region Get Swap History
    public async Task<RestCallResult<IEnumerable<BinanceBSwapRecord>>> GetLiquidityPoolSwapHistoryAsync(long? swapId = null, BSwapStatus? status = null, string quoteAsset = null, string baseAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 100);

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("swapId", swapId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("status", status.HasValue ? JsonConvert.SerializeObject(status.Value, new BSwapStatusConverter(false)) : null);
        parameters.AddOptionalParameter("baseAsset", baseAsset);
        parameters.AddOptionalParameter("quoteAsset", quoteAsset);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceBSwapRecord>>(GetUrl(bSwapSwapRecordsEndpoint, bSwapApi, bSwapVersion), HttpMethod.Get, ct, parameters, true, weight: 3000).ConfigureAwait(false);
    }
    #endregion

    #region Get Pool Configure
    public async Task<RestCallResult<IEnumerable<BinanceBSwapPoolConfig>>> GetLiquidityPoolConfigurationAsync(int poolId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "poolId", poolId }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceBSwapPoolConfig>>(GetUrl(bSwapPoolsConfigureEndpoint, bSwapApi, bSwapVersion), HttpMethod.Get, ct, parameters, signed: true, weight: 150).ConfigureAwait(false);
    }
    #endregion

    #region Add Liquidity Preview
    public async Task<RestCallResult<BinanceBSwapPreviewResult>> AddToLiquidityPoolPreviewAsync(int poolId, string asset, decimal quantity, LiquidityType type, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                {"poolId", poolId},
                {"quoteAsset", asset},
                {"type", JsonConvert.SerializeObject(type, new LiquidityTypeConverter(false))},
                {"quoteQty", quantity.ToString(CultureInfo.InvariantCulture)},
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBSwapPreviewResult>(GetUrl(bSwapAddLiquidityPreviewEndpoint, bSwapApi, bSwapVersion), HttpMethod.Get, ct, parameters, true, weight: 150).ConfigureAwait(false);
    }
    #endregion

    #region Remove Liquidity Preview
    public async Task<RestCallResult<BinanceBSwapPreviewResult>> RemoveFromLiquidityPoolPreviewAsync(int poolId, string asset, decimal quantity, LiquidityType type, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                {"poolId", poolId},
                {"quoteAsset", asset},
                {"type", JsonConvert.SerializeObject(type, new LiquidityTypeConverter(false))},
                {"shareAmount", quantity.ToString(CultureInfo.InvariantCulture)}
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBSwapPreviewResult>(GetUrl(bSwapRemoveLiquidityPreviewEndpoint, bSwapApi, bSwapVersion), HttpMethod.Get, ct, parameters, true, weight: 150).ConfigureAwait(false);
    }
    #endregion

}