namespace Binance.Api.SubAccount;

/// <summary>
/// Sub account details
/// </summary>
public record BinanceSubAccountEmail
{
    /// <summary>
    /// The email associated with the sub account
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;
}
