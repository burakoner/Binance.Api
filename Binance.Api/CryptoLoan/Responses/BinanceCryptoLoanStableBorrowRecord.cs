namespace Binance.Api.CryptoLoan;

/// <summary>
/// Borrow record
/// </summary>
public record BinanceCryptoLoanStableBorrowRecord
{
    /// <summary>
    /// The loaning asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; } = string.Empty;

    /// <summary>
    /// The collateral asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public string CollateralAsset { get; set; } = string.Empty;

    /// <summary>
    /// The loan quantity
    /// </summary>
    [JsonProperty("initialLoanAmount")]
    public decimal InitialLoanQuantity { get; set; }

    /// <summary>
    /// The collateral quantity
    /// </summary>
    [JsonProperty("initialCollateralAmount")]
    public decimal InitialCollateralQuantity { get; set; }

    /// <summary>
    /// Hourly interest rate
    /// </summary>
    [JsonProperty("hourlyInterestRate")]
    public decimal HourlyInterestRate { get; set; }

    /// <summary>
    /// Loan term
    /// </summary>
    [JsonProperty("loanTerm")]
    public int LoanTerm { get; set; }

    /// <summary>
    /// Borrow order id
    /// </summary>
    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    /// <summary>
    /// Borrow timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("borrowTime")]
    public DateTime BorrowTime { get; set; }

    /// <summary>
    /// Status of the order
    /// </summary>
    [JsonProperty("status")]
    public BinanceCryptoLoanStableBorrowStatus Status { get; set; }
}
