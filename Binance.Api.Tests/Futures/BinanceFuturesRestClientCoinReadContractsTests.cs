using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientCoinReadContractsTests
{
    [Fact]
    public async Task AccountInformation_UsesCurrentSignedContractAndResponseSchema()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            {
              "assets": [
                {
                  "asset": "BTC",
                  "walletBalance": "0.00241969",
                  "unrealizedProfit": "0.00000000",
                  "marginBalance": "0.00241969",
                  "maintMargin": "0.00000000",
                  "initialMargin": "0.00000000",
                  "positionInitialMargin": "0.00000000",
                  "openOrderInitialMargin": "0.00000000",
                  "maxWithdrawAmount": "0.00241969",
                  "crossWalletBalance": "0.00241969",
                  "crossUnPnl": "0.00000000",
                  "availableBalance": "0.00241969",
                  "updateTime": 1625474304765
                }
              ],
              "positions": [
                {
                  "symbol": "BTCUSD_201225",
                  "positionAmt": "2",
                  "initialMargin": "0.1",
                  "maintMargin": "0.01",
                  "unrealizedProfit": "0.00012345",
                  "positionInitialMargin": "0.1",
                  "openOrderInitialMargin": "0.02",
                  "leverage": "125",
                  "isolated": false,
                  "positionSide": "BOTH",
                  "entryPrice": "35000.5",
                  "breakEvenPrice": "35001.5",
                  "maxQty": "50",
                  "updateTime": 1625474304765,
                  "notionalValue": "100.25"
                }
              ],
              "canDeposit": true,
              "canTrade": true,
              "canWithdraw": true,
              "feeTier": 3000000000,
              "updateTime": 1625474304765
            }
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.CoinFutures.GetAccountInfoAsync(60_000);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/dapi/v1/account", handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var accountApiKeys));
        Assert.Equal("api-key", Assert.Single(accountApiKeys!));
        Assert.Contains("recvWindow=60000", DecodedQuery(handler));
        Assert.Contains("timestamp=", DecodedQuery(handler));
        Assert.Contains("signature=", DecodedQuery(handler));
        Assert.Contains(limiter.Requests, request =>
            request.Endpoint == "/dapi/v1/account" && request.Weight == 5 && request.Signed);

        var account = result.Data!;
        Assert.True(account.CanDeposit);
        Assert.True(account.CanTrade);
        Assert.True(account.CanWithdraw);
        Assert.Equal(3_000_000_000L, account.FeeTier);
        Assert.Equal(
            DateTimeOffset.FromUnixTimeMilliseconds(1_625_474_304_765).UtcDateTime,
            account.UpdateTime);
        Assert.Null(typeof(BinanceFuturesCoinAccountInfo).GetProperty("UpdateTier"));

        var asset = Assert.Single(account.Assets);
        Assert.Equal("BTC", asset.Asset);
        Assert.Equal(0.00241969m, asset.WalletBalance);
        Assert.Equal(0m, asset.UnrealizedPnl);
        Assert.Equal(0.00241969m, asset.MarginBalance);
        Assert.Equal(0m, asset.MaintMargin);
        Assert.Equal(0m, asset.InitialMargin);
        Assert.Equal(0m, asset.PositionInitialMargin);
        Assert.Equal(0m, asset.OpenOrderInitialMargin);
        Assert.Equal(0.00241969m, asset.MaxWithdrawQuantity);
        Assert.Equal(0.00241969m, asset.CrossWalletBalance);
        Assert.Equal(0m, asset.CrossUnrealizedPnl);
        Assert.Equal(0.00241969m, asset.AvailableBalance);
        Assert.Equal(
            DateTimeOffset.FromUnixTimeMilliseconds(1_625_474_304_765).UtcDateTime,
            asset.UpdateTime);

        var position = Assert.Single(account.Positions);
        Assert.Equal("BTCUSD_201225", position.Symbol);
        Assert.Equal(2m, position.Quantity);
        Assert.Equal(0.1m, position.InitialMargin);
        Assert.Equal(0.01m, position.MaintMargin);
        Assert.Equal(0.00012345m, position.UnrealizedProfit);
        Assert.Equal(0.1m, position.PositionInitialMargin);
        Assert.Equal(0.02m, position.OpenOrderInitialMargin);
        Assert.Equal(125, position.Leverage);
        Assert.False(position.Isolated);
        Assert.Equal(BinancePositionSide.Both, position.PositionSide);
        Assert.Equal(35_000.5m, position.EntryPrice);
        Assert.Equal(100.25m, position.NotionalValue);
        Assert.Equal(35_001.5m, position.BreakEvenPrice);
        Assert.Equal(50m, position.MaxQuantity);
        Assert.Equal(
            DateTimeOffset.FromUnixTimeMilliseconds(1_625_474_304_765).UtcDateTime,
            position.UpdateTime);
    }

    [Theory]
    [InlineData("BTCUSD_PERP", null, 1)]
    [InlineData(null, "BTCUSD", 40)]
    [InlineData(null, null, 40)]
    public async Task CurrentOpenOrders_SupportCurrentFiltersAndWeights(
        string? symbol,
        string? pair,
        int expectedWeight)
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler, limiter);

        var result = await client.CoinFutures.GetOpenOrdersAsync(
            symbol: symbol,
            pair: pair,
            receiveWindow: 60_000);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/dapi/v1/openOrders", handler.RequestUri!.AbsolutePath);
        var query = DecodedQuery(handler);
        Assert.Equal(symbol is not null, query.Contains($"symbol={symbol}"));
        Assert.Equal(pair is not null, query.Contains($"pair={pair}"));
        Assert.Contains("recvWindow=60000", query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        Assert.Contains(limiter.Requests, request =>
            request.Endpoint == "/dapi/v1/openOrders"
            && request.Weight == expectedWeight
            && request.Signed);
    }

    [Fact]
    public async Task PositionInformation_UsesRestSpecificCurrentSchema()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            [
              {
                "symbol": "BTCUSD_201225",
                "positionAmt": "2",
                "entryPrice": "35000.5",
                "breakEvenPrice": "35001.5",
                "markPrice": "36000.25",
                "unRealizedProfit": "0.00012345",
                "liquidationPrice": "10000",
                "leverage": "125",
                "maxQty": "50",
                "marginType": "cross",
                "isolatedMargin": "0.00000000",
                "isAutoAddMargin": "false",
                "positionSide": "BOTH",
                "updateTime": 1625474304765
              }
            ]
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.CoinFutures.GetPositionsAsync(
            marginAsset: "BTC",
            pair: "BTCUSD",
            receiveWindow: 60_000);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/dapi/v1/positionRisk", handler.RequestUri!.AbsolutePath);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var positionApiKeys));
        Assert.Equal("api-key", Assert.Single(positionApiKeys!));
        var query = DecodedQuery(handler);
        Assert.Contains("marginAsset=BTC", query);
        Assert.Contains("pair=BTCUSD", query);
        Assert.Contains("recvWindow=60000", query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        Assert.Contains(limiter.Requests, request =>
            request.Endpoint == "/dapi/v1/positionRisk" && request.Weight == 1 && request.Signed);

        var position = Assert.Single(result.Data!);
        Assert.IsType<BinanceFuturesCoinPositionRisk>(position);
        Assert.Null(typeof(BinanceFuturesCoinPositionRisk).GetProperty("NotionalValue"));
        Assert.Equal("BTCUSD_201225", position.Symbol);
        Assert.Equal(2m, position.Quantity);
        Assert.Equal(35_000.5m, position.EntryPrice);
        Assert.Equal(35_001.5m, position.BreakEvenPrice);
        Assert.Equal(36_000.25m, position.MarkPrice);
        Assert.Equal(0.00012345m, position.UnrealizedProfit);
        Assert.Equal(10_000m, position.LiquidationPrice);
        Assert.Equal(125, position.Leverage);
        Assert.Equal(50m, position.MaxQuantity);
        Assert.Equal(BinanceFuturesMarginType.Cross, position.MarginType);
        Assert.Equal(0m, position.IsolatedMargin);
        Assert.False(position.IsAutoAddMargin);
        Assert.Equal(BinancePositionSide.Both, position.PositionSide);
        Assert.Equal(
            DateTimeOffset.FromUnixTimeMilliseconds(1_625_474_304_765).UtcDateTime,
            position.UpdateTime);
    }

    [Fact]
    public async Task CurrentReadContracts_RejectReceiveWindowAboveMaximumBeforeSending()
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using (var client = CreateClient(handler))
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                client.CoinFutures.GetAccountInfoAsync(60_001));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                client.CoinFutures.GetOpenOrdersAsync(receiveWindow: 60_001));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                client.CoinFutures.GetPositionsAsync(receiveWindow: 60_001));
        }
        Assert.Null(handler.RequestUri);

        var configuredHandler = new RecordingHttpMessageHandler("{}");
        using var configuredClient = CreateClient(
            configuredHandler,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.CoinFutures.GetAccountInfoAsync());
        Assert.Null(configuredHandler.RequestUri);
    }

    [Fact]
    public async Task DynamicWeightQueries_RejectBlankIdentifiersBeforeSending()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.CoinFutures.GetBracketsAsync(symbol: " "));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.CoinFutures.GetOpenOrdersAsync(symbol: " "));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.CoinFutures.GetOpenOrdersAsync(pair: " "));

        Assert.Null(handler.RequestUri);
    }

    private static string DecodedQuery(RecordingHttpMessageHandler handler)
        => Uri.UnescapeDataString(handler.RequestUri!.Query);

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
