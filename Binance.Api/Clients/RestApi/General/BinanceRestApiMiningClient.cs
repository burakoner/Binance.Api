using Binance.Api.Models.RestApi.Mining;

namespace Binance.Api.Clients.RestApi.General;

public class BinanceRestApiMiningClient
{
    // Mining
    private const string algorithmEndpoint = "mining/pub/algoList";
    private const string coinListEndpoint = "mining/pub/coinList";
    private const string minerDetailsEndpoint = "mining/worker/detail";
    private const string minerListEndpoint = "mining/worker/list";
    private const string miningRevenueEndpoint = "mining/payment/list";
    private const string miningOtherRevenueEndpoint = "mining/payment/other";
    private const string miningHashrateResaleListEndpoint = "mining/hash-transfer/config/details/list";
    private const string miningHashrateResaleDetailsEndpoint = "mining/hash-transfer/profit/details";
    private const string miningHashrateResaleRequest = "mining/hash-transfer/config";
    private const string miningHashrateResaleCancel = "mining/hash-transfer/config/cancel";
    private const string miningStatisticsEndpoint = "mining/statistics/user/status";
    private const string miningAccountListEndpoint = "mining/statistics/user/list";
    // TODO: Mining Account Earning (USER_DATA)

    // Internal References
    internal BinanceRestApiGeneralClient MainClient { get; }
    internal BinanceRestApiClientOptions Options { get => MainClient.RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal BinanceRestApiMiningClient(BinanceRestApiGeneralClient main)
    {
        MainClient = main;
    }

    #region Acquiring Algorithm 
    public async Task<RestCallResult<IEnumerable<BinanceMiningAlgorithm>>> GetMiningAlgorithmListAsync(CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        var result = await SendRequestInternal<BinanceResult<IEnumerable<BinanceMiningAlgorithm>>>(GetUrl(algorithmEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        if (!result.Success)
            return result.As<IEnumerable<BinanceMiningAlgorithm>>(default);

        if (result.Data?.Code != 0)
            return result.AsError<IEnumerable<BinanceMiningAlgorithm>>(new ServerError(result.Data!.Code, result.Data!.Message));

        return result.As(result.Data.Data);
    }
    #endregion

    #region Acquiring CoinName
    public async Task<RestCallResult<IEnumerable<BinanceMiningCoin>>> GetMiningCoinListAsync(CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        var result = await SendRequestInternal<BinanceResult<IEnumerable<BinanceMiningCoin>>>(GetUrl(coinListEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        if (!result.Success)
            return result.As<IEnumerable<BinanceMiningCoin>>(default);

        if (result.Data?.Code != 0)
            return result.AsError<IEnumerable<BinanceMiningCoin>>(new ServerError(result.Data!.Code, result.Data!.Message));

        return result.As(result.Data.Data);
    }
    #endregion

    #region Request Detail Miner List
    public async Task<RestCallResult<IEnumerable<BinanceMinerDetails>>> GetMinerDetailsAsync(string algorithm, string userName, string workerName, CancellationToken ct = default)
    {
        algorithm.ValidateNotNull(nameof(algorithm));
        userName.ValidateNotNull(nameof(userName));
        workerName.ValidateNotNull(nameof(workerName));

        var parameters = new Dictionary<string, object>()
            {
                {"algo", algorithm},
                {"userName", userName},
                {"workerName", workerName}
            };

        var result = await SendRequestInternal<BinanceResult<IEnumerable<BinanceMinerDetails>>>(GetUrl(minerDetailsEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success)
            return result.As<IEnumerable<BinanceMinerDetails>>(default);

        if (result.Data?.Code != 0)
            return result.AsError<IEnumerable<BinanceMinerDetails>>(new ServerError(result.Data!.Code, result.Data!.Message));

        return result.As(result.Data.Data);
    }
    #endregion

    #region Request Miner List
    public async Task<RestCallResult<BinanceMinerList>> GetMinerListAsync(string algorithm, string userName, int? page = null, bool? sortAscending = null, string sortColumn = null, MinerStatus? workerStatus = null, CancellationToken ct = default)
    {
        algorithm.ValidateNotNull(nameof(algorithm));
        userName.ValidateNotNull(nameof(userName));

        var parameters = new Dictionary<string, object>()
            {
                {"algo", algorithm},
                {"userName", userName}
            };

        parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("sortAscending", sortAscending == null ? null : sortAscending == true ? "1" : "0");
        parameters.AddOptionalParameter("sortColumn", sortColumn);
        parameters.AddOptionalParameter("workerStatus", workerStatus == null ? null : JsonConvert.SerializeObject(workerStatus, new MinerStatusConverter(false)));

        var result = await SendRequestInternal<BinanceResult<BinanceMinerList>>(GetUrl(minerListEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success)
            return result.As<BinanceMinerList>(default);

        if (result.Data?.Code != 0)
            return result.AsError<BinanceMinerList>(new ServerError(result.Data!.Code, result.Data!.Message));

        return result.As(result.Data.Data);
    }
    #endregion

    #region Earnings List
    public async Task<RestCallResult<BinanceRevenueList>> GetMiningRevenueListAsync(string algorithm, string userName, string coin = null, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
    {
        algorithm.ValidateNotNull(nameof(algorithm));
        userName.ValidateNotNull(nameof(userName));

        var parameters = new Dictionary<string, object>()
            {
                {"algo", algorithm},
                {"userName", userName}
            };

        parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("pageSize", pageSize?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("coin", coin);
        parameters.AddOptionalParameter("startDate", startDate.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endDate", endDate.ConvertToMilliseconds());

        var result = await SendRequestInternal<BinanceResult<BinanceRevenueList>>(GetUrl(miningRevenueEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success)
            return result.As<BinanceRevenueList>(default);

        if (result.Data?.Code != 0)
            return result.AsError<BinanceRevenueList>(new ServerError(result.Data!.Code, result.Data!.Message));

        return result.As(result.Data.Data);
    }
    #endregion

    #region Extra Bonus List
    public async Task<RestCallResult<BinanceOtherRevenueList>> GetMiningOtherRevenueListAsync(string algorithm, string userName, string coin = null, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
    {
        algorithm.ValidateNotNull(nameof(algorithm));
        userName.ValidateNotNull(nameof(userName));

        var parameters = new Dictionary<string, object>()
            {
                {"algo", algorithm},
                {"userName", userName}
            };

        parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("pageSize", pageSize?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("coin", coin);
        parameters.AddOptionalParameter("startDate", startDate.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endDate", endDate.ConvertToMilliseconds());

        var result = await SendRequestInternal<BinanceResult<BinanceOtherRevenueList>>(GetUrl(miningOtherRevenueEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success)
            return result.As<BinanceOtherRevenueList>(default);

        if (result.Data?.Code != 0)
            return result.AsError<BinanceOtherRevenueList>(new ServerError(result.Data!.Code, result.Data!.Message));

        return result.As(result.Data.Data);
    }
    #endregion

    #region Hashrate Resale List
    public async Task<RestCallResult<BinanceHashrateResaleList>> GetHashrateResaleListAsync(int? page = null, int? pageSize = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("pageIndex", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("pageSize", pageSize?.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceResult<BinanceHashrateResaleList>>(GetUrl(miningHashrateResaleListEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success)
            return result.As<BinanceHashrateResaleList>(default);

        if (result.Data?.Code != 0)
            return result.AsError<BinanceHashrateResaleList>(new ServerError(result.Data!.Code, result.Data!.Message));

        return result.As(result.Data.Data);
    }
    #endregion

    #region Hashrate Resale Details
    public async Task<RestCallResult<BinanceHashrateResaleDetails>> GetHashrateResaleDetailsAsync(int configId, string userName, int? page = null, int? pageSize = null, CancellationToken ct = default)
    {
        userName.ValidateNotNull(nameof(userName));

        var parameters = new Dictionary<string, object>()
            {
                { "configId", configId.ToString(CultureInfo.InvariantCulture) },
                { "userName", userName }
            };

        parameters.AddOptionalParameter("pageIndex", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("pageSize", pageSize?.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceResult<BinanceHashrateResaleDetails>>(GetUrl(miningHashrateResaleDetailsEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success)
            return result.As<BinanceHashrateResaleDetails>(default);

        if (result.Data?.Code != 0)
            result.AsError<BinanceHashrateResaleDetails>(new ServerError(result.Data!.Code, result.Data!.Message));

        return result.As(result.Data.Data);
    }
    #endregion

    #region Hashrate Resale Request
    public async Task<RestCallResult<int>> PlaceHashrateResaleRequestAsync(string userName, string algorithm, DateTime startDate, DateTime endDate, string toUser, decimal hashRate, CancellationToken ct = default)
    {
        userName.ValidateNotNull(nameof(userName));
        algorithm.ValidateNotNull(nameof(algorithm));
        toUser.ValidateNotNull(nameof(toUser));

        var parameters = new Dictionary<string, object>()
            {
                { "userName", userName },
                { "algo", algorithm },
                { "startDate", startDate.ConvertToMilliseconds()! },
                { "endDate",endDate.ConvertToMilliseconds()! },
                { "toPoolUser", toUser },
                { "hashRate", hashRate }
            };

        var result = await SendRequestInternal<BinanceResult<int>>(GetUrl(miningHashrateResaleRequest, "sapi", "1"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success)
            return result.As<int>(default);

        if (result.Data?.Code != 0)
            return result.AsError<int>(new ServerError(result.Data!.Code, result.Data!.Message));

        return result.As(result.Data.Data);
    }
    #endregion

    #region Cancel Hashrate Resale Configuration
    public async Task<RestCallResult<bool>> CancelHashrateResaleRequestAsync(int configId, string userName, CancellationToken ct = default)
    {
        userName.ValidateNotNull(nameof(userName));

        var parameters = new Dictionary<string, object>()
            {
                { "configId", configId },
                { "userName", userName }
            };

        var result = await SendRequestInternal<BinanceResult<bool>>(GetUrl(miningHashrateResaleCancel, "sapi", "1"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success)
            return result.As<bool>(default);

        if (result.Data?.Code != 0)
            return result.AsError<bool>(new ServerError(result.Data!.Code, result.Data!.Message));

        return result.As(result.Data.Data);
    }
    #endregion

    #region Statistics List
    public async Task<RestCallResult<BinanceMiningStatistic>> GetMiningStatisticsAsync(string algorithm, string userName, CancellationToken ct = default)
    {
        algorithm.ValidateNotNull(nameof(algorithm));
        userName.ValidateNotNull(nameof(userName));

        var parameters = new Dictionary<string, object>()
            {
                {"algo", algorithm},
                {"userName", userName}
            };

        var result = await SendRequestInternal<BinanceResult<BinanceMiningStatistic>>(GetUrl(miningStatisticsEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success)
            return result.As<BinanceMiningStatistic>(default);

        if (result.Data?.Code != 0)
            return result.AsError<BinanceMiningStatistic>(new ServerError(result.Data!.Code, result.Data!.Message));

        return result.As(result.Data.Data);
    }
    #endregion

    #region Account List
    public async Task<RestCallResult<IEnumerable<BinanceMiningAccount>>> GetMiningAccountListAsync(string algorithm, string userName, CancellationToken ct = default)
    {
        algorithm.ValidateNotNull(nameof(algorithm));
        userName.ValidateNotNull(nameof(userName));

        var parameters = new Dictionary<string, object>()
            {
                {"algo", algorithm},
                {"userName", userName}
            };

        var result = await SendRequestInternal<BinanceResult<IEnumerable<BinanceMiningAccount>>>(GetUrl(miningAccountListEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success)
            return result.As<IEnumerable<BinanceMiningAccount>>(default);

        if (result.Data?.Code != 0)
            return result.AsError<IEnumerable<BinanceMiningAccount>>(new ServerError(result.Data!.Code, result.Data!.Message));

        return result.As(result.Data.Data);
    }
    #endregion
}