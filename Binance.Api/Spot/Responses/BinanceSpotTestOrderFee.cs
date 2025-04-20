namespace Binance.Net.Objects.Models.Spot;

/// <summary>
/// Test order commission info
/// </summary>
public record BinanceSpotTestOrderFee
{
    /// <summary>
    /// Standard fee rates on trades from the order
    /// </summary>
    [JsonProperty("standardCommissionForOrder")]
    public BinanceFee StandardFeeForOrder { get; set; } = null!;

    /// <summary>
    /// Tax fee rates on trades from the order
    /// </summary>
    [JsonProperty("taxCommissionForOrder")]
    public BinanceFee TaxFeeForOrder { get; set; } = null!;

    /// <summary>
    /// Discount info
    /// </summary>
    public BinanceDiscount Discount { get; set; } = null!;
}

/// <summary>
/// Fee rates
/// </summary>
public record BinanceFee
{
    /// <summary>
    /// Maker fee
    /// </summary>
    public decimal Maker { get; set; }

    /// <summary>
    /// Taker fee
    /// </summary>
    public decimal Taker { get; set; }
}

/// <summary>
/// Discount info
/// </summary>
public record BinanceDiscount
{
    /// <summary>
    /// Is discount enabled for the account
    /// </summary>
    public bool EnabledForAccount { get; set; }

    /// <summary>
    /// Is discount enabled for the symbol
    /// </summary>
    public bool EnabledForSymbol { get; set; }

    /// <summary>
    /// The discount asset
    /// </summary>
    public string DiscountAsset { get; set; } = "";

    /// <summary>
    /// Discount rate
    /// </summary>
    public decimal Discount { get; set; }
}
