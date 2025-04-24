using Binance.Api.Futures;
using Binance.Api.Spot;

namespace Binance.Net.Objects.Models.Futures;

/// <summary>
/// The result of query order
/// </summary>
public record BinanceFuturesOrder
{
    /// <summary>
    /// The symbol the order is for
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Pair
    /// </summary>
    [JsonProperty("pair")]
    public string? Pair { get; set; }

    /// <summary>
    /// The order id as assigned by Binance
    /// </summary>
    [JsonProperty("orderId")]
    public long Id { get; set; }

    /*
    /// <summary>
    /// The order id as assigned by the client
    /// </summary>
    [JsonProperty("clientOrderId")]
    [JsonConverterCtor(typeof(ReplaceConverter), 
        $"{BinanceExchange.ClientOrderIdPrefixSpot}->",
        $"{BinanceExchange.ClientOrderIdPrefixFutures}->")]
    public string ClientOrderId { get; set; } = string.Empty;

    /// <summary>
    /// Whether or not this order is a liquidation order
    /// </summary>
    [JsonIgnore]
    public bool IsLiquidationOrder => ClientOrderId?.StartsWith("autoclose-") == true;
    /// <summary>
    /// Whether or not this order is an ADL auto close order
    /// </summary>
    [JsonIgnore]
    public bool IsAdlAutoCloseOrder => ClientOrderId?.StartsWith("adl_autoclose-") == true;
    /// <summary>
    /// Whether or not this order is a delisting/delivery settlement order
    /// </summary>
    [JsonIgnore]
    public bool IsSettlementOrder => ClientOrderId?.StartsWith("delivery_autoclose-") == true;
    */

    /// <summary>
    /// The price of the order
    /// </summary>
    [JsonProperty("price")]
    public decimal Price { get; set; }
    /// <summary>
    /// The average price of the order
    /// </summary>
    [JsonProperty("avgPrice")]
    public decimal AveragePrice { get; set; }
    /// <summary>
    /// Quantity that has been filled
    /// </summary>
    [JsonProperty("executedQty")]
    public decimal QuantityFilled { get; set; }
    /// <summary>
    /// Cumulative quantity
    /// </summary>
    [JsonProperty("cumQty")]
    public decimal? CumulativeQuantity { get; set; }
    /// <summary>
    /// Cumulative quantity in quote asset ( for USD futures )
    /// </summary>
    [JsonProperty("cumQuote")]
    public decimal? QuoteQuantityFilled { get; set; }

    /// <summary>
    /// Cumulative quantity in quote asset ( for Coin futures )
    /// </summary>
    [JsonProperty("cumBase")]
    public decimal? BaseQuantityFilled { get; set; }
    /// <summary>
    /// The original quantity of the order
    /// </summary>
    [JsonProperty("origQty")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// Reduce Only
    /// </summary>
    [JsonProperty("reduceOnly")]
    public bool ReduceOnly { get; set; }

    /// <summary>
    /// If order is for closing a position
    /// </summary>
    [JsonProperty("closePosition")]
    public bool ClosePosition { get; set; }

    /// <summary>
    /// The side of the order
    /// </summary>
    [JsonProperty("side")]
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// The current status of the order
    /// </summary>
    [JsonProperty("status")]
    public BinanceOrderStatus Status { get; set; }

    /// <summary>
    /// Stop price for the order
    /// </summary>
    [JsonProperty("stopPrice")]
    public decimal? StopPrice { get; set; }

    /// <summary>
    /// For what time the order lasts
    /// </summary>
    [JsonProperty("timeInForce")]
    public BinanceTimeInForce TimeInForce { get; set; }

    /// <summary>
    /// The type of the order
    /// </summary>
    [JsonProperty("type")]
    public BinanceFuturesOrderType Type { get; set; }

    /// <summary>
    /// The type of the order
    /// </summary>
    [JsonProperty("origType")]
    public BinanceFuturesOrderType OriginalType { get; set; }

    /// <summary>
    /// Activation price, only return with TRAILING_STOP_MARKET order
    /// </summary>
    [JsonProperty("activatePrice")]
    public decimal? ActivatePrice { get; set; }

    /// <summary>
    /// Callback rate, only return with TRAILING_STOP_MARKET order
    /// </summary>
    [JsonProperty("priceRate")]
    public decimal? CallbackRate { get; set; }

    /// <summary>
    /// The time the order was updated
    /// </summary>
    [JsonProperty("updateTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// The time the order was created
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// The working type
    /// </summary>
    [JsonProperty("workingType")]
    public WorkingType WorkingType { get; set; }

    /// <summary>
    /// The position side of the order
    /// </summary>
    [JsonProperty("positionSide")]
    public BinancePositionSide PositionSide { get; set; }
    
    /// <summary>
    /// Price protect
    /// </summary>
    [JsonProperty("priceProtect")]
    public bool PriceProtect { get; set; }

    /// <summary>
    /// Price match type
    /// </summary>
    //[JsonProperty("priceMatch"), JsonConverter(typeof(EnumConverter))]
    //public PriceMatch PriceMatch { get; set; }

    /// <summary>
    /// Self trade prevention mode
    /// </summary>
    [JsonProperty("selfTradePreventionMode")]
    public BinanceSelfTradePreventionMode? SelfTradePreventionMode { get; set; }
}

/// <summary>
/// Usdt futures order
/// </summary>
public record BinanceUsdFuturesOrder : BinanceFuturesOrder
{

    /// <summary>
    /// Auto cancel at this date
    /// </summary>
    [JsonProperty("goodTillDate"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime? GoodTillDate { get; set; }
}
