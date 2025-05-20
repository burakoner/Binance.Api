namespace Binance.Api.Shared;

/// <summary>
/// Query results
/// </summary>
/// <typeparam name="T"></typeparam>
public record BinanceListRecords<T>
{
    /// <summary>
    /// The total count of the records
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// The list records
    /// </summary>
    public List<T> List { get; set; } = [];
}
