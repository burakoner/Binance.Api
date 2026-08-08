namespace Binance.Api.Margin;

/// <summary>
/// Fee data
/// </summary>
public record BinanceIsolatedMarginFee
{
    /// <summary>
    /// Vip level
    /// </summary>
    public long VipLevel { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Leverage
    /// </summary>
    public string Leverage { get; set; } = string.Empty;

    /// <summary>
    /// Data
    /// </summary>
    public List<BinanceIsolatedMarginFeeData> Data { get; set; } = [];
}

/// <summary>
/// Fee info
/// </summary>
public record BinanceIsolatedMarginFeeData
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("coin")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Daily interest
    /// </summary>
    public decimal DailyInterest { get; set; }

    /// <summary>
    /// Borrow limit
    /// </summary>
    public decimal BorrowLimit { get; set; }
}
