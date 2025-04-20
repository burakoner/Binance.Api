namespace Binance.Api.Models.RestApi.Account;

/// <summary>
/// IP restriction info
/// </summary>
public record BinanceIpRestriction
{
    /// <summary>
    /// Is currently restricted
    /// </summary>
    [JsonProperty("ipRestrict")]
    public bool IpRestricted { get; set; }
    /// <summary>
    /// Ip whitelist
    /// </summary>
    public IEnumerable<string> IpList { get; set; } = [];
    /// <summary>
    /// Update Time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateTime { get; set; }
    /// <summary>
    /// The API key
    /// </summary>
    public string ApiKey { get; set; } = "";
}
