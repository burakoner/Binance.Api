namespace Binance.Api.Models.RestApi.Account;

/// <summary>
/// Deposit address info
/// </summary>
public record BinanceDepositAddress
{
    /// <summary>
    /// The deposit address
    /// </summary>
    public string Address { get; set; } = "";
    /// <summary>
    /// Url
    /// </summary>
    public string Url { get; set; } = "";
    /// <summary>
    /// Address tag
    /// </summary>
    public string Tag { get; set; } = "";
    /// <summary>
    /// Asset the address is for
    /// </summary>
    [JsonProperty("coin")]
    public string Asset { get; set; } = "";
}
