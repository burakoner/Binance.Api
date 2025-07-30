namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Transaction Id
/// </summary>
public record BinancePortfolioMarginTransactionId
{
    /// <summary>
    /// Transaction Id
    /// </summary>
    [JsonProperty("tranId")]
    public long TransactionId { get; set; }
} 