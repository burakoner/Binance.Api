namespace Binance.Api.Broker;

internal partial class BinanceBrokerRestClientLinkAndTradePortfolioMargin(BinanceBrokerRestClientLinkAndTrade parent) : IBinanceBrokerRestClientLinkAndTradePortfolioMargin
{
    // Api
    private const string v1 = "1";
    private const string v2 = "2";
    private const string v3 = "3";
    private const string papi = "papi";

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
        var url = BinanceAddress.Default.BrokerLinkAndTradeFuturesRestApiAddress;
        if (!string.IsNullOrEmpty(api)) url = url.AppendPath($"{api}");
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }

    public Task<RestCallResult<BinanceBrokerPortfolioMarginIfNewUser>> GetIfNewUserAsync(string brokerId, BinanceFuturesType? type=null,  int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "brokerId", brokerId }
        };
        parameters.AddOptionalEnum("type", type);
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerPortfolioMarginIfNewUser>(GetUrl(papi, v1, "apiReferral/ifNewUser"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<BinanceBrokerFuturesCustomerIdClient>> SetCustomerIdByClientAsync(string customerId, string apiAgentCode, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "apiAgentCode", apiAgentCode },
            { "customerId", customerId },
        };
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerFuturesCustomerIdClient>(GetUrl(papi, v1, "apiReferral/userCustomization"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<BinanceBrokerFuturesCustomerIdClient>> GetCustomerIdByClientAsync(string apiAgentCode, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "apiAgentCode", apiAgentCode },
        };
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerFuturesCustomerIdClient>(GetUrl(papi, v1, "apiReferral/userCustomization"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }
}