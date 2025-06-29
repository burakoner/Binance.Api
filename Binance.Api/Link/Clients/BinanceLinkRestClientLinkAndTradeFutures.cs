﻿namespace Binance.Api.Link;

internal partial class BinanceLinkRestClientLinkAndTradeFutures(BinanceLinkRestClientLinkAndTrade parent) : IBinanceLinkRestClientLinkAndTradeFutures
{
    // Api
    private const string v1 = "1";
    private const string v2 = "2";
    private const string v3 = "3";
    private const string fapi = "fapi";

    // Parent
    private BinanceRestApiClient ___ { get; } = parent.__;
    private BinanceLinkRestClient __ { get; } = parent._;
    private BinanceLinkRestClientLinkAndTrade _ { get; } = parent;

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
        var url = BinanceAddress.Default.LinkAndTradeFuturesRestApiAddress;
        if (!string.IsNullOrEmpty(api)) url = url.AppendPath($"{api}");
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }

    public Task<RestCallResult<BinanceLinkFuturesIfNewUser>> GetIfNewUserAsync(string brokerId, BinanceFuturesType? type = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "brokerId", brokerId }
        };
        parameters.AddOptionalEnum("type", type);
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceLinkFuturesIfNewUser>(GetUrl(fapi, v1, "apiReferral/ifNewUser"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<BinanceLinkFuturesCustomerIdPartner>> SetCustomerIdByPartnerAsync(string email, string customerId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "email", email },
            { "customerId", customerId },
        };
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceLinkFuturesCustomerIdPartner>(GetUrl(fapi, v1, "apiReferral/customization"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<List<BinanceLinkFuturesCustomerIdPartner>>> GetCustomerIdByPartnerAsync(string? email = null, string? customerId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("email", email);
        parameters.AddOptional("customerId", customerId);
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceLinkFuturesCustomerIdPartner>>(GetUrl(fapi, v1, "apiReferral/customization"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<BinanceLinkFuturesCustomerIdClient>> SetCustomerIdByClientAsync(string customerId, string apiAgentCode, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "apiAgentCode", apiAgentCode },
            { "customerId", customerId },
        };
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceLinkFuturesCustomerIdClient>(GetUrl(fapi, v1, "apiReferral/userCustomization"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<BinanceLinkFuturesCustomerIdClient>> GetCustomerIdByClientAsync(string apiAgentCode, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "apiAgentCode", apiAgentCode },
        };
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceLinkFuturesCustomerIdClient>(GetUrl(fapi, v1, "apiReferral/userCustomization"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<List<BinanceLinkFuturesIncomeRecord>>> GetIncomeHistoryAsync(string? symbol = null, BinanceLinkIncomeType? incomeType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptionalEnum("incomeType", incomeType);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceLinkFuturesIncomeRecord>>(GetUrl(fapi, v1, "income"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<List<BinanceLinkFuturesTraderNumber>>> GetTraderNumberAsync(BinanceFuturesType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalEnum("type", type);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceLinkFuturesTraderNumber>>(GetUrl(fapi, v1, "apiReferral/traderNum"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }


    public Task<RestCallResult<BinanceLinkFuturesRebateOverview>> GetRebateDataOverviewAsync(BinanceFuturesType? type = null,int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalEnum("type", type);
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceLinkFuturesRebateOverview>(GetUrl(fapi, v1, "apiReferral/overview"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<List<BinanceLinkFuturesTradeVolume>>> GetUserTradeVolumeAsync(BinanceFuturesType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalEnum("type", type);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceLinkFuturesTradeVolume>>(GetUrl(fapi, v1, "apiReferral/tradeVol"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<List<BinanceLinkFuturesRebateVolume>>> GetUserRebateVolumeAsync(BinanceFuturesType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalEnum("type", type);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceLinkFuturesRebateVolume>>(GetUrl(fapi, v1, "apiReferral/rebateVol"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<List<BinanceLinkFuturesTraderDetail>>> GetTraderDetailsAsync(string? customerId=null, BinanceFuturesType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("customerId", customerId);
        parameters.AddOptionalEnum("type", type);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", ___.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceLinkFuturesTraderDetail>>(GetUrl(fapi, v1, "apiReferral/traderSummary"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }
}