namespace Binance.Api.Futures;

/// <summary>
/// Wrapper for continuous kline information for a symbol
/// </summary>
public record BinanceStreamContinuousKlineData: BinanceFuturesStreamEvent
{
    /// <summary>
    /// The symbol the data is for
    /// </summary>
    [JsonProperty("ps")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// The contract type
    /// </summary>
    [JsonProperty("ct")]
    public BinanceFuturesContractType ContractType { get; set; } = BinanceFuturesContractType.Unknown;

    /// <summary>
    /// The data
    /// </summary>
    [JsonProperty("k")]
    public BinanceFuturesStreamKline Data { get; set; } = default!;
}
