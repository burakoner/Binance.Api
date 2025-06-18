namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Download Link Information
/// </summary>
public record BinancePortfolioMarginDownloadLink
{
    /// <summary>
    /// Download id
    /// </summary>
    [JsonProperty("downloadId")]
    public string DownloadId { get; set; } = string.Empty;

    /// <summary>
    /// Is ready to download
    /// </summary>
    [JsonProperty("status")]
    public BinanceDownloadStatus Status { get; set; }

    /// <summary>
    /// Download url
    /// </summary>
    [JsonProperty("url")]
    public string? Url { get; set; }

    /// <summary>
    /// S3 Link
    /// </summary>
    [JsonProperty("s3Link")]
    public string? S3Link { get; set; }

    /// <summary>
    /// Is Notified?
    /// </summary>
    [JsonProperty("notified")]
    public bool Notified { get; set; }

    /// <summary>
    /// Link expiration time
    /// </summary>
    [JsonProperty("expirationTimestamp")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime ExpirationTime { get; set; }

    /// <summary>
    /// Is expired
    /// </summary>
    [JsonProperty("isExpired")]
    public bool? IsExpired { get; set; }
}
