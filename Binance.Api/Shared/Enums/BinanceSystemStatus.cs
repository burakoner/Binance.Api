namespace Binance.Api.Shared;

/// <summary>
/// Status of the Binance system
/// </summary>
public enum BinanceSystemStatus : byte
{
    /// <summary>
    /// Operational
    /// </summary>
    [Map("0")]
    Normal = 0,

    /// <summary>
    /// In maintenance
    /// </summary>
    [Map("1")]
    Maintenance = 1
}
