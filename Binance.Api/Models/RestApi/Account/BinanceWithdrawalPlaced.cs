namespace Binance.Api.Models.RestApi.Account;

/// <summary>
/// Result of placing a withdrawal
/// </summary>
public record BinanceWithdrawalPlaced
{
    /// <summary>
    /// The id
    /// </summary>
    public string Id { get; set; } = "";
}
