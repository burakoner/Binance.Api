namespace Binance.Api.NFT;

internal partial class BinanceNftRestClient(BinanceRestApiClient root) : IBinanceNftRestClient
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
        var url = BinanceAddress.Default.NFTRestApiAddress;
        if (!string.IsNullOrEmpty(api)) url = url.AppendPath($"{api}");
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }

    public Task<RestCallResult<BinanceListRecords<BinanceNftDeposit>>> GetDepositsAsync(DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? page = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("page", page);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceListRecords<BinanceNftDeposit>>(GetUrl(sapi, v1, "nft/history/deposit"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 3000);
    }

    public Task<RestCallResult<BinanceListRecords<BinanceNftWithdrawal>>> GetWithdrawalsAsync(DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? page = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("page", page);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceListRecords<BinanceNftWithdrawal>>(GetUrl(sapi, v1, "nft/history/withdraw"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 3000);
    }

    public Task<RestCallResult<BinanceListRecords<BinanceNftTransaction>>> GetTransactionsAsync(BinanceNftOrderType orderType, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? page = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddEnum("orderType", orderType);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("page", page);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceListRecords<BinanceNftTransaction>>(GetUrl(sapi, v1, "nft/history/transactions"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 3000);
    }

    public Task<RestCallResult<BinanceListRecords<BinanceNftAsset>>> GetAssetsAsync(int? limit = null, int? page = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("page", page);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceListRecords<BinanceNftAsset>>(GetUrl(sapi, v1, "nft/user/getAsset"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 3000);
    }
}