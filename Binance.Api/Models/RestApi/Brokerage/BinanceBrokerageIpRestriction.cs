namespace Binance.Api.Models.RestApi.Brokerage;

/// <summary>
/// IP Restriction
/// </summary>
public record BinanceBrokerageIpRestrictionBase
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
    /// IP list
    /// </summary>
    public IEnumerable<string> IpList { get; set; } = [];

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("updateTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateDate { get; set; }
}

/// <summary>
/// IP Restriction
/// </summary>
public record BinanceBrokerageIpRestriction : BinanceBrokerageIpRestrictionBase
{
    /// <summary>
    /// Ip Restrict
    /// </summary>
    public bool IpRestrict { get; set; }
}