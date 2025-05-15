using ApiSharp.Rest;

namespace Binance.Api.Broker;

internal partial class BinanceBrokerRestClientExchangeLink
{
    public Task<RestCallResult<BinanceBrokerSubAccountCreateResult>> CreateSubAccountAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerSubAccountCreateResult>(GetUrl(sapi, v1, "broker/subAccount"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<List<BinanceBrokerSubAccount>>> GetSubAccountsAsync(string? subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("subAccountId", subAccountId);
        parameters.AddOptional("page", page?.ToString(BinanceConstants.CI));
        parameters.AddOptional("size", size?.ToString(BinanceConstants.CI));
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceBrokerSubAccount>>(GetUrl(sapi, v1, "broker/subAccount"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerEnableFuturesResult>> EnableFuturesAsync(string subAccountId, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));
        var parameters = new ParameterCollection
        {
            {"subAccountId", subAccountId},
            {"futures", true},  // only true for now
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerEnableFuturesResult>(GetUrl(sapi, v1, "broker/subAccount/futures"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerEnableMarginResult>> EnableMarginAsync(string subAccountId, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new ParameterCollection
        {
            {"subAccountId", subAccountId},
            {"margin", true},  // only true for now
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerEnableMarginResult>(GetUrl(sapi, v1, "broker/subAccount/margin"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerEnableLeverageTokenResult>> EnableLeverageTokenAsync(string subAccountId, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new ParameterCollection
        {
            {"subAccountId", subAccountId},
            {"blvt", true},  // only true for now
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerEnableLeverageTokenResult>(GetUrl(sapi, v1, "broker/subAccount/blvt"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerApiKeyCreateResult>> CreateApiKeyAsync(string subAccountId, bool isSpotTradingEnabled, bool? isMarginTradingEnabled = null, bool? isFuturesTradingEnabled = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new ParameterCollection
        {
            {"subAccountId", subAccountId},
            {"canTrade", isSpotTradingEnabled}
        };
        parameters.AddOptional("marginTrade", isMarginTradingEnabled?.ToString().ToLower());
        parameters.AddOptional("futuresTrade", isFuturesTradingEnabled?.ToString().ToLower());
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerApiKeyCreateResult>(GetUrl(sapi, v1, "broker/subAccountApi"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerSubAccountApiKey>> SetApiKeyPermissionAsync(string subAccountId, string apiKey, bool isSpotTradingEnabled, bool isMarginTradingEnabled, bool isFuturesTradingEnabled, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));
        apiKey.ValidateNotNull(nameof(apiKey));

        var parameters = new ParameterCollection
        {
            {"subAccountId", subAccountId},
            {"subAccountApiKey", apiKey},
            {"canTrade", isSpotTradingEnabled.ToString().ToLower()},
            {"marginTrade", isMarginTradingEnabled.ToString().ToLower()},
            {"futuresTrade", isFuturesTradingEnabled.ToString().ToLower()}
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerSubAccountApiKey>(GetUrl(sapi, v1, "broker/subAccountApi/permission"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerIpRestrictionV2>> SetApiKeyIpRestrictionAsync(string subAccountId, string apiKey, BinanceBrokerIpRestrictionStatus status, string? ipAddress = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));
        apiKey.ValidateNotNull(nameof(apiKey));

        var parameters = new ParameterCollection
        {
            {"subAccountId", subAccountId},
            {"subAccountApiKey", apiKey},
        };
        parameters.AddEnum("status", status);
        parameters.AddOptional("ipAddress", ipAddress);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerIpRestrictionV2>(GetUrl(sapi, v2, "broker/subAccountApi/ipRestriction"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerIpRestrictionBase>> DeleteApiKeyIpRestrictionAsync(string subAccountId, string apiKey, string ipAddress, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));
        apiKey.ValidateNotNull(nameof(apiKey));
        ipAddress.ValidateNotNull(nameof(ipAddress));

        var parameters = new ParameterCollection
        {
            {"subAccountId", subAccountId},
            {"subAccountApiKey", apiKey},
            {"ipAddress", ipAddress}
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerIpRestrictionBase>(GetUrl(sapi, v1, "broker/subAccountApi/ipRestriction/ipList"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public async Task<RestCallResult<bool>> DeleteApiKeyAsync(string subAccountId, string apiKey, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));
        apiKey.ValidateNotNull(nameof(apiKey));

        var parameters = new ParameterCollection
        {
            {"subAccountId", subAccountId},
            {"subAccountApiKey", apiKey}
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<object>(GetUrl(sapi, v1, "broker/subAccountApi"), HttpMethod.Delete, ct, true, queryParameters: parameters, requestWeight: 0).ConfigureAwait(false);
        return result.As(result.Success);
    }

    public Task<RestCallResult<BinanceBrokerIpRestriction>> GetApiKeyIpRestrictionAsync(string subAccountId, string apiKey, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));
        apiKey.ValidateNotNull(nameof(apiKey));

        var parameters = new ParameterCollection
        {
            {"subAccountId", subAccountId},
            {"subAccountApiKey", apiKey}
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerIpRestriction>(GetUrl(sapi, v1, "broker/subAccountApi/ipRestriction"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerSubAccountApiKey>> GetApiKeyAsync(string subAccountId, string? apiKey = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new ParameterCollection
        {
            {"subAccountId", subAccountId},
        };
        parameters.AddOptional("subAccountApiKey", apiKey);
        parameters.AddOptional("page", page);
        parameters.AddOptional("size", size);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerSubAccountApiKey>(GetUrl(sapi, v1, "broker/subAccountApi"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerAccountInfo>> GetBrokerAccountAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerAccountInfo>(GetUrl(sapi, v1, "broker/info"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerChangeBnbBurnSpotAndMarginResult>> SetBnbBurnForSpotAndMarginAsync(string subAccountId, bool spotBnbBurn, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new ParameterCollection
        {
            {"subAccountId", subAccountId},
            {"spotBNBBurn", spotBnbBurn.ToString().ToLower()}
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerChangeBnbBurnSpotAndMarginResult>(GetUrl(sapi, v1, "broker/subAccount/bnbBurn/spot"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerChangeBnbBurnMarginInterestResult>> SetBnbBurnForMarginInterestAsync(string subAccountId, bool interestBnbBurn, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new ParameterCollection
        {
            {"subAccountId", subAccountId},
            {"interestBNBBurn", interestBnbBurn.ToString().ToLower()}
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerChangeBnbBurnMarginInterestResult>(GetUrl(sapi, v1, "broker/subAccount/bnbBurn/marginInterest"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceBrokerBnbBurnStatus>> GetBnbBurnStatusAsync(string subAccountId, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new ParameterCollection
        {
            {"subAccountId", subAccountId}
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBrokerBnbBurnStatus>(GetUrl(sapi, v1, "broker/subAccount/bnbBurn/status"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }
}