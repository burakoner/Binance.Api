namespace Binance.Api.Options;

internal partial class BinanceOptionsRestClient
{
    public Task<RestCallResult<BinanceOptionsListenKey>> StartUserStreamAsync(CancellationToken ct = default)
        => RequestAsync<BinanceOptionsListenKey>(GetUrl(eapi, v1, "listenKey"), HttpMethod.Post, ct, false, requestWeight: 1);

    public async Task<RestCallResult<bool>> KeepAliveUserStreamAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<object>(GetUrl(eapi, v1, "listenKey"), HttpMethod.Put, ct, false, requestWeight: 1).ConfigureAwait(false);
        return result.As(result.Success);
    }

    public async Task<RestCallResult<bool>> StopUserStreamAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<object>(GetUrl(eapi, v1, "listenKey"), HttpMethod.Delete, ct, false, requestWeight: 1).ConfigureAwait(false);
        return result.As(result.Success);
    }
}
