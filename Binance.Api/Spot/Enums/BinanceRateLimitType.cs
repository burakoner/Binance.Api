namespace Binance.Api.Spot.Enums
{
    /// <summary>
    /// Type of rate limit
    /// </summary>
    public enum BinanceRateLimitType
    {
        /// <summary>
        /// Request weight
        /// </summary>
        [Map("REQUEST_WEIGHT")]
        RequestWeight,
        /// <summary>
        /// Order amount
        /// </summary>
        [Map("ORDERS")]
        Orders,
        /// <summary>
        /// Raw requests
        /// </summary>
        [Map("RAW_REQUESTS")]
        RawRequests,
        /// <summary>
        /// Connections
        /// </summary>
        [Map("CONNECTIONS")]
        Connections
    }
}
