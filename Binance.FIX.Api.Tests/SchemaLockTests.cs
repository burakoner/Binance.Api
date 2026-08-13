using System.Reflection;
using System.Text.Json;
using Xunit;

namespace Binance.FIX.Api.Tests;

public class SchemaLockTests
{
    private const string ResourceName = "Binance.FIX.Api.Tests.binance-spot-fix-schema-lock.json";
    private const string SourceCommit = "b483413fcdf4da783cd3fcaad6fab7200a93297f";

    private static readonly IReadOnlyDictionary<string, (long Size, string Sha256)> ExpectedArtifacts =
        new Dictionary<string, (long, string)>(StringComparer.Ordinal)
        {
            ["fix/schemas/spot-fix-oe.xml"] = (24513, "55891d2ae2c7b5a5e9dbec0003ddcfd3034ba5846e0b7fa6561defa8f44797e9"),
            ["fix/schemas/spot-fix-md.xml"] = (12374, "b18432105ae64f24acc49e1ff1e357811ccfb5a84c7475cfba7347ad9e30a2ec"),
            ["sbe/schemas/spot-fixsbe-1_1.xml"] = (48186, "6b44afe558d439ddc94323fd6ff026a949184ddd79a2cf033f610194c2537e70"),
            ["sbe/schemas/spot_fix_prod_latest.xml"] = (19, "ca27b50105e9a6caebf4906bf701031920c77b9cd1d683d0c376b681eb29f4e2"),
            ["sbe/schemas/spot_fix_testnet_latest.xml"] = (19, "ca27b50105e9a6caebf4906bf701031920c77b9cd1d683d0c376b681eb29f4e2"),
            ["sbe/schemas/sbe_fix_schema_lifecycle_prod.json"] = (355, "8c1b60d87c86510af1dad77d0cdac6d462be4d39a6ec1dba4175f4d8af076c1a"),
            ["sbe/schemas/sbe_fix_schema_lifecycle_testnet.json"] = (358, "d0fecdfffb3bac97edc2af9436084b2d1ab40d862f74a692a6ee0a9029104c8f")
        };

    [Fact]
    public void LockPinsEveryCurrentFixArtifactByCommitSizeAndHash()
    {
        using var document = LoadLock();
        var root = document.RootElement;
        Assert.Equal(
            "https://github.com/binance/binance-spot-api-docs",
            root.GetProperty("sourceRepository").GetString());
        Assert.Equal(SourceCommit, root.GetProperty("sourceCommit").GetString());
        Assert.Equal(
            $"https://raw.githubusercontent.com/binance/binance-spot-api-docs/{SourceCommit}/",
            root.GetProperty("sourceRawBaseUrl").GetString());

        var artifacts = root.GetProperty("artifacts").EnumerateArray()
            .ToDictionary(artifact => artifact.GetProperty("upstreamPath").GetString()!, StringComparer.Ordinal);
        Assert.Equal(ExpectedArtifacts.Count, artifacts.Count);

        foreach (var expected in ExpectedArtifacts)
        {
            var artifact = artifacts[expected.Key];
            Assert.Equal(expected.Value.Size, artifact.GetProperty("size").GetInt64());
            Assert.Equal(expected.Value.Sha256, artifact.GetProperty("sha256").GetString());
        }
    }

    [Fact]
    public void LockDoesNotClaimRawSchemaRedistributionRights()
    {
        using var document = LoadLock();
        var root = document.RootElement;
        Assert.False(root.GetProperty("rawArtifactsRedistributed").GetBoolean());

        var terms = root.GetProperty("sourceTerms").EnumerateArray()
            .Select(value => value.GetString()
                ?? throw new InvalidOperationException("A source terms URI cannot be null."))
            .ToArray();
        Assert.Equal(
            ["https://www.binance.com/en/terms", "https://www.binance.com/en/about-legal/terms-testnets"],
            terms);
    }

    [Fact]
    public void RuntimeDictionaryVerifierUsesTheCommittedTextFixLocks()
    {
        Assert.Equal(
            ExpectedArtifacts["fix/schemas/spot-fix-oe.xml"],
            (BinanceFixDataDictionaryVerifier.OrderEntryLength, BinanceFixDataDictionaryVerifier.OrderEntrySha256));
        Assert.Equal(
            ExpectedArtifacts["fix/schemas/spot-fix-md.xml"],
            (BinanceFixDataDictionaryVerifier.MarketDataLength, BinanceFixDataDictionaryVerifier.MarketDataSha256));
    }

    [Fact]
    public void LockContainsOnlyFixOwnedInputsAndReviewedCurrentIdentities()
    {
        using var document = LoadLock();
        var artifacts = document.RootElement.GetProperty("artifacts").EnumerateArray().ToArray();

        Assert.All(artifacts, artifact =>
            Assert.Contains(artifact.GetProperty("owner").GetString(), new[] { "TextFix", "FixSbe" }));
        Assert.Equal(2, artifacts.Count(artifact => artifact.GetProperty("format").GetString() == "QuickFixDataDictionary"));

        var sbe = Assert.Single(artifacts, artifact => artifact.GetProperty("format").GetString() == "SbeSchema");
        Assert.Equal(1, sbe.GetProperty("schemaId").GetInt32());
        Assert.Equal(1, sbe.GetProperty("schemaVersion").GetInt32());
        Assert.Equal(29, sbe.GetProperty("messageCount").GetInt32());

        var aliases = artifacts.Where(artifact => artifact.GetProperty("format").GetString() == "Alias").ToArray();
        Assert.Equal(2, aliases.Length);
        Assert.All(aliases, alias => Assert.Equal("spot-fixsbe-1_1.xml", alias.GetProperty("expectedTarget").GetString()));

        var lifecycles = artifacts.Where(artifact => artifact.GetProperty("format").GetString() == "Lifecycle").ToArray();
        Assert.Equal(2, lifecycles.Length);
        Assert.All(lifecycles, lifecycle =>
        {
            Assert.False(lifecycle.GetProperty("strictJson").GetBoolean());
            Assert.Equal(1, lifecycle.GetProperty("latestSchemaId").GetInt32());
            Assert.Equal(1, lifecycle.GetProperty("latestSchemaVersion").GetInt32());
        });
    }

    private static JsonDocument LoadLock()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException($"Missing embedded resource '{ResourceName}'.");
        return JsonDocument.Parse(stream);
    }
}
