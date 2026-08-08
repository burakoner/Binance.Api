namespace Binance.Api.Margin;

/// <summary>
/// Margin order-list information.
/// </summary>
public record BinanceMarginOrderList : BinanceOrderOcoList
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

/// <summary>
/// Margin OCO information.
/// </summary>
public record BinanceMarginOrderOcoList : BinanceMarginOrderList
{
}
