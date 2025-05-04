﻿namespace Binance.Api.Futures;

/// <summary>
/// Strategy update
/// </summary>
public record BinanceFuturesStreamStrategyUpdate: BinanceFuturesStreamEvent
{
    /// <summary>
    /// Update info
    /// </summary>
    [JsonProperty("su")]
    public BinanceStrategyInfo StrategyUpdate { get; set; } = null!;

    /// <summary>
    /// Transaction time
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime TransactionTime { get; set; }

    /// <summary>
    /// The listen key the update was for
    /// </summary>
    [JsonIgnore]
    public string ListenKey { get; set; } = string.Empty;
}

/// <summary>
/// Strategy update info
/// </summary>
public record BinanceStrategyInfo
{
    /// <summary>
    /// The strategy id
    /// </summary>
    [JsonProperty("si")]
    public int StrategyId { get; set; }

    /// <summary>
    /// Strategy type
    /// </summary>
    [JsonProperty("st")]
    public string StrategyType { get; set; } = string.Empty;

    /// <summary>
    /// Strategy status
    /// </summary>
    [JsonProperty("ss")]
    public string StrategyStatus { get; set; } = string.Empty;

    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Update time
    /// </summary>
    [JsonProperty("ut")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// Op code
    /// </summary>
    [JsonProperty("c")]
    public int OpCode { get; set; }
}
