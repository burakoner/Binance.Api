namespace Binance.Api.Margin;

internal partial class BinanceMarginRestClient
{
    public async Task<RestCallResult<string>> StartRiskDataStreamAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceListenKey>(
            GetUrl(sapi, v1, "margin/listen-key"),
            HttpMethod.Post,
            ct,
            false,
            requestWeight: 1).ConfigureAwait(false);
        return result.As(result.Data?.ListenKey!);
    }

    public async Task<RestCallResult<bool>> KeepAliveRiskDataStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new ParameterCollection { { "listenKey", listenKey } };
        var result = await RequestAsync<object>(
            GetUrl(sapi, v1, "margin/listen-key"),
            HttpMethod.Put,
            ct,
            false,
            bodyParameters: parameters,
            requestWeight: 1).ConfigureAwait(false);
        return result.As(result.Success);
    }

    public async Task<RestCallResult<bool>> CloseRiskDataStreamAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<object>(
            GetUrl(sapi, v1, "margin/listen-key"),
            HttpMethod.Delete,
            ct,
            false,
            requestWeight: 3_000).ConfigureAwait(false);
        return result.As(result.Success);
    }
}
