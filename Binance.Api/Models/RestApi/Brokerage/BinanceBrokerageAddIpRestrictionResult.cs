namespace Binance.Api.Models.RestApi.Brokerage;

/// <summary>
/// Add IP Restriction Result
/// </summary>
public record BinanceBrokerageAddIpRestrictionResult
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    public string SubAccountId { get; set; } = "";

    /// <summary>
    /// Api key
    /// </summary>
    public string ApiKey { get; set; } = "";

    /// <summary>
    /// IP
    /// </summary>
    public string Ip { get; set; } = "";

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("updateTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateDate { get; set; }
}