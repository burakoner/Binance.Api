namespace Binance.Api.Options;

/// <summary>
/// Options Transaction History Download Link
/// </summary>
public record BinanceOptionsDownloadLink
{
    /// <summary>
    /// Download Id
    /// </summary>
    public long DownloadId { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Url
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Notified
    /// </summary>
    public bool Notified { get; set; }

    /// <summary>
    /// Expiration Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime ExpirationTimestamp { get; set; }

    /// <summary>
    /// Is the download link expired?
    /// </summary>
    public bool? IsExpired { get; set; }
}