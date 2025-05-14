namespace Binance.Api.SubAccount;

/// <summary>
/// Sub account margin transfer type
/// </summary>
public enum BinanceSubAccountMarginTransferType : byte
{
    /// <summary>
    /// Sub account spot to sub account margin
    /// </summary>
    [Map("1")]
    FromSubAccountSpotToSubAccountMargin = 1,

    /// <summary>
    /// From sub account margin to sub account spot
    /// </summary>
    [Map("2")]
    FromSubAccountMarginToSubAccountSpot = 2
}
