namespace Binance.Api.Broker;

internal partial class BinanceBrokerRestClientLinkAndTradeFastApi(BinanceBrokerRestClientLinkAndTrade parent) : IBinanceBrokerRestClientLinkAndTradeFastApi
{
    // Api
    private const string v1 = "1";
    private const string v2 = "2";
    private const string v3 = "3";

    // Parent
    private BinanceRestApiClient ___ { get; } = parent.__;
    private BinanceBrokerRestClient __ { get; } = parent._;
    private BinanceBrokerRestClientLinkAndTrade _ { get; } = parent;

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceRestApiClientOptions RestOptions => _.RestOptions;

    private Task<RestCallResult<T>> RequestAsync<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object>? queryParameters = null,
        Dictionary<string, object>? bodyParameters = null,
        Dictionary<string, string>? headerParameters = null,
        ArraySerialization? serialization = null,
        JsonSerializer? deserializer = null,
        bool ignoreRatelimit = false,
        int requestWeight = 1) where T : class
        => ___.RequestAsync<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    private static Uri GetUrl(string api, string version, string endpoint)
    {
        var url = BinanceAddress.Default.BrokerLinkAndTradeFastApiRestApiAddress;
        if (!string.IsNullOrEmpty(api)) url = url.AppendPath($"{api}");
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }

    public async Task<RestCallResult<BinanceBrokerFastUserStatus>> GetUserStatusAsync(string accessToken, CancellationToken ct = default)
    {
        var headers = new Dictionary<string, string>();
        headers.Add("Authorization", $"Bearer {accessToken}");

        var result = await RequestAsync<BinanceDataResponse<BinanceBrokerFastUserStatus>>(GetUrl("", v1, "api-key/user-status"), HttpMethod.Get, ct, true, headerParameters: headers, requestWeight: 0).ConfigureAwait(false);
        if (!result.Success) return result.AsError<BinanceBrokerFastUserStatus>(result.Error!);
        if (result.Data.Data == null) return result.AsError<BinanceBrokerFastUserStatus>(new ServerError("No data returned from the server."));
        if (System.Convert.ToInt32(result.Data.Code) != 0) return result.AsError<BinanceBrokerFastUserStatus>(new ServerError(result.Data.Code, result.Data.Message));

        return result.As(result.Data.Data);
    }

    public async Task<RestCallResult<BinanceBrokerFastApiKey>> CreateApiKeyForUserAsync(
        string accessToken, 
        string apiName, 
        bool enableTrade,
        bool enableFutureTrade,
        bool enableMargin,
        bool enableEuropeanOptions,
        string publicKey,
        string? apiKeyPublicKey = null,
        BinanceBrokerApiKeyIpRestriction? status=null,
        string? ipAddress=null, 
        string? thirdPartyName = null,
        CancellationToken ct = default)
    {
        var headers = new Dictionary<string, string>();
        headers.Add("Authorization", $"Bearer {accessToken}");

        var parameters = new ParameterCollection();
        parameters.AddParameter("apiName", apiName);
        parameters.AddParameter("enableTrade", enableTrade);
        parameters.AddParameter("enableFutureTrade", enableFutureTrade);
        parameters.AddParameter("enableMargin", enableMargin);
        parameters.AddParameter("enableEuropeanOptions", enableEuropeanOptions);
        parameters.AddParameter("publicKey", publicKey);
        parameters.AddOptional("apiKeyPublicKey", apiKeyPublicKey);
        parameters.AddOptionalEnum("status", status);
        parameters.AddOptional("ipAddress", ipAddress);
        parameters.AddOptional("thirdPartyName", thirdPartyName);

        var result = await RequestAsync<BinanceDataResponse<BinanceBrokerFastApiKey>>(GetUrl("", v1, "api-key/create"), HttpMethod.Post, ct, true, headerParameters: headers, bodyParameters: parameters, requestWeight: 0).ConfigureAwait(false);
        if (!result.Success) return result.AsError<BinanceBrokerFastApiKey>(result.Error!);
        if (result.Data.Data == null) return result.AsError<BinanceBrokerFastApiKey>(new ServerError("No data returned from the server."));
        if (System.Convert.ToInt32(result.Data.Code) != 0) return result.AsError<BinanceBrokerFastApiKey>(new ServerError(result.Data.Code, result.Data.Message));

        return result.As(result.Data.Data);
    }
}