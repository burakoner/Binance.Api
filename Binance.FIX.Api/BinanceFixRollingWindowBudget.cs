using System;
using System.Collections.Generic;

namespace Binance.FIX.Api;

internal sealed class BinanceFixRollingWindowBudget
{
    private readonly object gate = new();
    private readonly Queue<long> acceptedTimestamps = new();
    private readonly BinanceFixWindowLimit limit;
    private readonly TimeProvider timeProvider;

    internal BinanceFixRollingWindowBudget(
        BinanceFixWindowLimit limit,
        TimeProvider? timeProvider = null)
    {
        this.limit = limit ?? throw new ArgumentNullException(nameof(limit));
        this.timeProvider = timeProvider ?? TimeProvider.System;
    }

    internal BinanceFixBudgetDecision TryAcquire()
    {
        lock (gate)
        {
            var now = timeProvider.GetTimestamp();
            RemoveExpired(now);

            var acquired = acceptedTimestamps.Count < limit.MaximumCount;
            if (acquired)
            {
                acceptedTimestamps.Enqueue(now);
            }

            return new BinanceFixBudgetDecision(acquired, CreateSnapshot(now));
        }
    }

    internal BinanceFixBudgetSnapshot Observe()
    {
        lock (gate)
        {
            var now = timeProvider.GetTimestamp();
            RemoveExpired(now);
            return CreateSnapshot(now);
        }
    }

    private void RemoveExpired(long now)
    {
        while (acceptedTimestamps.TryPeek(out var acceptedAt) &&
               timeProvider.GetElapsedTime(acceptedAt, now) >= limit.Window)
        {
            acceptedTimestamps.Dequeue();
        }
    }

    private BinanceFixBudgetSnapshot CreateSnapshot(long now)
    {
        var used = acceptedTimestamps.Count;
        var oldestEntryExpiresAfter = TimeSpan.Zero;

        if (acceptedTimestamps.TryPeek(out var acceptedAt))
        {
            var elapsed = timeProvider.GetElapsedTime(acceptedAt, now);
            oldestEntryExpiresAfter = elapsed >= limit.Window
                ? TimeSpan.Zero
                : limit.Window - elapsed;
        }

        return new BinanceFixBudgetSnapshot(
            limit.MaximumCount,
            used,
            limit.MaximumCount - used,
            oldestEntryExpiresAfter);
    }
}

internal readonly record struct BinanceFixBudgetDecision(
    bool Acquired,
    BinanceFixBudgetSnapshot Snapshot);

internal readonly record struct BinanceFixBudgetSnapshot(
    int Limit,
    int Used,
    int Remaining,
    TimeSpan OldestEntryExpiresAfter);
