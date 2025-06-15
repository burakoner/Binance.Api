namespace Binance.Api.Fiat;

internal partial class BinanceFiatRestClient(BinanceRestApiClient root) : IBinanceFiatRestClient
{
    // Api
    private const string v1 = "1";
    private const string sapi = "sapi";

    // Parent
    private BinanceRestApiClient _ { get; } = root;

    // Internal
    private ILogger Logger => _.Logger;
    private BinanceRestApiClientOptions Options => _.ApiOptions;

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
        var url = BinanceAddress.Default.FiatRestApiAddress;
        if (!string.IsNullOrEmpty(api)) url = url.AppendPath($"{api}");
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }

    public Task<RestCallResult<BinanceDataTotalResponse<BinanceFiatTransaction>>> GetDepositHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? rows = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("transactionType", "deposit");
        parameters.AddOptionalMilliseconds("beginTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("page", page);
        parameters.AddOptional("rows", rows);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceDataTotalResponse<BinanceFiatTransaction>>(GetUrl(sapi, v1, "fiat/orders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceDataTotalResponse<BinanceFiatTransaction>>> GetWithdrawalHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? rows = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("transactionType", "withdraw");
        parameters.AddOptionalMilliseconds("beginTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("page", page);
        parameters.AddOptional("rows", rows);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceDataTotalResponse<BinanceFiatTransaction>>(GetUrl(sapi, v1, "fiat/orders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceDataTotalResponse<BinanceFiatPayment>>> GetPaymentHistoryAsync(BinanceFiatPaymentType type, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? rows = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddEnum("transactionType", type);
        parameters.AddOptionalMilliseconds("beginTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("page", page);
        parameters.AddOptional("rows", rows);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceDataTotalResponse<BinanceFiatPayment>>(GetUrl(sapi, v1, "fiat/payments"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }
}