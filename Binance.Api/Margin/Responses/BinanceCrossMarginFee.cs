namespace Binance.Api.Margin;

/// <summary>
/// Cross Margin fee data
/// </summary>
public record BinanceCrossMarginFee
{
    /// <summary>
    /// VIP level
    /// </summary>
    public long VipLevel { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("coin")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Whether the asset can be transferred into Cross Margin
    /// </summary>
    public bool TransferIn { get; set; }

    /// <summary>
    /// Whether the asset can be borrowed in Cross Margin
    /// </summary>
    public bool Borrowable { get; set; }

    /// <summary>
    /// Daily interest rate
    /// </summary>
    public decimal DailyInterest { get; set; }

    /// <summary>
    /// Yearly interest rate
    /// </summary>
    public decimal YearlyInterest { get; set; }

    /// <summary>
    /// Borrow limit
    /// </summary>
    public decimal BorrowLimit { get; set; }

    /// <summary>
    /// Cross Margin pairs available for the asset
    /// </summary>
    public string[] MarginablePairs { get; set; } = [];
}
