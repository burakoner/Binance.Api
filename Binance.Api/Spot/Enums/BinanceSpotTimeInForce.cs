namespace Binance.Api.Spot;

/// <summary>
/// The time the order will be active for
/// </summary>
public enum BinanceSpotTimeInForce : byte
{
    /// <summary>
    /// GoodTillCanceled orders will stay active until they are filled or canceled
    /// </summary>
    [Map("GTC")]
    GoodTillCanceled = 1,

    /// <summary>
    /// ImmediateOrCancel orders have to be at least partially filled upon placing or will be automatically canceled
    /// </summary>
    [Map("IOC")]
    ImmediateOrCancel = 2,

    /// <summary>
    /// FillOrKill orders have to be entirely filled upon placing or will be automatically canceled
    /// </summary>
    [Map("FOK")]
    FillOrKill = 3,

    /// <summary>
    /// GoodTillCrossing orders will post only
    /// </summary>
    [Map("GTX")]
    GoodTillCrossing = 4,

    /// <summary>
    /// Good til the order expires or is canceled
    /// </summary>
    [Map("GTE_GTC")]
    GoodTillExpiredOrCanceled = 5,

    /// <summary>
    /// Good til date
    /// </summary>
    [Map("GTD")]
    GoodTillDate = 6
}
