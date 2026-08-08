using Binance.Api.Futures;
using Binance.Api.Shared;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientUsdTradeTests
{
    [Fact]
    public async Task GetMarginChangeHistoryAsync_UsesV1QueryContractAndDeserializesResponse()
    {
        var handler = new RecordingHttpMessageHandler(
            """
            [
              {
                "symbol": "BTCUSDT",
                "type": 1,
                "deltaType": "USER_ADJUST",
                "amount": "23.36332311",
                "asset": "USDT",
                "time": 1578047897183,
                "positionSide": "BOTH"
              }
            ]
            """);
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient);
        var startTime = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var endTime = startTime.AddDays(30);

        var result = await client.UsdFutures.GetMarginChangeHistoryAsync(
            "BTCUSDT",
            BinanceFuturesMarginChangeDirectionType.Add,
            startTime,
            endTime,
            limit: 25,
            receiveWindow: 5_000);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/fapi/v1/positionMargin/history", handler.RequestUri!.AbsolutePath);
        Assert.Contains("symbol=BTCUSDT", handler.RequestUri.Query);
        Assert.Contains("type=1", handler.RequestUri.Query);
        Assert.Contains("startTime=1767225600000", handler.RequestUri.Query);
        Assert.Contains("endTime=1769817600000", handler.RequestUri.Query);
        Assert.Contains("limit=25", handler.RequestUri.Query);
        Assert.Contains("recvWindow=5000", handler.RequestUri.Query);
        Assert.Null(handler.Body);

        var item = Assert.Single(result.Data);
        Assert.Equal("BTCUSDT", item.Symbol);
        Assert.Equal(BinanceFuturesMarginChangeDirectionType.Add, item.Type);
        Assert.Equal("USER_ADJUST", item.DeltaType);
        Assert.Equal(23.36332311m, item.Quantity);
        Assert.Equal("USDT", item.Asset);
        Assert.Equal(BinancePositionSide.Both, item.PositionSide);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(31)]
    public async Task GetMarginChangeHistoryAsync_RejectsInvalidTimeRange(int endDayOffset)
    {
        using var httpClient = new HttpClient(new RecordingHttpMessageHandler("[]"));
        using var client = CreateClient(httpClient);
        var startTime = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.UsdFutures.GetMarginChangeHistoryAsync(
            "BTCUSDT",
            startTime: startTime,
            endTime: startTime.AddDays(endDayOffset)));
    }

    [Fact]
    public async Task GetPositionAdlQuantileEstimationAsync_SendsQueryAndReadsArrayForSymbol()
    {
        var handler = new RecordingHttpMessageHandler(
            """
            [
              {
                "symbol": "ETHUSDT",
                "adlQuantile": {
                  "LONG": 3,
                  "SHORT": 2,
                  "HEDGE": 0,
                  "BOTH": 1
                }
              }
            ]
            """);
        using var httpClient = new HttpClient(handler);
        using var client = new BinanceRestApiClient(new BinanceRestApiClientOptions("api-key", "api-secret")
        {
            AutoTimestamp = false,
            HttpClient = httpClient,
            RateLimiterEnabled = false
        });

        var result = await client.UsdFutures.GetPositionAdlQuantileEstimationAsync(
            "ETHUSDT",
            receiveWindow: 5_000);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/fapi/v1/adlQuantile", handler.RequestUri!.AbsolutePath);
        Assert.Contains("symbol=ETHUSDT", handler.RequestUri.Query);
        Assert.Contains("recvWindow=5000", handler.RequestUri.Query);
        Assert.Contains("timestamp=", handler.RequestUri.Query);
        Assert.Contains("signature=", handler.RequestUri.Query);
        Assert.Null(handler.Body);
        var estimation = Assert.Single(result.Data);
        Assert.Equal("ETHUSDT", estimation.Symbol);
        Assert.Equal(3, estimation.AdlQuantile!.Long);
        Assert.Equal(2, estimation.AdlQuantile.Short);
        Assert.Equal(0, estimation.AdlQuantile.Hedge);
        Assert.Equal(1, estimation.AdlQuantile.Both);
    }

    private static BinanceRestApiClient CreateClient(HttpClient httpClient)
        => new(new BinanceRestApiClientOptions("api-key", "api-secret")
        {
            AutoTimestamp = false,
            HttpClient = httpClient,
            RateLimiterEnabled = false
        });
}
