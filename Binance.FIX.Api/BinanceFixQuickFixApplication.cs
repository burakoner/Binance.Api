using System;
using System.Threading.Tasks;
using QuickFix;
using QuickFix.Fields;

namespace Binance.FIX.Api;

internal sealed class BinanceFixQuickFixApplication : IApplication
{
    private readonly object lifecycleGate = new();
    private readonly BinanceFixSessionOptions options;
    private readonly BinanceFixEd25519Credentials credentials;
    private readonly BinanceFixSessionBudgets budgets;
    private TaskCompletionSource logoutResponse = CreateLogoutResponse();
    private TaskCompletionSource sessionEnded = CreateLifecycleSignal();
    private bool validatedInboundLogoutObserved;

    internal BinanceFixQuickFixApplication(
        BinanceFixSessionOptions options,
        BinanceFixEd25519Credentials credentials,
        BinanceFixSessionBudgets budgets)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));
        this.credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
        this.budgets = budgets ?? throw new ArgumentNullException(nameof(budgets));
    }

    internal Task CurrentLogoutResponse
    {
        get
        {
            lock (lifecycleGate)
            {
                return logoutResponse.Task;
            }
        }
    }

    internal Task CurrentSessionEnded
    {
        get
        {
            lock (lifecycleGate)
            {
                return sessionEnded.Task;
            }
        }
    }

    public void ToAdmin(Message message, SessionID sessionId)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(sessionId);

        var messageType = message.Header.GetString(Tags.MsgType);
        if (string.Equals(messageType, MsgType.RESEND_REQUEST, StringComparison.Ordinal))
        {
            Disconnect(sessionId, "Binance does not support ResendRequest");
            throw new DoNotSend();
        }

        if (!budgets.TryAcquireOutboundMessage().Acquired)
        {
            if (string.Equals(messageType, MsgType.LOGOUT, StringComparison.Ordinal))
            {
                lock (lifecycleGate)
                {
                    validatedInboundLogoutObserved = false;
                }
            }

            Disconnect(sessionId, "Local Binance FIX outbound-message budget exhausted");
            throw new DoNotSend();
        }

        if (string.Equals(messageType, MsgType.LOGON, StringComparison.Ordinal))
        {
            BinanceFixLogonAuthenticator.Apply(message, options, credentials);
        }
    }

    public void FromAdmin(Message message, SessionID sessionId)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(sessionId);

        var messageType = message.Header.GetString(Tags.MsgType);
        if (!string.Equals(messageType, MsgType.RESEND_REQUEST, StringComparison.Ordinal))
        {
            if (string.Equals(messageType, MsgType.LOGOUT, StringComparison.Ordinal))
            {
                lock (lifecycleGate)
                {
                    validatedInboundLogoutObserved = true;
                }
            }

            return;
        }

        Disconnect(sessionId, "Binance does not support ResendRequest");
        throw new QuickFIXException("Binance does not support ResendRequest.");
    }

    public void ToApp(Message message, SessionID sessionId)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(sessionId);

        if (!budgets.TryAcquireOutboundMessage().Acquired)
        {
            throw new DoNotSend();
        }
    }

    public void FromApp(Message message, SessionID sessionId)
    {
        // Typed role clients attach application-message handling in later slices.
    }

    public void OnCreate(SessionID sessionId)
    {
    }

    public void OnLogout(SessionID sessionId)
    {
        lock (lifecycleGate)
        {
            if (validatedInboundLogoutObserved)
            {
                logoutResponse.TrySetResult();
            }

            validatedInboundLogoutObserved = false;
            sessionEnded.TrySetResult();
        }
    }

    public void OnLogon(SessionID sessionId)
    {
        lock (lifecycleGate)
        {
            validatedInboundLogoutObserved = false;

            if (logoutResponse.Task.IsCompleted)
            {
                logoutResponse = CreateLogoutResponse();
            }

            if (sessionEnded.Task.IsCompleted)
            {
                sessionEnded = CreateLifecycleSignal();
            }
        }
    }

    private static TaskCompletionSource CreateLogoutResponse()
        => CreateLifecycleSignal();

    private static TaskCompletionSource CreateLifecycleSignal()
        => new(TaskCreationOptions.RunContinuationsAsynchronously);

    private static void Disconnect(SessionID sessionId, string reason)
        => Session.LookupSession(sessionId)?.Disconnect(reason);
}
