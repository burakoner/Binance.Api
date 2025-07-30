namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Amendment
/// </summary>
public record BinancePortfolioMarginAmendment
{
    /// <summary>
    /// Adjustment ID
    /// </summary>
    [JsonProperty("amendmentId")]
    public long AmendmentId { get; set; }

    /// <summary>
    /// Symbol of the asset being amended
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Pair of the asset being amended
    /// </summary>
    [JsonProperty("pair")]
    public string Pair { get; set; } = "";

    /// <summary>
    /// Order ID of the original order
    /// </summary>
    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    /// <summary>
    /// Client Order ID of the original order
    /// </summary>
    [JsonProperty("clientOrderId")]
    public string ClientOrderId { get; set; } = "";

    /// <summary>
    /// Time
    /// </summary>
    [JsonProperty("time")]
    public long Time { get; set; }

    /// <summary>
    /// Amendment
    /// </summary>
    [JsonProperty("amendment")]
    public BinancePortfolioMarginAmendmentData Amendment { get; set; } = new();

    /// <summary>
    /// Price match
    /// </summary>
    [JsonProperty("priceMatch")]
    public string PriceMatch { get; set; } = "";
}

/// <summary>
/// Binance Portfolio Margin Amendment Data
/// </summary>
public record BinancePortfolioMarginAmendmentData
{
    /// <summary>
    /// Price before and after the amendment
    /// </summary>
    [JsonProperty("price")]
    public BinancePortfolioMarginAmendmentDataPrice Price { get; set; } = new();

    /// <summary>
    /// Quantity before and after the amendment
    /// </summary>
    [JsonProperty("origQty")]
    public BinancePortfolioMarginAmendmentDataQuantity OrigQty { get; set; } = new();

    /// <summary>
    /// Count of amendments
    /// </summary>
    [JsonProperty("count")]
    public int Count { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Amendment Data Price
/// </summary>
public record BinancePortfolioMarginAmendmentDataPrice
{
    /// <summary>
    /// Before price
    /// </summary>
    [JsonProperty("before")]
    public decimal? Before { get; set; }

    /// <summary>
    /// After price
    /// </summary>
    [JsonProperty("after")]
    public decimal? After { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Amendment Data Quantity
/// </summary>
public record BinancePortfolioMarginAmendmentDataQuantity
{
    /// <summary>
    /// Before quantity
    /// </summary>
    [JsonProperty("before")]
    public decimal Before { get; set; }

    /// <summary>
    /// After quantity
    /// </summary>
    [JsonProperty("after")]
    public decimal After { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Amendment CM (Cross Margin)
/// </summary>
public record BinancePortfolioMarginAmendmentCM: BinancePortfolioMarginAmendment
{
}

/// <summary>
/// Binance Portfolio Margin Amendment UM (Isolated Margin)
/// </summary>
public record BinancePortfolioMarginAmendmentUM : BinancePortfolioMarginAmendment
{
}