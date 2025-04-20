namespace Binance.Api.Options.RestApi;

public class BinanceRestApiCoinFuturesClientOptions
{
    // Base Address
    public string BaseAddress { get; set; }

    // Trade Rules
    public BinanceTradeRulesBehavior TradeRulesBehavior { get; set; }
    public TimeSpan TradeRulesUpdateInterval { get; set; }

    public BinanceRestApiCoinFuturesClientOptions()
    {
        // Base Address
        BaseAddress = "https://dapi.binance.com";

        // Trade Rules Behaviour
        TradeRulesBehavior = BinanceTradeRulesBehavior.None;
        TradeRulesUpdateInterval = TimeSpan.FromMinutes(60);
    }
}