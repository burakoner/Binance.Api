namespace Binance.Api.Mining;

/// <summary>
/// Miner details
/// </summary>
public record BinanceMiningWorkerDetails
{
    /// <summary>
    /// Name of the worker
    /// </summary>
    public string WorkerName { get; set; } = string.Empty;

    /// <summary>
    /// Data type
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Hash rate data
    /// </summary>
    public List<BinanceMiningHashrate> HashRateDatas { get; set; } = [];
}