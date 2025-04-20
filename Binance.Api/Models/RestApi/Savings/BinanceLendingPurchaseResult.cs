namespace Binance.Api.Models.RestApi.Savings;

/// <summary>
/// Purchase result
/// </summary>
public record BinanceLendingPurchaseResult
{
    /// <summary>
    /// The id of the purchase
    /// </summary>
    public int PurchaseId { get; set; }
}
