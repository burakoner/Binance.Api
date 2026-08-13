using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging.Abstractions;
using QuickFix;
using QuickFix.DataDictionary;
using QuickFix.Store;
using Xunit;

namespace Binance.FIX.Api.Tests;

public class DataDictionaryTests
{
    [Fact]
    public void VerifiesExactLengthAndSha256WhileRetainingAnAbsolutePath()
    {
        using var file = TemporaryFile.Create("verified dictionary bytes");
        var expectedHash = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(file.Path))).ToLowerInvariant();

        using var verified = BinanceFixDataDictionaryVerifier.Verify(
            file.Path,
            new FileInfo(file.Path).Length,
            expectedHash);

        Assert.Equal(Path.GetFullPath(file.Path), verified.Path);
    }

    [Fact]
    public void RejectsMissingRoleDictionaryAndWrongLengthOrHash()
    {
        Assert.Throws<InvalidOperationException>(() =>
            BinanceFixDataDictionaryVerifier.VerifyRequired(BinanceFixSessionRole.OrderEntry, null));

        using var file = TemporaryFile.Create("dictionary");
        var bytes = File.ReadAllBytes(file.Path);
        var hash = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();

        Assert.Throws<InvalidDataException>(() =>
            BinanceFixDataDictionaryVerifier.Verify(file.Path, bytes.LongLength + 1, hash));
        Assert.Throws<InvalidDataException>(() =>
            BinanceFixDataDictionaryVerifier.Verify(
                file.Path,
                bytes.LongLength,
                new string('0', 64)));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("ABC")]
    [InlineData("55891D2AE2C7B5A5E9DBEC0003DDCFD3034BA5846E0B7FA6561DEFA8F44797E9")]
    public void RejectsMalformedExpectedHashes(string hash)
    {
        using var file = TemporaryFile.Create("dictionary");

        Assert.Throws<ArgumentException>(() =>
            BinanceFixDataDictionaryVerifier.Verify(file.Path, new FileInfo(file.Path).Length, hash));
    }

    [Fact]
    public void QuickFixSettingsUseOnlyTheVerifiedDictionaryPath()
    {
        using var file = TemporaryFile.Create("dictionary");
        var bytes = File.ReadAllBytes(file.Path);
        var hash = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
        using var verified = BinanceFixDataDictionaryVerifier.Verify(file.Path, bytes.LongLength, hash);
        var options = new BinanceFixSessionOptions(
            BinanceFixEnvironment.SpotTestnet,
            BinanceFixSessionRole.OrderEntry,
            "OE");
        var sessionId = BinanceFixQuickFixSessionSettings.CreateSessionId(options);

        var settings = BinanceFixQuickFixSessionSettings.Create(options, sessionId, verified);
        var session = settings.Get(sessionId);

        Assert.True(session.GetBool(SessionSettings.USE_DATA_DICTIONARY));
        Assert.Equal(verified.Path, session.GetString(SessionSettings.DATA_DICTIONARY));
    }

    [Fact]
    public void NormalizesOnlyTheReviewedListStatusUnionIntoAContentLockedRuntimeCache()
    {
        using var fixture = new FixListStatusDictionaryFixture();
        var runtimePath = fixture.RuntimePath;
        using var sourceStream = File.OpenRead(fixture.SourcePath);
        var originalDictionary = new DataDictionary(sourceStream);

        Assert.Throws<GroupDelimiterTagException>(() => fixture.ParseOfficialSample(originalDictionary));

        var parsed = fixture.ParseOfficialSample(fixture.Dictionary);
        Assert.Equal(2, parsed.GroupCount(BinanceFixListStatusParser.NumberOfOrdersTag));
        Assert.True(File.Exists(runtimePath));

        fixture.Dispose();

        Assert.True(File.Exists(runtimePath));
        Assert.Contains(
            Path.Combine("Binance.Api", "fix-dictionaries"),
            runtimePath,
            StringComparison.OrdinalIgnoreCase);
        var runtimeHash = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(runtimePath))).ToLowerInvariant();
        Assert.Equal($"spot-fix-oe-{runtimeHash}.xml", Path.GetFileName(runtimePath));
    }

    [Fact]
    public void QuickFixSessionFactoryLoadsTheNormalizedDictionaryFromTheHeldCachePath()
    {
        using var fixture = new FixListStatusDictionaryFixture();
        var options = new BinanceFixSessionOptions(
            BinanceFixEnvironment.SpotTestnet,
            BinanceFixSessionRole.OrderEntry,
            $"S{Guid.NewGuid():N}"[..8]);
        var sessionId = BinanceFixQuickFixSessionSettings.CreateSessionId(options);
        var settings = BinanceFixQuickFixSessionSettings.Create(options, sessionId, fixture.VerifiedRuntime);
        var bootstrap = new SessionSettings();
        var bootstrapSession = new SettingsDictionary();
        bootstrapSession.SetString(SessionSettings.CONNECTION_TYPE, "initiator");
        bootstrap.Set(
            new SessionID("FIX.4.4", $"B{Guid.NewGuid():N}"[..8], "STRAP"),
            bootstrapSession);
        var budgets = new BinanceFixSessionBudgets(options.Limits);
        using var initiator = new BinanceFixSocketInitiator(
            new NoOpApplication(),
            new MemoryStoreFactory(),
            bootstrap,
            NullLoggerFactory.Instance,
            new DefaultMessageFactory(),
            budgets);

        Assert.True(initiator.AddSession(sessionId, settings.Get(sessionId)));
        try
        {
            var session = Assert.IsType<Session>(Session.LookupSession(sessionId));
            Assert.True(session.SessionDataDictionary.IsGroup("N", BinanceFixListStatusParser.NumberOfOrdersTag));
        }
        finally
        {
            Assert.True(initiator.RemoveSession(sessionId, terminateActiveSession: true));
        }
    }
}

internal sealed class FixListStatusDictionaryFixture : IDisposable
{
    internal const string OfficialSample = "8=FIX.4.4|9=293|35=N|34=2|49=SPOT|52=20240607-02:19:07.837191|56=Eg13pOvN|55=BTCUSDT|60=20240607-02:19:07.836000|66=25|73=2|55=BTCUSDT|37=52|11=w1717726747805308656|55=BTCUSDT|37=53|11=p1717726747805308656|25010=1|25011=3|25012=0|25013=1|429=4|431=3|1385=2|25014=1717726747805308656|25015=1717726747805308656|10=162|";

    private const string SourceDictionary = """
        <fix major='4' type='FIX' servicepack='0' minor='4'>
         <header>
          <field name='BeginString' required='Y'/>
          <field name='BodyLength' required='Y'/>
          <field name='MsgType' required='Y'/>
          <field name='SenderCompID' required='Y'/>
          <field name='TargetCompID' required='Y'/>
          <field name='MsgSeqNum' required='Y'/>
          <field name='SendingTime' required='Y'/>
         </header>
         <messages>
          <message name='ListStatus' msgcat='app' msgtype='N'>
           <field name='Symbol' required='N'/>
           <field name='ListID' required='N'/>
           <field name='ClListID' required='N'/>
           <field name='OrigClListID' required='N'/>
           <field name='ContingencyType' required='N'/>
           <field name='ListStatusType' required='Y'/>
           <field name='ListOrderStatus' required='Y'/>
           <field name='ListRejectReason' required='N'/>
           <field name='TransactTime' required='N'/>
           <group name='NoOrders' required='N'>
            <field name='ClOrdID' required='Y'/>
            <field name='Symbol' required='Y'/>
            <field name='OrderID' required='N'/>
            <component name='ListTriggeringInstruction' required='N'/>
            <field name='OrdRejReason' required='N'/>
            <field name='ErrorCode' required='N'/>
            <field name='Text' required='N'/>
           </group>
          </message>
         </messages>
         <trailer><field name='CheckSum' required='Y'/></trailer>
         <components>
          <component name='ListTriggeringInstruction'>
           <group name='NoListTriggeringInstructions' required='N'>
            <field name='ListTriggerType' required='Y'/>
            <field name='ListTriggerTriggerIndex' required='Y'/>
            <field name='ListTriggerAction' required='Y'/>
           </group>
          </component>
         </components>
         <fields>
          <field number='8' name='BeginString' type='STRING'/>
          <field number='9' name='BodyLength' type='LENGTH'/>
          <field number='10' name='CheckSum' type='STRING'/>
          <field number='11' name='ClOrdID' type='STRING'/>
          <field number='34' name='MsgSeqNum' type='SEQNUM'/>
          <field number='35' name='MsgType' type='STRING'/>
          <field number='37' name='OrderID' type='INT'/>
          <field number='49' name='SenderCompID' type='STRING'/>
          <field number='52' name='SendingTime' type='UTCTIMESTAMP'/>
          <field number='55' name='Symbol' type='STRING'/>
          <field number='56' name='TargetCompID' type='STRING'/>
          <field number='58' name='Text' type='STRING'/>
          <field number='60' name='TransactTime' type='UTCTIMESTAMP'/>
          <field number='66' name='ListID' type='INT'/>
          <field number='73' name='NoOrders' type='NUMINGROUP'/>
          <field number='103' name='OrdRejReason' type='INT'/>
          <field number='429' name='ListStatusType' type='INT'/>
          <field number='431' name='ListOrderStatus' type='INT'/>
          <field number='1385' name='ContingencyType' type='INT'/>
          <field number='1386' name='ListRejectReason' type='INT'/>
          <field number='25010' name='NoListTriggeringInstructions' type='NUMINGROUP'/>
          <field number='25011' name='ListTriggerType' type='CHAR'/>
          <field number='25012' name='ListTriggerTriggerIndex' type='INT'/>
          <field number='25013' name='ListTriggerAction' type='CHAR'/>
          <field number='25014' name='ClListID' type='STRING'/>
          <field number='25015' name='OrigClListID' type='STRING'/>
          <field number='25016' name='ErrorCode' type='INT'/>
         </fields>
        </fix>
        """;

    private readonly string directory;
    private readonly BinanceFixVerifiedDataDictionary runtimeDictionary;
    private bool disposed;

    internal FixListStatusDictionaryFixture()
    {
        directory = Path.Combine(Path.GetTempPath(), $"BinanceApiFixTests-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);
        SourcePath = Path.Combine(directory, "spot-fix-oe.xml");
        File.WriteAllText(SourcePath, SourceDictionary, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        var bytes = File.ReadAllBytes(SourcePath);
        var hash = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
        var verifiedSource = BinanceFixDataDictionaryVerifier.Verify(SourcePath, bytes.LongLength, hash);
        runtimeDictionary = BinanceFixDataDictionaryVerifier.CreateOrderEntryRuntimeDictionary(verifiedSource);
        RuntimePath = runtimeDictionary.Path;
        using var runtimeStream = File.OpenRead(RuntimePath);
        Dictionary = new DataDictionary(runtimeStream);
    }

    internal string SourcePath { get; }

    internal string RuntimePath { get; }

    internal DataDictionary Dictionary { get; }

    internal BinanceFixVerifiedDataDictionary VerifiedRuntime => runtimeDictionary;

    internal Message ParseOfficialSample(DataDictionary dictionary)
    {
        var message = new Message();
        message.FromString(
            OfficialSample.Replace('|', Message.SOH),
            validate: true,
            transportDict: dictionary,
            appDict: dictionary,
            msgFactory: null,
            ignoreBody: false);
        return message;
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        runtimeDictionary.Dispose();
        try
        {
            File.Delete(SourcePath);
            Directory.Delete(directory);
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }

        disposed = true;
    }
}

internal sealed class NoOpApplication : IApplication
{
    public void OnCreate(SessionID sessionId)
    {
    }

    public void OnLogon(SessionID sessionId)
    {
    }

    public void OnLogout(SessionID sessionId)
    {
    }

    public void ToAdmin(Message message, SessionID sessionId)
    {
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
}

internal sealed class TemporaryFile : IDisposable
{
    private TemporaryFile(string path)
    {
        Path = path;
    }

    internal string Path { get; }

    internal static TemporaryFile Create(string content)
    {
        var path = System.IO.Path.Combine(
            System.IO.Path.GetTempPath(),
            $"BinanceApiFixTests-{Guid.NewGuid():N}.tmp");
        File.WriteAllText(path, content, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        return new TemporaryFile(path);
    }

    public void Dispose()
    {
        try
        {
            File.Delete(Path);
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}
