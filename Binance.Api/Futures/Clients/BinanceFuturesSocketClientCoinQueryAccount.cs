namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientCoin
{
    internal const string AccountQueryPath = "ws-dapi/v1";
    internal const string GetBalancesMethod = "account.balance";
    internal const string GetAccountMethod = "account.status";
    internal const int AccountQueryIpWeight = 5;

    public Task<CallResult<List<BinanceFuturesCoinAccountBalance>>> GetBalancesAsync(long? receiveWindow = null, CancellationToken ct = default)
        => RequestAsync<List<BinanceFuturesCoinAccountBalance>>(
            AccountQueryPath,
            GetBalancesMethod,
            CreateAccountQueryParameters(__.ReceiveWindow(receiveWindow)),
            true,
            true,
            weight: AccountQueryIpWeight,
            ct: ct);

    public Task<CallResult<BinanceFuturesCoinAccountInfo>> GetAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default)
        => RequestAsync<BinanceFuturesCoinAccountInfo>(
            AccountQueryPath,
            GetAccountMethod,
            CreateAccountQueryParameters(__.ReceiveWindow(receiveWindow)),
            true,
            true,
            weight: AccountQueryIpWeight,
            ct: ct);

    internal static ParameterCollection CreateAccountQueryParameters(long? receiveWindow)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", ValidateQueryReceiveWindow(receiveWindow));

        return parameters;
    }

    internal static long? ValidateQueryReceiveWindow(long? receiveWindow)
        => receiveWindow > 60_000
            ? throw new ArgumentOutOfRangeException(nameof(receiveWindow), receiveWindow, "receiveWindow cannot exceed 60000 milliseconds")
            : receiveWindow;
}
