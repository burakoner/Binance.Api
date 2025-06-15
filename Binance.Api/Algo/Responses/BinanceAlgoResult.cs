namespace Binance.Api.Algo;

/// <summary>
/// Algo order result
/// </summary>
public record BinanceAlgoResult
{
    /// <summary>
    /// Result code
    /// </summary>
    [JsonProperty("code")]
    public int Code { get; set; }

    /// <summary>
    /// Message
    /// </summary>
    [JsonProperty("msg")]
    public string? Message { get; set; }

    /// <summary>
    /// Algo order id
    /// </summary>
    [JsonProperty("algoId")]
    public long AlgoId { get; set; }

    /// <summary>
    /// Successful
    /// </summary>
    [JsonProperty("success")]
    public bool Success { get; set; }
}
