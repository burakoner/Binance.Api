namespace Binance.Api.Mining;

/// <summary>
/// Resale list
/// </summary>
public record BinanceMiningHashrateResaleDetails
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
    /// Transfer details
    /// </summary>
    [JsonProperty("profitTransferDetails")]
    public List<BinanceMiningHashrateResaleDetail> ProfitTransferDetails { get; set; } = [];
}

/// <summary>
/// Resale item
/// </summary>
public record BinanceMiningHashrateResaleDetail
{
    /// <summary>
    /// Config id
    /// </summary>
    public long ConfigId { get; set; }

    /// <summary>
    /// From user
    /// </summary>
    public string PoolUserName { get; set; } = string.Empty;

    /// <summary>
    /// To user
    /// </summary>
    public string ToPoolUserName { get; set; } = string.Empty;

    /// <summary>
    /// Algorithm
    /// </summary>
    public string AlgoName { get; set; } = string.Empty;

    /// <summary>
    /// Hash rate
    /// </summary>
    public decimal Hashrate { get; set; }

    /// <summary>
    /// Start day
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Day { get; set; }

    /// <summary>
    /// Coin name
    /// </summary>
    [JsonProperty("coinName")]
    public string Coin { get; set; } = string.Empty;

    /// <summary>
    /// Transferred income
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
}
