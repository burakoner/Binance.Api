using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Options;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Options;

public class BinanceOptionsReadOnlyTradeTests
{
    [Fact]
    public async Task GetBlockTrades_UsesCurrentSignedContractAndDeserializesResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """[{"parentOrderId":"4675011431944499201","crossType":"USER_BLOCK","legs":[{"createTime":1730170445600,"updateTime":1730170445601,"symbol":"BNB-241101-700-C","orderId":"4675011431944499203","orderPrice":2.8,"orderQuantity":1.2,"orderStatus":"FILLED","executedQty":1.2,"executedAmount":3.36,"fee":0.336,"orderType":"PREV_QUOTED","orderSide":"BUY","id":"1125899906900937837","tradeId":9223372036854775807,"tradePrice":2.8,"tradeQty":1.2,"tradeTime":1730170445602,"liquidity":"TAKER","commission":0.336}],"blockTradeSettlementKey":"7d085e6e-a229-2335-ab9d-6a581febcd25"}]""");
        using var client = CreateClient(handler, limiter);
        var startTime = DateTimeOffset.FromUnixTimeMilliseconds(1_730_000_000_000).UtcDateTime;
        var endTime = DateTimeOffset.FromUnixTimeMilliseconds(1_730_100_000_000).UtcDateTime;

        var result = await client.Options.MarketMaker.GetBlockTradesAsync("BTCUSDT", startTime, endTime, 60_000);

        Assert.True(result.Success);
        var trade = Assert.Single(result.Data);
        Assert.Equal("4675011431944499201", trade.ParentOrderId);
        Assert.Equal("USER_BLOCK", trade.CrossType);
        Assert.Equal("7d085e6e-a229-2335-ab9d-6a581febcd25", trade.BlockTradeSettlementKey);
        var leg = Assert.Single(trade.Legs);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_730_170_445_600).UtcDateTime, leg.CreateTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_730_170_445_601).UtcDateTime, leg.UpdateTime);
        Assert.Equal("BNB-241101-700-C", leg.Symbol);
        Assert.Equal("4675011431944499203", leg.OrderId);
        Assert.Equal(2.8m, leg.OrderPrice);
        Assert.Equal(1.2m, leg.OrderQuantity);
        Assert.Equal("FILLED", leg.OrderStatus);
        Assert.Equal(1.2m, leg.ExecutedQuantity);
        Assert.Equal(3.36m, leg.ExecutedAmount);
        Assert.Equal(0.336m, leg.Fee);
        Assert.Equal("PREV_QUOTED", leg.OrderType);
        Assert.Equal(BinanceOrderSide.Buy, leg.OrderSide);
        Assert.Equal("1125899906900937837", leg.Id);
        Assert.Equal(long.MaxValue, leg.TradeId);
        Assert.Equal(2.8m, leg.TradePrice);
        Assert.Equal(1.2m, leg.TradeQuantity);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_730_170_445_602).UtcDateTime, leg.TradeTime);
        Assert.Equal(BinanceOptionsLiquidity.Taker, leg.Liquidity);
        Assert.Equal(0.336m, leg.Commission);
        AssertSignedGet(handler, limiter, "/eapi/v1/block/user-trades", 5);
        var query = Uri.UnescapeDataString(handler.RequestUri!.Query);
        Assert.Contains("underlying=BTCUSDT", query);
        Assert.Contains("startTime=1730000000000", query);
        Assert.Contains("endTime=1730100000000", query);
        Assert.Contains("recvWindow=60000", query);
    }

    [Fact]
    public async Task GetUserCommission_UsesCurrentSignedContractAndDeserializesResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"commissions":[{"underlying":"BTCUSDT","makerFee":"0.0002","takerFee":"0.0004"},{"underlying":"ETHUSDT","makerFee":"0.0003","takerFee":"0.0005"}]}""");
        using var client = CreateClient(handler, limiter, TimeSpan.FromMilliseconds(5_000));

        var result = await client.Options.GetUserCommissionAsync();

        Assert.True(result.Success);
        Assert.Collection(
            result.Data.Commissions,
            commission =>
            {
                Assert.Equal("BTCUSDT", commission.Underlying);
                Assert.Equal(0.0002m, commission.MakerFee);
                Assert.Equal(0.0004m, commission.TakerFee);
            },
            commission =>
            {
                Assert.Equal("ETHUSDT", commission.Underlying);
                Assert.Equal(0.0003m, commission.MakerFee);
                Assert.Equal(0.0005m, commission.TakerFee);
            });
        AssertSignedGet(handler, limiter, "/eapi/v1/commission", 5);
        Assert.Contains("recvWindow=5000", Uri.UnescapeDataString(handler.RequestUri!.Query));
    }

    [Fact]
    public async Task TouchedQueries_RejectExplicitOrConfiguredReceiveWindowAboveMaximum()
    {
        using (var client = CreateClient(new RecordingHttpMessageHandler("[]")))
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.MarketMaker.GetBlockTradesAsync(receiveWindow: 60_001));

        using var configuredClient = CreateClient(
            new RecordingHttpMessageHandler("{}"),
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => configuredClient.Options.GetUserCommissionAsync());
    }

    private static void AssertSignedGet(
        RecordingHttpMessageHandler handler,
        RecordingRateLimiter limiter,
        string path,
        int weight)
    {
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal(path, handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == path && item.Weight == weight && item.Signed);
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
