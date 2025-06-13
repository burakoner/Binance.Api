namespace Binance.Api.Link;

/// <summary>
/// Enable Margin Result
/// </summary>
public record BinanceLinkEnableMarginResult
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    [JsonProperty("subaccountId")]
    public string SubAccountId { get; set; } = string.Empty;

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