using Binance.Api.Futures;
using Binance.Api.Shared;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesStreamAccountUpdateTests
{
    [Fact]
    public void UsdCrossFundingFee_MapsCurrentSymbolWithoutPosition()
    {
        const string payload = """
            {
              "e": "ACCOUNT_UPDATE",
              "E": 1564745798939,
              "T": 1564745798938,
              "a": {
                "m": "FUNDING_FEE",
                "S": "BTCUSDT",
                "B": [{
                  "a": "USDT",
                  "wb": "122624.12345678",
                  "cw": "100.12345678",
                  "bc": "-0.00010000"
                }]
              }
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesStreamAccountUpdate>(payload);

        Assert.NotNull(update);
        Assert.Equal("ACCOUNT_UPDATE", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_564_745_798_939).UtcDateTime, update.EventTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_564_745_798_938).UtcDateTime, update.TransactionTime);
        Assert.Null(update.AccountAlias);
        Assert.Equal(BinanceFuturesAccountUpdateReason.FundingFee, update.UpdateData.Reason);
        Assert.Equal("BTCUSDT", update.UpdateData.Symbol);
        var balance = Assert.Single(update.UpdateData.Balances);
        Assert.Equal("USDT", balance.Asset);
        Assert.Equal(122624.12345678m, balance.WalletBalance);
        Assert.Equal(100.12345678m, balance.CrossWalletBalance);
        Assert.Equal(-0.00010000m, balance.BalanceChange);
        Assert.Empty(update.UpdateData.Positions);
    }

    [Fact]
    public void CoinIsolatedFundingFee_MapsAliasSymbolAndCompletePosition()
    {
        const string payload = """
            {
              "e": "ACCOUNT_UPDATE",
              "E": 1591274595442,
              "T": 1591274595441,
              "i": "SfsR",
              "a": {
                "m": "FUNDING_FEE",
                "S": "BTCUSD_PERP",
                "B": [{
                  "a": "BTC",
                  "wb": "0.00200000",
                  "cw": "0.00100000",
                  "bc": "-0.00001000"
                }],
                "P": [{
                  "s": "BTCUSD_PERP",
                  "pa": "1",
                  "ep": "11707.70000003",
                  "bep": "11710.20000003",
                  "cr": "-0.00010000",
                  "up": "0.00020000",
                  "mt": "isolated",
                  "iw": "0.00100000",
                  "ps": "LONG"
                }]
              }
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesStreamAccountUpdate>(payload);

        Assert.NotNull(update);
        Assert.Equal("SfsR", update.AccountAlias);
        Assert.Equal(BinanceFuturesAccountUpdateReason.FundingFee, update.UpdateData.Reason);
        Assert.Equal("BTCUSD_PERP", update.UpdateData.Symbol);
        var position = Assert.Single(update.UpdateData.Positions);
        Assert.Equal("BTCUSD_PERP", position.Symbol);
        Assert.Equal(1m, position.Quantity);
        Assert.Equal(11707.70000003m, position.EntryPrice);
        Assert.Equal(11710.20000003m, position.BreakEvenPrice);
        Assert.Equal(-0.00010000m, position.RealizedPnl);
        Assert.Equal(0.00020000m, position.UnrealizedPnl);
        Assert.Equal(BinanceFuturesMarginType.Isolated, position.MarginType);
        Assert.Equal(0.00100000m, position.IsolatedMargin);
        Assert.Equal(BinancePositionSide.Long, position.PositionSide);
    }

    [Fact]
    public void NonFundingUpdate_LeavesConditionalSymbolAbsent()
    {
        const string payload = """
            {
              "e": "ACCOUNT_UPDATE",
              "E": 1564745798939,
              "T": 1564745798938,
              "a": { "m": "ORDER", "B": [], "P": [] }
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesStreamAccountUpdate>(payload);

        Assert.NotNull(update);
        Assert.Equal(BinanceFuturesAccountUpdateReason.Order, update.UpdateData.Reason);
        Assert.Null(update.UpdateData.Symbol);
    }
}
