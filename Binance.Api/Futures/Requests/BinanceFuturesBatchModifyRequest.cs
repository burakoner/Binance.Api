namespace Binance.Api.Futures;

/// <summary>
/// Parameters for editing an order
/// </summary>
public record BinanceFuturesBatchModifyRequest
{
    /// <summary>
    /// Id of the order to edit. This or ClientOrderId should be provided
    /// </summary>
    public long? OrderId { get; set; }

    /// <summary>
    /// Original client id of the order to edit. This or OrderId must be provided; OrderId takes precedence when both are sent
    /// </summary>
    public string? OriginalClientOrderId { get; set; }

    /// <summary>
    /// Symbol of the order
    /// </summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Side of the order
    /// </summary>
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// Complete new order quantity
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// New order price
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Published for USD-M, but currently unusable because price is required and cannot be combined with priceMatch. COIN-M does not publish it for batch modification. Non-null values are rejected
    /// </summary>
    public BinanceFuturesPriceMatch? PriceMatch { get; set; }

    /// <summary>
    /// Optional user-defined modification identifier passed through without uniqueness validation and returned only when supplied
    /// </summary>
    public long? ModifyId { get; set; }
}
