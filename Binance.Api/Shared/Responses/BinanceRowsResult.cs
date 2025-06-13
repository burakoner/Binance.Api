namespace Binance.Api.Shared;

/// <summary>
/// Rows Results
/// </summary>
/// <typeparam name="T"></typeparam>
public record BinanceRowsResult<T>
{
    /// <summary>
    /// The total count of the records
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// The list records
    /// </summary>
    public List<T> Rows { get; set; } = [];
}
