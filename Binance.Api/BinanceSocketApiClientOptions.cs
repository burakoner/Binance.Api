namespace Binance.Api;

/// <summary>
/// Binance WebSocket API Client Options
/// </summary>
public class BinanceSocketApiClientOptions : WebSocketApiClientOptions
{
    /// <summary>
    /// Receive Window
    /// </summary>
    public TimeSpan? ReceiveWindow { get; set; }

    /// <summary>
    /// Whether or not to automatically sync the local time with the server time
    /// </summary>
    public bool AutoTimestamp { get; set; } = true;

    /// <summary>
    /// How often the timestamp adjustment between client and server is recalculated. If you need a very small TimeSpan here you're probably better of syncing your server time more often
    /// </summary>
    public TimeSpan TimestampRecalculationInterval { get; set; } = TimeSpan.FromHours(1);

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
    /// Binance Spot WebSocket API Options
    /// </summary>
    public BinanceSocketApiClientSpotOptions SpotOptions { get; set; } = new();

    /// <summary>
    /// Binance USDⓈ-M Futures WebSocket API Options
    /// </summary>
    public BinanceSocketApiClientUsdtFuturesOptions UsdtFuturesOptions { get; set; } = new();

    /// <summary>
    /// Binance Coin-M Futures WebSocket API Options
    /// </summary>
    public BinanceSocketApiClientCoinFuturesOptions CoinFuturesOptions { get; set; } = new();

    /// <summary>
    /// Binance European Options WebSocket API Options
    /// </summary>
    public BinanceSocketApiClientEuropeanOptions EuropeanOptions { get; set; } = new();

    /// <summary>
    /// Binance Broker WebSocket API Options
    /// </summary>
    public BinanceSocketApiClientBrokerOptions BrokerOptions { get; set; } = new();
}

/// <summary>
/// Binance Spot WebSocket API Options
/// </summary>
public class BinanceSocketApiClientSpotOptions
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
/// Binance USDⓈ-M Futures WebSocket API Options
/// </summary>
public class BinanceSocketApiClientUsdtFuturesOptions
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
/// Binance Coin-M Futures WebSocket API Options
/// </summary>
public class BinanceSocketApiClientCoinFuturesOptions
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
/// Binance European Options WebSocket API Options
/// </summary>
public class BinanceSocketApiClientEuropeanOptions
{
}

/// <summary>
/// Binance Broker WebSocket API Options
/// </summary>
public class BinanceSocketApiClientBrokerOptions
{
}
