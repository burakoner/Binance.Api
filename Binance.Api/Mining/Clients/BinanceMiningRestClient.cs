namespace Binance.Api.Mining;

internal partial class BinanceMiningRestClient(BinanceRestApiClient root) : IBinanceMiningRestClient
{
    // Api
    private const string v1 = "1";
    private const string v3 = "3";
    private const string api = "api";
    private const string sapi = "sapi";

    // Parent
    private BinanceRestApiClient _ { get; } = root;

    // Internal
    private ILogger Logger => _.Logger;
    private BinanceRestApiClientOptions Options => _.RestOptions;

    private Task<RestCallResult<T>> RequestAsync<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object>? queryParameters = null,
        Dictionary<string, object>? bodyParameters = null,
        Dictionary<string, string>? headerParameters = null,
        ArraySerialization? serialization = null,
        JsonSerializer? deserializer = null,
        bool ignoreRatelimit = false,
        int requestWeight = 1) where T : class
        => _.RequestAsync<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    private Uri GetUrl(string api, string version, string endpoint)
    {
        var url = BinanceAddress.Default.MiningRestApiAddress;
        if (!string.IsNullOrEmpty(api)) url = url.AppendPath($"{api}");
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }

    public async Task<RestCallResult<List<BinanceMiningAlgorithm>>> GetAlgorithmsAsync(CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        var result = await RequestAsync<BinanceResult<List<BinanceMiningAlgorithm>>>(GetUrl(sapi, v1, "mining/pub/algoList"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        if (!result.Success) return result.As<List<BinanceMiningAlgorithm>>([]);
        if (result.Data?.Code != 0) return result.AsError<List<BinanceMiningAlgorithm>>(new ServerError(result.Data!.Code, result.Data!.Message!));

        return result.As(result.Data.Data);
    }

    public async Task<RestCallResult<List<BinanceMiningCoin>>> GetCoinsAsync(CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        var result = await RequestAsync<BinanceResult<List<BinanceMiningCoin>>>(GetUrl(sapi, v1, "mining/pub/coinList"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        if (!result.Success) return result.As<List<BinanceMiningCoin>>([]);
        if (result.Data?.Code != 0) return result.AsError<List<BinanceMiningCoin>>(new ServerError(result.Data!.Code, result.Data!.Message!));

        return result.As(result.Data.Data);
    }

    public async Task<RestCallResult<BinanceMiningHashrateResaleList>> GetHashrateResalesAsync(int? page = null, int? pageSize = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("pageIndex", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("pageSize", pageSize?.ToString(CultureInfo.InvariantCulture));

        var result = await RequestAsync<BinanceResult<BinanceMiningHashrateResaleList>>(GetUrl(sapi, v1, "mining/hash-transfer/config/details/list"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success) return result.As<BinanceMiningHashrateResaleList>(default!);
        if (result.Data?.Code != 0) return result.AsError<BinanceMiningHashrateResaleList>(new ServerError(result.Data!.Code, result.Data!.Message!));

        return result.As(result.Data.Data);
    }

    public async Task<RestCallResult<BinanceMiningWorkers>> GetWorkersAsync(string algorithm, string userName, int? page = null, bool? sortAscending = null, string? sortColumn = null, BinanceMiningMinerStatus? workerStatus = null, CancellationToken ct = default)
    {
        algorithm.ValidateNotNull(nameof(algorithm));
        userName.ValidateNotNull(nameof(userName));

        var parameters = new ParameterCollection()
            {
                {"algo", algorithm},
                {"userName", userName}
            };

        parameters.AddOptional("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("sortAscending", sortAscending == null ? null : sortAscending == true ? "1" : "0");
        parameters.AddOptional("sortColumn", sortColumn);
        parameters.AddOptionalEnum("workerStatus", workerStatus);

        var result = await RequestAsync<BinanceResult<BinanceMiningWorkers>>(GetUrl(sapi, v1, "mining/worker/list"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success) return result.As<BinanceMiningWorkers>(default!);
        if (result.Data?.Code != 0) return result.AsError<BinanceMiningWorkers>(new ServerError(result.Data!.Code, result.Data!.Message!));

        return result.As(result.Data.Data);
    }

    public async Task<RestCallResult<List<BinanceMiningWorkerDetails>>> GetWorkerDetailsAsync(string algorithm, string userName, string workerName, CancellationToken ct = default)
    {
        algorithm.ValidateNotNull(nameof(algorithm));
        userName.ValidateNotNull(nameof(userName));
        workerName.ValidateNotNull(nameof(workerName));

        var parameters = new ParameterCollection()
            {
                {"algo", algorithm},
                {"userName", userName},
                {"workerName", workerName}
            };

        var result = await RequestAsync<BinanceResult<List<BinanceMiningWorkerDetails>>>(GetUrl(sapi, v1, "mining/worker/detail"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success) return result.As<List<BinanceMiningWorkerDetails>>([]);
        if (result.Data?.Code != 0) return result.AsError<List<BinanceMiningWorkerDetails>>(new ServerError(result.Data!.Code, result.Data!.Message!));

        return result.As(result.Data.Data);
    }

    public async Task<RestCallResult<BinanceMiningOtherRevenues>> GetOtherRevenuesAsync(string algorithm, string userName, string? coin = null, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
    {
        algorithm.ValidateNotNull(nameof(algorithm));
        userName.ValidateNotNull(nameof(userName));

        var parameters = new ParameterCollection()
            {
                {"algo", algorithm},
                {"userName", userName}
            };

        parameters.AddOptional("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("pageSize", pageSize?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("coin", coin);
        parameters.AddOptionalMilliseconds("startDate", startDate);
        parameters.AddOptionalMilliseconds("endDate", endDate);

        var result = await RequestAsync<BinanceResult<BinanceMiningOtherRevenues>>(GetUrl(sapi, v1, "mining/payment/other"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success) return result.As<BinanceMiningOtherRevenues>(default!);
        if (result.Data?.Code != 0) return result.AsError<BinanceMiningOtherRevenues>(new ServerError(result.Data!.Code, result.Data!.Message!));

        return result.As(result.Data.Data);
    }

    public async Task<RestCallResult<BinanceMiningRevenues>> GetMiningRevenuesAsync(string algorithm, string userName, string? coin = null, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
    {
        algorithm.ValidateNotNull(nameof(algorithm));
        userName.ValidateNotNull(nameof(userName));

        var parameters = new ParameterCollection()
            {
                {"algo", algorithm},
                {"userName", userName}
            };

        parameters.AddOptional("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("pageSize", pageSize?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("coin", coin);
        parameters.AddOptionalMilliseconds("startDate", startDate);
        parameters.AddOptionalMilliseconds("endDate", endDate);

        var result = await RequestAsync<BinanceResult<BinanceMiningRevenues>>(GetUrl(sapi, v1, "mining/payment/list"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success) return result.As<BinanceMiningRevenues>(default!);
        if (result.Data?.Code != 0) return result.AsError<BinanceMiningRevenues>(new ServerError(result.Data!.Code, result.Data!.Message!));

        return result.As(result.Data.Data);
    }

    public async Task<RestCallResult<bool>> CancelHashrateResaleRequestAsync(int configId, string userName, CancellationToken ct = default)
    {
        userName.ValidateNotNull(nameof(userName));

        var parameters = new ParameterCollection()
        {
            { "configId", configId },
            { "userName", userName }
        };

        var result = await RequestAsync<BinanceResult<bool>>(GetUrl(sapi, v1, "mining/hash-transfer/config/cancel"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success) return result.As<bool>(default);
        if (result.Data?.Code != 0) return result.AsError<bool>(new ServerError(result.Data!.Code, result.Data!.Message!));

        return result.As(result.Data.Data);
    }

    public async Task<RestCallResult<BinanceMiningHashrateResaleDetails>> GetHashrateResaleDetailsAsync(int configId, string userName, int? page = null, int? pageSize = null, CancellationToken ct = default)
    {
        userName.ValidateNotNull(nameof(userName));

        var parameters = new ParameterCollection()
            {
                { "configId", configId.ToString(CultureInfo.InvariantCulture) },
                { "userName", userName }
            };

        parameters.AddOptional("pageIndex", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("pageSize", pageSize?.ToString(CultureInfo.InvariantCulture));

        var result = await RequestAsync<BinanceResult<BinanceMiningHashrateResaleDetails>>(GetUrl(sapi, v1, "mining/hash-transfer/profit/details"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success) return result.As<BinanceMiningHashrateResaleDetails>(default!);
        if (result.Data?.Code != 0) result.AsError<BinanceMiningHashrateResaleDetails>(new ServerError(result.Data!.Code, result.Data!.Message!));

        return result.As(result.Data.Data);
    }

    public async Task<RestCallResult<BinanceMiningEarnings>> GetEarningsAsync(string algo, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
            {
                { "algo", algo }
            };
        parameters.AddOptionalMilliseconds("startDate", startDate);
        parameters.AddOptionalMilliseconds("endDate", endDate);
        parameters.AddOptional("pageIndex", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("pageSize", pageSize?.ToString(CultureInfo.InvariantCulture));

        var result = await RequestAsync<BinanceResult<BinanceMiningEarnings>>(GetUrl(sapi, v1, "mining/payment/uid"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success) return result.As<BinanceMiningEarnings>(default!);
        if (result.Data?.Code != 0) return result.AsError<BinanceMiningEarnings>(new ServerError(result.Data!.Code, result.Data!.Message!));

        return result.As(result.Data.Data);
    }

    public async Task<RestCallResult<BinanceMiningStatistic>> GetStatisticsAsync(string algorithm, string userName, CancellationToken ct = default)
    {
        algorithm.ValidateNotNull(nameof(algorithm));
        userName.ValidateNotNull(nameof(userName));

        var parameters = new ParameterCollection()
        {
            {"algo", algorithm},
            {"userName", userName}
        };

        var result = await RequestAsync<BinanceResult<BinanceMiningStatistic>>(GetUrl(sapi, v1, "mining/statistics/user/status"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success) return result.As<BinanceMiningStatistic>(default!);
        if (result.Data?.Code != 0) return result.AsError<BinanceMiningStatistic>(new ServerError(result.Data!.Code, result.Data!.Message!));

        return result.As(result.Data.Data);
    }

    public async Task<RestCallResult<int>> PlaceHashrateResaleRequestAsync(string algorithm, string userName, DateTime startDate, DateTime endDate, string toUser, decimal hashRate, CancellationToken ct = default)
    {
        userName.ValidateNotNull(nameof(userName));
        algorithm.ValidateNotNull(nameof(algorithm));
        toUser.ValidateNotNull(nameof(toUser));

        var parameters = new ParameterCollection()
        {
            { "userName", userName },
            { "algo", algorithm },
            { "toPoolUser", toUser },
            { "hashRate", hashRate }
        };
        parameters.AddMilliseconds("startDate", startDate);
        parameters.AddMilliseconds("endDate", endDate);

        var result = await RequestAsync<BinanceResult<int>>(GetUrl(sapi, v1, "mining/hash-transfer/config"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success) return result.As<int>(default);
        if (result.Data?.Code != 0) return result.AsError<int>(new ServerError(result.Data!.Code, result.Data!.Message!));

        return result.As(result.Data.Data);
    }

    public async Task<RestCallResult<List<BinanceMiningAccount>>> GetAccountsAsync(string algorithm, string userName, CancellationToken ct = default)
    {
        algorithm.ValidateNotNull(nameof(algorithm));
        userName.ValidateNotNull(nameof(userName));

        var parameters = new ParameterCollection()
        {
            {"algo", algorithm},
            {"userName", userName}
        };

        var result = await RequestAsync<BinanceResult<List<BinanceMiningAccount>>>(GetUrl(sapi, v1, "mining/statistics/user/list"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success) return result.As<List<BinanceMiningAccount>>([]);
        if (result.Data?.Code != 0) return result.AsError<List<BinanceMiningAccount>>(new ServerError(result.Data!.Code, result.Data!.Message!));

        return result.As(result.Data.Data);
    }
}