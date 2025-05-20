namespace Binance.Api.Mining;

/// <summary>
/// Revenue list
/// </summary>
public record BinanceMiningOtherRevenues
{
    /// <summary>
    /// Total number of results
    /// </summary>
    [JsonProperty("totalNum")]
    public int TotalNumber { get; set; }

    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Revenue items
    /// </summary>
    [JsonProperty("otherProfits")]
    public List<BinanceMiningOtherRevenue> OtherProfits { get; set; } = [];
}

/// <summary>
/// Revenue
/// </summary>
public record BinanceMiningOtherRevenue
{
    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("time")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Coin
    /// </summary>
    [JsonProperty("coinName")]
    public string Coin { get; set; } = string.Empty;

    /// <summary>
    /// Earning type
    /// </summary>
    public BinanceMiningEarningType Type { get; set; }

    /// <summary>
    /// Profit quantity
    /// </summary>
    [JsonProperty("profitAmount")]
    public decimal ProfitQuantity { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public BinanceMiningMinerStatus Status { get; set; }
}
