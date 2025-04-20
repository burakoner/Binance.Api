namespace Binance.Api.Models.RestApi.Brokerage;

/// <summary>
/// Brokerage Rebate
/// </summary>
public record BinanceBrokerageRebate
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    public string SubAccountId { get; set; } = "";

    /// <summary>
    /// Income
    /// </summary>
    public decimal Income { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    public string Asset { get; set; } = "";

    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Trade Id
    /// </summary>
    public string TradeId { get; set; } = "";

    /// <summary>
    /// Date
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Date { get; set; }
}