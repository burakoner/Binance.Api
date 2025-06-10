namespace Binance.Api.Broker;

/// <summary>
/// Broker -> Futures -> Trader Details
/// </summary>
public record BinanceBrokerFuturesTraderDetail
{
    /// <summary>
    /// Customer Id
    /// </summary>
    [JsonProperty("customerId")]
    public string CustomerId { get; set; } = string.Empty;

    /// <summary>
    /// Unit
    /// </summary>
    [JsonProperty("unit")]
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// Trade Volume
    /// </summary>
    [JsonProperty("tradeVol")]
    public decimal TradeVolume { get; set; }

    /// <summary>
    /// Rebate Volume
    /// </summary>
    [JsonProperty("rebateVol")]
    public decimal RebateVolume { get; set; }

    /// <summary>
    /// Time
    /// </summary>
    [JsonProperty("time")]
    public DateTime Time { get; set; }
}
