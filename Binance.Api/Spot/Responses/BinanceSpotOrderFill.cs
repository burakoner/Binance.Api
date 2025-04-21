namespace Binance.Api.Spot;

/// <summary>
/// Binance Spot Order Fill
/// </summary>
public record BinanceSpotOrderFill
{
    /// <summary>
    /// Price of the trade
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Quantity of the trade
    /// </summary>
    [JsonProperty("qty")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Fee paid over this trade
    /// </summary>
    [JsonProperty("commission")]
    public decimal Fee { get; set; }

    /// <summary>
    /// The asset the fee is paid in
    /// </summary>
    [JsonProperty("commissionAsset")]
    public string FeeAsset { get; set; } = "";

    /// <summary>
    /// The id of the trade
    /// </summary>
    public long TradeId { get; set; }
}
