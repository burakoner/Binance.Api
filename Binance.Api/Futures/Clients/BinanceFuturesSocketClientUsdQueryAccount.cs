namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientUsd
{
    internal const string AccountQueryPath = "ws-fapi/v1";
    internal const string GetBalancesV1Method = "account.balance";
    internal const string GetBalancesV2Method = "v2/account.balance";
    internal const string GetAccountV1Method = "account.status";
    internal const string GetAccountV2Method = "v2/account.status";
    internal const int AccountQueryIpWeight = 5;

    public Task<CallResult<List<BinanceFuturesUsdAccountBalance>>> GetBalancesV1Async(long? receiveWindow = null, CancellationToken ct = default)
        => RequestAsync<List<BinanceFuturesUsdAccountBalance>>(
            AccountQueryPath,
            GetBalancesV1Method,
            CreateAccountQueryParameters(__.ReceiveWindow(receiveWindow)),
            true,
            true,
            weight: AccountQueryIpWeight,
            ct: ct);

    public Task<CallResult<List<BinanceFuturesUsdAccountBalance>>> GetBalancesAsync(long? receiveWindow = null, CancellationToken ct = default)
        => RequestAsync<List<BinanceFuturesUsdAccountBalance>>(
            AccountQueryPath,
            GetBalancesV2Method,
            CreateAccountQueryParameters(__.ReceiveWindow(receiveWindow)),
            true,
            true,
            weight: AccountQueryIpWeight,
            ct: ct);

    public Task<CallResult<BinanceFuturesAccountInfoV2>> GetAccountV1Async(long? receiveWindow = null, CancellationToken ct = default)
        => RequestAsync<BinanceFuturesAccountInfoV2>(
            AccountQueryPath,
            GetAccountV1Method,
            CreateAccountQueryParameters(__.ReceiveWindow(receiveWindow)),
            true,
            true,
            weight: AccountQueryIpWeight,
            ct: ct);

    public Task<CallResult<BinanceFuturesAccountInfo>> GetAccountAsync(long? receiveWindow = null, CancellationToken ct = default)
        => RequestAsync<BinanceFuturesAccountInfo>(
            AccountQueryPath,
            GetAccountV2Method,
            CreateAccountQueryParameters(__.ReceiveWindow(receiveWindow)),
            true,
            true,
            weight: AccountQueryIpWeight,
            ct: ct);

    internal static ParameterCollection CreateAccountQueryParameters(long? receiveWindow)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", receiveWindow);

        return parameters;
    }
}
