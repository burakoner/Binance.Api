
using Binance.Api.Spot;

namespace Binance.Api.Interfaces;

/// <summary>
/// Forced liquidation info
/// </summary>
public interface IBinanceFuturesLiquidation
{
    /// <summary>
    /// Symbol
    /// </summary>
    string Symbol { get; set; }
    /// <summary>
    /// Price
    /// </summary>
    decimal Price { get; set; }
    /// <summary>
    /// Total quantity
    /// </summary>
    decimal LastQuantityFilled { get; set; }
    /// <summary>
    /// The executed quantity
    /// </summary>
    decimal QuantityFilled { get; set; }
    /// <summary>
    /// Average price
    /// </summary>
    decimal AveragePrice { get; set; }
    /// <summary>
    /// Order status
    /// </summary>
    BinanceOrderStatus Status { get; set; }
    /// <summary>
    /// Time in force
    /// </summary>
    BinanceTimeInForce TimeInForce { get; set; }
    /// <summary>
    /// Side
    /// </summary>
    BinanceOrderSide Side { get; set; }
    /// <summary>
    /// Side
    /// </summary>
    FuturesOrderType Type { get; set; }
    /// <summary>
    /// Forced time
    /// </summary>
    DateTime Timestamp { get; set; }
}
