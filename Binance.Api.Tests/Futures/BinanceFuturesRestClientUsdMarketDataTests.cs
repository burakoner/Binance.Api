using Binance.Api.Futures;
using Binance.Api.Shared;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientUsdMarketDataTests
{
    [Theory]
    [InlineData("openInterestHist")]
    [InlineData("topLongShortPositionRatio")]
    [InlineData("topLongShortAccountRatio")]
    [InlineData("globalLongShortAccountRatio")]
    [InlineData("takerlongshortRatio")]
    [InlineData("basis")]
    public async Task FuturesDataMethods_SendRequestsToDocumentedRootPath(string endpoint)
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient);

        switch (endpoint)
        {
            case "openInterestHist":
                await client.UsdFutures.GetOpenInterestHistoryAsync("BTCUSDT", BinancePeriodInterval.FiveMinutes);
                break;
            case "topLongShortPositionRatio":
                await client.UsdFutures.GetTopLongShortPositionRatioAsync("BTCUSDT", BinancePeriodInterval.FiveMinutes);
                break;
            case "topLongShortAccountRatio":
                await client.UsdFutures.GetTopLongShortAccountRatioAsync("BTCUSDT", BinancePeriodInterval.FiveMinutes);
                break;
            case "globalLongShortAccountRatio":
                await client.UsdFutures.GetGlobalLongShortAccountRatioAsync("BTCUSDT", BinancePeriodInterval.FiveMinutes);
                break;
            case "takerlongshortRatio":
                await client.UsdFutures.GetTakerBuySellVolumeRatioAsync("BTCUSDT", BinancePeriodInterval.FiveMinutes);
                break;
            case "basis":
                await client.UsdFutures.GetBasisAsync("BTCUSDT", BinanceFuturesContractType.Perpetual, BinancePeriodInterval.FiveMinutes);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(endpoint), endpoint, null);
        }

        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal($"/futures/data/{endpoint}", handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
    }

    [Fact]
    public async Task GetOpenInterestHistoryAsync_DeserializesCirculatingSupply()
    {
        var handler = new RecordingHttpMessageHandler(
            """
            [
              {
                "symbol": "BTCUSDT",
                "sumOpenInterest": "20403.12345678",
                "sumOpenInterestValue": "176196512.12345678",
                "CMCCirculatingSupply": "165880.538",
                "timestamp": 1591261042378
              }
            ]
            """);
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient);

        var result = await client.UsdFutures.GetOpenInterestHistoryAsync(
            "BTCUSDT",
            BinancePeriodInterval.FiveMinutes);

        Assert.True(result.Success);
        var item = Assert.Single(result.Data);
        Assert.Equal(165880.538m, item.CmcCirculatingSupply);
    }

    [Theory]
    [InlineData(BinanceFuturesContractType.Unknown)]
    [InlineData(BinanceFuturesContractType.CurrentMonth)]
    public async Task GetBasisAsync_RejectsUnsupportedContractTypes(BinanceFuturesContractType contractType)
    {
        using var client = CreateClient(new HttpClient(new RecordingHttpMessageHandler("[]")));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.UsdFutures.GetBasisAsync(
            "BTCUSDT",
            contractType,
            BinancePeriodInterval.FiveMinutes));
    }

    private static BinanceRestApiClient CreateClient(HttpClient httpClient)
        => new(new BinanceRestApiClientOptions("api-key", "api-secret")
        {
            AutoTimestamp = false,
            HttpClient = httpClient,
            RateLimiterEnabled = false
        });
}
