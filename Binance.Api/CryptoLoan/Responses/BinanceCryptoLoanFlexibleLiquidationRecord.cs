namespace Binance.Api.CryptoLoan;

/// <summary>
/// Liquidation History
/// </summary>
public record BinanceCryptoLoanFlexibleLiquidationRecord
{
    /// <summary>
    /// The loaning asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; } = string.Empty;

    /// <summary>
    /// Liquidation Debt
    /// </summary>
    [JsonProperty("liquidationDebt")]
    public decimal LiquidationDebt { get; set; }

    /// <summary>
    /// The collateral asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public string CollateralAsset { get; set; } = string.Empty;

    /// <summary>
    /// Liquidation Collateral Quantity
    /// </summary>
    [JsonProperty("liquidationCollateralAmount")]
    public decimal LiquidationCollateralQuantity { get; set; }

    /// <summary>
    /// Return Collateral Quantity
    /// </summary>
    [JsonProperty("returnCollateralAmount")]
    public decimal ReturnCollateralQuantity { get; set; }

    /// <summary>
    /// Liquidation Fee
    /// </summary>
    [JsonProperty("liquidationFee")]
    public decimal LiquidationFee { get; set; }

    /// <summary>
    /// Liquidation Starting Price
    /// </summary>
    [JsonProperty("liquidationStartingPrice")]
    public decimal LiquidationStartingPrice { get; set; }

    /// <summary>
    /// Liquidation Starting Time
    /// </summary>
    [JsonProperty("liquidationStartingTime")]
    public DateTime LiquidationStartingTime { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public BinanceCryptoLoanFlexibleLiquidationStatus Status { get; set; }
}
