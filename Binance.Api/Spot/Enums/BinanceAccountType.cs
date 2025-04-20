namespace Binance.Api.Spot.Enums;

/// <summary>
/// Type of account
/// </summary>
public enum BinanceAccountType : byte
{
    /// <summary>
    /// Spot account type
    /// </summary>
    [Map("SPOT")]
    Spot = 1,

    /// <summary>
    /// Margin account type
    /// </summary>>
    [Map("MARGIN")]
    Margin,

    /// <summary>
    /// Futures account type
    /// </summary>
    [Map("FUTURES")]
    Futures,

    /// <summary>
    /// Leveraged account type
    /// </summary>
    [Map("LEVERAGED")]
    Leveraged
}
