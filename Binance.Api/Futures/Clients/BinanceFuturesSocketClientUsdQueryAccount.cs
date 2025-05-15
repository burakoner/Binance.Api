namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientUsd
{
    public Task<CallResult<List<BinanceFuturesUsdAccountBalance>>> GetBalancesAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceFuturesUsdAccountBalance>>("ws-fapi/v1", $"v2/account.balance", parameters, true, true, weight: 5, ct: ct);
    }

    public Task<CallResult<BinanceFuturesAccountInfoV3>> GetAccountAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesAccountInfoV3>("ws-fapi/v1", $"v2/account.status", parameters, true, true, weight: 5, ct: ct);
    }
}