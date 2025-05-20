namespace Binance.Api.SubAccount;

internal partial class BinanceSubAccountRestClient
{
    public Task<RestCallResult<BinanceSubAccountIpRestriction>> GetApiKeyIpRestrictionAsync(string email, string apiKey, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "email", email },
            { "subAccountApiKey", apiKey },
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountIpRestriction>(GetUrl(sapi, v1, "sub-account/subAccountApi/ipRestriction"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 3000);
    }

    public Task<RestCallResult<BinanceSubAccountIpRestriction>> RemoveApiKeyIpRestrictionAsync(string email, string apiKey, IEnumerable<string>? ipAddresses, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "email", email },
            { "subAccountApiKey", apiKey }
        };

        if (ipAddresses != null)
            parameters.AddOptional("ipAddress", string.Join(",", ipAddresses));
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountIpRestriction>(GetUrl(sapi, v1, "sub-account/subAccountApi/ipRestriction/ipList"), HttpMethod.Delete, ct, true, queryParameters: parameters, requestWeight: 3000);
    }

    public Task<RestCallResult<BinanceSubAccountIpRestriction>> SetApiKeyIpRestrictionAsync(string email, string apiKey, bool ipRestrict, IEnumerable<string>? ipAddresses, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "email", email },
            { "subAccountApiKey", apiKey },
            { "status", ipRestrict ? 2: 1 }
        };

        if (ipAddresses != null)
            parameters.AddOptional("ipAddress", string.Join(",", ipAddresses));
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountIpRestriction>(GetUrl(sapi, v2, "sub-account/subAccountApi/ipRestriction"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 3000);
    }
}