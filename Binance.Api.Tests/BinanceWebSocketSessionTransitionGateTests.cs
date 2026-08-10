using ApiSharp.Models;
using Binance.Api.Shared;

namespace Binance.Api.Tests;

public class BinanceWebSocketSessionTransitionGateTests
{
    [Fact]
    public async Task ExecuteAsync_SerializesSessionTransitions()
    {
        var gate = new BinanceWebSocketSessionTransitionGate();
        var firstEntered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseFirst = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var secondEntered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var activeTransitions = 0;

        var first = gate.ExecuteAsync(async () =>
        {
            Assert.Equal(1, Interlocked.Increment(ref activeTransitions));
            firstEntered.SetResult();
            await releaseFirst.Task;
            Interlocked.Decrement(ref activeTransitions);
            return new CallResult<int>(1);
        }, CancellationToken.None);

        await firstEntered.Task;

        var second = gate.ExecuteAsync(() =>
        {
            Assert.Equal(1, Interlocked.Increment(ref activeTransitions));
            secondEntered.SetResult();
            Interlocked.Decrement(ref activeTransitions);
            return Task.FromResult(new CallResult<int>(2));
        }, CancellationToken.None);

        Assert.False(secondEntered.Task.IsCompleted);
        releaseFirst.SetResult();

        var results = await Task.WhenAll(first, second);

        Assert.All(results, result => Assert.True(result.Success));
        Assert.Equal([1, 2], results.Select(result => result.Data));
    }

    [Fact]
    public async Task ExecuteAsync_CanceledWaiterDoesNotRunOrPoisonFollowingTransition()
    {
        var gate = new BinanceWebSocketSessionTransitionGate();
        var firstEntered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseFirst = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var first = gate.ExecuteAsync(async () =>
        {
            firstEntered.SetResult();
            await releaseFirst.Task;
            return new CallResult<int>(1);
        }, CancellationToken.None);
        await firstEntered.Task;

        using var cts = new CancellationTokenSource();
        var canceledTransitionRan = false;
        var canceled = gate.ExecuteAsync(() =>
        {
            canceledTransitionRan = true;
            return Task.FromResult(new CallResult<int>(2));
        }, cts.Token);
        cts.Cancel();

        var canceledResult = await canceled;
        Assert.False(canceledResult.Success);
        Assert.IsType<CancellationRequestedError>(canceledResult.Error);
        Assert.False(canceledTransitionRan);

        releaseFirst.SetResult();
        Assert.True((await first).Success);

        var following = await gate.ExecuteAsync(
            () => Task.FromResult(new CallResult<int>(3)),
            CancellationToken.None);
        Assert.True(following.Success);
        Assert.Equal(3, following.Data);
    }

    [Fact]
    public async Task ExecuteAsync_OrdersLogoutBeforeReconnectAuthenticationDecision()
    {
        var gate = new BinanceWebSocketSessionTransitionGate();
        var logoutEntered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseLogout = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var reconnectEntered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var authenticationRequired = true;

        var logout = gate.ExecuteAsync(async () =>
        {
            logoutEntered.SetResult();
            await releaseLogout.Task;
            authenticationRequired = false;
            return new CallResult<bool>(true);
        }, CancellationToken.None);
        await logoutEntered.Task;

        var reconnect = gate.ExecuteAsync(() =>
        {
            reconnectEntered.SetResult();
            return Task.FromResult(new CallResult<bool>(authenticationRequired));
        }, CancellationToken.None);

        Assert.False(reconnectEntered.Task.IsCompleted);
        releaseLogout.SetResult();

        Assert.True((await logout).Success);
        var reconnectResult = await reconnect;
        Assert.True(reconnectResult.Success);
        Assert.False(reconnectResult.Data);
    }
}
