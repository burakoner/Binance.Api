using System.Reflection;
using System.Text.Json;
using Xunit;

namespace Binance.SBE.Api.Tests;

public class GenerationToolchainLockTests
{
    private const string ResourceName = "Binance.SBE.Api.Tests.binance-spot-fix-sbe-toolchain-lock.json";

    private static readonly IReadOnlyDictionary<string, (long Size, string Sha256)> ExpectedRuntimeSources =
        new Dictionary<string, (long, string)>(StringComparer.Ordinal)
        {
            ["csharp/sbe-dll/ByteOrder.cs"] = (362, "4221e7e9331341fb7a03b5853a0aaa350168727eb9bc8dfa38705bb0bf9e6c23"),
            ["csharp/sbe-dll/DirectBuffer.cs"] = (32046, "023850825fb69a0497301f6c2f14fb6a3ef40b0969ff3fb9da23ce95db5b3dcd"),
            ["csharp/sbe-dll/EndianessConverter.cs"] = (5939, "a160bcefdae7dbd9d282cffe035391316af40c1238a805bc87b218e7c8f0096e"),
            ["csharp/sbe-dll/PrimitiveType.cs"] = (8468, "5f4feacd899f6b4be3e4bd354753598794a64bd4a636cc834e62f6a39feba8e0"),
            ["csharp/sbe-dll/PrimitiveValue.cs"] = (18210, "30f79101101b3da555ad253b38f8ab9d0115ba03387e1ead81c285ecb8fb1292"),
            ["csharp/sbe-dll/SbePrimitiveType.cs"] = (1125, "5dfbcecadaa0f83d934696e27b4b568d0dc7a75a8038b3a8121077bc74334db2"),
            ["csharp/sbe-dll/ThrowHelper.cs"] = (2259, "4f0158225f3a3da5a374db071f12a4cf579aaedf634bdff254fe9ebeee40981c")
        };

    [Fact]
    public void LockPinsLicensedSbeToolAndExactMatchingRuntimeSources()
    {
        using var document = LoadLock();
        var root = document.RootElement;
        Assert.False(root.GetProperty("artifactsRedistributed").GetBoolean());
        Assert.False(root.GetProperty("generatedArtifactsCommitted").GetBoolean());

        var sbe = root.GetProperty("sbe");
        Assert.Equal("1.39.0", sbe.GetProperty("version").GetString());
        Assert.Equal("e773b57cac6b2008ce30dd219a33de49766c6013", sbe.GetProperty("sourceCommit").GetString());
        Assert.Equal("Apache-2.0", sbe.GetProperty("license").GetString());

        var executable = sbe.GetProperty("executable");
        Assert.Equal(1061675, executable.GetProperty("size").GetInt64());
        Assert.Equal(
            "5c44ada658c7f02223d9db30ec0b419d31fe2663d9640ec7585feb16b339ef7a",
            executable.GetProperty("sha256").GetString());

        var runtimeSources = sbe.GetProperty("runtimeSources").EnumerateArray()
            .ToDictionary(source => source.GetProperty("upstreamPath").GetString()!, StringComparer.Ordinal);
        Assert.Equal(ExpectedRuntimeSources.Count, runtimeSources.Count);
        foreach (var expected in ExpectedRuntimeSources)
        {
            var source = runtimeSources[expected.Key];
            Assert.Equal(expected.Value.Size, source.GetProperty("size").GetInt64());
            Assert.Equal(expected.Value.Sha256, source.GetProperty("sha256").GetString());
        }
    }

    [Fact]
    public void LockPinsPortableJdkAndNetstandard20SupportAssembly()
    {
        using var document = LoadLock();
        var java = document.RootElement.GetProperty("java");
        Assert.Equal("Eclipse Temurin", java.GetProperty("distribution").GetString());
        Assert.Equal("17.0.20+8", java.GetProperty("version").GetString());
        Assert.Equal(190818901, java.GetProperty("archiveSize").GetInt64());
        Assert.Equal(
            "418497be5cf585bdd2203d6486a565d66d3f5e992d5630d45104cb873fab8122",
            java.GetProperty("archiveSha256").GetString());
        Assert.Equal(
            "5b463cad4fcd8e4c655cc1c6f45a3b2ebb002adac94ebad2fdaa4f43e2aee211",
            java.GetProperty("javaExecutableSha256").GetString());

        var systemMemory = document.RootElement.GetProperty("systemMemory");
        Assert.Equal("4.5.3", systemMemory.GetProperty("version").GetString());
        Assert.Equal("MIT", systemMemory.GetProperty("license").GetString());
        Assert.Equal(
            "0af97b45b45b46ef6a2b37910568dabd492c793da3859054595d523e2a545859",
            systemMemory.GetProperty("packageSha256").GetString());
        Assert.Equal(
            "41b5e1a4c59abdb1ce1467f58c3d9fd06d39dff4fc61d500a2410fece8037f4b",
            systemMemory.GetProperty("assemblySha256").GetString());
    }

    [Fact]
    public void LockPinsDeterministicInternalizedFiveTargetCompileGate()
    {
        using var document = LoadLock();
        var root = document.RootElement;
        var generation = root.GetProperty("generation");
        Assert.Equal("spot-fixsbe-1_1.xml", generation.GetProperty("schemaFileName").GetString());
        Assert.Equal(1, generation.GetProperty("schemaId").GetInt32());
        Assert.Equal(1, generation.GetProperty("schemaVersion").GetInt32());
        Assert.True(generation.GetProperty("generatePrecedenceChecks").GetBoolean());
        Assert.Equal("Fix_sbe", generation.GetProperty("namespace").GetString());
        Assert.False(generation.GetProperty("standardSbeXsdCompatible").GetBoolean());
        Assert.Equal(56, generation.GetProperty("vendorExponentAttributeCount").GetInt32());
        Assert.Equal(
            ["Exponent", "PriceExponent", "QtyExponent"],
            generation.GetProperty("vendorExponentReferences").EnumerateArray()
                .Select(reference => reference.GetString()
                    ?? throw new InvalidOperationException("A vendor exponent reference cannot be null."))
                .ToArray());

        var warning = Assert.Single(generation.GetProperty("expectedWarnings").EnumerateArray().ToArray());
        Assert.Contains("MarketDataIncrementalTrade", warning.GetString());
        Assert.Contains("numInGroup", warning.GetString());
        Assert.Equal(84, generation.GetProperty("generatedFileCount").GetInt32());
        Assert.Equal(1498986, generation.GetProperty("generatedBytes").GetInt64());
        Assert.Equal(
            "91e6310676c8bbcb0b5978ee28577069abbf4ccac6e16f685ee6cf52c88710c5",
            generation.GetProperty("generatedManifestSha256").GetString());
        Assert.Equal(84, generation.GetProperty("topLevelAccessibilityReplacements").GetInt32());
        Assert.Equal(
            "e788b0a516aac4fb468d1ac6efe39b188ebd5588136f3b5bd9255b09eb0ec8a7",
            generation.GetProperty("internalizedManifestSha256").GetString());

        var frameworks = root.GetProperty("compileTargetFrameworks").EnumerateArray()
            .Select(framework => framework.GetString()
                ?? throw new InvalidOperationException("A compile target framework cannot be null."))
            .ToArray();
        Assert.Equal(["netstandard2.0", "netstandard2.1", "net8.0", "net9.0", "net10.0"], frameworks);
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
