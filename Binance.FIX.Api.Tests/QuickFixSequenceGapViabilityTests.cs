using Microsoft.Extensions.Logging.Abstractions;
using QuickFix;
using QuickFix.Fields;
using QuickFix.Store;
using QuickFix.Transport;
using Xunit;

namespace Binance.FIX.Api.Tests;

public class QuickFixSequenceGapViabilityTests
{
    [Fact]
    public void BinanceAdapterDisconnectsBeforeResendRequestCanBePersistedOrSent()
    {
        // SocketInitiator rejects empty settings but creates configured sessions only on Start(),
        // which would perform network I/O. AddSession creates the tested initiator synchronously.
        var bootstrapSettings = new SessionSettings();
        var bootstrapSession = new SessionID("FIX.4.4", "BOOT", "STRAP");
        var bootstrapDictionary = new SettingsDictionary();
        bootstrapDictionary.SetString(SessionSettings.CONNECTION_TYPE, "initiator");
        bootstrapSettings.Set(bootstrapSession, bootstrapDictionary);

        var application = new FailClosedApplication();
        using var initiator = new SocketInitiator(
            application,
            new MemoryStoreFactory(),
            bootstrapSettings,
            NullLoggerFactory.Instance,
            new DefaultMessageFactory());

        var senderCompId = $"OE{Guid.NewGuid():N}"[..8];
        var sessionId = new SessionID("FIX.4.4", senderCompId, "SPOT");
        var sessionDictionary = new SettingsDictionary();
        sessionDictionary.SetString(SessionSettings.CONNECTION_TYPE, "initiator");
        sessionDictionary.SetBool(SessionSettings.USE_DATA_DICTIONARY, false);
        sessionDictionary.SetBool(SessionSettings.NON_STOP_SESSION, true);
        sessionDictionary.SetLong(SessionSettings.HEARTBTINT, 30);
        sessionDictionary.SetBool(SessionSettings.PERSIST_MESSAGES, true);
        sessionDictionary.SetBool(SessionSettings.CHECK_LATENCY, false);

        Assert.True(initiator.AddSession(sessionId, sessionDictionary));

        try
        {
            var session = Assert.IsType<Session>(Session.LookupSession(sessionId));
            var responder = new RecordingResponder();
            session.SetResponder(responder);

            var logonWithGap = new Message();
            logonWithGap.Header.SetField(new BeginString("FIX.4.4"));
            logonWithGap.Header.SetField(new MsgType(MsgType.LOGON));
            logonWithGap.Header.SetField(new SenderCompID("SPOT"));
            logonWithGap.Header.SetField(new TargetCompID(senderCompId));
            logonWithGap.Header.SetField(new MsgSeqNum(2));
            logonWithGap.Header.SetField(new SendingTime(DateTime.UtcNow));
            logonWithGap.SetField(new HeartBtInt(30));

            session.Next(logonWithGap.ConstructString());

            Assert.Equal(1, application.BlockedResendRequestCount);
            Assert.Equal(1UL, application.BlockedBeginSeqNo);
            Assert.Equal(0UL, application.BlockedEndSeqNo);
            Assert.Equal(1, responder.DisconnectCount);
            Assert.Empty(responder.SentMessages);
            Assert.Equal(1UL, session.NextSenderMsgSeqNum);
            Assert.Equal(1UL, session.NextTargetMsgSeqNum);

            var persistedMessages = new List<string>();
            session.MessageStore.Get(1, 1, persistedMessages);
            Assert.Empty(persistedMessages);
        }
        finally
        {
            Assert.True(initiator.RemoveSession(sessionId, terminateActiveSession: true));
        }
    }

    private sealed class FailClosedApplication : IApplication
    {
        public int BlockedResendRequestCount { get; private set; }

        public ulong BlockedBeginSeqNo { get; private set; }

        public ulong BlockedEndSeqNo { get; private set; }

        public void ToAdmin(Message message, SessionID sessionId)
        {
            if (message.Header.GetString(Tags.MsgType) != MsgType.RESEND_REQUEST)
            {
                return;
            }

            BlockedResendRequestCount++;
            BlockedBeginSeqNo = message.GetULong(Tags.BeginSeqNo);
            BlockedEndSeqNo = message.GetULong(Tags.EndSeqNo);

            var session = Session.LookupSession(sessionId)
                ?? throw new InvalidOperationException($"FIX session '{sessionId}' was not registered.");

            session.Disconnect("Binance does not support ResendRequest");
            throw new DoNotSend();
        }

        public void FromAdmin(Message message, SessionID sessionId)
        {
        }

        public void ToApp(Message message, SessionID sessionId)
        {
        }

        public void FromApp(Message message, SessionID sessionId)
        {
        }

        public void OnCreate(SessionID sessionId)
        {
        }

        public void OnLogout(SessionID sessionId)
        {
        }

        public void OnLogon(SessionID sessionId)
        {
        }
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

        public void Disconnect()
        {
            DisconnectCount++;
        }
    }
}
