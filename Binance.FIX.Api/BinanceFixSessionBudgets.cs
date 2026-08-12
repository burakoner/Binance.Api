using System;

namespace Binance.FIX.Api;

internal sealed class BinanceFixSessionBudgets
{
    private readonly BinanceFixRollingWindowBudget outboundMessages;
    private readonly BinanceFixRollingWindowBudget connectionAttempts;

    internal BinanceFixSessionBudgets(
        BinanceFixSessionLimits limits,
        TimeProvider? timeProvider = null)
    {
        ArgumentNullException.ThrowIfNull(limits);
        Limits = limits;

        outboundMessages = new BinanceFixRollingWindowBudget(limits.OutboundMessages, timeProvider);
        connectionAttempts = new BinanceFixRollingWindowBudget(limits.ConnectionAttempts, timeProvider);
    }

    internal BinanceFixSessionBudgets(
        BinanceFixSessionLimits limits,
        BinanceFixRollingWindowBudget sharedConnectionAttempts,
        TimeProvider? timeProvider = null)
    {
        ArgumentNullException.ThrowIfNull(limits);
        ArgumentNullException.ThrowIfNull(sharedConnectionAttempts);
        Limits = limits;

        if (sharedConnectionAttempts.Limit.MaximumCount != limits.ConnectionAttempts.MaximumCount ||
            sharedConnectionAttempts.Limit.Window != limits.ConnectionAttempts.Window)
        {
            throw new ArgumentException(
                "Shared connection-attempt budget must match the session role limits.",
                nameof(sharedConnectionAttempts));
        }

        outboundMessages = new BinanceFixRollingWindowBudget(limits.OutboundMessages, timeProvider);
        connectionAttempts = sharedConnectionAttempts;
    }

    internal BinanceFixSessionLimits Limits { get; }

    internal BinanceFixBudgetDecision TryAcquireOutboundMessage()
        => outboundMessages.TryAcquire();

    internal BinanceFixBudgetDecision TryAcquireConnectionAttempt()
        => connectionAttempts.TryAcquire();

    internal BinanceFixSessionBudgetSnapshot Observe()
        => new(outboundMessages.Observe(), connectionAttempts.Observe());
}

internal readonly record struct BinanceFixSessionBudgetSnapshot(
    BinanceFixBudgetSnapshot OutboundMessages,
    BinanceFixBudgetSnapshot ConnectionAttempts);
