namespace Binance.Api.Link;

/// <summary>
/// Link Rebate
/// </summary>
public record BinanceLinkRebate
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    [JsonProperty("subaccountId")]
    public string SubAccountId { get; set; } = string.Empty;

    /// <summary>
    /// Income
    /// </summary>
    [JsonProperty("income")]
    public decimal Income { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Trade Id
    /// </summary>
    [JsonProperty("tradeId")]
    public string TradeId { get; set; } = string.Empty;

    /// <summary>
    /// Date
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Date { get; set; }
}