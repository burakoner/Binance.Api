namespace Binance.Api.Margin;

internal partial class BinanceMarginRestClient
{
    public Task<RestCallResult<BinanceMarginListenToken>> CreateUserDataStreamAsync(
        string? symbol = null,
        bool? isIsolated = null,
        long? validity = null,
        CancellationToken ct = default)
    {
        if (isIsolated == true && string.IsNullOrEmpty(symbol))
            throw new ArgumentException("Symbol is required for an isolated Margin listen token.", nameof(symbol));
        if (validity > 86_400_000)
            throw new ArgumentOutOfRangeException(nameof(validity), "Listen token validity cannot exceed 24 hours (86400000 milliseconds).");

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("isIsolated", isIsolated?.ToString().ToLowerInvariant());
        parameters.AddOptional("validity", validity);

        return RequestAsync<BinanceMarginListenToken>(
            GetUrl(sapi, v1, "userListenToken"),
            HttpMethod.Post,
            ct,
            false,
            bodyParameters: parameters,
            requestWeight: 1);
    }
}
