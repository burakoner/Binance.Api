namespace Binance.Api.Mining;

/// <summary>
/// Revenue list
/// </summary>
public record BinanceMiningRevenues
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
    public List<BinanceMiningRevenue> AccountProfits { get; set; } = [];
}

/// <summary>
/// Revenue
/// </summary>
public record BinanceMiningRevenue
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
    /// Day hashrate
    /// </summary>
    public decimal DayHashRate { get; set; }

    /// <summary>
    /// Profit quantity
    /// </summary>
    [JsonProperty("profitAmount")]
    public decimal ProfitQuantity { get; set; }

    /// <summary>
    /// Hash transfer
    /// </summary>
    public decimal? HashTransfer { get; set; }

    /// <summary>
    /// Transfer quantity
    /// </summary>
    [JsonProperty("transferAmount")]
    public decimal? TransferQuantity { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public BinanceMiningMinerStatus Status { get; set; }
}
