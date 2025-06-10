namespace Binance.Api.Broker;

internal partial class BinanceBrokerRestClientLinkAndTradeSpot(BinanceBrokerRestClientLinkAndTrade parent) : IBinanceBrokerRestClientLinkAndTradeSpot
{
    // Api
    private const string v1 = "1";
    private const string v2 = "2";
    private const string v3 = "3";
    private const string fapi = "fapi";
    private const string sapi = "sapi";

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
        var url = BinanceAddress.Default.BrokerLinkAndTradeSpotRestApiAddress;
        if (!string.IsNullOrEmpty(api)) url = url.AppendPath($"{api}");
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }

    public Task<RestCallResult<BinanceBrokerSpotIfNewUser>> GetIfNewUserAsync(string apiAgentCode, int? receiveWindow = null, CancellationToken ct = default)
    {
        apiAgentCode.ValidateNotNull(nameof(apiAgentCode));

        var parameters = new ParameterCollection
        {
            { "apiAgentCode", apiAgentCode }
        };
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerSpotIfNewUser>(GetUrl(sapi, v1, "apiReferral/ifNewUser"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerSpotCustomerIdPartner>> SetCustomerIdByPartnerAsync(string email, string customerId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "email", email },
            { "customerId", customerId },
        };
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerSpotCustomerIdPartner>(GetUrl(sapi, v1, "apiReferral/customization"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<List<BinanceBrokerSpotCustomerIdPartner>>> GetCustomerIdByPartnerAsync(string? email = null, string? customerId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("email", email);
        parameters.AddOptional("customerId", customerId);
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceBrokerSpotCustomerIdPartner>>(GetUrl(sapi, v1, "apiReferral/customization"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerSpotCustomerIdClient>> SetCustomerIdByClientAsync(string customerId, string apiAgentCode, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "apiAgentCode", apiAgentCode },
            { "customerId", customerId },
        };
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerSpotCustomerIdClient>(GetUrl(sapi, v1, "apiReferral/userCustomization"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerSpotCustomerIdClient>> GetCustomerIdByClientAsync(string apiAgentCode, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "apiAgentCode", apiAgentCode },
        };
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerSpotCustomerIdClient>(GetUrl(sapi, v1, "apiReferral/userCustomization"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<List<BinanceBrokerSpotRebatePartner>>> GetRebateHistoryByPartnerAsync(DateTime startTime, DateTime endTime, string? customerId = null, int limit = 100, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddMilliseconds("startTime", startTime);
        parameters.AddMilliseconds("endTime", endTime);
        parameters.AddOptional("customerId", customerId);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceBrokerSpotRebatePartner>>(GetUrl(sapi, v1, "apiReferral/rebate/recentRecord"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<List<BinanceBrokerSpotRebateClient>>> GetRebateHistoryByClientAsync(DateTime? startTime = null, DateTime? endTime = null, int limit = 100, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceBrokerSpotRebateClient>>(GetUrl(sapi, v1, "apiReferral/kickback/recentRecord"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }
}