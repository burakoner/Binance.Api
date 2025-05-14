namespace Binance.Api.SubAccount;

/// <summary>
/// Sub account details
/// </summary>
public record BinanceSubAccountBlvt
{
    /// <summary>
    /// The email associated with the sub account
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Blvt enabled
    /// </summary>
    [JsonProperty("enableBlvt")]
    public bool EnableBlvt { get; set; }
}
