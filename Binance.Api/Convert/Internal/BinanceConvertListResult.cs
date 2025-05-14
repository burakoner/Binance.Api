namespace Binance.Api.Convert;

/// <summary>
/// List result
/// </summary>
/// <typeparam name="T"></typeparam>
internal record BinanceConvertListResult<T>
{
    /// <summary>
    /// The data
    /// </summary>
    [JsonProperty("list")]
    public IEnumerable<T> List { get; set; } = [];
}
