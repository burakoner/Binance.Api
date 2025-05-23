﻿namespace Binance.Api.Spot;

internal partial class BinanceSpotRestClient
{
    public async Task<RestCallResult<string>> StartUserStreamAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceListenKey>(GetUrl(api, v3, "userDataStream"), HttpMethod.Post, ct, false, requestWeight: 2);
        return result.As(result.Data?.ListenKey!);
    }

    public async Task<RestCallResult<bool>> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new ParameterCollection
        {
            { "listenKey", listenKey }
        };

        var result = await RequestAsync<object>(GetUrl(api, v3, "userDataStream"), HttpMethod.Put, ct, false, bodyParameters: parameters, requestWeight: 2);
        return result.As(result.Success);
    }

    public async Task<RestCallResult<bool>> StopUserStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new ParameterCollection
        {
            { "listenKey", listenKey }
        };

        var result = await RequestAsync<object>(GetUrl(api, v3, "userDataStream"), HttpMethod.Delete, ct, false, bodyParameters: parameters, requestWeight: 2);
        return result.As(result.Success);
    }
}