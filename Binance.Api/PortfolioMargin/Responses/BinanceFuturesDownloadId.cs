namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Download Id Information
/// </summary>
public record BinancePortfolioMarginDownloadId
{
    /// <summary>
    /// Average time taken for data download in the past 30 days
    /// </summary>
    [JsonProperty("avgCostTimestampOfLast30d")]
    public long AverageCostTimestampOfLast30Days { get; set; }

    /// <summary>
    /// Download id
    /// </summary>
    [JsonProperty("downloadId")]
    public string DownloadId { get; set; } = string.Empty;
}