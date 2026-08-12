using System;
using QuickFix;

namespace Binance.FIX.Api;

internal static class BinanceFixQuickFixTlsSettings
{
    internal static SettingsDictionary Create(BinanceFixSessionOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var settings = new SettingsDictionary();
        Apply(options, settings);
        return settings;
    }

    internal static void Apply(BinanceFixSessionOptions options, SettingsDictionary settings)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(settings);

        var endpoint = options.Endpoint;
        if (!string.Equals(endpoint.Scheme, "tcp+tls", StringComparison.Ordinal) || endpoint.Port != 9000)
        {
            throw new InvalidOperationException("Binance text FIX requires the official tcp+tls endpoint on port 9000.");
        }

        var host = endpoint.DnsSafeHost;
        settings.SetString(SessionSettings.SOCKET_CONNECT_HOST, host);
        settings.SetLong(SessionSettings.SOCKET_CONNECT_PORT, endpoint.Port);
        settings.SetBool(SessionSettings.SOCKET_IGNORE_PROXY, true);
        settings.SetBool(SessionSettings.SSL_ENABLE, true);
        settings.SetString(SessionSettings.SSL_SERVERNAME, host);
        settings.SetBool(SessionSettings.SSL_VALIDATE_CERTIFICATES, true);
        settings.SetBool(SessionSettings.SSL_CHECK_CERTIFICATE_REVOCATION, true);
    }
}
