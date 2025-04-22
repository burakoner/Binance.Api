namespace Binance.Api.Algo;

/// <summary>
/// Algo order result
/// </summary>
public record BinanceAlgoResult: BinanceResult
{
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
