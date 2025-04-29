namespace Binance.Api.Futures;

/// <summary>
/// Transaction download info
/// </summary>
public record BinanceFuturesDownloadLink
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
    public string Url { get; set; } = string.Empty;

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
