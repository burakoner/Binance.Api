namespace Binance.Api.PortfolioMargin;

public record BinancePortfolioMarginTransactionId
{
    [JsonProperty("tranId")]
    public long TransactionId { get; set; }
} 