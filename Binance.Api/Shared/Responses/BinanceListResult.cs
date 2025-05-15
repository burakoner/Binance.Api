namespace Binance.Api.Shared;

/// <summary>
/// List result
/// </summary>
/// <typeparam name="T"></typeparam>
public record BinanceListResult<T>
{
    /// <summary>
    /// The data
    /// </summary>
    [JsonProperty("list")]
    public List<T> List { get; set; } = [];

    /// <summary>
    /// Data start time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("startTime")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Emd to,e
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("endTime")]
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Limit
    /// </summary>
    [JsonProperty("limit")]
    public int Limit { get; set; }

    /// <summary>
    /// More data available
    /// </summary>
    [JsonProperty("moreData")]
    public bool MoreData { get; set; }
}
