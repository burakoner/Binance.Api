using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging.Abstractions;
using QuickFix;
using QuickFix.Fields;
using QuickFix.Store;
using QuickFix.Transport;
using Xunit;

namespace Binance.FIX.Api.Tests;

public class SessionCoreIntegrationTests
{
    private const string OfficialPrivateKey = """
        -----BEGIN PRIVATE KEY-----
        MC4CAQAwBQYDK2VwBCIEIIJEYWtGBrhACmb9Dvy+qa8WEf0lQOl1s4CLIAB9m89u
        -----END PRIVATE KEY-----
        """;

    [Theory]
    [InlineData(BinanceFixSessionRole.OrderEntry, 2)]
    [InlineData(BinanceFixSessionRole.DropCopy, 2)]
    [InlineData(BinanceFixSessionRole.MarketData, 1)]
    public void ProjectsFailClosedQuickFixSessionSettings(
        BinanceFixSessionRole role,
        int expectedReconnectIntervalSeconds)
    {
        var options = CreateOptions(role);
        var sessionId = BinanceFixQuickFixSessionSettings.CreateSessionId(options);

        var settings = BinanceFixQuickFixSessionSettings.Create(options, sessionId);
        var session = settings.Get(sessionId);

        Assert.Equal("FIX.4.4", sessionId.BeginString);
        Assert.Equal(options.SenderCompId, sessionId.SenderCompID);
        Assert.Equal("SPOT", sessionId.TargetCompID);
        Assert.Equal("initiator", session.GetString(SessionSettings.CONNECTION_TYPE));
        Assert.False(session.GetBool(SessionSettings.USE_DATA_DICTIONARY));
        Assert.True(session.GetBool(SessionSettings.NON_STOP_SESSION));
        Assert.Equal(options.HeartbeatIntervalSeconds, session.GetLong(SessionSettings.HEARTBTINT));
        Assert.False(session.GetBool(SessionSettings.PERSIST_MESSAGES));
        Assert.True(session.GetBool(SessionSettings.RESET_ON_LOGON));
        Assert.True(session.GetBool(SessionSettings.RESET_ON_LOGOUT));
        Assert.True(session.GetBool(SessionSettings.RESET_ON_DISCONNECT));
        Assert.False(session.GetBool(SessionSettings.SEND_REDUNDANT_RESENDREQUESTS));
        Assert.False(session.GetBool(SessionSettings.SEND_LOGOUT_BEFORE_TIMEOUT_DISCONNECT));
        Assert.Equal(expectedReconnectIntervalSeconds, settings.Get().GetLong(SessionSettings.RECONNECT_INTERVAL));
        Assert.True(session.GetBool(SessionSettings.SSL_ENABLE));
        Assert.True(session.GetBool(SessionSettings.SSL_VALIDATE_CERTIFICATES));
        Assert.True(session.GetBool(SessionSettings.SSL_CHECK_CERTIFICATE_REVOCATION));
    }

    [Fact]
    public void BindsEveryActualConnectInvocationToTheConnectionAttemptBudget()
    {
        var options = CreateOptions(BinanceFixSessionRole.DropCopy);
        using var credentials = CreateCredentials();
        var budgets = new BinanceFixSessionBudgets(options.Limits);
        var application = new BinanceFixQuickFixApplication(options, credentials, budgets);
        var sessionId = BinanceFixQuickFixSessionSettings.CreateSessionId(options);
        var settings = BinanceFixQuickFixSessionSettings.Create(options, sessionId);
        using var initiator = new RecordingSocketInitiator(
            application,
            CreateBootstrapSettings(),
            budgets);

        Assert.True(initiator.AddSession(sessionId, settings.Get(sessionId)));
        try
        {
            var session = Assert.IsType<Session>(Session.LookupSession(sessionId));
            for (var attempt = 0; attempt < options.Limits.ConnectionAttempts.MaximumCount + 5; attempt++)
            {
                initiator.AttemptConnection(session, settings.Get(sessionId));
            }

            Assert.Equal(options.Limits.ConnectionAttempts.MaximumCount, initiator.OpenConnectionCount);
            Assert.Equal(
                options.Limits.ConnectionAttempts.MaximumCount,
                budgets.Observe().ConnectionAttempts.Used);
        }
        finally
        {
            Assert.True(initiator.RemoveSession(sessionId, terminateActiveSession: true));
        }
    }

    [Fact]
    public void SharedAccountConnectionBudgetCannotBeMultipliedByCreatingAnotherSessionBudget()
    {
        var options = CreateOptions(BinanceFixSessionRole.OrderEntry);
        var sharedConnectionAttempts = new BinanceFixRollingWindowBudget(
            options.Limits.ConnectionAttempts);
        var firstSession = new BinanceFixSessionBudgets(
            options.Limits,
            sharedConnectionAttempts);
        var secondSession = new BinanceFixSessionBudgets(
            options.Limits,
            sharedConnectionAttempts);

        for (var attempt = 0; attempt < options.Limits.ConnectionAttempts.MaximumCount; attempt++)
        {
            Assert.True(firstSession.TryAcquireConnectionAttempt().Acquired);
        }

        Assert.False(secondSession.TryAcquireConnectionAttempt().Acquired);
        Assert.Equal(0, secondSession.Observe().ConnectionAttempts.Remaining);
        Assert.Equal(0, firstSession.Observe().OutboundMessages.Used);
        Assert.Equal(0, secondSession.Observe().OutboundMessages.Used);
    }

    [Fact]
    public void SessionCoreRejectsBudgetsFromAnotherRole()
    {
        var options = CreateOptions(BinanceFixSessionRole.OrderEntry);
        using var credentials = CreateCredentials();
        var wrongRoleOptions = CreateOptions(BinanceFixSessionRole.MarketData);
        var budgets = new BinanceFixSessionBudgets(wrongRoleOptions.Limits);

        var exception = Assert.Throws<ArgumentException>(() =>
            new BinanceFixSessionCore(options, credentials, budgets));

        Assert.Equal("budgets", exception.ParamName);
    }

    [Fact]
    public void ProductionApplicationSignsLogonAndCountsEveryOutboundMessageOnce()
    {
        var options = CreateOptions(BinanceFixSessionRole.OrderEntry);
        using var credentials = CreateCredentials();
        var budgets = new BinanceFixSessionBudgets(options.Limits);
        var application = new BinanceFixQuickFixApplication(options, credentials, budgets);
        var sessionId = BinanceFixQuickFixSessionSettings.CreateSessionId(options);
        var logon = CreateOutboundMessage(MsgType.LOGON, sessionId);

        application.ToAdmin(logon, sessionId);

        Assert.Equal("test-api-key", logon.GetString(Tags.Username));
        Assert.True(logon.GetBoolean(Tags.ResetSeqNumFlag));
        Assert.Equal(1, budgets.Observe().OutboundMessages.Used);

        application.ToAdmin(CreateOutboundMessage(MsgType.HEARTBEAT, sessionId), sessionId);
        application.ToApp(CreateOutboundMessage(MsgType.ORDER_SINGLE, sessionId), sessionId);

        Assert.Equal(3, budgets.Observe().OutboundMessages.Used);
    }

    [Fact]
    public void OutboundBudgetRejectsApplicationMessageBeforeWireAcceptance()
    {
        var options = CreateOptions(BinanceFixSessionRole.DropCopy);
        using var credentials = CreateCredentials();
        var budgets = new BinanceFixSessionBudgets(options.Limits);
        var application = new BinanceFixQuickFixApplication(options, credentials, budgets);
        var sessionId = BinanceFixQuickFixSessionSettings.CreateSessionId(options);

        for (var message = 0; message < options.Limits.OutboundMessages.MaximumCount; message++)
        {
            application.ToApp(CreateOutboundMessage(MsgType.ORDER_SINGLE, sessionId), sessionId);
        }

        Assert.Throws<DoNotSend>(() =>
            application.ToApp(CreateOutboundMessage(MsgType.ORDER_SINGLE, sessionId), sessionId));
        Assert.Equal(0, budgets.Observe().OutboundMessages.Remaining);
    }

    [Fact]
    public void EngineHeartbeatAndTestRequestUseProductionOutboundGateWithoutWaitingForWallClock()
    {
        using var fixture = CreateRegisteredSession(BinanceFixSessionRole.OrderEntry);
        MarkLoggedOn(fixture.Session);

        SetSessionClock(
            fixture.Session,
            lastSent: DateTime.UtcNow - TimeSpan.FromSeconds(6),
            lastReceived: DateTime.UtcNow);
        fixture.Session.Next();

        Assert.Equal(MsgType.HEARTBEAT, GetMessageType(Assert.Single(fixture.Responder.SentMessages)));
        Assert.Equal(1, fixture.Budgets.Observe().OutboundMessages.Used);

        fixture.Responder.SentMessages.Clear();
        SetSessionClock(
            fixture.Session,
            lastSent: DateTime.UtcNow,
            lastReceived: DateTime.UtcNow - TimeSpan.FromSeconds(7));
        fixture.Session.Next();

        Assert.Equal(MsgType.TEST_REQUEST, GetMessageType(Assert.Single(fixture.Responder.SentMessages)));
        Assert.Equal(2, fixture.Budgets.Observe().OutboundMessages.Used);
    }

    [Fact]
    public void EngineDisconnectsAfterHeartbeatTimeoutWithoutInventingLogoutSuccess()
    {
        using var fixture = CreateRegisteredSession(BinanceFixSessionRole.OrderEntry);
        MarkLoggedOn(fixture.Session);
        SetSessionClock(
            fixture.Session,
            lastSent: DateTime.UtcNow,
            lastReceived: DateTime.UtcNow - TimeSpan.FromSeconds(13));

        fixture.Session.Next();

        Assert.Equal(1, fixture.Responder.DisconnectCount);
        Assert.Empty(fixture.Responder.SentMessages);
        Assert.False(fixture.Application.CurrentLogoutResponse.IsCompleted);
        Assert.True(fixture.Application.CurrentSessionEnded.IsCompleted);
    }

    [Fact]
    public async Task GracefulStopSendsLogoutWaitsForResponseThenForcesBoundedEngineStop()
    {
        using var fixture = CreateRegisteredSession(BinanceFixSessionRole.OrderEntry);
        MarkLoggedOn(fixture.Session);
        fixture.Application.OnLogon(fixture.SessionId);
        var lifecycleInitiator = new RecordingInitiator { LoggedOn = true };
        await using var core = new BinanceFixSessionCore(
            fixture.Credentials,
            fixture.Application,
            fixture.Budgets,
            lifecycleInitiator,
            fixture.SessionId,
            TimeSpan.FromSeconds(1));
        fixture.TransferCredentialOwnership();
        core.Start();

        var stopTask = core.StopAsync();
        fixture.Session.Next();
        Assert.Equal(MsgType.LOGOUT, GetMessageType(Assert.Single(fixture.Responder.SentMessages)));
        fixture.Session.Next(CreateInboundMessage(MsgType.LOGOUT, fixture.SessionId).ConstructString());

        Assert.Equal(BinanceFixSessionStopOutcome.GracefulLogout, await stopTask);
        Assert.Equal(1, lifecycleInitiator.ForcedStopCount);
        Assert.True(fixture.Application.CurrentLogoutResponse.IsCompleted);
    }

    [Fact]
    public async Task DisconnectWithoutLogoutResponseIsReportedAsForcedStop()
    {
        using var fixture = CreateRegisteredSession(BinanceFixSessionRole.OrderEntry);
        MarkLoggedOn(fixture.Session);
        fixture.Application.OnLogon(fixture.SessionId);
        var lifecycleInitiator = new RecordingInitiator { LoggedOn = true };
        await using var core = new BinanceFixSessionCore(
            fixture.Credentials,
            fixture.Application,
            fixture.Budgets,
            lifecycleInitiator,
            fixture.SessionId,
            TimeSpan.FromSeconds(30));
        fixture.TransferCredentialOwnership();
        core.Start();

        var stopTask = core.StopAsync();
        fixture.Session.Disconnect("simulated network loss");

        Assert.Equal(BinanceFixSessionStopOutcome.Forced, await stopTask);
        Assert.False(fixture.Application.CurrentLogoutResponse.IsCompleted);
        Assert.True(fixture.Application.CurrentSessionEnded.IsCompleted);
        Assert.Equal(1, lifecycleInitiator.ForcedStopCount);
    }

    [Fact]
    public async Task CancellationForcesStopAndPreventsRestartOrCredentialReuse()
    {
        using var fixture = CreateRegisteredSession(BinanceFixSessionRole.OrderEntry);
        MarkLoggedOn(fixture.Session);
        fixture.Application.OnLogon(fixture.SessionId);
        var lifecycleInitiator = new RecordingInitiator { LoggedOn = true };
        var core = new BinanceFixSessionCore(
            fixture.Credentials,
            fixture.Application,
            fixture.Budgets,
            lifecycleInitiator,
            fixture.SessionId,
            TimeSpan.FromSeconds(1));
        fixture.TransferCredentialOwnership();
        core.Start();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        Assert.Equal(BinanceFixSessionStopOutcome.Forced, await core.StopAsync(cancellation.Token));
        Assert.Equal(1, lifecycleInitiator.ForcedStopCount);
        Assert.Throws<InvalidOperationException>(() => core.Start());

        await core.DisposeAsync();
        await core.DisposeAsync();
        Assert.Equal(1, lifecycleInitiator.DisposeCount);
        Assert.Throws<ObjectDisposedException>(() =>
            fixture.Credentials.SignBase64("payload"u8));
    }

    [Fact]
    public async Task TransportAcceptanceReturnsUnknownDeliveryAndNeverQueuesAReplay()
    {
        using var fixture = CreateRegisteredSession(BinanceFixSessionRole.OrderEntry);
        MarkLoggedOn(fixture.Session);
        var lifecycleInitiator = new RecordingInitiator { LoggedOn = true };
        var core = new BinanceFixSessionCore(
            fixture.Credentials,
            fixture.Application,
            fixture.Budgets,
            lifecycleInitiator,
            fixture.SessionId,
            TimeSpan.FromSeconds(1));
        fixture.TransferCredentialOwnership();
        core.Start();

        var result = core.SendMutation(CreateOutboundMessage(MsgType.ORDER_SINGLE, fixture.SessionId));

        Assert.Equal(BinanceFixDeliveryStatus.UnknownDelivery, result);
        Assert.Single(fixture.Responder.SentMessages);
        Assert.Equal(1, fixture.Budgets.Observe().OutboundMessages.Used);
        Assert.Equal(2UL, fixture.Session.NextSenderMsgSeqNum);
        var storedMessages = new List<string>();
        fixture.Session.MessageStore.Get(1, 1, storedMessages);
        Assert.Empty(storedMessages);

        lifecycleInitiator.LoggedOn = false;
        Assert.Equal(BinanceFixSessionStopOutcome.NoActiveSession, await core.StopAsync());
        await core.DisposeAsync();
    }

    [Fact]
    public async Task MutationIsNotHandedToEngineWhenSessionIsNotLoggedOn()
    {
        using var fixture = CreateRegisteredSession(BinanceFixSessionRole.OrderEntry);
        var lifecycleInitiator = new RecordingInitiator { LoggedOn = false };
        var core = new BinanceFixSessionCore(
            fixture.Credentials,
            fixture.Application,
            fixture.Budgets,
            lifecycleInitiator,
            fixture.SessionId,
            TimeSpan.FromSeconds(1));
        fixture.TransferCredentialOwnership();
        core.Start();

        var result = core.SendMutation(CreateOutboundMessage(MsgType.ORDER_SINGLE, fixture.SessionId));

        Assert.Equal(BinanceFixDeliveryStatus.NotSent, result);
        Assert.Empty(fixture.Responder.SentMessages);
        Assert.Equal(0, fixture.Budgets.Observe().OutboundMessages.Used);
        Assert.Equal(1UL, fixture.Session.NextSenderMsgSeqNum);

        await core.DisposeAsync();
    }

    [Fact]
    public void DeliveryStatusNeverClaimsTransportAcceptanceMeansDelivery()
    {
        Assert.Equal(
            [nameof(BinanceFixDeliveryStatus.NotSent), nameof(BinanceFixDeliveryStatus.UnknownDelivery)],
            Enum.GetNames<BinanceFixDeliveryStatus>());
    }

    private static RegisteredSessionFixture CreateRegisteredSession(BinanceFixSessionRole role)
    {
        var options = CreateOptions(role);
        var credentials = CreateCredentials();
        var budgets = new BinanceFixSessionBudgets(options.Limits);
        var application = new BinanceFixQuickFixApplication(options, credentials, budgets);
        var sessionId = BinanceFixQuickFixSessionSettings.CreateSessionId(options);
        var settings = BinanceFixQuickFixSessionSettings.Create(options, sessionId);
        var initiator = new SocketInitiator(
            application,
            new MemoryStoreFactory(),
            CreateBootstrapSettings(),
            NullLoggerFactory.Instance,
            new DefaultMessageFactory());
        Assert.True(initiator.AddSession(sessionId, settings.Get(sessionId)));
        var session = Assert.IsType<Session>(Session.LookupSession(sessionId));
        var responder = new RecordingResponder();
        session.SetResponder(responder);
        return new RegisteredSessionFixture(
            credentials,
            application,
            budgets,
            initiator,
            sessionId,
            session,
            responder);
    }

    private static BinanceFixSessionOptions CreateOptions(BinanceFixSessionRole role)
        => new(
            BinanceFixEnvironment.SpotTestnet,
            role,
            $"S{Guid.NewGuid():N}"[..8],
            heartbeatIntervalSeconds: 5);

    private static BinanceFixEd25519Credentials CreateCredentials()
        => new("test-api-key", Encoding.ASCII.GetBytes(OfficialPrivateKey));

    private static SessionSettings CreateBootstrapSettings()
    {
        var settings = new SessionSettings();
        var sessionId = new SessionID("FIX.4.4", $"B{Guid.NewGuid():N}"[..8], "STRAP");
        var session = new SettingsDictionary();
        session.SetString(SessionSettings.CONNECTION_TYPE, "initiator");
        settings.Set(sessionId, session);
        return settings;
    }

    private static Message CreateOutboundMessage(string messageType, SessionID sessionId)
    {
        var message = new Message();
        message.Header.SetField(new BeginString(sessionId.BeginString));
        message.Header.SetField(new MsgType(messageType));
        message.Header.SetField(new SenderCompID(sessionId.SenderCompID));
        message.Header.SetField(new TargetCompID(sessionId.TargetCompID));
        message.Header.SetField(new MsgSeqNum(1));
        message.Header.SetField(new SendingTime(DateTime.UtcNow));
        return message;
    }

    private static Message CreateInboundMessage(string messageType, SessionID sessionId)
    {
        var message = new Message();
        message.Header.SetField(new BeginString(sessionId.BeginString));
        message.Header.SetField(new MsgType(messageType));
        message.Header.SetField(new SenderCompID(sessionId.TargetCompID));
        message.Header.SetField(new TargetCompID(sessionId.SenderCompID));
        message.Header.SetField(new MsgSeqNum(1));
        message.Header.SetField(new SendingTime(DateTime.UtcNow));
        return message;
    }

    private static string GetMessageType(string wireMessage)
        => Message.GetMsgType(wireMessage);

    private static void MarkLoggedOn(Session session)
    {
        var state = GetSessionState(session);
        SetStateProperty(state, "SentLogon", true);
        SetStateProperty(state, "ReceivedLogon", true);
    }

    private static void SetSessionClock(Session session, DateTime lastSent, DateTime lastReceived)
    {
        var state = GetSessionState(session);
        SetStateProperty(state, "LastSentTimeDT", lastSent);
        SetStateProperty(state, "LastReceivedTimeDT", lastReceived);
        SetStateProperty(state, "TestRequestCounter", 0);
    }

    private static object GetSessionState(Session session)
        => typeof(Session)
            .GetField("_state", BindingFlags.Instance | BindingFlags.NonPublic)!
            .GetValue(session)!;

    private static void SetStateProperty(object state, string name, object value)
        => state.GetType().GetProperty(name)!.SetValue(state, value);

    private sealed class RecordingSocketInitiator : BinanceFixSocketInitiator
    {
        internal RecordingSocketInitiator(
            IApplication application,
            SessionSettings settings,
            BinanceFixSessionBudgets budgets)
            : base(
                application,
                new MemoryStoreFactory(),
                settings,
                NullLoggerFactory.Instance,
                new DefaultMessageFactory(),
                budgets)
        {
        }

        internal int OpenConnectionCount { get; private set; }

        internal void AttemptConnection(Session session, SettingsDictionary settings)
            => DoConnect(session, settings);

        protected override void ConnectSession(Session session, SettingsDictionary settings)
            => OpenConnectionCount++;
    }

    private sealed class RecordingInitiator : IInitiator
    {
        public bool IsStopped { get; private set; } = true;

        public bool LoggedOn { get; set; }

        public bool IsLoggedOn => LoggedOn;

        public int ForcedStopCount { get; private set; }

        public int DisposeCount { get; private set; }

        public void Start() => IsStopped = false;

        public void Stop() => Stop(force: false);

        public void Stop(bool force)
        {
            Assert.True(force);
            ForcedStopCount++;
            LoggedOn = false;
            IsStopped = true;
        }

        public HashSet<SessionID> GetSessionIDs() => [];

        public bool AddSession(SessionID sessionID, SettingsDictionary dict) => false;

        public bool RemoveSession(SessionID sessionID, bool terminateActiveSession) => false;

        public void Dispose() => DisposeCount++;
    }

    private sealed class RecordingResponder : IResponder
    {
        public List<string> SentMessages { get; } = [];

        public int DisconnectCount { get; private set; }

        public bool Send(string message)
        {
            SentMessages.Add(message);
            return true;
        }

        public void Disconnect() => DisconnectCount++;
    }

    private sealed class RegisteredSessionFixture : IDisposable
    {
        private bool ownsCredentials = true;

        internal RegisteredSessionFixture(
            BinanceFixEd25519Credentials credentials,
            BinanceFixQuickFixApplication application,
            BinanceFixSessionBudgets budgets,
            SocketInitiator initiator,
            SessionID sessionId,
            Session session,
            RecordingResponder responder)
        {
            Credentials = credentials;
            Application = application;
            Budgets = budgets;
            Initiator = initiator;
            SessionId = sessionId;
            Session = session;
            Responder = responder;
        }

        internal BinanceFixEd25519Credentials Credentials { get; }

        internal BinanceFixQuickFixApplication Application { get; }

        internal BinanceFixSessionBudgets Budgets { get; }

        internal SocketInitiator Initiator { get; }

        internal SessionID SessionId { get; }

        internal Session Session { get; }

        internal RecordingResponder Responder { get; }

        internal void TransferCredentialOwnership() => ownsCredentials = false;

        public void Dispose()
        {
            Initiator.RemoveSession(SessionId, terminateActiveSession: true);
            Initiator.Dispose();
            if (ownsCredentials)
            {
                Credentials.Dispose();
            }
        }
    }
}
