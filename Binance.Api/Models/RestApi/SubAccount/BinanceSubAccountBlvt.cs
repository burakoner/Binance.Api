namespace Binance.Api.Models.RestApi.SubAccount;

/// <summary>
/// Sub account details
/// </summary>
public class BinanceSubAccountBlvt
{
    /// <summary>
    /// The email associated with the sub account
    /// </summary>
    public string Email { get; set; }
    /// <summary>
    /// Blvt enabled
    /// </summary>
    public bool EnableBlvt { get; set; }
}
