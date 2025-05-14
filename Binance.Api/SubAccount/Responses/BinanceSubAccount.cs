namespace Binance.Api.SubAccount;

/// <summary>
/// Sub account details
/// </summary>
public record BinanceSubAccount
{
    /// <summary>
    /// The email associated with the sub account
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Is account frozen
    /// </summary>
    [JsonProperty("isFreeze")]
    public bool IsFreeze { get; set; } = false;

    /// <summary>
    /// The time the sub account was created
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("createTime")]
    public DateTime CreateTime { get; set; }
}
