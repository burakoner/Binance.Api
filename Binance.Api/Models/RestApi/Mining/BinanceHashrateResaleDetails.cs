namespace Binance.Api.Models.RestApi.Mining;

/// <summary>
/// Resale list
/// </summary>
public record BinanceHashrateResaleDetails
{
    /// <summary>
    /// Total number of results
    /// </summary>
    public int TotalNum { get; set; }
    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; set; }
    /// <summary>
    /// Transfer details
    /// </summary>
    public IEnumerable<BinanceHashrateResaleDetailsItem> ProfitTransferDetails { get; set; } = [];
}

/// <summary>
/// Resale item
/// </summary>
public record BinanceHashrateResaleDetailsItem
{
    /// <summary>
    /// Config id
    /// </summary>
    public long ConfigId { get; set; }
    /// <summary>
    /// From user
    /// </summary>
    public string PoolUserName { get; set; } = "";
    /// <summary>
    /// To user
    /// </summary>
    public string ToPoolUserName { get; set; } = "";
    /// <summary>
    /// Algorithm
    /// </summary>
    public string AlgoName { get; set; } = "";
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
    public string Coin { get; set; } = "";
    /// <summary>
    /// Transferred income
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
}
