namespace Binance.Api.Mining;

/// <summary>
/// Resale list
/// </summary>
public record BinanceMiningHashrateResaleList
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
    /// Details
    /// </summary>
    [JsonProperty("configDetails")]
    public List<BinanceHashrateResaleItem> ResaleItmes { get; set; } = [];
}

/// <summary>
/// Resale item
/// </summary>
public record BinanceHashrateResaleItem
{
    /// <summary>
    /// Mining id
    /// </summary>
    public int ConfigId { get; set; }

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
    public DateTime StartDay { get; set; }

    /// <summary>
    /// End day
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime EndDay { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public BinanceMiningHashrateResaleStatus Status { get; set; }
}
