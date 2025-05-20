namespace Binance.Api.Mining;

/// <summary>
/// Earning info
/// </summary>
public record BinanceMiningEarnings
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
    /// Profit items
    /// </summary>
    public List<BinanceMiningEarning> AccountProfits { get; set; } = [];
}

/// <summary>
/// Earning info
/// </summary>
public record BinanceMiningEarning
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
    /// Sub account id
    /// </summary>
    [JsonProperty("puid")]
    public long? SubAccountId { get; set; }

    /// <summary>
    /// Mining account
    /// </summary>
    public string SubName { get; set; } = string.Empty;

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
}
