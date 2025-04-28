namespace Binance.Api.Models.WebSocketApi.Futures;

/// <summary>
/// Information about leverage of symbol changed
/// </summary>
public record BinanceFuturesStreamConfigUpdate : BinanceSocketEvent
{
    /// <summary>
    /// Leverage Update data
    /// </summary>
    [JsonProperty("ac")]
    public BinanceFuturesStreamLeverageUpdateData LeverageUpdateData { get; set; } = new BinanceFuturesStreamLeverageUpdateData();

    /// <summary>
    /// Position mode Update data
    /// </summary>
    [JsonProperty("ai")]
    public BinanceFuturesStreamConfigUpdateData ConfigUpdateData { get; set; } = new BinanceFuturesStreamConfigUpdateData();

    /// <summary>
    /// Transaction time
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime TransactionTime { get; set; }
    /// <summary>
    /// The listen key the update was for
    /// </summary>
    public string ListenKey { get; set; } = "";
}

/// <summary>
/// Config update data
/// </summary>
public record BinanceFuturesStreamLeverageUpdateData
{
    /// <summary>
    /// The symbol this balance is for
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The symbol this leverage is for
    /// </summary>
    [JsonProperty("l")]
    public int Leverage { get; set; }
}

/// <summary>
/// Position mode update data
/// </summary>
public record BinanceFuturesStreamConfigUpdateData
{
    /// <summary>
    /// Multi-Assets Mode
    /// </summary>
    [JsonProperty("j")]
    public bool MultiAssetsMode { get; set; }
}
