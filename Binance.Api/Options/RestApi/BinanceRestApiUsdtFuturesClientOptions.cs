namespace Binance.Api.Options.RestApi;

public class BinanceRestApiUsdtFuturesClientOptions
{
    // Base Address
    public string BaseAddress { get; set; }

    // Trade Rules
    public BinanceTradeRulesBehavior TradeRulesBehavior { get; set; }
    public TimeSpan TradeRulesUpdateInterval { get; set; }

    public BinanceRestApiUsdtFuturesClientOptions()
    {
        // Base Address
        BaseAddress = "https://fapi.binance.com";

        // Trade Rules Behaviour
        TradeRulesBehavior = BinanceTradeRulesBehavior.None;
        TradeRulesUpdateInterval = TimeSpan.FromMinutes(60);
    }
}