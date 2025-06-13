namespace Binance.Api.Link;

/// <summary>
/// Sub Account
/// </summary>
public record BinanceLinkSubAccount : BinanceLinkSubAccountCommission
{
    /// <summary>
    /// Email
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Tag
    /// </summary>
    [JsonProperty("tag")]
    public string Tag { get; set; } = string.Empty;

    /// <summary>
    /// Create Date
    /// </summary>
    [JsonProperty("createTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateDate { get; set; }
}