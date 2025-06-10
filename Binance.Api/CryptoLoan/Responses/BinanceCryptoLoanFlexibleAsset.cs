namespace Binance.Api.CryptoLoan;

/// <summary>
/// Loanable asset info
/// </summary>
public record BinanceCryptoLoanFlexibleAsset
{
    /// <summary>
    /// Loan asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; } = string.Empty;

    /// <summary>
    /// Flexible Interest Rate
    /// </summary>
    public decimal FlexibleInterestRate { get; set; }

    /// <summary>
    /// Flexible Minimum Limit
    /// </summary>
    [JsonProperty("flexibleMinLimit")]
    public decimal FlexibleMinimumLimit { get; set; }

    /// <summary>
    /// Flexible Maximum Limit
    /// </summary>
    [JsonProperty("flexibleMaxLimit")]
    public decimal FlexibleMaximumLimit { get; set; }
}
