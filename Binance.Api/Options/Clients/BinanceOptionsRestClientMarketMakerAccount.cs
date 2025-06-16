namespace Binance.Api.Options;

internal partial class BinanceOptionsRestClientMarketMaker
{
    public Task<RestCallResult<BinanceOptionsMarketMakerAccount>> GetAccountAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsMarketMakerAccount>(GetUrl(eapi, v1, "marginAccount"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 3);
    }

    public Task<RestCallResult<BinanceOptionsMarketMakerProtection>> GetProtectionAsync(string underlying, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "underlying", underlying }
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsMarketMakerProtection>(GetUrl(eapi, v1, "mmp"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceOptionsMarketMakerAutoCancelAll>> GetCancelAllCountdownAsync(string underlying, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "underlying", underlying }
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsMarketMakerAutoCancelAll>(GetUrl(eapi, v1, "countdownCancelAll"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceOptionsMarketMakerProtection>> SetProtectionAsync(string underlying, int windowTimeInMilliseconds, int frozenTimeInMilliseconds, decimal quantityLimit, decimal deltaLimit, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "underlying", underlying },
            { "windowTimeInMilliseconds", windowTimeInMilliseconds },
            { "frozenTimeInMilliseconds", frozenTimeInMilliseconds },
            { "qtyLimit", quantityLimit },
            { "deltaLimit", deltaLimit }
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsMarketMakerProtection>(GetUrl(eapi, v1, "mmpSet"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceOptionsMarketMakerUnderlyings>> CancelAllCountdownHeartbeatAsync(IEnumerable<string> underlyings, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "underlyings", string.Join(",",underlyings) },
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsMarketMakerUnderlyings>(GetUrl(eapi, v1, "countdownCancelAllHeartBeat"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceOptionsMarketMakerProtection>> ResetProtectionAsync(string underlying, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "underlying", underlying },
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsMarketMakerProtection>(GetUrl(eapi, v1, "mmpReset"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceOptionsMarketMakerCountdown>> SetCancelAllCountdownAsync(string underlying, int countdownTime,  int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "underlyings", underlying },
            { "countdownTime", countdownTime }
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsMarketMakerCountdown>(GetUrl(eapi, v1, "countdownCancelAll"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

}