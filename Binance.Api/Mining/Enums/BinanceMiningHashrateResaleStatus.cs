namespace Binance.Api.Mining;

/// <summary>
/// Resale status
/// </summary>
[JsonConverter(typeof(MapConverter))]
public enum BinanceMiningHashrateResaleStatus : byte
{
    /// <summary>
    /// Processing
    /// </summary>
    [Map("0")]
    Processing = 0,

    /// <summary>
    /// Canceled
    /// </summary>
    [Map("1")]
    Canceled = 1,

    /// <summary>
    /// Terminated
    /// </summary>
    [Map("2")]
    Terminated = 2
}
