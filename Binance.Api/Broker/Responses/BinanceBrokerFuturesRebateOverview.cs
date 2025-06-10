namespace Binance.Api.Broker;

/// <summary>
/// Futures Commission Rebate Overview
/// </summary>
public record BinanceBrokerFuturesRebateOverview
{
    /// <summary>
    /// Broker Id
    /// </summary>
    [JsonProperty("brokerId")]
    public string BrokerId { get; set; } = string.Empty;

    /// <summary>
    /// New Trader Rebate Commission
    /// </summary>
    [JsonProperty("newTraderRebateCommission")]
    public decimal NewTraderRebateCommission { get; set; }

    /// <summary>
    /// Old Trader Rebate Commission
    /// </summary>
    [JsonProperty("oldTraderRebateCommission")]
    public decimal OldTraderRebateCommission { get; set; }

    /// <summary>
    /// Total Rebate Commission
    /// </summary>
    [JsonProperty("totalTradeUser")]
    public int TotalTradeUser { get; set; }

    /// <summary>
    /// Unit of the Rebate Commission
    /// </summary>
    [JsonProperty("unit")]
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// Total Trade Volume
    /// </summary>
    [JsonProperty("totalTradeVol")]
    public decimal TotalTradeVolume { get; set; }

    /// <summary>
    /// Total Rebate Volume
    /// </summary>
    [JsonProperty("totalRebateVol")]
    public decimal TotalRebateVolume { get; set; }

    /// <summary>
    /// Time
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Date { get; set; }
}