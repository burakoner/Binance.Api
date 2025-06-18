namespace Binance.Api.PortfolioMargin;

public  record BinancePortfolioMarginCrossOrder
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    [JsonProperty("clientOrderId")]
    public string ClientOrderId { get; set; }

    [JsonProperty("price")]
    public decimal? Price { get; set; }

    [JsonProperty("origQty")]
    public decimal OriginalQuantity { get; set; }

    [JsonProperty("executedQty")]
    public decimal ExecutedQuantity { get; set; }

    [JsonProperty("cummulativeQuoteQty")]
    public decimal CummulativeQuoteQuantity { get; set; }

    [JsonProperty("status")]
    public BinancePortfolioMarginOrderStatus Status { get; set; } // TODO: Enum

    [JsonProperty("timeInForce")]
    public BinanceTimeInForce TimeInForce { get; set; }

    [JsonProperty("type")]
    public BinancePortfolioMarginOrderType Type { get; set; }

    [JsonProperty("side")]
    public BinanceOrderSide Side { get; set; }
}

public record BinancePortfolioMarginCrossOrderFill
{
    [JsonProperty("price")]
    public string Price { get; set; }

    [JsonProperty("qty")]
    public string Quantity { get; set; }

    [JsonProperty("commission")]
    public string Commission { get; set; }

    [JsonProperty("commissionAsset")]
    public string CommissionAsset { get; set; }
}

public record BinancePortfolioMarginCrossOrderPlaced : BinancePortfolioMarginCrossOrder
{
    [JsonProperty("transactTime")]
    public DateTime TransactionTime { get; set; }

    [JsonProperty("marginBuyBorrowAmount")]
    public decimal MarginBuyBorrowQuantity { get; set; }

    [JsonProperty("marginBuyBorrowAsset")]
    public string MarginBuyBorrowAsset { get; set; } = "";

    [JsonProperty("isIsolated")]
    public bool IsIsolated { get; set; }

    [JsonProperty("fills")]
    public List<BinancePortfolioMarginCrossOrderFill> Fills { get; set; } = [];
}

public record BinancePortfolioMarginCrossOrderCanceled : BinancePortfolioMarginCrossOrder
{
    [JsonProperty("origClientOrderId")]
    public string OriginalClientOrderId { get; set; }

}

public record BinancePortfolioMarginCrossOrderQuery : BinancePortfolioMarginCrossOrder
{
    [JsonProperty("icebergQty")]
    public decimal? IcebergQuantity { get; set; }

    [JsonProperty("isWorking")]
    public bool IsWorking { get; set; }

    [JsonProperty("stopPrice")]
    public decimal? StopPrice { get; set; }

    [JsonProperty("time")]
    public DateTime Time { get; set; }

    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }

    [JsonProperty("accountId")]
    public long AccountId { get; set; }

    [JsonProperty("selfTradePreventionMode")]
    public BinanceSelfTradePreventionMode SelfTradePreventionMode { get; set; }

    [JsonProperty("preventedMatchId")]
    public long? PreventedMatchId { get; set; }

    [JsonProperty("preventedQuantity")]
    public decimal? PpreventedQuantity { get; set; }
}