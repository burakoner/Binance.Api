namespace Binance.Api.Shared.Responses;

/// <summary>
/// Query result
/// </summary>
public record BinanceResult
{
    /// <summary>
    /// Result code
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// Message
    /// </summary>
    [JsonProperty("msg")]
    public string Message { get; set; }
}

/// <summary>
/// Query result
/// </summary>
/// <typeparam name="T"></typeparam>
internal record BinanceResult<T> : BinanceResult
{
    /// <summary>
    /// The data
    /// </summary>
    public T Data { get; set; } = default!;
}
