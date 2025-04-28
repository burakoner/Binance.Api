namespace Binance.Api.Margin;

/// <summary>
/// Status of a margin action
/// </summary>
public enum BinanceMarginStatus : byte
{
    /// <summary>
    /// Pending to execution
    /// </summary>
    [Map("PENDING")]
    Pending = 1,

    /// <summary>
    /// Executed, waiting to be confirmed
    /// </summary>
    [Map("COMPLETED")]
    Completed,

    /// <summary>
    /// Successfully loaned/repaid
    /// </summary>
    [Map("CONFIRMED")]
    Confirmed,

    /// <summary>
    /// execution failed, nothing happened to your account
    /// </summary>
    [Map("FAILED")]
    Failed
}
