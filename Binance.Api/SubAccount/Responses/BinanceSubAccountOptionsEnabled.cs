namespace Binance.Api.SubAccount;

/// <summary>
/// Sub account Options trading enabled
/// </summary>
public record BinanceSubAccountOptionsEnabled
{
    /// <summary>
    /// Email of the account
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Whether Options trading is enabled
    /// </summary>
    [JsonProperty("isEOptionsEnabled")]
    public bool IsOptionsEnabled { get; set; }
}
