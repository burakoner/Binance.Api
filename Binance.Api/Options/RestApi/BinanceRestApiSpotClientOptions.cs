namespace Binance.Api.Options.RestApi;

public class BinanceRestApiSpotClientOptions
{
    // Base Address
    public string BaseAddress { get; set; }

    // Trade Rules
    public TradeRulesBehavior TradeRulesBehavior { get; set; }
    public TimeSpan TradeRulesUpdateInterval { get; set; }

    public BinanceRestApiSpotClientOptions()
    {
        // Base Address
        BaseAddress = "https://api.binance.com";

        // Trade Rules Behaviour
        TradeRulesBehavior = TradeRulesBehavior.None;
        TradeRulesUpdateInterval = TimeSpan.FromMinutes(60);
    }
}