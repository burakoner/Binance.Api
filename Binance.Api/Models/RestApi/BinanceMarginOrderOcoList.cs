namespace Binance.Api.Models.RestApi;

/// <summary>
/// Oco info
/// </summary>
public record BinanceMarginOrderOcoList : BinanceOrderOcoList
{
    /// <summary>
    /// Margin buy borrow quantity
    /// </summary>
    [JsonProperty("marginBuyBorrowAmount")]
    public decimal? MarginBuyBorrowQuantity { get; set; }
    /// <summary>
    /// Margin buy borrow asset
    /// </summary>
    public string MarginBuyBorrowAsset { get; set; } = "";
    /// <summary>
    /// Is isolated margin
    /// </summary>
    public bool IsIsolated { get; set; }
}
