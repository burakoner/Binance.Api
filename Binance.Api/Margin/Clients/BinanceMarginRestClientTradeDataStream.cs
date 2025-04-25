using Binance.Api.Spot;

namespace Binance.Api.Margin;

internal partial class BinanceMarginRestClient
{

    public async Task<RestCallResult<string>> StartMarginUserStreamAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceListenKey>(GetUrl(sapi, v1, "userDataStream"), HttpMethod.Post, ct, true, requestWeight: 1).ConfigureAwait(false);
        return result.As(result.Data?.ListenKey!);
    }

    public async Task<RestCallResult<bool>> KeepAliveMarginUserStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new ParameterCollection
        {
            { "listenKey", listenKey },
        };

        var result = await RequestAsync<object>(GetUrl(sapi, v1, "userDataStream"), HttpMethod.Put, ct, true, bodyParameters: parameters, requestWeight: 1);
        return result.As(result.Success);
    }

    public async Task<RestCallResult<bool>> StopMarginUserStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new ParameterCollection
            {
                { "listenKey", listenKey }
            };

        var result = await RequestAsync<object>(GetUrl(sapi, v1, "userDataStream"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 1);
        return result.As(result.Success);
    }

    public async Task<RestCallResult<string>> StartIsolatedMarginUserStreamAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            {"symbol", symbol}
        };

        var result = await RequestAsync<BinanceListenKey>(GetUrl(sapi, v1, "userDataStream/isolated"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
        return result.As(result.Data?.ListenKey!);
    }

    public async Task<RestCallResult<bool>> KeepAliveIsolatedMarginUserStreamAsync(string symbol, string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new ParameterCollection
            {
                { "listenKey", listenKey },
                {"symbol", symbol}
            };

        var result = await RequestAsync<object>(GetUrl(sapi, v1, "userDataStream/isolated"), HttpMethod.Put, ct, true, bodyParameters: parameters, requestWeight: 1);
        return result.As(result.Success);
    }

    public async Task<RestCallResult<bool>> CloseIsolatedMarginUserStreamAsync(string symbol, string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new ParameterCollection
        {
            { "listenKey", listenKey },
            { "symbol", symbol}
        };

        var result = await RequestAsync<object>(GetUrl(sapi, v1, "userDataStream/isolated"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 1);
        return result.As(result.Success);
    }

}