using ApiSharp.WebSocket;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesStreamAlgoUpdateTests
{
    [Fact]
    public void AlgoUpdate_RoutesCompleteCurrentPayloadAndPlaceholder()
    {
        const string message = """
            {
              "stream": "listen-key",
              "data": {
                "e": "ALGO_UPDATE",
                "E": 1750515742303,
                "T": 1750515742297,
                "o": {
                  "caid": "client-algo-id",
                  "aid": 9223372036854775806,
                  "at": "CONDITIONAL",
                  "o": "TRAILING_STOP_MARKET",
                  "s": "BTCUSDT",
                  "S": "SELL",
                  "ps": "SHORT",
                  "f": "GTC",
                  "q": "0.00100000",
                  "X": "TRIGGERED",
                  "ai": "9223372036854775808",
                  "ap": "105000.12345678",
                  "aq": "0.00100000",
                  "act": "MARKET",
                  "tp": "104000.87654321",
                  "p": "0",
                  "V": "EXPIRE_TAKER",
                  "wt": "MARK_PRICE",
                  "pm": "NONE",
                  "cp": false,
                  "pP": true,
                  "R": false,
                  "tt": 1750515742296,
                  "gtd": 1750602142000,
                  "rm": "",
                  "ia": false
                }
              }
            }
            """;
        var envelope = JToken.Parse(message);
        var root = new BinanceSocketApiClient();
        var client = Assert.IsType<BinanceFuturesSocketClientUsd>(root.UsdFutures);
        BinanceFuturesStreamAlgoUpdate? update = null;

        client.DispatchAlgoUpdate(
            envelope,
            envelope["data"]!,
            new WebSocketDataEvent<string>(message, DateTime.UtcNow),
            data => update = data.Data);

        Assert.NotNull(update);
        Assert.Equal("ALGO_UPDATE", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_750_515_742_303).UtcDateTime, update.EventTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_750_515_742_297).UtcDateTime, update.TransactionTime);
        Assert.Equal("listen-key", update.ListenKey);
        Assert.Equal("client-algo-id", update.UpdateData.ClientAlgoId);
        Assert.Equal(9_223_372_036_854_775_806L, update.UpdateData.AlgoId);
        Assert.Equal("CONDITIONAL", update.UpdateData.AlgoType);
        Assert.Equal("TRAILING_STOP_MARKET", update.UpdateData.OrderType);
        Assert.Equal("BTCUSDT", update.UpdateData.Symbol);
        Assert.Equal("SELL", update.UpdateData.Side);
        Assert.Equal("SHORT", update.UpdateData.PositionSide);
        Assert.Equal("GTC", update.UpdateData.TimeInForce);
        Assert.Equal(0.00100000m, update.UpdateData.Quantity);
        Assert.Equal("TRIGGERED", update.UpdateData.Status);
        Assert.Equal("9223372036854775808", update.UpdateData.ActualOrderId);
        Assert.Equal(105000.12345678m, update.UpdateData.ActualPrice);
        Assert.Equal(0.00100000m, update.UpdateData.ActualQuantity);
        Assert.Equal("MARKET", update.UpdateData.ActualOrderType);
        Assert.Equal(104000.87654321m, update.UpdateData.TriggerPrice);
        Assert.Equal(0m, update.UpdateData.Price);
        Assert.Equal("EXPIRE_TAKER", update.UpdateData.SelfTradePreventionMode);
        Assert.Equal("MARK_PRICE", update.UpdateData.WorkingType);
        Assert.Equal("NONE", update.UpdateData.PriceMatch);
        Assert.False(update.UpdateData.ClosePosition);
        Assert.True(update.UpdateData.PriceProtect);
        Assert.False(update.UpdateData.ReduceOnly);
        Assert.Equal(1_750_515_742_296L, update.UpdateData.TriggerTime);
        Assert.Equal(1_750_602_142_000L, update.UpdateData.GoodTillDate);
        Assert.Equal(string.Empty, update.UpdateData.FailureReason);
        Assert.False(update.UpdateData.IsActivated);
    }

    [Fact]
    public void AlgoUpdate_PreservesOptionalActivationAbsence()
    {
        var update = JsonConvert.DeserializeObject<BinanceFuturesStreamAlgoUpdate>(
            """{"e":"ALGO_UPDATE","E":1750515742303,"T":1750515742297,"o":{"aid":1}}""");

        Assert.NotNull(update);
        Assert.Equal(1, update.UpdateData.AlgoId);
        Assert.Null(update.UpdateData.IsActivated);
        Assert.Null(update.UpdateData.ActualOrderId);
    }
}
