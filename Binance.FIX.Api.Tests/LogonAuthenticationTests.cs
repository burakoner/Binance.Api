using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using QuickFix;
using QuickFix.Fields;
using Xunit;

namespace Binance.FIX.Api.Tests;

public class LogonAuthenticationTests
{
    private const string OfficialPrivateKey = """
        -----BEGIN PRIVATE KEY-----
        MC4CAQAwBQYDK2VwBCIEIIJEYWtGBrhACmb9Dvy+qa8WEf0lQOl1s4CLIAB9m89u
        -----END PRIVATE KEY-----
        """;

    private const string OfficialSignature =
        "4MHXelVVcpkdwuLbl6n73HQUXUf1dse2PCgT1DYqW9w8AVZ1RACFGM+5UdlGPrQHrgtS3CvsRURC1oj73j8gCA==";

    [Fact]
    public void AppliesOfficialSignatureVectorAndRequiredOrderEntryFields()
    {
        var options = new BinanceFixSessionOptions(
            BinanceFixEnvironment.SpotTestnet,
            BinanceFixSessionRole.OrderEntry,
            "EXAMPLE");
        using var credentials = CreateOfficialCredentials("test-api-key");
        var logon = CreateLogon("EXAMPLE", "SPOT", "1", "20240627-11:17:25.223");

        BinanceFixLogonAuthenticator.Apply(logon, options, credentials);

        Assert.Equal(OfficialSignature, logon.GetString(Tags.RawData));
        Assert.Equal(Encoding.ASCII.GetByteCount(OfficialSignature), logon.GetInt(Tags.RawDataLength));
        Assert.Equal("test-api-key", logon.GetString(Tags.Username));
        Assert.Equal(0, logon.GetInt(Tags.EncryptMethod));
        Assert.Equal(30, logon.GetInt(Tags.HeartBtInt));
        Assert.True(logon.GetBoolean(Tags.ResetSeqNumFlag));
        Assert.Equal(2, logon.GetInt(BinanceFixLogonAuthenticator.MessageHandlingTag));
        Assert.Equal(1, logon.GetInt(BinanceFixLogonAuthenticator.ResponseModeTag));
        Assert.False(logon.IsSetField(BinanceFixLogonAuthenticator.DropCopyFlagTag));

        var serialized = logon.ConstructString();
        Assert.Contains($"\u000195=88\u000196={OfficialSignature}\u0001", serialized, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(BinanceFixSessionRole.OrderEntry, true, false)]
    [InlineData(BinanceFixSessionRole.DropCopy, true, true)]
    [InlineData(BinanceFixSessionRole.MarketData, false, false)]
    public void AppliesOnlyRoleSupportedLogonFields(
        BinanceFixSessionRole role,
        bool expectsResponseMode,
        bool expectsDropCopyFlag)
    {
        var options = new BinanceFixSessionOptions(
            BinanceFixEnvironment.SpotTestnet,
            role,
            "SESSION");
        using var credentials = CreateOfficialCredentials("api-key");
        var logon = CreateLogon("SESSION", "SPOT", "7", "20260812-10:11:12.123456");

        BinanceFixLogonAuthenticator.Apply(logon, options, credentials);

        Assert.Equal(expectsResponseMode, logon.IsSetField(BinanceFixLogonAuthenticator.ResponseModeTag));
        Assert.Equal(expectsDropCopyFlag, logon.IsSetField(BinanceFixLogonAuthenticator.DropCopyFlagTag));
        if (expectsDropCopyFlag)
        {
            Assert.True(logon.GetBoolean(BinanceFixLogonAuthenticator.DropCopyFlagTag));
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("line\nbreak")]
    [InlineData("UNICODÉ")]
    public void RejectsApiKeysOutsidePrintableAscii(string? apiKey)
    {
        var privateKey = Encoding.ASCII.GetBytes(OfficialPrivateKey);

        var exception = Assert.Throws<ArgumentException>(() =>
            new BinanceFixEd25519Credentials(apiKey!, privateKey));

        Assert.Equal("apiKey", exception.ParamName);
    }

    [Fact]
    public void RejectsNonEd25519KeyWithoutEchoingCredentialMaterial()
    {
        using var rsa = RSA.Create(2048);
        var rsaPem = rsa.ExportPkcs8PrivateKeyPem();
        var privateKey = Encoding.ASCII.GetBytes(rsaPem);

        var exception = Assert.Throws<ArgumentException>(() =>
            new BinanceFixEd25519Credentials("api-key", privateKey));

        Assert.Equal("pkcs8PrivateKeyPem", exception.ParamName);
        Assert.DoesNotContain("BEGIN PRIVATE KEY", exception.ToString(), StringComparison.Ordinal);
        Assert.DoesNotContain("api-key", exception.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public void RejectsSigningAfterCredentialsAreDisposed()
    {
        var options = new BinanceFixSessionOptions(
            BinanceFixEnvironment.SpotTestnet,
            BinanceFixSessionRole.MarketData,
            "SESSION");
        var credentials = CreateOfficialCredentials("api-key");
        credentials.Dispose();
        var logon = CreateLogon("SESSION", "SPOT", "1", "20260812-10:11:12");
        AddStaleAuthenticationFields(logon);

        Assert.Throws<ObjectDisposedException>(() =>
            BinanceFixLogonAuthenticator.Apply(logon, options, credentials));
        AssertAuthenticationFieldsCleared(logon);
    }

    [Fact]
    public void CredentialObjectHasNoPublicSecretAccessorAndRedactsItsStringForm()
    {
        using var credentials = CreateOfficialCredentials("api-key-that-must-not-leak");
        var publicProperties = typeof(BinanceFixEd25519Credentials)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public);

        Assert.Empty(publicProperties);
        Assert.Equal("BinanceFixEd25519Credentials ([REDACTED])", credentials.ToString());
        Assert.DoesNotContain("api-key-that-must-not-leak", credentials.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public void RefusesIncompleteHeaderBeforeAddingCredentials()
    {
        var options = new BinanceFixSessionOptions(
            BinanceFixEnvironment.SpotTestnet,
            BinanceFixSessionRole.OrderEntry,
            "SESSION");
        using var credentials = CreateOfficialCredentials("api-key");
        var logon = CreateLogon("SESSION", "SPOT", "1", "20260812-10:11:12");
        AddStaleAuthenticationFields(logon);
        logon.Header.RemoveField(Tags.SendingTime);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            BinanceFixLogonAuthenticator.Apply(logon, options, credentials));

        Assert.Contains("52", exception.Message, StringComparison.Ordinal);
        AssertAuthenticationFieldsCleared(logon);
    }

    [Theory]
    [InlineData("D", "SESSION", "SPOT", "1", "20260812-10:11:12")]
    [InlineData("A", "OTHER", "SPOT", "1", "20260812-10:11:12")]
    [InlineData("A", "SESSION", "OTHER", "1", "20260812-10:11:12")]
    [InlineData("A", "SESSION", "SPOT", "-1", "20260812-10:11:12")]
    [InlineData("A", "SESSION", "SPOT", "0", "20260812-10:11:12")]
    [InlineData("A", "SESSION", "SPOT", "1", "2026-08-12T10:11:12Z")]
    public void RefusesHeaderValuesOutsideSignedSessionContract(
        string messageType,
        string senderCompId,
        string targetCompId,
        string messageSequenceNumber,
        string sendingTime)
    {
        var options = new BinanceFixSessionOptions(
            BinanceFixEnvironment.SpotTestnet,
            BinanceFixSessionRole.OrderEntry,
            "SESSION");
        using var credentials = CreateOfficialCredentials("api-key");
        var logon = CreateLogon(senderCompId, targetCompId, messageSequenceNumber, sendingTime, messageType);
        AddStaleAuthenticationFields(logon);

        Assert.Throws<InvalidOperationException>(() =>
            BinanceFixLogonAuthenticator.Apply(logon, options, credentials));
        AssertAuthenticationFieldsCleared(logon);
    }

    [Fact]
    public void AcceptsQuickFixUnsigned64BitSequenceNumber()
    {
        var options = new BinanceFixSessionOptions(
            BinanceFixEnvironment.SpotTestnet,
            BinanceFixSessionRole.OrderEntry,
            "SESSION");
        using var credentials = CreateOfficialCredentials("api-key");
        var logon = CreateLogon(
            "SESSION",
            "SPOT",
            ulong.MaxValue.ToString(System.Globalization.CultureInfo.InvariantCulture),
            "20260812-10:11:12");

        BinanceFixLogonAuthenticator.Apply(logon, options, credentials);

        Assert.NotEmpty(logon.GetString(Tags.RawData));
    }

    [Fact]
    public void RedactsEveryWireCredentialFieldWithoutTouchingTagCollisions()
    {
        const char soh = '\u0001';
        var wireMessage =
            $"8=FIX.4.4{soh}96=signature-one{soh}553=api-key{soh}1096=keep{soh}58=text 553=keep{soh}96=signature-two{soh}";

        var redacted = BinanceFixLogRedactor.Redact(wireMessage);

        Assert.Equal(
            $"8=FIX.4.4{soh}96=[REDACTED]{soh}553=[REDACTED]{soh}1096=keep{soh}58=text 553=keep{soh}96=[REDACTED]{soh}",
            redacted);
        Assert.DoesNotContain("signature-one", redacted, StringComparison.Ordinal);
        Assert.DoesNotContain("signature-two", redacted, StringComparison.Ordinal);
        Assert.DoesNotContain("api-key", redacted, StringComparison.Ordinal);
    }

    private static BinanceFixEd25519Credentials CreateOfficialCredentials(string apiKey)
        => new(apiKey, Encoding.ASCII.GetBytes(OfficialPrivateKey));

    private static void AddStaleAuthenticationFields(Message logon)
    {
        logon.SetField(new RawDataLength(5));
        logon.SetField(new RawData("stale"));
        logon.SetField(new Username("stale-api-key"));
        logon.SetField(new IntField(BinanceFixLogonAuthenticator.ResponseModeTag, 99));
        logon.SetField(new BooleanField(BinanceFixLogonAuthenticator.DropCopyFlagTag, true));
    }

    private static void AssertAuthenticationFieldsCleared(Message logon)
    {
        Assert.False(logon.IsSetField(Tags.RawDataLength));
        Assert.False(logon.IsSetField(Tags.RawData));
        Assert.False(logon.IsSetField(Tags.Username));
        Assert.False(logon.IsSetField(BinanceFixLogonAuthenticator.ResponseModeTag));
        Assert.False(logon.IsSetField(BinanceFixLogonAuthenticator.DropCopyFlagTag));
    }

    private static Message CreateLogon(
        string senderCompId,
        string targetCompId,
        string messageSequenceNumber,
        string sendingTime,
        string messageType = MsgType.LOGON)
    {
        var logon = new Message();
        logon.Header.SetField(new BeginString("FIX.4.4"));
        logon.Header.SetField(new StringField(Tags.MsgType, messageType));
        logon.Header.SetField(new StringField(Tags.SenderCompID, senderCompId));
        logon.Header.SetField(new StringField(Tags.TargetCompID, targetCompId));
        logon.Header.SetField(new StringField(Tags.MsgSeqNum, messageSequenceNumber));
        logon.Header.SetField(new StringField(Tags.SendingTime, sendingTime));
        return logon;
    }
}
