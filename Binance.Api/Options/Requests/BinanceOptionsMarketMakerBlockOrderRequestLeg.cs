namespace Binance.Api.Options;

/// <summary>
/// Parameters for the single leg of a new market maker block trade order
/// </summary>
public record BinanceOptionsMarketMakerBlockOrderRequestLeg
{
    /// <summary>
    /// Creates a new market maker block trade order leg
    /// </summary>
    /// <param name="symbol">Option symbol</param>
    /// <param name="side">Buy or sell side</param>
    /// <param name="type">Order type; only LIMIT is supported</param>
    /// <param name="quantity">Order quantity</param>
    public BinanceOptionsMarketMakerBlockOrderRequestLeg(
        string symbol,
        BinanceOrderSide side,
        BinanceOptionsOrderType type,
        decimal quantity)
    {
        Symbol = symbol;
        Side = side;
        Type = type;
        Quantity = quantity;
    }

    /// <summary>
    /// Option symbol
    /// </summary>
    public string Symbol { get; set; }

    /// <summary>
    /// Buy or sell side
    /// </summary>
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// Order type; only LIMIT is supported
    /// </summary>
    public BinanceOptionsOrderType Type { get; set; }

    /// <summary>
    /// Order quantity
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Optional order price
    /// </summary>
    public decimal? Price { get; set; }
}
