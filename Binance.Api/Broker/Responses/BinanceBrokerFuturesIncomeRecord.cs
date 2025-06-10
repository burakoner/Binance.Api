namespace Binance.Api.Broker;

/// <summary>
/// Broker -> Futures -> Income History Record
/// </summary>
public record BinanceBrokerFuturesIncomeRecord
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Income Type
    /// </summary>
    [JsonProperty("incomeType")]
    public BinanceBrokerIncomeType IncomeType { get; set; }

    /// <summary>
    /// Income Amount
    /// </summary>
    [JsonProperty("income")]
    public decimal Income { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Information
    /// </summary>
    [JsonProperty("info")]
    public string Info { get; set; } = string.Empty;

    /// <summary>
    /// Time of the income record
    /// </summary>
    [JsonProperty("time")]
    public DateTime Time { get; set; }
}
