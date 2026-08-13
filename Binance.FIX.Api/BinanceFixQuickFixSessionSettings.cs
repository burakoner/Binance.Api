using System;
using QuickFix;

namespace Binance.FIX.Api;

internal static class BinanceFixQuickFixSessionSettings
{
    internal static SessionID CreateSessionId(BinanceFixSessionOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return new SessionID("FIX.4.4", options.SenderCompId, BinanceFixSessionOptions.TargetCompId);
    }

    internal static SessionSettings Create(
        BinanceFixSessionOptions options,
        SessionID sessionId,
        BinanceFixVerifiedDataDictionary? dataDictionary = null)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(sessionId);

        var defaults = new SettingsDictionary("DEFAULT");
        defaults.SetLong(SessionSettings.RECONNECT_INTERVAL, CalculateReconnectIntervalSeconds(options));

        var session = new SettingsDictionary("SESSION");
        session.SetString(SessionSettings.CONNECTION_TYPE, "initiator");
        session.SetBool(SessionSettings.USE_DATA_DICTIONARY, dataDictionary is not null);
        if (dataDictionary is not null)
        {
            session.SetString(SessionSettings.DATA_DICTIONARY, dataDictionary.Path);
        }
        session.SetBool(SessionSettings.NON_STOP_SESSION, true);
        session.SetLong(SessionSettings.HEARTBTINT, options.HeartbeatIntervalSeconds);
        session.SetBool(SessionSettings.PERSIST_MESSAGES, false);
        session.SetBool(SessionSettings.RESET_ON_LOGON, true);
        session.SetBool(SessionSettings.RESET_ON_LOGOUT, true);
        session.SetBool(SessionSettings.RESET_ON_DISCONNECT, true);
        session.SetBool(SessionSettings.SEND_REDUNDANT_RESENDREQUESTS, false);
        session.SetBool(SessionSettings.SEND_LOGOUT_BEFORE_TIMEOUT_DISCONNECT, false);
        session.SetString(SessionSettings.ENCODING, "utf-8");
        BinanceFixQuickFixTlsSettings.Apply(options, session);

        var settings = new SessionSettings();
        settings.Set(defaults);
        settings.Set(sessionId, session);
        return settings;
    }

    private static int CalculateReconnectIntervalSeconds(BinanceFixSessionOptions options)
    {
        var limit = options.Limits.ConnectionAttempts;
        return Math.Max(
            1,
            (int)Math.Ceiling(limit.Window.TotalSeconds / limit.MaximumCount));
    }
}
