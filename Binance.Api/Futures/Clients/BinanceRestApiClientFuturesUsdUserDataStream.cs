using ApiSharp;

namespace Binance.Api.Futures;

internal partial class BinanceRestApiClientFuturesUsd
{
    /*
    #region Start User Data Stream
    /// <inheritdoc />
    public async Task<RestCallResult<string>> StartUserStreamAsync(CancellationToken ct = default)
    {
        var request = _definitions.GetOrCreate(HttpMethod.Post, "fapi/v1/listenKey", BinanceExchange.RateLimiter.FuturesRest, 1);
        var result = await _baseClient.SendAsync<Objects.Models.Spot.BinanceListenKey>(request, null, ct).ConfigureAwait(false);
        return result.As(result.Data?.ListenKey!);
    }

    #endregion

    #region Keepalive User Data Stream

    /// <inheritdoc />
    public async Task<WebCallResult> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));

        var parameters = new ParameterCollection
        {
            { "listenKey", listenKey }
        };

        var request = _definitions.GetOrCreate(HttpMethod.Put, "fapi/v1/listenKey", BinanceExchange.RateLimiter.FuturesRest, 1);
        return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    #region Close User Data Stream

    /// <inheritdoc />
    public async Task<WebCallResult> StopUserStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new ParameterCollection
        {
            { "listenKey", listenKey }
        };

        var request = _definitions.GetOrCreate(HttpMethod.Delete, "fapi/v1/listenKey", BinanceExchange.RateLimiter.FuturesRest, 1);
        return await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion
    */
}