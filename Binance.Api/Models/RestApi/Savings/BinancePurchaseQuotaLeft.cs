namespace Binance.Api.Models.RestApi.Savings;

/// <summary>
/// Purchase quota left
/// </summary>
public record BinancePurchaseQuotaLeft
{
    /// <summary>
    /// The asset
    /// </summary>
    public string Asset { get; set; } = "";

    /// <summary>
    /// The quota left
    /// </summary>
    public decimal LeftQuota { get; set; }
}
