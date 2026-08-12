using System.Reflection;
using Xunit;

namespace Binance.FIX.Api.Tests;

public class RoleLimitBudgetTests
{
    [Theory]
    [InlineData(BinanceFixSessionRole.OrderEntry, 10_000, 10, 15, 30, 10, null)]
    [InlineData(BinanceFixSessionRole.DropCopy, 60, 60, 15, 30, 10, null)]
    [InlineData(BinanceFixSessionRole.MarketData, 2_000, 60, 300, 300, 100, 1_000)]
    public void ExposesExactPublishedRoleLimits(
        BinanceFixSessionRole role,
        int messageLimit,
        int messageWindowSeconds,
        int connectionAttemptLimit,
        int connectionAttemptWindowSeconds,
        int maximumConcurrentConnections,
        int? maximumStreams)
    {
        var options = new BinanceFixSessionOptions(
            BinanceFixEnvironment.SpotTestnet,
            role,
            "SESSION");

        Assert.Equal(messageLimit, options.Limits.OutboundMessages.MaximumCount);
        Assert.Equal(TimeSpan.FromSeconds(messageWindowSeconds), options.Limits.OutboundMessages.Window);
        Assert.Equal(connectionAttemptLimit, options.Limits.ConnectionAttempts.MaximumCount);
        Assert.Equal(TimeSpan.FromSeconds(connectionAttemptWindowSeconds), options.Limits.ConnectionAttempts.Window);
        Assert.Equal(maximumConcurrentConnections, options.Limits.MaximumConcurrentConnectionsPerAccount);
        Assert.Equal(maximumStreams, options.Limits.MaximumMarketDataStreamsPerConnection);
    }

    [Fact]
    public void PublishedLimitMetadataIsImmutable()
    {
        var types = new[] { typeof(BinanceFixSessionLimits), typeof(BinanceFixWindowLimit) };

        foreach (var type in types)
        {
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            Assert.NotEmpty(properties);
            Assert.All(properties, property => Assert.False(property.CanWrite, $"{type.Name}.{property.Name}"));
            Assert.Empty(type.GetConstructors(BindingFlags.Instance | BindingFlags.Public));
        }
    }

    [Fact]
    public void RollingBudgetRejectsAtLimitAndReleasesAtExactWindowBoundary()
    {
        var timeProvider = new ManualTimeProvider();
        var budget = new BinanceFixRollingWindowBudget(
            new BinanceFixWindowLimit(3, TimeSpan.FromSeconds(10)),
            timeProvider);

        Assert.True(budget.TryAcquire().Acquired);
        Assert.True(budget.TryAcquire().Acquired);
        Assert.True(budget.TryAcquire().Acquired);

        var rejected = budget.TryAcquire();
        Assert.False(rejected.Acquired);
        Assert.Equal(new BinanceFixBudgetSnapshot(3, 3, 0, TimeSpan.FromSeconds(10)), rejected.Snapshot);

        timeProvider.Advance(TimeSpan.FromSeconds(10) - TimeSpan.FromTicks(1));
        Assert.False(budget.TryAcquire().Acquired);

        timeProvider.Advance(TimeSpan.FromTicks(1));
        var acquiredAtBoundary = budget.TryAcquire();
        Assert.True(acquiredAtBoundary.Acquired);
        Assert.Equal(new BinanceFixBudgetSnapshot(3, 1, 2, TimeSpan.FromSeconds(10)), acquiredAtBoundary.Snapshot);
    }

    [Fact]
    public void ObservingBudgetDoesNotConsumeCapacity()
    {
        var budget = new BinanceFixRollingWindowBudget(
            new BinanceFixWindowLimit(2, TimeSpan.FromSeconds(5)),
            new ManualTimeProvider());

        Assert.Equal(new BinanceFixBudgetSnapshot(2, 0, 2, TimeSpan.Zero), budget.Observe());
        Assert.Equal(new BinanceFixBudgetSnapshot(2, 0, 2, TimeSpan.Zero), budget.Observe());
        Assert.True(budget.TryAcquire().Acquired);
        Assert.Equal(new BinanceFixBudgetSnapshot(2, 1, 1, TimeSpan.FromSeconds(5)), budget.Observe());
    }

    [Fact]
    public void ConcurrentAttemptsCannotOversubscribeRollingBudget()
    {
        const int limit = 20;
        var budget = new BinanceFixRollingWindowBudget(
            new BinanceFixWindowLimit(limit, TimeSpan.FromMinutes(1)),
            new ManualTimeProvider());
        var acquired = 0;

        Parallel.For(0, 200, _ =>
        {
            if (budget.TryAcquire().Acquired)
            {
                Interlocked.Increment(ref acquired);
            }
        });

        Assert.Equal(limit, acquired);
        Assert.Equal(new BinanceFixBudgetSnapshot(limit, limit, 0, TimeSpan.FromMinutes(1)), budget.Observe());
    }

    private sealed class ManualTimeProvider : TimeProvider
    {
        private long timestamp;

        public override long TimestampFrequency => TimeSpan.TicksPerSecond;

        public override long GetTimestamp() => Interlocked.Read(ref timestamp);

        internal void Advance(TimeSpan elapsed)
        {
            if (elapsed < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(elapsed));
            }

            Interlocked.Add(ref timestamp, elapsed.Ticks);
        }
    }
}
