﻿namespace Binance.Api.Pay;

internal partial class BinancePayRestClientMerchant(BinancePayRestClient parent) : IBinancePayRestClientMerchant
{
    // Api
    private const string v1 = "1";
    private const string sapi = "sapi";

    // Parent
    private BinanceRestApiClient __ { get; } = parent._;
    private BinancePayRestClient _ { get; } = parent;

    // Internal
    private ILogger Logger => _.Logger;
    private BinanceRestApiClientOptions Options => __.ApiOptions;

    private Task<RestCallResult<T>> RequestAsync<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object>? queryParameters = null,
        Dictionary<string, object>? bodyParameters = null,
        Dictionary<string, string>? headerParameters = null,
        ArraySerialization? serialization = null,
        JsonSerializer? deserializer = null,
        bool ignoreRatelimit = false,
        int requestWeight = 1) where T : class
        => __.RequestAsync<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    private Uri GetUrl(string api, string version, string endpoint)
    {
        var url = BinanceAddress.Default.PayMerchantRestApiAddress;
        if (!string.IsNullOrEmpty(api)) url = url.AppendPath($"{api}");
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }
}