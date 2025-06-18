namespace Binance.Api.PortfolioMargin;

public  record BinancePortfolioMarginTrade
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    [JsonProperty("side")]
    public BinanceOrderSide Side { get; set; }

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("qty")]
    public string Quantity { get; set; }

    [JsonProperty("realizedPnl")]
    public string RealizedPnl { get; set; }

    [JsonProperty("commission")]
    public decimal Commission { get; set; }

    [JsonProperty("commissionAsset")]
    public string CommissionAsset { get; set; }

    [JsonProperty("time")]
    public DateTime Time { get; set; }

    [JsonProperty("buyer")]
    public bool IsBuyer { get; set; }

    [JsonProperty("maker")]
    public bool IsMaker { get; set; }

    [JsonProperty("positionSide")]
    public BinancePositionSide PositionSide { get; set; }
}

public  record BinancePortfolioMarginTradeCM: BinancePortfolioMarginTrade
{
    [JsonProperty("pair")]
    public string Pair { get; set; } = "";

    [JsonProperty("marginAsset")]
    public string MarginAsset { get; set; } = "";

    [JsonProperty("baseQty")]
    public decimal BaseQuantity { get; set; }
}

public  record BinancePortfolioMarginTradeUM : BinancePortfolioMarginTrade
{
    [JsonProperty("quoteQty")]
    public decimal QuoteQuantity { get; set; }
}