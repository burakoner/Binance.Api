using ApiSharp.Converters;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesStreamSymbolUpdateTests
{
    [Fact]
    public void Topic_UsesCanonicalMergedAllMarketName()
    {
        Assert.Equal("!contractInfo", BinanceFuturesSocketClientCoin.ContractInfoStreamTopic);
        Assert.Equal("!contractInfo", BinanceFuturesSocketClientUsd.ContractInfoStreamTopic);
    }

    [Fact]
    public void CoinBracketUpdate_MapsCompleteCurrentOfficialSchema()
    {
        const string payload = """
            {
              "e": "contractInfo",
              "E": 1753344000001,
              "s": "BTCUSD_260925",
              "ps": "BTCUSD",
              "ct": "CURRENT_QUARTER",
              "dt": 1758758400000,
              "ot": 1747872000000,
              "cs": "TRADING",
              "bks": [{
                "bs": 2147483648,
                "bnf": 3000000000,
                "bnc": 9000000000,
                "mmr": 0.0065,
                "cf": 4000000000,
                "mi": 51,
                "ma": 75
              }],
              "st": 2
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesStreamSymbolUpdate>(payload);

        Assert.NotNull(update);
        Assert.Equal("contractInfo", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_001).UtcDateTime, update.EventTime);
        Assert.Equal("BTCUSD_260925", update.Symbol);
        Assert.Equal("BTCUSD", update.Pair);
        Assert.Equal(BinanceFuturesContractType.CurrentQuarter, update.ContractType);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_758_758_400_000).UtcDateTime, update.DeliveryDate);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_747_872_000_000).UtcDateTime, update.OnboardDate);
        Assert.Equal(BinanceSymbolStatus.Trading, update.Status);
        Assert.Equal(2, update.SymbolType);

        var bracket = Assert.Single(update.Brackets!);
        Assert.Equal(2_147_483_648L, bracket.NotionalBracket);
        Assert.Equal(3_000_000_000L, bracket.FloorNotional);
        Assert.Equal(9_000_000_000L, bracket.MaxNotional);
        Assert.Equal(0.0065m, bracket.MaintenanceRatio);
        Assert.Equal(4_000_000_000L, bracket.Auxiliary);
        Assert.Equal(51L, bracket.MinLeverage);
        Assert.Equal(75L, bracket.MaxLeverage);
    }

    [Fact]
    public void MergedUsdMarginedListingUpdate_AllowsAbsentPairAndBrackets()
    {
        const string payload = """
            {
              "e": "contractInfo",
              "E": 1753344000001,
              "s": "BTCUSDT",
              "ct": "PERPETUAL",
              "dt": 4133404800000,
              "ot": 1569398400000,
              "cs": "TRADING",
              "st": 1
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesStreamSymbolUpdate>(payload);

        Assert.NotNull(update);
        Assert.Equal("contractInfo", update.Event);
        Assert.Equal("BTCUSDT", update.Symbol);
        Assert.Empty(update.Pair);
        Assert.Equal(BinanceFuturesContractType.Perpetual, update.ContractType);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(4_133_404_800_000).UtcDateTime, update.DeliveryDate);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_569_398_400_000).UtcDateTime, update.OnboardDate);
        Assert.Equal(BinanceSymbolStatus.Trading, update.Status);
        Assert.Null(update.Brackets);
        Assert.Equal(1, update.SymbolType);
    }

    [Fact]
    public void ContractType_UsesCompleteCurrentUsdAndCoinMarginedWireValues()
    {
        var expected = new Dictionary<BinanceFuturesContractType, string>
        {
            [BinanceFuturesContractType.Perpetual] = "PERPETUAL",
            [BinanceFuturesContractType.CurrentMonth] = "CURRENT_MONTH",
            [BinanceFuturesContractType.NextMonth] = "NEXT_MONTH",
            [BinanceFuturesContractType.CurrentQuarter] = "CURRENT_QUARTER",
            [BinanceFuturesContractType.NextQuarter] = "NEXT_QUARTER",
            [BinanceFuturesContractType.PerpetualDelivering] = "PERPETUAL_DELIVERING",
            [BinanceFuturesContractType.CurrentQuarterDelivering] = "CURRENT_QUARTER_DELIVERING",
            [BinanceFuturesContractType.NextQuarterDelivering] = "NEXT_QUARTER_DELIVERING",
            [BinanceFuturesContractType.TradFiPerpetual] = "TRADIFI_PERPETUAL"
        };

        Assert.Equal(expected, Enum.GetValues<BinanceFuturesContractType>()
            .Where(value => value != BinanceFuturesContractType.Unknown)
            .ToDictionary(value => value, value => MapConverter.GetString(value)!));
    }

    [Fact]
    public void SymbolStatus_UsesCompleteCurrentUsdAndCoinMarginedWireValues()
    {
        var expected = new Dictionary<BinanceSymbolStatus, string>
        {
            [BinanceSymbolStatus.PendingTrading] = "PENDING_TRADING",
            [BinanceSymbolStatus.Trading] = "TRADING",
            [BinanceSymbolStatus.PreDelivering] = "PRE_DELIVERING",
            [BinanceSymbolStatus.Delivering] = "DELIVERING",
            [BinanceSymbolStatus.Delivered] = "DELIVERED",
            [BinanceSymbolStatus.PreSettle] = "PRE_SETTLE",
            [BinanceSymbolStatus.Settling] = "SETTLING",
            [BinanceSymbolStatus.Close] = "CLOSE",
            [BinanceSymbolStatus.TradingHalt] = "TRADING_HALT",
            [BinanceSymbolStatus.TradingCancelOnly] = "TRADING_CANCEL_ONLY"
        };

        Assert.Equal(expected, Enum.GetValues<BinanceSymbolStatus>()
            .ToDictionary(value => value, value => MapConverter.GetString(value)!));
    }
}
