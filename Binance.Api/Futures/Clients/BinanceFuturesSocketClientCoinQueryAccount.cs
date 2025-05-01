namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientCoin
{
    public  Task<CallResult<IEnumerable<BinanceFuturesCoinAccountBalance>>> GetBalancesAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceFuturesCoinAccountBalance>>("ws-dapi/v1", $"account.balance", parameters, true, true, weight: 5, ct: ct);
    }

    public  Task<CallResult<BinanceFuturesCoinAccountInfo>> GetAccountInfoAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));
        
        return RequestAsync<BinanceFuturesCoinAccountInfo>("ws-dapi/v1", $"account.status", parameters, true, true, weight: 5, ct: ct);
    }
}