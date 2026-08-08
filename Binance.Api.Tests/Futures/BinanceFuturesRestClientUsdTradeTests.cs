namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientUsdTradeTests
{
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
}
