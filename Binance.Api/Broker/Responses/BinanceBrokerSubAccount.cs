namespace Binance.Api.Broker;

/// <summary>
/// Sub Account
/// </summary>
public record BinanceBrokerSubAccount : BinanceBrokerSubAccountCommission
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