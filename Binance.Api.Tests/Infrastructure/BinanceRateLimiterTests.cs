using ApiSharp.Throttling;

namespace Binance.Api.Tests.Infrastructure;

public class BinanceRateLimiterTests
{
    [Fact]
    public async Task DisabledRateLimiter_DoesNotApplyConfiguredLimiters()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var httpClient = new HttpClient(handler);
        var options = new BinanceRestApiClientOptions("api-key", "api-secret")
        {
            AutoTimestamp = false,
            HttpClient = httpClient,
            RateLimiterEnabled = false
        };
#pragma warning disable CS0612
        options.RateLimiters =
        [
            new RateLimiter().AddPartialEndpointLimit("/api/", 1, TimeSpan.FromMinutes(1))
        ];
#pragma warning restore CS0612
        using var client = new BinanceRestApiClient(options);

        var result = await client.Spot.GetRecentTradesAsync("BTCUSDT");

        Assert.True(result.Success);
        Assert.Equal("/api/v3/trades", handler.RequestUri!.AbsolutePath);
    }
}
