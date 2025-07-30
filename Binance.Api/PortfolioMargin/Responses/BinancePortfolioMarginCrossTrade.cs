namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Cross Trade
/// </summary>
public record BinancePortfolioMarginCrossTrade
{
    /// <summary>
    /// Symbol of the asset traded
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Id
    /// </summary>
    [JsonProperty("id")]
    public long Id { get; set; }

    /// <summary>
    /// Order ID of the trade
    /// </summary>
    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    /// <summary>
    /// Price at which the trade was executed
    /// </summary>
    [JsonProperty("price")]
    public decimal Price { get; set; }

    /// <summary>
    /// Quantity of the asset traded
    /// </summary>
    [JsonProperty("qty")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Commission charged for the trade
    /// </summary>
    [JsonProperty("commission")]
    public decimal Commission { get; set; }

    /// <summary>
    /// Commission asset used for the trade
    /// </summary>
    [JsonProperty("commissionAsset")]
    public string CommissionAsset { get; set; } = "";

    /// <summary>
    /// Timestamp of the trade
    /// </summary>
    [JsonProperty("time")]
    public DateTime Time { get; set; }

    /// <summary>
    /// Is best match
    /// </summary>
    [JsonProperty("isBestMatch")]
    public bool IsBestMatch { get; set; }

    /// <summary>
    /// Is buyer
    /// </summary>
    [JsonProperty("isBuyer")]
    public bool IsBuyer { get; set; }

    /// <summary>
    /// Is maker
    /// </summary>
    [JsonProperty("isMaker")]
    public bool IsMaker { get; set; }
}