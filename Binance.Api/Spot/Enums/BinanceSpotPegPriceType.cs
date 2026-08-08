namespace Binance.Api.Spot;

/// <summary>
/// Reference side used to determine a pegged order's price.
/// </summary>
public enum BinanceSpotPegPriceType : byte
{
    /// <summary>Peg to the best price on the same side of the order book.</summary>
    [Map("PRIMARY_PEG")]
    Primary = 1,

    /// <summary>Peg to the best price on the opposite side of the order book.</summary>
    [Map("MARKET_PEG")]
    Market
}
