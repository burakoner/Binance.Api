namespace Binance.Api.Spot;

/// <summary>
/// Unit used for a pegged order's price offset.
/// </summary>
public enum BinanceSpotPegOffsetType : byte
{
    /// <summary>Offset expressed as an order-book price level.</summary>
    [Map("PRICE_LEVEL")]
    PriceLevel = 1
}
