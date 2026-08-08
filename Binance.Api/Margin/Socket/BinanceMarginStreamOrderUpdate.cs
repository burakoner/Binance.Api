using Binance.Api.Spot;

namespace Binance.Api.Margin;

/// <summary>
/// Margin order execution update.
/// </summary>
public record BinanceMarginStreamOrderUpdate : BinanceMarginStreamUpdate
{
    /// <summary>Symbol.</summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>Client order identifier.</summary>
    [JsonProperty("c")]
    public string ClientOrderId { get; set; } = string.Empty;

    /// <summary>Order side.</summary>
    [JsonProperty("S")]
    public BinanceOrderSide Side { get; set; }

    /// <summary>Order type.</summary>
    [JsonProperty("o")]
    public BinanceSpotOrderType Type { get; set; }

    /// <summary>Time in force.</summary>
    [JsonProperty("f")]
    public BinanceTimeInForce TimeInForce { get; set; }

    /// <summary>Original order quantity.</summary>
    [JsonProperty("q")]
    public decimal Quantity { get; set; }

    /// <summary>Order price.</summary>
    [JsonProperty("p")]
    public decimal Price { get; set; }

    /// <summary>Stop price.</summary>
    [JsonProperty("P")]
    public decimal StopPrice { get; set; }

    /// <summary>Iceberg quantity.</summary>
    [JsonProperty("F")]
    public decimal IcebergQuantity { get; set; }

    /// <summary>Order-list identifier, or -1 when not in a list.</summary>
    [JsonProperty("g")]
    public long OrderListId { get; set; }

    /// <summary>Original client order identifier for a cancellation.</summary>
    [JsonProperty("C")]
    public string? OriginalClientOrderId { get; set; }

    /// <summary>Current execution type.</summary>
    [JsonProperty("x")]
    public BinanceSpotOrderExecutionType ExecutionType { get; set; }

    /// <summary>Current order status.</summary>
    [JsonProperty("X")]
    public BinanceOrderStatus Status { get; set; }

    /// <summary>Order rejection reason or error code.</summary>
    [JsonProperty("r")]
    public string RejectReason { get; set; } = string.Empty;

    /// <summary>Exchange-assigned order identifier.</summary>
    [JsonProperty("i")]
    public long Id { get; set; }

    /// <summary>Last executed quantity.</summary>
    [JsonProperty("l")]
    public decimal LastQuantityFilled { get; set; }

    /// <summary>Cumulative filled quantity.</summary>
    [JsonProperty("z")]
    public decimal QuantityFilled { get; set; }

    /// <summary>Last executed price.</summary>
    [JsonProperty("L")]
    public decimal LastPriceFilled { get; set; }

    /// <summary>Commission amount.</summary>
    [JsonProperty("n")]
    public decimal Commission { get; set; }

    /// <summary>Commission asset.</summary>
    [JsonProperty("N")]
    public string? CommissionAsset { get; set; }

    /// <summary>Transaction time.</summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime TransactionTime { get; set; }

    /// <summary>Trade identifier.</summary>
    [JsonProperty("t")]
    public long TradeId { get; set; }

    /// <summary>Execution identifier.</summary>
    [JsonProperty("I")]
    public long ExecutionId { get; set; }

    /// <summary>Whether the order is on the order book.</summary>
    [JsonProperty("w")]
    public bool IsWorking { get; set; }

    /// <summary>Whether this trade is the maker side.</summary>
    [JsonProperty("m")]
    public bool IsMaker { get; set; }

    /// <summary>Ignored field returned by Binance.</summary>
    [JsonProperty("M")]
    public bool Ignore { get; set; }

    /// <summary>Order creation time.</summary>
    [JsonProperty("O"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateTime { get; set; }

    /// <summary>Cumulative quote quantity.</summary>
    [JsonProperty("Z")]
    public decimal QuoteQuantityFilled { get; set; }

    /// <summary>Last quote quantity.</summary>
    [JsonProperty("Y")]
    public decimal LastQuoteQuantity { get; set; }

    /// <summary>Quote order quantity.</summary>
    [JsonProperty("Q")]
    public decimal QuoteQuantity { get; set; }

    /// <summary>Time when the order entered the order book.</summary>
    [JsonProperty("W"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime? WorkingTime { get; set; }

    /// <summary>Self-trade prevention mode.</summary>
    [JsonProperty("V"), JsonConverter(typeof(MapConverter))]
    public BinanceSelfTradePreventionMode? SelfTradePreventionMode { get; set; }

    /// <summary>Trailing delta.</summary>
    [JsonProperty("d")]
    public long? TrailingDelta { get; set; }

    /// <summary>Trailing activation time.</summary>
    [JsonProperty("D"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime? TrailingTime { get; set; }

    /// <summary>Strategy identifier.</summary>
    [JsonProperty("j")]
    public long? StrategyId { get; set; }

    /// <summary>Strategy type.</summary>
    [JsonProperty("J")]
    public long? StrategyType { get; set; }

    /// <summary>Prevented match identifier.</summary>
    [JsonProperty("v")]
    public long? PreventedMatchId { get; set; }

    /// <summary>Prevented quantity.</summary>
    [JsonProperty("A")]
    public decimal? PreventedQuantity { get; set; }

    /// <summary>Last prevented quantity.</summary>
    [JsonProperty("B")]
    public decimal? LastPreventedQuantity { get; set; }

    /// <summary>Trade group identifier.</summary>
    [JsonProperty("u")]
    public long? TradeGroupId { get; set; }

    /// <summary>Counter-order identifier.</summary>
    [JsonProperty("U")]
    public long? CounterOrderId { get; set; }

    /// <summary>Counter symbol.</summary>
    [JsonProperty("Cs")]
    public string? CounterSymbol { get; set; }

    /// <summary>Prevented execution quantity.</summary>
    [JsonProperty("pl")]
    public decimal? PreventedExecutionQuantity { get; set; }

    /// <summary>Prevented execution price.</summary>
    [JsonProperty("pL")]
    public decimal? PreventedExecutionPrice { get; set; }

    /// <summary>Prevented execution quote quantity.</summary>
    [JsonProperty("pY")]
    public decimal? PreventedExecutionQuoteQuantity { get; set; }

    /// <summary>Allocation match type.</summary>
    [JsonProperty("b")]
    public string? MatchType { get; set; }

    /// <summary>Allocation identifier.</summary>
    [JsonProperty("a")]
    public long? AllocationId { get; set; }

    /// <summary>Working floor.</summary>
    [JsonProperty("k")]
    public string? WorkingFloor { get; set; }

    /// <summary>Whether Smart Order Routing was used.</summary>
    [JsonProperty("uS")]
    public bool? UsedSor { get; set; }
}
