namespace Binance.Api.Futures;

/// <summary>
/// Update data about a trade
/// </summary>
public record BinanceFuturesStreamTradeUpdate : BinanceFuturesStreamEvent
{
    /// <summary>
    /// Transaction time
    /// </summary>
    [JsonProperty("T")]
    public DateTime TransactionTime { get; set; }

    /// <summary>
    /// The symbol the order is for
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// The quantity of the order
    /// </summary>
    [JsonProperty("q")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// The price of the order
    /// </summary>
    [JsonProperty("p")]
    public decimal Price { get; set; }

    /// <summary>
    /// Whether the buyer is the maker
    /// </summary>
    [JsonProperty("m")]
    public bool BuyerIsMaker { get; set; }

    /// <summary>
    /// The new client order id
    /// </summary>
    [JsonProperty("c")]
    public string ClientOrderId { get; set; } = string.Empty;

    /// <summary>
    /// The side of the order
    /// </summary>
    [JsonProperty("S")]
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// The price of the last filled trade
    /// </summary>
    [JsonProperty("L")]
    public decimal PriceLastFilledTrade { get; set; }

    /// <summary>
    /// The quantity of the last filled trade of this order
    /// </summary>
    [JsonProperty("l")]
    public decimal QuantityOfLastFilledTrade { get; set; }

    /// <summary>
    /// The trade id
    /// </summary>
    [JsonProperty("t")]
    public long TradeId { get; set; }

    /// <summary>
    /// The id of the order as assigned by Binance
    /// </summary>
    [JsonProperty("i")]
    public long OrderId { get; set; }

    /// <summary>
    /// The listen key the update was for
    /// </summary>
    public string ListenKey { get; set; } = string.Empty;
}
