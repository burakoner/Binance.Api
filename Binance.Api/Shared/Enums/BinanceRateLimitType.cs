namespace Binance.Api.Shared;

/// <summary>
/// Type of rate limit
/// </summary>
public enum BinanceRateLimitType : byte
{
    /// <summary>
    /// Request weight
    /// </summary>
    [Map("REQUEST_WEIGHT")]
    RequestWeight = 1,

    /// <summary>
    /// Order amount
    /// </summary>
    [Map("ORDERS")]
    Orders = 2,

    /// <summary>
    /// Raw requests
    /// </summary>
    [Map("RAW_REQUESTS")]
    RawRequests = 3,

    /// <summary>
    /// Connections
    /// </summary>
    [Map("CONNECTIONS")]
    Connections = 4
}
