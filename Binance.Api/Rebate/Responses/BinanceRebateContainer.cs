namespace Binance.Api.Rebate;

/// <summary>
/// Binance Rebate Container
/// </summary>
/// <typeparam name="T"></typeparam>
public record BinanceRebateContainer<T>
{
    /// <summary>
    /// Page Number
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Total Records
    /// </summary>
    public int TotalRecords { get; set; }

    /// <summary>
    /// Total Page Number
    /// </summary>
    [JsonProperty("totalPageNum")]
    public int TotalPageNumber { get; set; }

    /// <summary>
    /// Data
    /// </summary>
    public List<T> Data { get; set; } = [];
}
