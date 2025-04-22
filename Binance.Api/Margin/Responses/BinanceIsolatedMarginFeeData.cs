namespace Binance.Net.Objects.Models.Spot.Margin;

/// <summary>
/// Fee data
/// </summary>
public record BinanceIsolatedMarginFeeData
{
    /// <summary>
    /// Vip level
    /// </summary>
    public int VipLevel { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Leverage
    /// </summary>
    public int Leverage { get; set; }

    /// <summary>
    /// Data
    /// </summary>
    public IEnumerable<BinanceIsolatedMarginFeeInfo> Data { get; set; } = [];
}

/// <summary>
/// Fee info
/// </summary>
public record BinanceIsolatedMarginFeeInfo
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
