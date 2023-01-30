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

    public BinanceRestApiClientOptions() : this(new ApiCredentials("", ""))
    {
    }

    public BinanceRestApiClientOptions(string apikey, string secret) : this(new ApiCredentials(apikey, secret))
    {
    }

    public BinanceRestApiClientOptions(ApiCredentials credentials)
    {
        // API Credentials
        ApiCredentials = credentials;

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
