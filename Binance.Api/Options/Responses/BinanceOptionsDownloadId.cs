namespace Binance.Api.Options;

/// <summary>
/// Options Transaction History Download Id
/// </summary>
public record BinanceOptionsDownloadId
{
    /// <summary>
    /// Average time taken for data download in the past 30 days
    /// </summary>
    [JsonProperty("avgCostTimestampOfLast30d")]
    public long AverageTimeCostOfLast30Days { get; set; }

    /// <summary>
    /// Download Id
    /// </summary>
    public long DownloadId { get; set; }
}