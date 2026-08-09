namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientUsd
{
    internal const string UserDataStreamPath = "ws-fapi/v1";
    internal const string StartUserDataStreamMethod = "userDataStream.start";
    internal const string KeepAliveUserDataStreamMethod = "userDataStream.ping";
    internal const string StopUserDataStreamMethod = "userDataStream.stop";
    internal const int UserDataStreamIpWeight = 1;
    internal const bool UserDataStreamRequiresApiKey = true;
    internal const bool UserDataStreamRequiresSignature = false;

    public async Task<CallResult<string>> StartUserDataStreamAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceListenKey>(
            UserDataStreamPath,
            StartUserDataStreamMethod,
            [],
            authenticated: UserDataStreamRequiresApiKey,
            sign: UserDataStreamRequiresSignature,
            weight: UserDataStreamIpWeight,
            ct: ct).ConfigureAwait(false);

        return result.As(result.Data?.ListenKey!);
    }

    public async Task<CallResult<string>> KeepAliveUserDataStreamAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceListenKey>(
            UserDataStreamPath,
            KeepAliveUserDataStreamMethod,
            [],
            authenticated: UserDataStreamRequiresApiKey,
            sign: UserDataStreamRequiresSignature,
            weight: UserDataStreamIpWeight,
            ct: ct).ConfigureAwait(false);

        return result.As(result.Data?.ListenKey!);
    }

    public async Task<CallResult<bool>> StopUserDataStreamAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<object>(
            UserDataStreamPath,
            StopUserDataStreamMethod,
            [],
            authenticated: UserDataStreamRequiresApiKey,
            sign: UserDataStreamRequiresSignature,
            weight: UserDataStreamIpWeight,
            ct: ct).ConfigureAwait(false);

        return result.As(result.Success);
    }
}
