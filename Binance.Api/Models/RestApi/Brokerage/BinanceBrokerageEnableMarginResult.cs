namespace Binance.Api.Models.RestApi.Brokerage;

/// <summary>
/// Enable Margin Result
/// </summary>
public record BinanceBrokerageEnableMarginResult
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    public string SubAccountId { get; set; } = "";

    /// <summary>
    /// Is Margin Enabled
    /// </summary>
    [JsonProperty("enableMargin")]
    public bool IsMarginEnabled { get; set; }

    /// <summary>
    /// Update Date
    /// </summary>
    [JsonProperty("updateTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateDate { get; set; }
}