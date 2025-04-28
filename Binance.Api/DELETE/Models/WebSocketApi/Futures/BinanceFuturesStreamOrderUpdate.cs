using Binance.Api.Futures;

namespace Binance.Api.Models.WebSocketApi.Futures;

/// <summary>
/// Order update
/// </summary>
public record BinanceFuturesStreamOrderUpdate : BinanceSocketEvent
{
    /// <summary>
    /// Update data
    /// </summary>
    [JsonProperty("o")]
    public BinanceFuturesStreamOrderUpdateData UpdateData { get; set; } = new BinanceFuturesStreamOrderUpdateData();

    /// <summary>
    /// Transaction time
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime TransactionTime { get; set; }
    /// <summary>
    /// The listen key the update was for
    /// </summary>
    public string ListenKey { get; set; } = "";
}

/// <summary>
/// Update data about an order
/// </summary>
public record BinanceFuturesStreamOrderUpdateData
{
    /// <summary>
    /// The symbol the order is for
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";
    /// <summary>
    /// The new client order id
    /// </summary>
    [JsonProperty("c")]
    public string ClientOrderId { get; set; } = "";
    /// <summary>
    /// The side of the order
    /// </summary>
    [JsonProperty("S"), JsonConverter(typeof(MapConverter))]
    public BinanceOrderSide Side { get; set; }
    /// <summary>
    /// The type of the order
    /// </summary>
    [JsonProperty("o"), JsonConverter(typeof(MapConverter))]
    public BinanceFuturesOrderType Type { get; set; }
    /// <summary>
    /// The timespan the order is active
    /// </summary>
    [JsonProperty("f"), JsonConverter(typeof(MapConverter))]
    public BinanceTimeInForce TimeInForce { get; set; }
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
    /// The average price of the order
    /// </summary>
    [JsonProperty("ap")]
    public decimal AveragePrice { get; set; }
    /// <summary>
    /// The stop price of the order
    /// </summary>
    [JsonProperty("sp")]
    public decimal StopPrice { get; set; }
    /// <summary>
    /// The execution type
    /// </summary>
    [JsonProperty("x"), JsonConverter(typeof(MapConverter))]
    public BinanceFuturesExecutionType ExecutionType { get; set; }
    /// <summary>
    /// The status of the order
    /// </summary>
    [JsonProperty("X"), JsonConverter(typeof(MapConverter))]
    public BinanceOrderStatus Status { get; set; }
    /// <summary>
    /// The id of the order as assigned by Binance
    /// </summary>
    [JsonProperty("i")]
    public long OrderId { get; set; }
    /// <summary>
    /// The quantity of the last filled trade of this order
    /// </summary>
    [JsonProperty("l")]
    public decimal QuantityOfLastFilledTrade { get; set; }
    /// <summary>
    /// The quantity of all trades that were filled for this order
    /// </summary>
    [JsonProperty("z")]
    public decimal AccumulatedQuantityOfFilledTrades { get; set; }
    /// <summary>
    /// The price of the last filled trade
    /// </summary>
    [JsonProperty("L")]
    public decimal PriceLastFilledTrade { get; set; }
    /// <summary>
    /// The fee payed
    /// </summary>
    [JsonProperty("n")]
    public decimal Fee { get; set; }
    /// <summary>
    /// The asset the fee was taken from
    /// </summary>
    [JsonProperty("N")]
    public string FeeAsset { get; set; } = "";
    /// <summary>
    /// The time of the update
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateTime { get; set; }
    /// <summary>
    /// The trade id
    /// </summary>
    [JsonProperty("t")]
    public long TradeId { get; set; }
    /// <summary>
    /// Bid Notional
    /// </summary>
    [JsonProperty("b")]
    public decimal BidNotional { get; set; }
    /// <summary>
    /// Ask Notional
    /// </summary>
    [JsonProperty("a")]
    public decimal AskNotional { get; set; }
    /// <summary>
    /// Whether the buyer is the maker
    /// </summary>
    [JsonProperty("m")]
    public bool BuyerIsMaker { get; set; }
    /// <summary>
    /// Is this reduce only
    /// </summary>
    [JsonProperty("R")]
    public bool IsReduce { get; set; }
    /// <summary>
    /// Stop price working type
    /// </summary>
    [JsonProperty("wt"), JsonConverter(typeof(MapConverter))]
    public BinanceFuturesWorkingType StopPriceWorking { get; set; }
    /// <summary>
    /// Original Order Type
    /// </summary>
    [JsonProperty("ot"), JsonConverter(typeof(MapConverter))]
    public BinanceFuturesOrderType OriginalType { get; set; }
    /// <summary>
    /// Position side
    /// </summary>
    [JsonProperty("ps"), JsonConverter(typeof(MapConverter))]
    public BinancePositionSide PositionSide { get; set; }
    /// <summary>
    /// If Close-All, only pushed with conditional order
    /// </summary>
    [JsonProperty("cp")]
    public bool PushedConditionalOrder { get; set; }
    /// <summary>
    /// Activation Price, only pushed with TRAILING_STOP_MARKET order
    /// </summary>
    [JsonProperty("AP")]
    public decimal ActivationPrice { get; set; }
    /// <summary>
    /// Callback Rate, only pushed with TRAILING_STOP_MARKET order
    /// </summary>
    [JsonProperty("cr")]
    public decimal CallbackRate { get; set; }
    /// <summary>
    /// Realized profit of the trade
    /// </summary>
    [JsonProperty("rp")]
    public decimal RealizedProfit { get; set; }
}
