namespace Binance.Api.Futures;

internal partial class BinanceFuturesRestClientUsd
{
    public async Task<RestCallResult<string>> StartUserStreamAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceListenKey>(GetUrl(fapi, v1, "listenKey"), HttpMethod.Post, ct, false, requestWeight: 1);
        return result.As(result.Data?.ListenKey!);
    }

    public async Task<RestCallResult<string>> KeepAliveUserStreamAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceListenKey>(GetUrl(fapi, v1, "listenKey"), HttpMethod.Put, ct, false, requestWeight: 1);
        return result.As(result.Data?.ListenKey!);
    }

    public async Task<RestCallResult<bool>> StopUserStreamAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<object>(GetUrl(fapi, v1, "listenKey"), HttpMethod.Delete, ct, false, requestWeight: 1);
        return result.As(result.Success);
    }
}
