using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using QuickFix;
using QuickFix.Fields;
using QuickFix.Store;

namespace Binance.FIX.Api;

internal sealed class BinanceFixSessionCore : IAsyncDisposable
{
    private static readonly TimeSpan MaximumGracefulLogoutWait = TimeSpan.FromSeconds(10);

    private readonly object lifecycleGate = new();
    private readonly BinanceFixEd25519Credentials credentials;
    private readonly BinanceFixQuickFixApplication application;
    private readonly IInitiator initiator;
    private readonly BinanceFixVerifiedDataDictionary? dataDictionary;
    private readonly SessionID sessionId;
    private readonly TimeSpan gracefulLogoutWait;
    private BinanceFixSessionCoreState state;
    private Task<BinanceFixSessionStopOutcome>? stopTask;
    private bool credentialsDisposed;

    internal BinanceFixSessionCore(
        BinanceFixSessionOptions options,
        BinanceFixEd25519Credentials credentials,
        BinanceFixSessionBudgets budgets)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(credentials);
        ArgumentNullException.ThrowIfNull(budgets);

        if (!ReferenceEquals(options.Limits, budgets.Limits))
        {
            throw new ArgumentException(
                "Session budgets must match the selected Binance FIX role.",
                nameof(budgets));
        }

        this.credentials = credentials;
        Budgets = budgets;

        dataDictionary = BinanceFixDataDictionaryVerifier.VerifyRequired(
            options.Role,
            options.DataDictionaryPath);
        try
        {
            sessionId = BinanceFixQuickFixSessionSettings.CreateSessionId(options);
            var settings = BinanceFixQuickFixSessionSettings.Create(options, sessionId, dataDictionary);
            application = new BinanceFixQuickFixApplication(options, credentials, Budgets);
            initiator = new BinanceFixSocketInitiator(
                application,
                new MemoryStoreFactory(),
                settings,
                NullLoggerFactory.Instance,
                new DefaultMessageFactory(),
                Budgets);
        }
        catch
        {
            dataDictionary.Dispose();
            throw;
        }

        gracefulLogoutWait = TimeSpan.FromSeconds(
            Math.Min(options.HeartbeatIntervalSeconds, MaximumGracefulLogoutWait.TotalSeconds));
    }

    internal BinanceFixSessionCore(
        BinanceFixEd25519Credentials credentials,
        BinanceFixQuickFixApplication application,
        BinanceFixSessionBudgets budgets,
        IInitiator initiator,
        SessionID sessionId,
        TimeSpan gracefulLogoutWait)
    {
        this.credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
        this.application = application ?? throw new ArgumentNullException(nameof(application));
        Budgets = budgets ?? throw new ArgumentNullException(nameof(budgets));
        this.initiator = initiator ?? throw new ArgumentNullException(nameof(initiator));
        this.sessionId = sessionId ?? throw new ArgumentNullException(nameof(sessionId));
        if (gracefulLogoutWait <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(gracefulLogoutWait),
                gracefulLogoutWait,
                "Graceful logout wait must be positive.");
        }

        this.gracefulLogoutWait = gracefulLogoutWait;
    }

    internal BinanceFixSessionBudgets Budgets { get; }

    internal void Start(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        lock (lifecycleGate)
        {
            if (state is not BinanceFixSessionCoreState.Created)
            {
                throw new InvalidOperationException("A Binance FIX session core can only be started once.");
            }

            state = BinanceFixSessionCoreState.Started;
            try
            {
                initiator.Start();
            }
            catch
            {
                state = BinanceFixSessionCoreState.Stopping;
                ForceStop();
                state = BinanceFixSessionCoreState.Stopped;
                throw;
            }
        }
    }

    internal BinanceFixDeliveryStatus SendMutation(Message message)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (!message.Header.IsSetField(Tags.MsgType) ||
            Message.IsAdminMsgType(message.Header.GetString(Tags.MsgType)))
        {
            throw new ArgumentException("Only FIX application messages can be sent as mutations.", nameof(message));
        }

        lock (lifecycleGate)
        {
            if (state is not BinanceFixSessionCoreState.Started || !initiator.IsLoggedOn)
            {
                return BinanceFixDeliveryStatus.NotSent;
            }
        }

        try
        {
            return Session.SendToTarget(message, sessionId)
                ? BinanceFixDeliveryStatus.UnknownDelivery
                : BinanceFixDeliveryStatus.NotSent;
        }
        catch (SessionNotFound)
        {
            return BinanceFixDeliveryStatus.NotSent;
        }
        catch (Exception exception) when (exception is IOException or SocketException or ObjectDisposedException)
        {
            Session.LookupSession(sessionId)?.Disconnect("Ambiguous outbound FIX mutation write");
            return BinanceFixDeliveryStatus.UnknownDelivery;
        }
    }

    internal Task<BinanceFixSessionStopOutcome> StopAsync(CancellationToken cancellationToken = default)
    {
        lock (lifecycleGate)
        {
            stopTask ??= StopCoreAsync(state, cancellationToken);
            if (state is not BinanceFixSessionCoreState.Disposed)
            {
                state = BinanceFixSessionCoreState.Stopping;
            }

            return stopTask;
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            await StopAsync().ConfigureAwait(false);
        }
        finally
        {
            lock (lifecycleGate)
            {
                if (state is not BinanceFixSessionCoreState.Disposed)
                {
                    try
                    {
                        initiator.Dispose();
                    }
                    finally
                    {
                        try
                        {
                            dataDictionary?.Dispose();
                        }
                        finally
                        {
                            if (!credentialsDisposed)
                            {
                                credentials.Dispose();
                                credentialsDisposed = true;
                            }

                            state = BinanceFixSessionCoreState.Disposed;
                        }
                    }
                }
            }
        }

        GC.SuppressFinalize(this);
    }

    private async Task<BinanceFixSessionStopOutcome> StopCoreAsync(
        BinanceFixSessionCoreState initialState,
        CancellationToken cancellationToken)
    {
        var outcome = BinanceFixSessionStopOutcome.NoActiveSession;

        try
        {
            if (initialState is BinanceFixSessionCoreState.Started && initiator.IsLoggedOn)
            {
                var logoutResponse = application.CurrentLogoutResponse;
                var sessionEnded = application.CurrentSessionEnded;
                var session = Session.LookupSession(sessionId);
                if (session is null)
                {
                    outcome = BinanceFixSessionStopOutcome.Forced;
                }
                else
                {
                    session.Logout();
                    var timeoutOrCancellation = Task.Delay(gracefulLogoutWait, cancellationToken);
                    var completed = await Task
                        .WhenAny(logoutResponse, sessionEnded, timeoutOrCancellation)
                        .ConfigureAwait(false);
                    outcome = ReferenceEquals(completed, logoutResponse)
                        ? BinanceFixSessionStopOutcome.GracefulLogout
                        : BinanceFixSessionStopOutcome.Forced;
                }
            }
        }
        finally
        {
            await Task.Run(ForceStop, CancellationToken.None).ConfigureAwait(false);
            lock (lifecycleGate)
            {
                if (state is not BinanceFixSessionCoreState.Disposed)
                {
                    state = BinanceFixSessionCoreState.Stopped;
                }
            }
        }

        return outcome;
    }

    private void ForceStop()
    {
        if (!initiator.IsStopped)
        {
            initiator.Stop(force: true);
        }
    }
}

internal enum BinanceFixSessionStopOutcome
{
    NoActiveSession = 0,
    GracefulLogout = 1,
    Forced = 2
}

internal enum BinanceFixSessionCoreState
{
    Created = 0,
    Started = 1,
    Stopping = 2,
    Stopped = 3,
    Disposed = 4
}
