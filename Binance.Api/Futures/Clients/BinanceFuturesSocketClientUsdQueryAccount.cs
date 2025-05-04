namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientUsd
{
    public Task<CallResult<IEnumerable<BinanceFuturesUsdAccountBalance>>> GetBalancesAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture));

        return RequestAsync<IEnumerable<BinanceFuturesUsdAccountBalance>>("ws-fapi/v1", $"v2/account.balance", parameters, true, true, weight: 5, ct: ct);
    }

    public Task<CallResult<BinanceFuturesAccountInfoV3>> GetAccountAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture));
        return RequestAsync<BinanceFuturesAccountInfoV3>("ws-fapi/v1", $"v2/account.status", parameters, true, true, weight: 5, ct: ct);
    }
}