namespace Binance.Api.Tests.Margin;

public class BinanceMarginRestClientTradeTests
{
    [Fact]
    public async Task CancelMarginOrderAsync_SendsDocumentedDeleteRequest()
    {
        var handler = new RecordingHttpMessageHandler(
            """
            {
              "symbol": "BTCUSDT",
              "orderId": 28,
              "origClientOrderId": "original-order",
              "clientOrderId": "cancel-order",
              "status": "CANCELED",
              "timeInForce": "GTC",
              "type": "LIMIT",
              "side": "SELL",
              "isIsolated": false
            }
            """);
        using var httpClient = new HttpClient(handler);
        using var client = new BinanceRestApiClient(new BinanceRestApiClientOptions("api-key", "api-secret")
        {
            AutoTimestamp = false,
            HttpClient = httpClient,
            RateLimiterEnabled = false
        });

        var result = await client.Margin.CancelMarginOrderAsync(
            "BTCUSDT",
            orderId: 28,
            newClientOrderId: "cancel-order",
            isIsolated: false,
            receiveWindow: 5_000);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Delete, handler.Method);
        Assert.Equal("/sapi/v1/margin/order", handler.RequestUri!.AbsolutePath);
        Assert.Contains("signature=", handler.RequestUri.Query);
        Assert.Contains("symbol=BTCUSDT", handler.Body);
        Assert.Contains("orderId=28", handler.Body);
        Assert.Contains("newClientOrderId=cancel-order", handler.Body);
        Assert.Contains("isIsolated=False", handler.Body);
        Assert.Contains("recvWindow=5000", handler.Body);
        Assert.Contains("timestamp=", handler.Body);
        Assert.Equal("application/x-www-form-urlencoded", handler.ContentType);
    }

}
