namespace Binance.Api.Models.RestApi.Futures;

/// <summary>
/// Result of setting a countdown timer
/// </summary>
public record BinanceFuturesCountDownResult
{
    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; } = "";
    /// <summary>
    /// Count down time in milliseconds
    /// </summary>
    public int CountDownTime { get; set; }
}
