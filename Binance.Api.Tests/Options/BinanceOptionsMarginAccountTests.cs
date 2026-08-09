using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Options;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Options;

public class BinanceOptionsMarginAccountTests
{
    [Fact]
    public async Task GetMarginAccount_UsesCurrentSignedContractAndDeserializesCompleteResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            {
              "asset": [
                {
                  "asset": "USDT",
                  "marginBalance": "10099.448",
                  "equity": "10094.44662",
                  "available": "8725.92524",
                  "initialMargin": "1084.52138",
                  "maintMargin": "151.00138",
                  "unrealizedPNL": "-5.00138",
                  "adjustedEquity": "34.13282285"
                }
              ],
              "greek": [
                {
                  "underlying": "BTCUSDT",
                  "delta": "-0.05",
                  "gamma": "-0.002",
                  "theta": "-0.05",
                  "vega": "-0.002"
                }
              ],
              "time": 1762843368098,
              "canTrade": true,
              "canDeposit": true,
              "canWithdraw": true,
              "reduceOnly": false,
              "tradeGroupId": 9223372036854775806
            }
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.Options.GetMarginAccountAsync(60_000);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/eapi/v1/marginAccount", handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("recvWindow=60000", query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        Assert.Contains(limiter.Requests, item =>
            item.Endpoint == "/eapi/v1/marginAccount" && item.Weight == 3 && item.Signed);

        var data = result.Data!;
        var balance = Assert.Single(data.Balances);
        Assert.Equal("USDT", balance.Asset);
        Assert.Equal(10099.448m, balance.MarginBalance);
        Assert.Equal(10094.44662m, balance.Equity);
        Assert.Equal(8725.92524m, balance.Available);
        Assert.Equal(1084.52138m, balance.InitialMargin);
        Assert.Equal(151.00138m, balance.MaintenanceMargin);
        Assert.Equal(-5.00138m, balance.UnrealizedPNL);
        Assert.Equal(34.13282285m, balance.AdjustedEquity);
        var greek = Assert.Single(data.Greek);
        Assert.Equal("BTCUSDT", greek.Underlying);
        Assert.Equal(-0.05m, greek.Delta);
        Assert.Equal(-0.002m, greek.Gamma);
        Assert.Equal(-0.05m, greek.Theta);
        Assert.Equal(-0.002m, greek.Vega);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1762843368098).UtcDateTime, data.Time);
        Assert.True(data.CanTrade);
        Assert.True(data.CanDeposit);
        Assert.True(data.CanWithdraw);
        Assert.False(data.ReduceOnly);
        Assert.Equal(9223372036854775806L, data.TradeGroupId);
    }

    [Fact]
    public async Task GetMarginAccount_UsesConfiguredReceiveWindowWithoutInventingAnUndocumentedCeiling()
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using var client = CreateClient(
            handler,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(3_000_000_000L));

        var result = await client.Options.GetMarginAccountAsync();

        Assert.True(result.Success);
        Assert.Contains("recvWindow=3000000000", Uri.UnescapeDataString(handler.RequestUri!.Query));
    }

    private static BinanceRestApiClient CreateClient(
        RecordingHttpMessageHandler handler,
        IRateLimiter? limiter = null,
        TimeSpan? defaultReceiveWindow = null)
    {
        var options = new BinanceRestApiClientOptions(new ApiCredentials("api-key", "api-secret"))
        {
            AutoTimestamp = false,
            HttpClient = new HttpClient(handler),
            RateLimiterEnabled = limiter != null,
            ReceiveWindow = defaultReceiveWindow
        };
#pragma warning disable CS0612
        options.RateLimiters = limiter == null ? [] : [limiter];
#pragma warning restore CS0612
        return new BinanceRestApiClient(options);
    }

    private sealed class RecordingRateLimiter : IRateLimiter
    {
        internal List<(string Endpoint, int Weight, bool Signed)> Requests { get; } = [];

        public Task<CallResult<int>> LimitRequestAsync(
            ILogger logger,
            string endpoint,
            HttpMethod method,
            bool signed,
            SensitiveString? apiKey,
            RateLimitingBehavior limitBehaviour,
            int requestWeight,
            CancellationToken ct)
        {
            Requests.Add((endpoint, requestWeight, signed));
            return Task.FromResult(new CallResult<int>(0));
        }
    }
}
