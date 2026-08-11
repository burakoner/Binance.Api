using System.Reflection;
using System.Text.Json;
using Xunit;

namespace Binance.FIX.Api.Tests;

public class DictionaryGenerationToolchainLockTests
{
    private const string ResourceName = "Binance.FIX.Api.Tests.binance-spot-fix-ddtool-lock.json";

    [Fact]
    public void LockPinsCurrentQuickFixNSourceToolAndUnmodifiedCorePackage()
    {
        using var document = LoadLock();
        var root = document.RootElement;
        Assert.False(root.GetProperty("artifactsRedistributed").GetBoolean());
        Assert.False(root.GetProperty("generatedArtifactsCommitted").GetBoolean());
        Assert.Equal("Windows", root.GetProperty("hostOperatingSystem").GetString());
        Assert.Equal(10, root.GetProperty("minimumDotnetSdkMajor").GetInt32());

        var quickFix = root.GetProperty("quickFixN");
        Assert.Equal("1.14.1", quickFix.GetProperty("version").GetString());
        Assert.Equal("v1.14.1", quickFix.GetProperty("releaseTag").GetString());
        Assert.Equal("44c111acdb253335fd3c035589795468ff60c400", quickFix.GetProperty("sourceCommit").GetString());
        Assert.Equal("The QuickFIX Software License, Version 1.0", quickFix.GetProperty("license").GetString());

        var source = quickFix.GetProperty("sourceArchive");
        Assert.Equal(5_930_664, source.GetProperty("size").GetInt64());
        Assert.Equal("f0584c3dbe2df312382308b7343d6a6cf8e9ae1a1641e092816b7b57c12e030a", source.GetProperty("sha256").GetString());

        var package = quickFix.GetProperty("corePackage");
        Assert.Equal("QuickFIXn.Core", package.GetProperty("id").GetString());
        Assert.Equal(510_580, package.GetProperty("size").GetInt64());
        Assert.Equal("1a30dc9bef15dee380279aeae44290a34a5d4a3581582c77a53704742f9244d2", package.GetProperty("sha256").GetString());
        Assert.Equal(quickFix.GetProperty("sourceCommit").GetString(), package.GetProperty("repositoryCommit").GetString());

        var assemblies = package.GetProperty("assemblies").EnumerateArray()
            .ToDictionary(assembly => assembly.GetProperty("targetFramework").GetString()!, StringComparer.Ordinal);
        Assert.Equal(["net10.0", "net8.0"], assemblies.Keys.Order(StringComparer.Ordinal).ToArray());
        Assert.All(assemblies.Values, assembly => Assert.Equal(559_616, assembly.GetProperty("size").GetInt64()));
    }

    [Fact]
    public void LockOwnsBothRoleDictionariesWithoutNamespaceOrFieldCollisions()
    {
        using var document = LoadLock();
        var root = document.RootElement;
        var dictionaries = root.GetProperty("dictionaries").EnumerateArray()
            .ToDictionary(dictionary => dictionary.GetProperty("role").GetString()!, StringComparer.Ordinal);
        Assert.Equal(["MarketData", "OrderEntry"], dictionaries.Keys.Order(StringComparer.Ordinal).ToArray());

        var orderEntry = dictionaries["OrderEntry"];
        Assert.Equal("BinanceSpotOrderEntry", orderEntry.GetProperty("customName").GetString());
        Assert.Equal("Binance.FIX.Api.Internal.OrderEntry", orderEntry.GetProperty("generatedNamespace").GetString());
        Assert.Equal(120, orderEntry.GetProperty("fieldCount").GetInt32());
        Assert.Equal(19, orderEntry.GetProperty("messageCount").GetInt32());
        Assert.Equal(19, orderEntry.GetProperty("messageNames").GetArrayLength());
        Assert.Equal(19, orderEntry.GetProperty("messageTypes").GetArrayLength());

        var marketData = dictionaries["MarketData"];
        Assert.Equal("BinanceSpotMarketData", marketData.GetProperty("customName").GetString());
        Assert.Equal("Binance.FIX.Api.Internal.MarketData", marketData.GetProperty("generatedNamespace").GetString());
        Assert.Equal(67, marketData.GetProperty("fieldCount").GetInt32());
        Assert.Equal(14, marketData.GetProperty("messageCount").GetInt32());
        Assert.Equal(14, marketData.GetProperty("messageNames").GetArrayLength());
        Assert.Equal(14, marketData.GetProperty("messageTypes").GetArrayLength());

        var generation = root.GetProperty("generation");
        Assert.Equal(148, generation.GetProperty("unionFieldCount").GetInt32());
        Assert.Equal(39, generation.GetProperty("sharedFieldCount").GetInt32());
        Assert.Equal(
            ["Fields/FieldTags.cs", "Fields/Fields.cs"],
            generation.GetProperty("expectedQuickFixSourceMutations").EnumerateArray()
                .Select(path => path.GetString()
                    ?? throw new InvalidOperationException("A source mutation path cannot be null."))
                .Order(StringComparer.Ordinal).ToArray());
    }

    [Fact]
    public void LockPinsDeterministicInternalizedThreeTargetCompileGate()
    {
        using var document = LoadLock();
        var root = document.RootElement;
        var generation = root.GetProperty("generation");
        Assert.Equal(39, generation.GetProperty("rawSourceFileCount").GetInt32());
        Assert.Equal(259_112, generation.GetProperty("rawSourceBytes").GetInt64());
        Assert.Equal("5efdf1d2c0dc57f0b88daba7a2072a557450c5d4aa58c1aa7638291b06b327d0", generation.GetProperty("rawSourceManifestSha256").GetString());
        Assert.Equal(148, generation.GetProperty("internalFieldAccessibilityReplacements").GetInt32());
        Assert.Equal(1, generation.GetProperty("internalTagAccessibilityReplacements").GetInt32());
        Assert.Equal(37, generation.GetProperty("internalMessageAccessibilityReplacements").GetInt32());
        Assert.Equal(39, generation.GetProperty("adaptedSourceFileCount").GetInt32());
        Assert.Equal(261_717, generation.GetProperty("adaptedSourceBytes").GetInt64());
        Assert.Equal("5877d1deb46c1c433d41e1f9e1e1b03e3c3836d9383353cd2a610c3bf043811b", generation.GetProperty("adaptedSourceManifestSha256").GetString());

        Assert.Equal(
            ["net8.0", "net9.0", "net10.0"],
            root.GetProperty("compileTargetFrameworks").EnumerateArray()
                .Select(framework => framework.GetString()
                    ?? throw new InvalidOperationException("A compile target framework cannot be null."))
                .ToArray());
        Assert.Equal("1.14.1.0", root.GetProperty("expectedReferencedQuickFixAssemblyVersion").GetString());
        Assert.Equal(205, root.GetProperty("expectedDefinedTypeCount").GetInt32());
        Assert.Equal(149, root.GetProperty("expectedInternalFieldNamespaceTypeCount").GetInt32());
        Assert.Equal(0, root.GetProperty("expectedExportedTypeCount").GetInt32());
    }

    private static JsonDocument LoadLock()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException($"Missing embedded resource '{ResourceName}'.");
        return JsonDocument.Parse(stream);
    }
}
