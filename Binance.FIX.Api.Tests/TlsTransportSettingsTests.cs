using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using QuickFix;
using QuickFix.Logger;
using Xunit;

namespace Binance.FIX.Api.Tests;

public class TlsTransportSettingsTests
{
    [Theory]
    [InlineData(BinanceFixEnvironment.Production, BinanceFixSessionRole.OrderEntry, "fix-oe.binance.com")]
    [InlineData(BinanceFixEnvironment.Production, BinanceFixSessionRole.DropCopy, "fix-dc.binance.com")]
    [InlineData(BinanceFixEnvironment.Production, BinanceFixSessionRole.MarketData, "fix-md.binance.com")]
    [InlineData(BinanceFixEnvironment.SpotTestnet, BinanceFixSessionRole.OrderEntry, "fix-oe.testnet.binance.vision")]
    [InlineData(BinanceFixEnvironment.SpotTestnet, BinanceFixSessionRole.DropCopy, "fix-dc.testnet.binance.vision")]
    [InlineData(BinanceFixEnvironment.SpotTestnet, BinanceFixSessionRole.MarketData, "fix-md.testnet.binance.vision")]
    public void ProjectsOnlyOfficialDirectTlsEndpoints(
        BinanceFixEnvironment environment,
        BinanceFixSessionRole role,
        string expectedHost)
    {
        var options = new BinanceFixSessionOptions(environment, role, "SESSION");

        var settings = BinanceFixQuickFixTlsSettings.Create(options);
        var socketSettings = new SocketSettings();
        socketSettings.Configure(settings);

        Assert.Equal(expectedHost, settings.GetString(SessionSettings.SOCKET_CONNECT_HOST));
        Assert.Equal(9000L, settings.GetLong(SessionSettings.SOCKET_CONNECT_PORT));
        Assert.True(settings.GetBool(SessionSettings.SOCKET_IGNORE_PROXY));
        Assert.True(settings.GetBool(SessionSettings.SSL_ENABLE));
        Assert.Equal(expectedHost, settings.GetString(SessionSettings.SSL_SERVERNAME));
        Assert.True(settings.GetBool(SessionSettings.SSL_VALIDATE_CERTIFICATES));
        Assert.True(settings.GetBool(SessionSettings.SSL_CHECK_CERTIFICATE_REVOCATION));
        Assert.True(socketSettings.UseSSL);
        Assert.Equal(expectedHost, socketSettings.ServerCommonName);
        Assert.True(socketSettings.ValidateCertificates);
        Assert.True(socketSettings.CheckCertificateRevocation);
        Assert.Null(socketSettings.CertificatePath);
        Assert.Null(socketSettings.CertificatePassword);
        Assert.Null(socketSettings.CACertificatePath);
    }

    [Fact]
    public async Task QuickFixHandshakeSendsExactOfficialSniAndAcceptsMatchingTrustedCertificate()
    {
        const string expectedHost = "fix-oe.testnet.binance.vision";
        using var certificateAuthority = CreateCertificateAuthority("Matching Test CA");
        using var serverCertificate = CreateServerCertificate(certificateAuthority, expectedHost);

        var result = await AttemptQuickFixHandshakeAsync(
            BinanceFixSessionRole.OrderEntry,
            expectedHost,
            serverCertificate,
            certificateAuthority);

        Assert.Null(result.ServerException);
        Assert.Null(result.ClientException);
        Assert.Equal(expectedHost, result.ObservedServerName);
    }

    [Fact]
    public async Task QuickFixHandshakeRejectsHostnameMismatchWhileStillSendingOfficialSni()
    {
        const string expectedHost = "fix-oe.testnet.binance.vision";
        using var certificateAuthority = CreateCertificateAuthority("Mismatch Test CA");
        using var serverCertificate = CreateServerCertificate(certificateAuthority, "wrong.testnet.binance.vision");

        var result = await AttemptQuickFixHandshakeAsync(
            BinanceFixSessionRole.OrderEntry,
            expectedHost,
            serverCertificate,
            certificateAuthority);

        Assert.IsType<AuthenticationException>(result.ClientException);
        Assert.Equal(expectedHost, result.ObservedServerName);
    }

    [Fact]
    public async Task QuickFixHandshakeRejectsCertificateFromUntrustedAuthority()
    {
        const string expectedHost = "fix-md.testnet.binance.vision";
        using var trustedCertificateAuthority = CreateCertificateAuthority("Trusted Test CA");
        using var untrustedCertificateAuthority = CreateCertificateAuthority("Untrusted Test CA");
        using var serverCertificate = CreateServerCertificate(untrustedCertificateAuthority, expectedHost);

        var result = await AttemptQuickFixHandshakeAsync(
            BinanceFixSessionRole.MarketData,
            expectedHost,
            serverCertificate,
            trustedCertificateAuthority);

        Assert.IsType<AuthenticationException>(result.ClientException);
        Assert.Equal(expectedHost, result.ObservedServerName);
    }

    private static async Task<TlsAttemptResult> AttemptQuickFixHandshakeAsync(
        BinanceFixSessionRole role,
        string expectedHost,
        X509Certificate2 serverCertificate,
        X509Certificate2 trustedCertificateAuthority)
    {
        var certificateAuthorityPath = Path.Combine(
            Path.GetTempPath(),
            $"BinanceFixTls-{Guid.NewGuid():N}.cer");
        await File.WriteAllBytesAsync(
            certificateAuthorityPath,
            trustedCertificateAuthority.Export(X509ContentType.Cert));

        var listener = new TcpListener(IPAddress.Loopback, 0);

        try
        {
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            string? observedServerName = null;
            var serverTask = Task.Run(async () =>
            {
                try
                {
                    using var acceptedClient = await listener.AcceptTcpClientAsync().WaitAsync(TimeSpan.FromSeconds(10));
                    using var serverStream = new SslStream(acceptedClient.GetStream(), leaveInnerStreamOpen: false);
                    await serverStream.AuthenticateAsServerAsync(new SslServerAuthenticationOptions
                    {
                        ServerCertificateSelectionCallback = (_, serverName) =>
                        {
                            observedServerName = serverName;
                            return serverCertificate;
                        },
                        ClientCertificateRequired = false,
                        EnabledSslProtocols = SslProtocols.None,
                        CertificateRevocationCheckMode = X509RevocationMode.NoCheck
                    }).WaitAsync(TimeSpan.FromSeconds(10));
                    return (Exception?)null;
                }
                catch (Exception exception)
                {
                    return exception;
                }
            });

            Exception? clientException = null;
            using (var client = new TcpClient(AddressFamily.InterNetwork))
            {
                await client.ConnectAsync(IPAddress.Loopback, port).WaitAsync(TimeSpan.FromSeconds(10));
                var options = new BinanceFixSessionOptions(
                    BinanceFixEnvironment.SpotTestnet,
                    role,
                    "SESSION");
                var quickFixSettings = BinanceFixQuickFixTlsSettings.Create(options);
                Assert.Equal(expectedHost, quickFixSettings.GetString(SessionSettings.SSL_SERVERNAME));
                Assert.True(quickFixSettings.GetBool(SessionSettings.SSL_CHECK_CERTIFICATE_REVOCATION));
                quickFixSettings.SetString(SessionSettings.SSL_CA_CERTIFICATE, certificateAuthorityPath);
                // The ephemeral test CA has no CRL/OCSP service. Production remains fail-closed with
                // revocation enabled; this test-only override isolates hostname and trust-chain behavior.
                quickFixSettings.SetBool(SessionSettings.SSL_CHECK_CERTIFICATE_REVOCATION, false);
                var socketSettings = new SocketSettings();
                socketSettings.Configure(quickFixSettings);

                try
                {
                    using var authenticatedStream = CreateQuickFixClientStream(
                        socketSettings,
                        client.GetStream());
                }
                catch (Exception exception)
                {
                    clientException = exception;
                }
            }

            var serverException = await serverTask.WaitAsync(TimeSpan.FromSeconds(10));
            return new TlsAttemptResult(observedServerName, clientException, serverException);
        }
        finally
        {
            listener.Stop();
            File.Delete(certificateAuthorityPath);
        }
    }

    private static Stream CreateQuickFixClientStream(SocketSettings settings, Stream networkStream)
    {
        var factoryType = typeof(SocketSettings).Assembly.GetType(
            "QuickFix.Transport.SslStreamFactory",
            throwOnError: true)!;
        var constructor = Assert.Single(factoryType.GetConstructors(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
        var loggerAdapterType = typeof(SocketSettings).Assembly.GetType(
            "QuickFix.Logger.LogFactoryAdapter",
            throwOnError: true)!;
        var loggerAdapterConstructor = Assert.Single(loggerAdapterType.GetConstructors(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
        var loggerAdapter = loggerAdapterConstructor.Invoke([new NullLogFactory()]);
        var factory = constructor.Invoke(
            [settings, loggerAdapter]);
        var createMethod = factoryType.GetMethod(
            "CreateClientStreamAndAuthenticate",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Pinned QuickFIX/n TLS client factory method was not found.");

        try
        {
            return Assert.IsAssignableFrom<Stream>(createMethod.Invoke(factory, [networkStream]));
        }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
            throw;
        }
    }

    private static X509Certificate2 CreateCertificateAuthority(string commonName)
    {
        using var key = RSA.Create(2048);
        var request = new CertificateRequest(
            $"CN={commonName}",
            key,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);
        request.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, true));
        request.CertificateExtensions.Add(new X509KeyUsageExtension(
            X509KeyUsageFlags.KeyCertSign | X509KeyUsageFlags.CrlSign,
            true));
        request.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(request.PublicKey, false));
        return request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(2));
    }

    private static X509Certificate2 CreateServerCertificate(
        X509Certificate2 certificateAuthority,
        string dnsName)
    {
        using var key = RSA.Create(2048);
        var request = new CertificateRequest(
            $"CN={dnsName}",
            key,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);
        request.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, true));
        request.CertificateExtensions.Add(new X509KeyUsageExtension(
            X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment,
            true));
        request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(
            new OidCollection { new("1.3.6.1.5.5.7.3.1") },
            true));
        var subjectAlternativeName = new SubjectAlternativeNameBuilder();
        subjectAlternativeName.AddDnsName(dnsName);
        request.CertificateExtensions.Add(subjectAlternativeName.Build());
        request.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(request.PublicKey, false));

        var serialNumber = RandomNumberGenerator.GetBytes(16);
        serialNumber[0] &= 0x7f;
        serialNumber[^1] |= 0x01;
        using var publicCertificate = request.Create(
            certificateAuthority,
            DateTimeOffset.UtcNow.AddHours(-1),
            DateTimeOffset.UtcNow.AddDays(1),
            serialNumber);
        using var certificateWithPrivateKey = publicCertificate.CopyWithPrivateKey(key);
        return X509CertificateLoader.LoadPkcs12(
            certificateWithPrivateKey.Export(X509ContentType.Pfx),
            password: null,
            X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.Exportable);
    }

    private sealed record TlsAttemptResult(
        string? ObservedServerName,
        Exception? ClientException,
        Exception? ServerException);
}
