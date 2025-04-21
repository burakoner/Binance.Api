namespace Binance.Api.Shared;

/// <summary>
/// Query results
/// </summary>
/// <typeparam name="T"></typeparam>
public record BinanceQueryRecords<T>
{
    /// <summary>
    /// The list records
    /// </summary>
    public IEnumerable<T> Rows { get; set; } = [];

    /// <summary>
    /// The total count of the records
    /// </summary>
    public int Total { get; set; }
}
