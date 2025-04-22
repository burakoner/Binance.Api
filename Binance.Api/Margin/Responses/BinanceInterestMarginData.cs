namespace Binance.Api.Margin;

/// <summary>
/// Cross margin interest data
/// </summary>
public record BinanceInterestMarginData
{
    /// <summary>
    /// Vip level
    /// </summary>
    public string VipLevel { get; set; } = "";

    /// <summary>
    /// The coin
    /// </summary>        
    [JsonProperty("coin")]
    public string Asset { get; set; } = "";

    /// <summary>
    /// If coin can be transferred into cross
    /// </summary>
    [JsonProperty("transferIn")]
    public bool TransferIn { get; set; } = false;

    /// <summary>
    /// If coin can be borrowed in cross
    /// </summary>        
    public bool Borrowable { get; set; } = false;

    /// <summary>
    /// The daily interest
    /// </summary>
    public decimal DailyInterest { get; set; }

    /// <summary>
    /// The yearly interest
    /// </summary>
    public decimal YearlyInterest { get; set; }

    /// <summary>
    /// The yearly interest
    /// </summary>
    public decimal BorrowLimit { get; set; }

    /// <summary>
    /// Cross marginable pairs for this coin
    /// </summary>
    public string[] MarginablePairs { get; set; } = [];

}
