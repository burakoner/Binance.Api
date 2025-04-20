namespace Binance.Api.Models.RestApi.SubAccount;

/// <summary>
/// Sub account futures trading enabled
/// </summary>
public record BinanceSubAccountFuturesEnabled
{
    /// <summary>
    /// Email of the account
    /// </summary>
    public string Email { get; set; } = "";
    /// <summary>
    /// Whether futures trading is enabled
    /// </summary>
    public bool IsFuturesEnabled { get; set; }
}
