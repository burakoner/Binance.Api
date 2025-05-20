namespace Binance.Api.Mining;

/// <summary>
/// Mining coin info
/// </summary>
public record BinanceMiningCoin
{
    /// <summary>
    /// The name of the coin
    /// </summary>
    public string CoinName { get; set; } = string.Empty;

    /// <summary>
    /// The id of the coin
    /// </summary>
    public string CoinId { get; set; } = string.Empty;

    /// <summary>
    /// The pool index
    /// </summary>
    public int PoolIndex { get; set; }

    /// <summary>
    /// Algorithm id
    /// </summary>
    [JsonProperty("algoId")]
    public string AlgorithmId { get; set; } = string.Empty;

    /// <summary>
    /// Algorithm name
    /// </summary>
    [JsonProperty("algoName")]
    public string AlgorithmName { get; set; } = string.Empty;
}
