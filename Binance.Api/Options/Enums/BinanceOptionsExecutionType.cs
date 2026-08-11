namespace Binance.Api.Options;

/// <summary>
/// Options order execution type
/// </summary>
public enum BinanceOptionsExecutionType : byte
{
    /// <summary>
    /// New order
    /// </summary>
    [Map("NEW")]
    New = 1,

    /// <summary>
    /// Canceled order
    /// </summary>
    [Map("CANCELED")]
    Canceled = 2,

    /// <summary>
    /// Expired order
    /// </summary>
    [Map("EXPIRED")]
    Expired = 3,

    /// <summary>
    /// Trade execution
    /// </summary>
    [Map("TRADE")]
    Trade = 4
}
