namespace Binance.Api.Models.RestApi.SubAccount;

/// <summary>
/// Sub account details
/// </summary>
public record BinanceSubAccountEmail
{
    /// <summary>
    /// The email associated with the sub account
    /// </summary>
    public string Email { get; set; } = "";
}
