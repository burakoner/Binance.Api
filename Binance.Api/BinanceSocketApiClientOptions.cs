namespace Binance.Api;

public class BinanceSocketApiClientOptions : WebSocketApiClientOptions
{
    /// <summary>
    /// Whether to allow the client to adjust the clientOrderId parameter send by the user when placing orders to include a client reference. This reference is used by the exchange to allocate a small percentage of the paid trading fees to developer of this library. Defaults to false.<br />
    /// Note that:<br />
    /// * It does not impact the amount of fees a user pays in any way<br />
    /// * It does not impact functionality. The reference is added just before sending the request and removed again during data deserialization<br />
    /// * It does respect client order id field limitations. For example if the user provided client order id parameter is too long to fit the reference it will not be added<br />
    /// * Toggling this option might fail operations using a clientOrderId parameter for pre-existing orders which were placed before the toggle. Operations on orders placed after the toggle will work as expected. It's advised to toggle when there are no open orders
    /// </summary>
    public bool AllowAppendingClientOrderId { get; set; } = true;

    // Platform Based Options
    public BinanceSocketApiClientSpotOptions SpotOptions { get; set; } = new();
    public BinanceSocketApiClientUsdtFuturesOptions UsdtFuturesOptions { get; set; } = new();
    public BinanceSocketApiClientCoinFuturesOptions CoinFuturesOptions { get; set; } = new();
    public BinanceSocketApiClientEuropeanOptions EuropeanOptions { get; set; } = new();
    public BinanceSocketApiClientBrokerOptions BrokerOptions { get; set; } = new();
}

public class BinanceSocketApiClientSpotOptions
{
    // Trade Rules
    public BinanceTradeRulesBehavior TradeRulesBehavior { get; set; } = BinanceTradeRulesBehavior.None;
    public TimeSpan TradeRulesUpdateInterval { get; set; } = TimeSpan.FromMinutes(60);
}

public class BinanceSocketApiClientUsdtFuturesOptions
{
    // Trade Rules
    public BinanceTradeRulesBehavior TradeRulesBehavior { get; set; } = BinanceTradeRulesBehavior.None;
    public TimeSpan TradeRulesUpdateInterval { get; set; } = TimeSpan.FromMinutes(60);
}

public class BinanceSocketApiClientCoinFuturesOptions
{
    // Trade Rules
    public BinanceTradeRulesBehavior TradeRulesBehavior { get; set; } = BinanceTradeRulesBehavior.None;
    public TimeSpan TradeRulesUpdateInterval { get; set; } = TimeSpan.FromMinutes(60);
}

public class BinanceSocketApiClientEuropeanOptions
{
}

public class BinanceSocketApiClientBrokerOptions
{
}
