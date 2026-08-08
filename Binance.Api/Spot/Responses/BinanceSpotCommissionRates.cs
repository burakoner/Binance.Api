namespace Binance.Api.Spot;

/// <summary>
/// Current commission rates for a symbol.
/// </summary>
public record BinanceSpotCommissionRates
{
    /// <summary>The symbol.</summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>Standard commission rates.</summary>
    public BinanceSpotCommissionRate StandardCommission { get; set; } = default!;

    /// <summary>Special commission rates.</summary>
    public BinanceSpotCommissionRate SpecialCommission { get; set; } = default!;

    /// <summary>Tax commission rates.</summary>
    public BinanceSpotCommissionRate TaxCommission { get; set; } = default!;

    /// <summary>Commission-discount information.</summary>
    public BinanceSpotCommissionDiscount Discount { get; set; } = default!;
}

/// <summary>
/// Maker, taker, buyer, and seller commission rates.
/// </summary>
public record BinanceSpotCommissionRate
{
    /// <summary>Maker rate.</summary>
    public decimal Maker { get; set; }

    /// <summary>Taker rate.</summary>
    public decimal Taker { get; set; }

    /// <summary>Buyer rate.</summary>
    public decimal Buyer { get; set; }

    /// <summary>Seller rate.</summary>
    public decimal Seller { get; set; }
}

/// <summary>
/// Discount applied when commission is paid in the discount asset.
/// </summary>
public record BinanceSpotCommissionDiscount
{
    /// <summary>Whether the discount is enabled for the account.</summary>
    public bool EnabledForAccount { get; set; }

    /// <summary>Whether the discount is enabled for the symbol.</summary>
    public bool EnabledForSymbol { get; set; }

    /// <summary>The asset used to pay discounted commission.</summary>
    public string DiscountAsset { get; set; } = string.Empty;

    /// <summary>The discount rate.</summary>
    public decimal Discount { get; set; }
}
