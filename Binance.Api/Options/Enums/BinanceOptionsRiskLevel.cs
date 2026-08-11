namespace Binance.Api.Options;

/// <summary>
/// Options account risk level
/// </summary>
public enum BinanceOptionsRiskLevel : byte
{
    /// <summary>
    /// Normal account operation
    /// </summary>
    [Map("NORMAL")]
    Normal = 1,

    /// <summary>
    /// Account is restricted to reducing positions
    /// </summary>
    [Map("REDUCE_ONLY")]
    ReduceOnly = 2
}
