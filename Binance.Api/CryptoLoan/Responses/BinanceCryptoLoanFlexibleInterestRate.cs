namespace Binance.Api.CryptoLoan;

/// <summary>
/// Crypto Loan Interest Rate History Record
/// </summary>
public record BinanceCryptoLoanFlexibleInterestRate
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("coin")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Annualized Interest Rate
    /// </summary>
    public decimal AnnualizedInterestRate { get; set; }

    /// <summary>
    /// Time
    /// </summary>
    public DateTime Time { get; set; }
}
