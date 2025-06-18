namespace Binance.Api.PortfolioMargin;

public  record BinancePortfolioMarginConditionalOrder
{
    [JsonProperty("newClientStrategyId")]
    public string NewClientStrategyId { get; set; }

    [JsonProperty("strategyId")]
    public long StrategyId { get; set; }

    [JsonProperty("strategyStatus")]
    public string StrategyStatus { get; set; } // TODO: Enum

    [JsonProperty("strategyType")]
    public string StrategyType { get; set; } // TODO: Enum

    [JsonProperty("origQty")]
    public decimal OriginalQuantity { get; set; }

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("reduceOnly")]
    public bool ReduceOnly { get; set; }

    [JsonProperty("side")]
    public BinanceOrderSide Side { get; set; }

    [JsonProperty("positionSide")]
    public BinancePositionSide PositionSide { get; set; }

    [JsonProperty("stopPrice")]
    public decimal? StopPrice { get; set; }

    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    [JsonProperty("timeInForce")]
    public BinanceTimeInForce TimeInForce { get; set; }

    [JsonProperty("activatePrice")]
    public decimal? ActivatePrice { get; set; }

    [JsonProperty("priceRate")]
    public decimal? PriceRate { get; set; }

    [JsonProperty("bookTime")]
    public DateTime? BookTime { get; set; }

    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }

    [JsonProperty("workingType")]
    public string WorkingType { get; set; } // TODO: Enum

    [JsonProperty("priceProtect")]
    public bool PriceProtect { get; set; }
} 

public record BinancePortfolioMarginConditionalOrderCM : BinancePortfolioMarginConditionalOrder
{
    [JsonProperty("pair")]
    public string Pair { get; set; } = "";
}

public record BinancePortfolioMarginConditionalOrderUM : BinancePortfolioMarginConditionalOrder
{
    [JsonProperty("selfTradePreventionMode")]
    public BinanceSelfTradePreventionMode SelfTradePreventionMode { get; set; }

    [JsonProperty("goodTillDate")]
    public DateTime? GoodTillDate { get; set; }

    [JsonProperty("priceMatch")]
    public string PriceMatch { get; set; } // TODO: Enum
}