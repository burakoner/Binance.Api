using System.Reflection;
using Xunit;

namespace Binance.FIX.Api.Tests;

public class SessionOptionsTests
{
    [Theory]
    [InlineData(BinanceFixEnvironment.Production, BinanceFixSessionRole.OrderEntry, "tcp+tls://fix-oe.binance.com:9000/")]
    [InlineData(BinanceFixEnvironment.Production, BinanceFixSessionRole.DropCopy, "tcp+tls://fix-dc.binance.com:9000/")]
    [InlineData(BinanceFixEnvironment.Production, BinanceFixSessionRole.MarketData, "tcp+tls://fix-md.binance.com:9000/")]
    [InlineData(BinanceFixEnvironment.SpotTestnet, BinanceFixSessionRole.OrderEntry, "tcp+tls://fix-oe.testnet.binance.vision:9000/")]
    [InlineData(BinanceFixEnvironment.SpotTestnet, BinanceFixSessionRole.DropCopy, "tcp+tls://fix-dc.testnet.binance.vision:9000/")]
    [InlineData(BinanceFixEnvironment.SpotTestnet, BinanceFixSessionRole.MarketData, "tcp+tls://fix-md.testnet.binance.vision:9000/")]
    public void ResolvesOnlyOfficialTextFixEndpoints(
        BinanceFixEnvironment environment,
        BinanceFixSessionRole role,
        string expectedEndpoint)
    {
        var options = new BinanceFixSessionOptions(environment, role, "SESSION1");

        Assert.Equal(expectedEndpoint, options.Endpoint.AbsoluteUri);
        Assert.Equal(environment, options.Environment);
        Assert.Equal(role, options.Role);
        Assert.Equal("SESSION1", options.SenderCompId);
    }

    [Fact]
    public void UsesSafetyFirstLogonDefaults()
    {
        var options = new BinanceFixSessionOptions(
            BinanceFixEnvironment.SpotTestnet,
            BinanceFixSessionRole.OrderEntry,
            "OE_1");

        Assert.Equal("SPOT", BinanceFixSessionOptions.TargetCompId);
        Assert.Equal(30, options.HeartbeatIntervalSeconds);
        Assert.Equal(BinanceFixMessageHandling.Sequential, options.MessageHandling);
        Assert.Equal(BinanceFixResponseMode.Everything, options.ResponseMode);
        Assert.Null(options.DataDictionaryPath);
    }

    [Fact]
    public void OmitsResponseModeFromMarketDataLogonContract()
    {
        var options = new BinanceFixSessionOptions(
            BinanceFixEnvironment.SpotTestnet,
            BinanceFixSessionRole.MarketData,
            "MD_1");

        Assert.Null(options.ResponseMode);

        var exception = Assert.Throws<ArgumentException>(() => new BinanceFixSessionOptions(
            BinanceFixEnvironment.SpotTestnet,
            BinanceFixSessionRole.MarketData,
            "MD_1",
            responseMode: BinanceFixResponseMode.Everything));

        Assert.Equal("responseMode", exception.ParamName);
    }

    [Fact]
    public void RequiresExplicitOptInForNonDefaultLogonModes()
    {
        var options = new BinanceFixSessionOptions(
            BinanceFixEnvironment.SpotTestnet,
            BinanceFixSessionRole.OrderEntry,
            "OE_1",
            messageHandling: BinanceFixMessageHandling.Unordered,
            responseMode: BinanceFixResponseMode.OnlyAcknowledgements);

        Assert.Equal(BinanceFixMessageHandling.Unordered, options.MessageHandling);
        Assert.Equal(BinanceFixResponseMode.OnlyAcknowledgements, options.ResponseMode);
    }

    [Fact]
    public void DoesNotExposeMutableValidatedProperties()
    {
        var properties = typeof(BinanceFixSessionOptions).GetProperties(BindingFlags.Instance | BindingFlags.Public);

        Assert.NotEmpty(properties);
        Assert.All(properties, property => Assert.False(property.CanWrite, property.Name));
    }

    [Theory]
    [InlineData("A")]
    [InlineData("aZ09-_")]
    [InlineData("12345678")]
    public void AcceptsPublishedSenderCompIdDomain(string senderCompId)
    {
        var options = new BinanceFixSessionOptions(
            BinanceFixEnvironment.Production,
            BinanceFixSessionRole.DropCopy,
            senderCompId);

        Assert.Equal(senderCompId, options.SenderCompId);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("123456789")]
    [InlineData("WITH SPACE")]
    [InlineData("DOT.ID")]
    [InlineData("UNICODÉ")]
    public void RejectsSenderCompIdsOutsidePublishedAsciiDomain(string? senderCompId)
    {
        var exception = Assert.Throws<ArgumentException>(() => new BinanceFixSessionOptions(
            BinanceFixEnvironment.Production,
            BinanceFixSessionRole.OrderEntry,
            senderCompId!));

        Assert.Equal("senderCompId", exception.ParamName);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(60)]
    public void AcceptsPublishedHeartbeatBoundaries(int heartbeatIntervalSeconds)
    {
        var options = new BinanceFixSessionOptions(
            BinanceFixEnvironment.SpotTestnet,
            BinanceFixSessionRole.MarketData,
            "MD",
            heartbeatIntervalSeconds);

        Assert.Equal(heartbeatIntervalSeconds, options.HeartbeatIntervalSeconds);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(61)]
    public void RejectsHeartbeatOutsidePublishedBoundaries(int heartbeatIntervalSeconds)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new BinanceFixSessionOptions(
            BinanceFixEnvironment.SpotTestnet,
            BinanceFixSessionRole.MarketData,
            "MD",
            heartbeatIntervalSeconds));

        Assert.Equal("heartbeatIntervalSeconds", exception.ParamName);
    }

    [Fact]
    public void RejectsUndefinedEnvironmentBeforeAnyEndpointCanBeSelected()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new BinanceFixSessionOptions(
            (BinanceFixEnvironment)99,
            BinanceFixSessionRole.OrderEntry,
            "OE"));

        Assert.Equal("environment", exception.ParamName);
    }

    [Fact]
    public void RejectsUndefinedRoleBeforeAnyEndpointCanBeSelected()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new BinanceFixSessionOptions(
            BinanceFixEnvironment.Production,
            (BinanceFixSessionRole)99,
            "OE"));

        Assert.Equal("role", exception.ParamName);
    }

    [Fact]
    public void RejectsUndefinedLogonModes()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new BinanceFixSessionOptions(
            BinanceFixEnvironment.Production,
            BinanceFixSessionRole.OrderEntry,
            "OE",
            messageHandling: (BinanceFixMessageHandling)99));

        Assert.Throws<ArgumentOutOfRangeException>(() => new BinanceFixSessionOptions(
            BinanceFixEnvironment.Production,
            BinanceFixSessionRole.OrderEntry,
            "OE",
            responseMode: (BinanceFixResponseMode)99));
    }

    [Fact]
    public void StoresAConfiguredDictionaryAsAnAbsolutePathAndRejectsWhitespace()
    {
        var options = new BinanceFixSessionOptions(
            BinanceFixEnvironment.SpotTestnet,
            BinanceFixSessionRole.OrderEntry,
            "OE",
            dataDictionaryPath: ".\\spot-fix-oe.xml");

        Assert.Equal(Path.GetFullPath(".\\spot-fix-oe.xml"), options.DataDictionaryPath);
        Assert.Throws<ArgumentException>(() => new BinanceFixSessionOptions(
            BinanceFixEnvironment.SpotTestnet,
            BinanceFixSessionRole.OrderEntry,
            "OE",
            dataDictionaryPath: " "));
    }
}
