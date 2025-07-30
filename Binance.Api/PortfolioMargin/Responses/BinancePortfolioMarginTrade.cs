namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Trade
/// </summary>
public record BinancePortfolioMarginTrade
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Id
    /// </summary>
    [JsonProperty("id")]
    public long Id { get; set; }

    /// <summary>
    /// Order Id
    /// </summary>
    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    /// <summary>
    /// Side
    /// </summary>
    [JsonProperty("side")]
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// Price
    /// </summary>
    [JsonProperty("price")]
    public decimal Price { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("qty")]
    public decimal? Quantity { get; set; }

    /// <summary>
    /// Realized Profit and Loss
    /// </summary>
    [JsonProperty("realizedPnl")]
    public decimal? RealizedPnl { get; set; }

    /// <summary>
    /// Commission
    /// </summary>
    [JsonProperty("commission")]
    public decimal Commission { get; set; }

    /// <summary>
    /// Commission Asset
    /// </summary>
    [JsonProperty("commissionAsset")]
    public string CommissionAsset { get; set; } = "";

    /// <summary>
    /// Time
    /// </summary>
    [JsonProperty("time")]
    public DateTime Time { get; set; }

    /// <summary>
    /// Is Buyer
    /// </summary>
    [JsonProperty("buyer")]
    public bool IsBuyer { get; set; }

    /// <summary>
    /// Is Maker
    /// </summary>
    [JsonProperty("maker")]
    public bool IsMaker { get; set; }

    /// <summary>
    /// Position Side
    /// </summary>
    [JsonProperty("positionSide")]
    public BinancePositionSide PositionSide { get; set; }
}

/// <summary>
/// Represents a trade in the Binance Portfolio Margin system, including details about the trading pair, margin asset,
/// and base quantity.
/// </summary>
public  record BinancePortfolioMarginTradeCM: BinancePortfolioMarginTrade
{
    /// <summary>
    /// Pair
    /// </summary>
    [JsonProperty("pair")]
    public string Pair { get; set; } = "";

    /// <summary>
    /// Margin Asset
    /// </summary>
    [JsonProperty("marginAsset")]
    public string MarginAsset { get; set; } = "";

    /// <summary>
    /// Base Quantity
    /// </summary>
    [JsonProperty("baseQty")]
    public decimal BaseQuantity { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Trade for USDT-Margined Futures
/// </summary>
public record BinancePortfolioMarginTradeUM : BinancePortfolioMarginTrade
{
    /// <summary>
    /// Quote Quantity
    /// </summary>
    [JsonProperty("quoteQty")]
    public decimal QuoteQuantity { get; set; }
}