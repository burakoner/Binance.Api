using System;
using Microsoft.Extensions.Logging;
using QuickFix;
using QuickFix.Store;
using QuickFix.Transport;

namespace Binance.FIX.Api;

internal class BinanceFixSocketInitiator : SocketInitiator
{
    private readonly BinanceFixSessionBudgets budgets;

    internal BinanceFixSocketInitiator(
        IApplication application,
        IMessageStoreFactory storeFactory,
        SessionSettings settings,
        ILoggerFactory loggerFactory,
        IMessageFactory messageFactory,
        BinanceFixSessionBudgets budgets)
        : base(application, storeFactory, settings, loggerFactory, messageFactory)
    {
        this.budgets = budgets ?? throw new ArgumentNullException(nameof(budgets));
    }

    protected override void DoConnect(Session session, SettingsDictionary settings)
    {
        if (budgets.TryAcquireConnectionAttempt().Acquired)
        {
            ConnectSession(session, settings);
        }
    }

    protected virtual void ConnectSession(Session session, SettingsDictionary settings)
        => base.DoConnect(session, settings);
}
