namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientCoin
{
    public  Task<CallResult<IEnumerable<BinanceCoinFuturesAccountBalance>>> GetBalancesAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture));

        return RequestAsync<IEnumerable<BinanceCoinFuturesAccountBalance>>("ws-dapi/v1", $"account.balance", parameters, true, true, weight: 5, ct: ct);
    }

    public  Task<CallResult<BinanceFuturesCoinAccountInfo>> GetAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture));
        return RequestAsync<BinanceFuturesCoinAccountInfo>("ws-dapi/v1", $"account.status", parameters, true, true, weight: 5, ct: ct);
    }
}