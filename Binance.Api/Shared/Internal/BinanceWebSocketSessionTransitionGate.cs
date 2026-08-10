namespace Binance.Api.Shared;

internal sealed class BinanceWebSocketSessionTransitionGate
{
    private readonly SemaphoreSlim semaphore = new(1, 1);

    internal async Task<CallResult<T>> ExecuteAsync<T>(
        Func<Task<CallResult<T>>> transition,
        CancellationToken ct)
    {
        try
        {
            await semaphore.WaitAsync(ct).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            return new CallResult<T>(new CancellationRequestedError());
        }

        try
        {
            return await transition().ConfigureAwait(false);
        }
        finally
        {
            semaphore.Release();
        }
    }
}
