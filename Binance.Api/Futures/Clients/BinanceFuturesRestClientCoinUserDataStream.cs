namespace Binance.Api.Futures;

internal partial class BinanceFuturesRestClientCoin
{
    public async Task<RestCallResult<string>> StartUserStreamAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceListenKey>(GetUrl(dapi, v1, "listenKey"), HttpMethod.Post, ct, true, requestWeight: 1);
        return result.As(result.Data?.ListenKey!);
    }

    public async Task<RestCallResult<bool>> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));

        var parameters = new ParameterCollection
        {
            { "listenKey", listenKey }
        };

        var result = await RequestAsync<object>(GetUrl(dapi, v1, "listenKey"), HttpMethod.Put, ct, true, bodyParameters: parameters, requestWeight: 2);
        return result.As(result.Success);
    }

    public async Task<RestCallResult<bool>> StopUserStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new ParameterCollection
        {
            { "listenKey", listenKey }
        };

        var result = await RequestAsync<object>(GetUrl(dapi, v1, "listenKey"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 2);
        return result.As(result.Success);
    }
}