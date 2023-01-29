using Binance.Api.Models.RestApi.Futures;

namespace Binance.Api.Clients.RestApi.UsdtFutures;

public class BinanceRestApiUsdtFuturesAccountClient
{
    // Api
    private const string api = "fapi";
    private const string signedVersion = "1";

    // Account
    private const string positionModeSideSetEndpoint = "positionSide/dual";
    private const string positionModeSideGetEndpoint = "positionSide/dual";
    private const string futuresAccountMultiAssetsModeSetEndpoint = "multiAssetsMargin";
    private const string futuresAccountMultiAssetsModeGetEndpoint = "multiAssetsMargin";
    private const string futuresAccountBalanceEndpoint = "balance";
    private const string accountInfoEndpoint = "account";
    private const string changeInitialLeverageEndpoint = "leverage";
    private const string changeMarginTypeEndpoint = "marginType";
    private const string positionMarginEndpoint = "positionMargin";
    private const string positionMarginChangeHistoryEndpoint = "positionMargin/history";
    private const string positionInformationEndpoint = "positionRisk";
    private const string incomeHistoryEndpoint = "income";
    private const string leverageBracketEndpoint = "leverageBracket";
    private const string adlQuantileEndpoint = "adlQuantile";
    private const string tradingStatusEndpoint = "apiTradingStatus";
    private const string futuresAccountUserCommissionRateEndpoint = "commissionRate";
    private const string downloadIdEndpoint = "income/asyn";
    private const string downloadLinkEndpoint = "income/asyn/id";

    // Internal References
    internal BinanceRestApiClient RootClient { get; }
    internal BinanceRestApiUsdtFuturesClient UsdtFuturesClient { get; }
    internal BinanceRestApiClientOptions Options { get => RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => UsdtFuturesClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await UsdtFuturesClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiUsdtFuturesAccountClient(BinanceRestApiClient root, BinanceRestApiUsdtFuturesClient usdt)
    {
        RootClient = root;
        UsdtFuturesClient = usdt;
    }

    #region Change Position Mode
    public async Task<RestCallResult<BinanceResult>> ModifyPositionModeAsync(bool dualPositionSide, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "dualSidePosition", dualPositionSide.ToString().ToLower() }
        };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceResult>(GetUrl(positionModeSideSetEndpoint, api, signedVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Get Current Position Mode
    public async Task<RestCallResult<BinanceFuturesPositionMode>> GetPositionModeAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesPositionMode>(GetUrl(positionModeSideGetEndpoint, api, signedVersion), HttpMethod.Get, ct, parameters, true, weight: 30).ConfigureAwait(false);
    }
    #endregion

    #region Change Multi-Assets Mode
    public async Task<RestCallResult<BinanceResult>> SetMultiAssetsModeAsync(bool enabled, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "multiAssetsMargin", enabled.ToString() }
        };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceResult>(GetUrl(futuresAccountMultiAssetsModeSetEndpoint, api, "1"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Get Current Multi-Assets Mode
    public async Task<RestCallResult<BinanceFuturesMultiAssetMode>> GetMultiAssetsModeAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesMultiAssetMode>(GetUrl(futuresAccountMultiAssetsModeGetEndpoint, api, "1"), HttpMethod.Get, ct, parameters, true, weight: 30).ConfigureAwait(false);
    }
    #endregion

    #region Future Account Balance
    public async Task<RestCallResult<IEnumerable<BinanceFuturesAccountBalance>>> GetBalancesAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFuturesAccountBalance>>(GetUrl(futuresAccountBalanceEndpoint, api, "2"), HttpMethod.Get, ct, parameters, true, weight: 5).ConfigureAwait(false);
    }
    #endregion

    #region Account Information
    public async Task<RestCallResult<BinanceFuturesAccountInfo>> GetAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesAccountInfo>(GetUrl(accountInfoEndpoint, api, "2"), HttpMethod.Get, ct, parameters, true, weight: 5).ConfigureAwait(false);
    }
    #endregion

    #region Change Initial Leverage
    public async Task<RestCallResult<BinanceFuturesInitialLeverageChangeResult>> ChangeInitialLeverageAsync(string symbol, int leverage, long? receiveWindow = null, CancellationToken ct = default)
    {
        leverage.ValidateIntBetween(nameof(leverage), 1, 125);
        var parameters = new Dictionary<string, object>
        {
            { "symbol", symbol },
            { "leverage", leverage }
        };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceFuturesInitialLeverageChangeResult>(GetUrl(changeInitialLeverageEndpoint, api, signedVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Change Margin Type
    public async Task<RestCallResult<BinanceFuturesChangeMarginTypeResult>> ChangeMarginTypeAsync(string symbol, FuturesMarginType marginType, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "symbol", symbol },
            { "marginType", JsonConvert.SerializeObject(marginType, new FuturesMarginTypeConverter(false)) }
        };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceFuturesChangeMarginTypeResult>(GetUrl(changeMarginTypeEndpoint, api, signedVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Modify Isolated Position Margin
    public async Task<RestCallResult<BinanceFuturesPositionMarginResult>> ModifyPositionMarginAsync(string symbol, decimal quantity, FuturesMarginChangeDirectionType type, PositionSide? positionSide = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "symbol", symbol },
            { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
            { "type", JsonConvert.SerializeObject(type, new FuturesMarginChangeDirectionTypeConverter(false)) }
        };
        parameters.AddOptionalParameter("positionSide", positionSide == null ? null : JsonConvert.SerializeObject(positionSide, new PositionSideConverter(false)));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesPositionMarginResult>(GetUrl(positionMarginEndpoint, api, signedVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Get Postion Margin Change History
    public async Task<RestCallResult<IEnumerable<BinanceFuturesMarginChangeHistoryResult>>> GetMarginChangeHistoryAsync(string symbol, FuturesMarginChangeDirectionType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "symbol", symbol }
        };
        parameters.AddOptionalParameter("type", type.HasValue ? JsonConvert.SerializeObject(type, new FuturesMarginChangeDirectionTypeConverter(false)) : null);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFuturesMarginChangeHistoryResult>>(GetUrl(positionMarginChangeHistoryEndpoint, api, signedVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Position Information
    public async Task<RestCallResult<IEnumerable<BinancePositionDetailsUsdt>>> GetPositionInformationAsync(string symbol = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinancePositionDetailsUsdt>>(GetUrl(positionInformationEndpoint, api, "2"), HttpMethod.Get, ct, parameters, true, weight: 5).ConfigureAwait(false);
    }
    #endregion

    #region Get Income History
    public async Task<RestCallResult<IEnumerable<BinanceFuturesIncomeHistory>>> GetIncomeHistoryAsync(string symbol = null, string incomeType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("incomeType", incomeType);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFuturesIncomeHistory>>(GetUrl(incomeHistoryEndpoint, api, signedVersion), HttpMethod.Get, ct, parameters, true, weight: 30).ConfigureAwait(false);
    }
    #endregion

    #region Notional and Leverage Brackets
    public async Task<RestCallResult<IEnumerable<BinanceFuturesSymbolBracket>>> GetBracketsAsync(string symbolOrPair = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var url = GetUrl(leverageBracketEndpoint, api, signedVersion);
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter(url.ToString().Contains("dapi") ? "pair" : "symbol", symbolOrPair);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFuturesSymbolBracket>>(url, HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Position ADL Quantile Estimations
    public async Task<RestCallResult<IEnumerable<BinanceFuturesQuantileEstimation>>> GetPositionAdlQuantileEstimationAsync(string symbol = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        if (symbol == null)
            return await SendRequestInternal<IEnumerable<BinanceFuturesQuantileEstimation>>(GetUrl(adlQuantileEndpoint, api, signedVersion), HttpMethod.Get, ct, parameters, true, weight: 5).ConfigureAwait(false);

        var result = await SendRequestInternal<BinanceFuturesQuantileEstimation>(GetUrl(adlQuantileEndpoint, api, signedVersion), HttpMethod.Get, ct, parameters, true, weight: 5).ConfigureAwait(false);
        if (!result)
            return result.As<IEnumerable<BinanceFuturesQuantileEstimation>>(null);

        return result.As<IEnumerable<BinanceFuturesQuantileEstimation>>(new[] { result.Data });
    }
    #endregion

    #region Futures Trading Quantitative Rules Indicators
    public async Task<RestCallResult<BinanceFuturesTradingStatus>> GetTradingStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesTradingStatus>(GetUrl(tradingStatusEndpoint, api, "1"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);

    }
    #endregion

    #region User Commission Rate
    public async Task<RestCallResult<BinanceFuturesAccountUserCommissionRate>> GetUserCommissionRateAsync(string symbol, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "symbol", symbol}
        };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceFuturesAccountUserCommissionRate>(GetUrl(futuresAccountUserCommissionRateEndpoint, "fapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 20).ConfigureAwait(false);
    }
    #endregion

    #region Get Download Id For Futures Transaction History
    public async Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForTransactionHistoryAsync(DateTime startTime, DateTime endTime, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "startTime", startTime.ConvertToMilliseconds() },
            { "endTime", endTime.ConvertToMilliseconds() },
        };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceFuturesDownloadIdInfo>(GetUrl(downloadIdEndpoint, "fapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 5).ConfigureAwait(false);
    }
    #endregion

    #region Get Futures Transaction History Download Link by Id
    public async Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForTransactionHistoryAsync(string downloadId, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
    {
        { "downloadId", downloadId }
    };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceFuturesDownloadLink>(GetUrl(downloadLinkEndpoint, "fapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 5).ConfigureAwait(false);
    }
    #endregion

}