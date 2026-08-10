namespace Binance.Api.Options;

/// <summary>
/// Result of cancelling all Options orders for an underlying
/// </summary>
public record BinanceOptionsCancelAllOrdersByUnderlyingResult
{
    /// <summary>
    /// Result code
    /// </summary>
    [JsonProperty("code")]
    public long Code { get; set; }

    /// <summary>
    /// Result message
    /// </summary>
    [JsonProperty("msg")]
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// Result of cancelling all Options orders for a symbol
/// </summary>
public record BinanceOptionsCancelAllOrdersBySymbolResult
{
    /// <summary>
    /// Result code
    /// </summary>
    [JsonProperty("code")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Result message
    /// </summary>
    [JsonProperty("msg")]
    public string Message { get; set; } = string.Empty;
}
