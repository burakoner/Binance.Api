namespace Binance.Api.Mining;

/// <summary>
/// Miner list
/// </summary>
public record BinanceMiningWorkers
{
    /// <summary>
    /// Total number of entries
    /// </summary>
    public int TotalNum { get; set; }

    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Worker data
    /// </summary>
    public List<BinanceMiningWorker> WorkerDatas { get; set; } = [];
}

/// <summary>
/// Miner details
/// </summary>
public record BinanceMiningWorker
{
    /// <summary>
    /// Worker id
    /// </summary>
    public string WorkerId { get; set; } = string.Empty;

    /// <summary>
    /// Worker name
    /// </summary>
    public string WorkerName { get; set; } = string.Empty;

    /// <summary>
    /// Status
    /// </summary>
    public BinanceMiningMinerStatus Status { get; set; }

    /// <summary>
    /// Hash rate
    /// </summary>
    public decimal HashRate { get; set; }

    /// <summary>
    /// Day hash rate
    /// </summary>
    public decimal DayHashRate { get; set; }

    /// <summary>
    /// Reject rate
    /// </summary>
    public decimal RejectRate { get; set; }

    /// <summary>
    /// Last share time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime LastShareTime { get; set; }
}
