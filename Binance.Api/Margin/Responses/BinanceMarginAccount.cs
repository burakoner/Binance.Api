namespace Binance.Api.Margin;

/// <summary>
/// Information about margin account
/// </summary>
public record BinanceMarginAccount
{
    /// <summary>
    /// Boolean indicating if this account can borrow
    /// </summary>
    public bool BorrowEnabled { get; set; }

    /// <summary>
    /// Boolean indicating if this account can trade
    /// </summary>
    public bool TradeEnabled { get; set; }

    /// <summary>
    /// Boolean indicating if this account can transfer
    /// </summary>
    public bool TransferEnabled { get; set; }

    /// <summary>
    /// Aggregate level of margin
    /// </summary>
    public decimal MarginLevel { get; set; }

    /// <summary>
    /// Aggregate total balance as BTC
    /// </summary>
    public decimal TotalAssetOfBtc { get; set; }

    /// <summary>
    /// Aggregate total liability balance of BTC
    /// </summary>
    public decimal TotalLiabilityOfBtc { get; set; }

    /// <summary>
    /// Aggregate total available net balance of BTC
    /// </summary>
    public decimal TotalNetAssetOfBtc { get; set; }

    /// <summary>
    /// Account type
    /// </summary>
    public string AccountType { get; set; } = string.Empty;

    /// <summary>
    /// Balance list
    /// </summary>
    [JsonProperty("userAssets")]
    public List<BinanceMarginAccountBalance> Balances { get; set; } = [];
}