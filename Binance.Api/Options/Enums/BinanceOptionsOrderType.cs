namespace Binance.Api.Options;

/// <summary>
/// Options Order Type
/// </summary>
public enum BinanceOptionsOrderType : byte
{
    /// <summary>
    /// Limit
    /// </summary>
    [Map("LIMIT")]
    Limit = 1,
}
