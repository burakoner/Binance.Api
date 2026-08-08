namespace Binance.Api.Spot;

/// <summary>
/// Exchange, symbol, and asset filters relevant to an account for a symbol.
/// </summary>
public record BinanceSpotAccountFilters
{
    /// <summary>Account-level exchange filters.</summary>
    public List<BinanceSpotAccountExchangeFilter> ExchangeFilters { get; set; } = [];

    /// <summary>Symbol filters.</summary>
    public List<BinanceSymbolFilter> SymbolFilters { get; set; } = [];

    /// <summary>Account asset filters.</summary>
    public List<BinanceSpotAccountAssetFilter> AssetFilters { get; set; } = [];
}

/// <summary>
/// An exchange-wide account order-count filter.
/// </summary>
public record BinanceSpotAccountExchangeFilter
{
    /// <summary>The official filter type.</summary>
    public string FilterType { get; set; } = string.Empty;

    /// <summary>Maximum number of open orders, when applicable.</summary>
    public long? MaxNumOrders { get; set; }

    /// <summary>Maximum number of open algorithmic orders, when applicable.</summary>
    public long? MaxNumAlgoOrders { get; set; }

    /// <summary>Maximum number of open iceberg orders, when applicable.</summary>
    public long? MaxNumIcebergOrders { get; set; }

    /// <summary>Maximum number of open order lists, when applicable.</summary>
    public long? MaxNumOrderLists { get; set; }
}

/// <summary>
/// An account-level maximum-asset filter.
/// </summary>
public record BinanceSpotAccountAssetFilter
{
    /// <summary>The official filter type.</summary>
    public string FilterType { get; set; } = string.Empty;

    /// <summary>The decimal exponent used for quantities.</summary>
    [JsonProperty("qtyExponent")]
    public int? QuantityExponent { get; set; }

    /// <summary>The asset.</summary>
    public string Asset { get; set; } = string.Empty;

    /// <summary>The maximum quantity or notional value.</summary>
    public decimal Limit { get; set; }
}
