namespace Binance.Api;

public class BinanceRestApiClientOptions : RestApiClientOptions
{
    // Receive Window
    public TimeSpan ReceiveWindow { get; set; }

    // Auto Timestamp
    public bool AutoTimestamp { get; set; }
    public TimeSpan TimestampRecalculationInterval { get; set; }

    // Platform Based Options
    public BinanceRestApiSpotClientOptions SpotOptions { get; set; }
    public BinanceRestApiUsdtFuturesClientOptions UsdtFuturesOptions { get; set; }
    public BinanceRestApiCoinFuturesClientOptions CoinFuturesOptions { get; set; }
    public BinanceRestApiEuropeanClientOptions EuropeanOptions { get; set; }
    public BinanceRestApiBrokerClientOptions BrokerOptions { get; set; }

    public BinanceRestApiClientOptions()
    {
        // Api Addresses
        BaseAddress = "https://api.binance.com";

        // Rate Limiters
        RateLimiters = new List<IRateLimiter>
        {
            new RateLimiter()
                .AddPartialEndpointLimit("/api/", 1200, TimeSpan.FromMinutes(1))
                .AddPartialEndpointLimit("/sapi/", 12000, TimeSpan.FromMinutes(1))
                .AddEndpointLimit("/api/v3/order", 50, TimeSpan.FromSeconds(10), HttpMethod.Post, true)
        };

        // Receive Window
        ReceiveWindow = TimeSpan.FromSeconds(5);

        // Auto Timestamp
        AutoTimestamp = true;
        TimestampRecalculationInterval = TimeSpan.FromHours(1);

        // Platform Based Options
        SpotOptions = new();
        UsdtFuturesOptions = new();
        CoinFuturesOptions = new();
        EuropeanOptions = new();
        BrokerOptions = new();
    }
}

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

public class BinanceRestApiUsdtFuturesClientOptions
{
    // Base Address
    public string BaseAddress { get; set; }

    // Trade Rules
    public TradeRulesBehavior TradeRulesBehavior { get; set; }
    public TimeSpan TradeRulesUpdateInterval { get; set; }

    public BinanceRestApiUsdtFuturesClientOptions()
    {
        // Base Address
        BaseAddress = "https://fapi.binance.com";

        // Trade Rules Behaviour
        TradeRulesBehavior = TradeRulesBehavior.None;
        TradeRulesUpdateInterval = TimeSpan.FromMinutes(60);
    }
}

public class BinanceRestApiCoinFuturesClientOptions
{
    // Base Address
    public string BaseAddress { get; set; }

    // Trade Rules
    public TradeRulesBehavior TradeRulesBehavior { get; set; }
    public TimeSpan TradeRulesUpdateInterval { get; set; }

    public BinanceRestApiCoinFuturesClientOptions()
    {
        // Base Address
        BaseAddress = "https://dapi.binance.com";

        // Trade Rules Behaviour
        TradeRulesBehavior = TradeRulesBehavior.None;
        TradeRulesUpdateInterval = TimeSpan.FromMinutes(60);
    }
}

public class BinanceRestApiEuropeanClientOptions
{
    // Base Address
    public string BaseAddress { get; set; }

    public BinanceRestApiEuropeanClientOptions()
    {
        // Base Address
        BaseAddress = "https://eapi.binance.com";
    }
}

public class BinanceRestApiBrokerClientOptions
{
    // Base Address
    public string BaseAddress { get; set; }

    public BinanceRestApiBrokerClientOptions()
    {
        // Base Address
        BaseAddress = "https://api.binance.com";
    }
}
