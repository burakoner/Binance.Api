namespace Binance.Api.Options;

internal partial class BinanceOptionsRestClientMarketMaker
{
    public Task<RestCallResult<BinanceOptionsMarketMakerProtection>> GetProtectionAsync(string underlying, long? receiveWindow = null, CancellationToken ct = default)
    {
        ValidateRequiredValue(underlying, nameof(underlying));
        var parameters = new ParameterCollection
        {
            { "underlying", underlying }
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsMarketMakerProtection>(GetUrl(eapi, v1, "mmp"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceOptionsMarketMakerAutoCancelAll>> GetCancelAllCountdownAsync(string? underlying = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (underlying != null)
            ValidateRequiredValue(underlying, nameof(underlying));

        var parameters = new ParameterCollection();
        parameters.AddOptional("underlying", underlying);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsMarketMakerAutoCancelAll>(GetUrl(eapi, v1, "countdownCancelAll"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceOptionsMarketMakerProtection>> SetProtectionAsync(string underlying, long windowTimeInMilliseconds, long frozenTimeInMilliseconds, decimal quantityLimit, decimal deltaLimit, long? receiveWindow = null, CancellationToken ct = default)
    {
        ValidateRequiredValue(underlying, nameof(underlying));
        if (windowTimeInMilliseconds < 0 || windowTimeInMilliseconds > 5_000)
            throw new ArgumentOutOfRangeException(nameof(windowTimeInMilliseconds), "windowTimeInMilliseconds must be between 0 and 5000 inclusive");

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

    public Task<RestCallResult<BinanceOptionsMarketMakerUnderlyings>> CancelAllCountdownHeartbeatAsync(IEnumerable<string> underlyings, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (underlyings == null)
            throw new ArgumentNullException(nameof(underlyings));
        var materializedUnderlyings = underlyings.ToList();
        if (materializedUnderlyings.Count == 0)
            throw new ArgumentException("At least one underlying must be provided", nameof(underlyings));
        foreach (var underlying in materializedUnderlyings)
            ValidateRequiredValue(underlying, nameof(underlyings));

        var parameters = new ParameterCollection
        {
            { "underlyings", string.Join(",", materializedUnderlyings) },
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsMarketMakerUnderlyings>(GetUrl(eapi, v1, "countdownCancelAllHeartBeat"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<BinanceOptionsMarketMakerProtection>> ResetProtectionAsync(string underlying, long? receiveWindow = null, CancellationToken ct = default)
    {
        ValidateRequiredValue(underlying, nameof(underlying));
        var parameters = new ParameterCollection
        {
            { "underlying", underlying },
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsMarketMakerProtection>(GetUrl(eapi, v1, "mmpReset"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceOptionsMarketMakerCountdown>> SetCancelAllCountdownAsync(string underlying, long countdownTime, long? receiveWindow = null, CancellationToken ct = default)
    {
        ValidateRequiredValue(underlying, nameof(underlying));
        if (countdownTime != 0 && countdownTime < 5_000)
            throw new ArgumentOutOfRangeException(nameof(countdownTime), "countdownTime must be 0 or at least 5000 milliseconds");

        var parameters = new ParameterCollection
        {
            { "underlying", underlying },
            { "countdownTime", countdownTime }
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsMarketMakerCountdown>(GetUrl(eapi, v1, "countdownCancelAll"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    private static void ValidateRequiredValue(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{parameterName} must be provided", parameterName);
    }
}
