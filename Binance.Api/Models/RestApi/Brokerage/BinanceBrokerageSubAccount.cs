namespace Binance.Api.Models.RestApi.Brokerage;

/// <summary>
/// Sub Account
/// </summary>
public record BinanceBrokerageSubAccount : BinanceBrokerageSubAccountCommission
{
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = "";

    /// <summary>
    /// Tag
    /// </summary>
    public string Tag { get; set; } = "";

    /// <summary>
    /// Create Date
    /// </summary>
    [JsonProperty("createTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateDate { get; set; }
}