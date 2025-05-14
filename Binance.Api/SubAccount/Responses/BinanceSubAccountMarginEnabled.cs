namespace Binance.Api.SubAccount;

/// <summary>
/// Sub account margin trading enabled
/// </summary>
public record BinanceSubAccountMarginEnabled
{
    /// <summary>
    /// Email of the account
    /// </summary>
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Whether Margin trading is enabled
    /// </summary>
    [JsonProperty("isMarginEnabled")]
    public bool IsMarginEnabled { get; set; }
}
