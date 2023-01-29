namespace Binance.Api.Models.RestApi.CryptoLoans;

/// <summary>
/// Borrow info
/// </summary>
public class BinanceCryptoLoanBorrow
{
    /// <summary>
    /// The loaning asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; }
    /// <summary>
    /// The collateral asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public string CollateralAsset { get; set; }
    /// <summary>
    /// The loan quantity
    /// </summary>
    [JsonProperty("loanAmount")]
    public decimal LoanQuantity { get; set; }
    /// <summary>
    /// The collateral quantity
    /// </summary>
    [JsonProperty("collateralAmount")]
    public decimal CollateralQuantity { get; set; }
    /// <summary>
    /// Hourly interest rate
    /// </summary>
    public decimal HourlyInterestRate { get; set; }
    /// <summary>
    /// Borrow order id
    /// </summary>
    public long OrderId { get; set; }
}
