namespace Binance.Api.Options;

/// <summary>
/// Symbol info
/// </summary>
public record BinanceOptionsSymbol
{
    /// <summary>
    /// Trading pair name
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// expiry time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime ExpiryDate { get; set; }

    /// <summary>
    /// Direction
    /// </summary>
    public BinanceOptionsSide Side { get; set; }

    /// <summary>
    /// Strike price
    /// </summary>
    public decimal StrikePrice { get; set; }

    /// <summary>
    /// Underlying asset of the contract
    /// </summary>
    public string Underlying { get; set; } = "";

    /// <summary>
    /// Contract unit, the quantity of the underlying asset represented by a single contract.
    /// </summary>
    public long Unit { get; set; }

    /// <summary>
    /// Liquidation commission rate
    /// </summary>
    public decimal LiquidationFeeRate { get; set; }

    /// <summary>
    /// Minimum order quantity
    /// </summary>
    [JsonProperty("minQty")]
    public decimal MinimumQuantity { get; set; }

    /// <summary>
    /// Maximum order quantity
    /// </summary>
    [JsonProperty("maxQty")]
    public decimal MaximumQuantity { get; set; }

    /// <summary>
    /// Initial Magin Ratio
    /// </summary>
    public decimal InitialMargin { get; set; }

    /// <summary>
    /// Maintenance Margin Ratio
    /// </summary>
    public decimal MaintenanceMargin { get; set; }

    /// <summary>
    /// Min Initial Margin Ratio
    /// </summary>
    [JsonProperty("minInitialMargin")]
    public decimal MinimumInitialMargin { get; set; }

    /// <summary>
    /// Min Maintenance Margin Ratio
    /// </summary>
    [JsonProperty("minMaintenanceMargin")]
    public decimal MinimumMaintenanceMargin { get; set; }

    /// <summary>
    /// price precision
    /// </summary>
    public long PriceScale { get; set; }

    /// <summary>
    /// quantity precision
    /// </summary>
    public long QuantityScale { get; set; }

    /// <summary>
    /// The quote asset
    /// </summary>
    public string QuoteAsset { get; set; } = "";

    /// <summary>
    /// Contract type
    /// </summary>
    public string ContractType { get; set; } = "";

    /// <summary>
    /// Underlying type
    /// </summary>
    public string UnderlyingType { get; set; } = "";

    /// <summary>
    /// Whether naked selling is allowed
    /// </summary>
    public bool NakedSell { get; set; }

    /// <summary>
    /// Trading status
    /// </summary>
    public string Status { get; set; } = "";

    /// <summary>
    /// Filters for order on this symbol
    /// </summary>
    public List<BinanceOptionsSymbolFilter> Filters { get; set; } = [];

}
