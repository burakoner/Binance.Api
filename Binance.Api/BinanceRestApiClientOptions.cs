namespace Binance.Api;

public class BinanceRestApiClientOptions : RestApiClientOptions
{
    /// <summary>
    /// Receive Window
    /// </summary>
    public TimeSpan? ReceiveWindow { get; set; }

    /// <summary>
    /// Whether to allow the client to adjust the clientOrderId parameter send by the user when placing orders to include a client reference. This reference is used by the exchange to allocate a small percentage of the paid trading fees to developer of this library. Defaults to false.<br />
    /// Note that:<br />
    /// * It does not impact the amount of fees a user pays in any way<br />
    /// * It does not impact functionality. The reference is added just before sending the request and removed again during data deserialization<br />
    /// * It does respect client order id field limitations. For example if the user provided client order id parameter is too long to fit the reference it will not be added<br />
    /// * Toggling this option might fail operations using a clientOrderId parameter for pre-existing orders which were placed before the toggle. Operations on orders placed after the toggle will work as expected. It's advised to toggle when there are no open orders
    /// </summary>
    public bool AllowAppendingClientOrderId { get; set; } = true;

    /// <summary>
    /// Whether or not to automatically sync the local time with the server time
    /// </summary>
    public bool AutoTimestamp { get; set; } = true;

    /// <summary>
    /// How often the timestamp adjustment between client and server is recalculated. If you need a very small TimeSpan here you're probably better of syncing your server time more often
    /// </summary>
    public TimeSpan TimestampRecalculationInterval { get; set; } = TimeSpan.FromHours(1);

    // Platform Based Options
    public BinanceRestApiClientSpotOptions SpotOptions { get; set; } = new();
    public BinanceRestApiClientUsdtFuturesOptions UsdtFuturesOptions { get; set; } = new();
    public BinanceRestApiClientCoinFuturesOptions CoinFuturesOptions { get; set; } = new();
    public BinanceRestApiClientEuropeanOptions EuropeanOptions { get; set; } = new();
    public BinanceRestApiClientBrokerOptions BrokerOptions { get; set; } = new();

    public BinanceRestApiClientOptions() : this("", "")
    {
    }

    public BinanceRestApiClientOptions(string apikey, string secret) : this(new ApiCredentials(apikey, secret))
    {
    }

    public BinanceRestApiClientOptions(ApiCredentials credentials)
    {
        // API Credentials
        ApiCredentials = credentials;

        // Rate Limiters
        RateLimiters =
        [
            new RateLimiter()
                .AddPartialEndpointLimit("/api/", 1200, TimeSpan.FromMinutes(1))
                .AddPartialEndpointLimit("/sapi/", 12000, TimeSpan.FromMinutes(1))
                .AddEndpointLimit("/api/v3/order", 50, TimeSpan.FromSeconds(10), HttpMethod.Post, true)
        ];
    }
}


public class BinanceRestApiClientSpotOptions
{
    // Trade Rules
    public BinanceTradeRulesBehavior TradeRulesBehavior { get; set; } = BinanceTradeRulesBehavior.None;
    public TimeSpan TradeRulesUpdateInterval { get; set; } = TimeSpan.FromMinutes(60);
}

public class BinanceRestApiClientUsdtFuturesOptions
{
    // Trade Rules
    public BinanceTradeRulesBehavior TradeRulesBehavior { get; set; } = BinanceTradeRulesBehavior.None;
    public TimeSpan TradeRulesUpdateInterval { get; set; } = TimeSpan.FromMinutes(60);
}

public class BinanceRestApiClientCoinFuturesOptions
{
    // Trade Rules
    public BinanceTradeRulesBehavior TradeRulesBehavior { get; set; } = BinanceTradeRulesBehavior.None;
    public TimeSpan TradeRulesUpdateInterval { get; set; } = TimeSpan.FromMinutes(60);
}

public class BinanceRestApiClientEuropeanOptions
{
}

public class BinanceRestApiClientBrokerOptions
{
}
