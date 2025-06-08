namespace Binance.Api;

/// <summary>
/// Binance Rest API Client Options
/// </summary>
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
    public bool AllowAppendingClientOrderId { get; set; } = false;

    /// <summary>
    /// Whether or not to automatically sync the local time with the server time
    /// </summary>
    public bool AutoTimestamp { get; set; } = true;

    /// <summary>
    /// How often the timestamp adjustment between client and server is recalculated. If you need a very small TimeSpan here you're probably better of syncing your server time more often
    /// </summary>
    public TimeSpan TimestampRecalculationInterval { get; set; } = TimeSpan.FromHours(1);

    /// <summary>
    /// Binance Spot Rest API Options
    /// </summary>
    public BinanceRestApiClientSpotOptions SpotOptions { get; set; } = new();

    /// <summary>
    /// Binance USDⓈ-M Futures Rest API Options
    /// </summary>
    public BinanceRestApiClientUsdtFuturesOptions UsdtFuturesOptions { get; set; } = new();

    /// <summary>
    /// Binance Coin-M Futures Rest API Options
    /// </summary>
    public BinanceRestApiClientCoinFuturesOptions CoinFuturesOptions { get; set; } = new();

    /// <summary>
    /// Binance European Options Rest API Options
    /// </summary>
    public BinanceRestApiClientEuropeanOptions EuropeanOptions { get; set; } = new();

    /// <summary>
    /// Binance Broker Rest API Options
    /// </summary>
    public BinanceRestApiClientBrokerOptions BrokerOptions { get; set; } = new();

    /// <summary>
    /// Constructor
    /// </summary>
    public BinanceRestApiClientOptions() : this("", "")
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="apikey">API Key</param>
    /// <param name="secret">API Secret</param>
    public BinanceRestApiClientOptions(string apikey, string secret) : this(new ApiCredentials(apikey, secret))
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="credentials">API Credentials</param>
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

/// <summary>
/// Binance Rest API Client Spot Options
/// </summary>
public class BinanceRestApiClientSpotOptions
{
    /// <summary>
    /// Trade Rules Behavior
    /// </summary>
    public BinanceTradeRulesBehavior TradeRulesBehavior { get; set; } = BinanceTradeRulesBehavior.None;

    /// <summary>
    /// Trade Rules Update Interval
    /// </summary>
    public TimeSpan TradeRulesUpdateInterval { get; set; } = TimeSpan.FromMinutes(60);
}

/// <summary>
/// Binance Rest API Client USDⓈ-M Futures Options
/// </summary>
public class BinanceRestApiClientUsdtFuturesOptions
{
    /// <summary>
    /// Trade Rules Behavior
    /// </summary>
    public BinanceTradeRulesBehavior TradeRulesBehavior { get; set; } = BinanceTradeRulesBehavior.None;

    /// <summary>
    /// Trade Rules Update Interval
    /// </summary>
    public TimeSpan TradeRulesUpdateInterval { get; set; } = TimeSpan.FromMinutes(60);
}

/// <summary>
/// Binance Rest API Client Coin-M Futures Options
/// </summary>
public class BinanceRestApiClientCoinFuturesOptions
{
    /// <summary>
    /// Trade Rules Behavior
    /// </summary>
    public BinanceTradeRulesBehavior TradeRulesBehavior { get; set; } = BinanceTradeRulesBehavior.None;

    /// <summary>
    /// Trade Rules Update Interval
    /// </summary>
    public TimeSpan TradeRulesUpdateInterval { get; set; } = TimeSpan.FromMinutes(60);
}

/// <summary>
/// Binance Rest API Client European Options Options
/// </summary>
public class BinanceRestApiClientEuropeanOptions
{
}

/// <summary>
/// Binance Rest API Client Broker Options
/// </summary>
public class BinanceRestApiClientBrokerOptions
{
}
