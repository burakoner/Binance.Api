namespace Binance.Api.Spot.Enums
{
    /// <summary>
    /// Rate limit on what unit
    /// </summary>
    public enum BinanceRateLimitInterval
    {
        /// <summary>
        /// Seconds
        /// </summary>
        [Map("SECOND")]
        Second,
        /// <summary>
        /// Minutes
        /// </summary>
        [Map("MINUTE")]
        Minute,
        /// <summary>
        /// Days
        /// </summary>
        [Map("DAY")]
        Day
    }
}
