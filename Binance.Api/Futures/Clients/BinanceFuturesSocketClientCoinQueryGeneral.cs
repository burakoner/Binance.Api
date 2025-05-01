namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientCoin
{
    public async Task<CallResult<TimeSpan>> PingAsync(CancellationToken ct = default)
    {

        var sw = Stopwatch.StartNew();
        var result = await RequestAsync<object>("ws-dapi/v1", $"ping", [], ct: ct).ConfigureAwait(false);
        sw.Stop();

        return result.Success
            ? result.As(sw.Elapsed)
            : result.AsError<TimeSpan>(result.Error!);
    }

    public async Task<CallResult<DateTime>> GetTimeAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceServerTime>("ws-dapi/v1", $"time", [], false, ct: ct).ConfigureAwait(false);
        if (!result) return result.AsError<DateTime>(result.Error!);

        return result.As(result.Data.ServerTime);
    }
}