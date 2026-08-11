using Binance.Api.Options;

namespace Binance.Api.Tests.Options;

public class BinanceOptionsCurrentRestSurfaceTests
{
    [Fact]
    public void UnsupportedWrapperOnlyOperations_AreAbsentFromPublicInterfaces()
    {
        var accountMethods = typeof(IBinanceOptionsRestClientAccount)
            .GetMethods()
            .Select(method => method.Name)
            .ToHashSet(StringComparer.Ordinal);
        var marketDataMethods = typeof(IBinanceOptionsRestClientMarketData)
            .GetMethods()
            .Select(method => method.Name)
            .ToHashSet(StringComparer.Ordinal);

        Assert.DoesNotContain("GetAccountAsync", accountMethods);
        Assert.DoesNotContain("GetTransactionHistoryDownloadIdAsync", accountMethods);
        Assert.DoesNotContain("GetTransactionHistoryDownloadLinkAsync", accountMethods);
        Assert.DoesNotContain("GetHistoricalTradesAsync", marketDataMethods);
    }

    [Fact]
    public void UnsupportedWrapperOnlyResponseTypes_AreAbsentFromPublicAssembly()
    {
        var assembly = typeof(IBinanceOptionsRestClientAccount).Assembly;
        var removedTypeNames = new[]
        {
            "Binance.Api.Options.BinanceOptionsAccount",
            "Binance.Api.Options.BinanceOptionsAccountBalance",
            "Binance.Api.Options.BinanceOptionsAccountGreek",
            "Binance.Api.Options.BinanceOptionsDownloadId",
            "Binance.Api.Options.BinanceOptionsDownloadLink"
        };

        foreach (var removedTypeName in removedTypeNames)
            Assert.Null(assembly.GetType(removedTypeName));
    }
}
