namespace Binance.Api.Shared;

/// <summary>
/// Download status
/// </summary>
public enum BinanceDownloadStatus : byte
{
    /// <summary>
    /// Processing
    /// </summary>
    [Map("processing")]
    Processing = 1,

    /// <summary>
    /// Ready for download
    /// </summary>
    [Map("completed")]
    Completed = 2
}
