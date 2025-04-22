namespace Binance.Api.Shared;

/// <summary>
/// Order urgency
/// </summary>
public enum BinanceUrgency : byte
{
    /// <summary>
    /// Low urgency
    /// </summary>
    [Map("LOW")]
    Low = 1,

    /// <summary>
    /// Medium urgency
    /// </summary>
    [Map("MEDIUM")]
    Medium = 2,

    /// <summary>
    /// High urgency
    /// </summary>
    [Map("HIGH")]
    High = 3
}
