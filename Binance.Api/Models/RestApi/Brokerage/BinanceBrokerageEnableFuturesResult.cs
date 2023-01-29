namespace Binance.Api.Models.RestApi.Brokerage;

/// <summary>
/// Enable Futures Result
/// </summary>
public class BinanceBrokerageEnableFuturesResult
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    public string SubAccountId { get; set; }

    /// <summary>
    /// Is Futures Enabled
    /// </summary>
    [JsonProperty("enableFutures")]
    public bool IsFuturesEnabled { get; set; }

    /// <summary>
    /// Update Date
    /// </summary>
    [JsonProperty("updateTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateDate { get; set; }
}