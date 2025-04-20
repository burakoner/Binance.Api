using Binance.Api.Spot.Converters;

namespace Binance.Api.Spot.Responses;

/// <summary>
/// Symbol info
/// </summary>
public record BinanceSymbol
{
    /// <summary>
    /// The symbol
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The status of the symbol
    /// </summary>
    [JsonConverter(typeof(SymbolStatusConverter))]
    public BinanceSymbolStatus Status { get; set; }

    /// <summary>
    /// The base asset
    /// </summary>
    public string BaseAsset { get; set; } = "";

    /// <summary>
    /// The precision of the base asset
    /// </summary>
    public int BaseAssetPrecision { get; set; }

    /// <summary>
    /// The quote asset
    /// </summary>
    public string QuoteAsset { get; set; } = "";

    /// <summary>
    /// The precision of the quote asset
    /// </summary>
    [JsonProperty("quotePrecision")]
    public int QuoteAssetPrecision { get; set; }

    /// <summary>
    /// Allowed order types
    /// </summary>
    [JsonProperty(ItemConverterType = typeof(SpotOrderTypeConverter))]
    public IEnumerable<BinanceSpotOrderType> OrderTypes { get; set; } = [];

    /// <summary>
    /// Ice berg orders allowed
    /// </summary>
    public bool IceBergAllowed { get; set; }

    /// <summary>
    /// Cancel replace allowed
    /// </summary>
    public bool CancelReplaceAllowed { get; set; }

    /// <summary>
    /// Spot trading orders allowed
    /// </summary>
    public bool IsSpotTradingAllowed { get; set; }

    /// <summary>
    /// Trailling stop orders are allowed
    /// </summary>
    public bool AllowTrailingStop { get; set; }

    /// <summary>
    /// Margin trading orders allowed
    /// </summary>
    public bool IsMarginTradingAllowed { get; set; }

    /// <summary>
    /// If OCO(One Cancels Other) orders are allowed
    /// </summary>
    public bool OCOAllowed { get; set; }

    /// <summary>
    /// If OTO(One Triggers Other) orders are allowed
    /// </summary>
    public bool OTOAllowed { get; set; }

    /// <summary>
    /// Whether or not it is allowed to specify the quantity of a market order in the quote asset
    /// </summary>
    [JsonProperty("quoteOrderQtyMarketAllowed")]
    public bool QuoteOrderQuantityMarketAllowed { get; set; }

    /// <summary>
    /// The precision of the base asset fee
    /// </summary>
    [JsonProperty("baseCommissionPrecision")]
    public int BaseFeePrecision { get; set; }

    /// <summary>
    /// The precision of the quote asset fee
    /// </summary>
    [JsonProperty("quoteCommissionPrecision")]
    public int QuoteFeePrecision { get; set; }

    /// <summary>
    /// Permissions types
    /// </summary>
    public IEnumerable<BinancePermissionType> Permissions { get; set; } = [];

    /// <summary>
    /// Permission sets
    /// </summary>
    [JsonProperty("permissionSets")]
    [JsonConverter(typeof(BinancePermissionTypeConverter))]
    public IEnumerable<IEnumerable<BinancePermissionType>> PermissionSets { get; set; } = [];

    /// <summary>
    /// Default self trade prevention
    /// </summary>
    [JsonProperty("defaultSelfTradePreventionMode")]
    [JsonConverter(typeof(MapConverter))]
    public BinanceSelfTradePreventionMode DefaultSelfTradePreventionMode { get; set; }

    /// <summary>
    /// Allowed self trade prevention modes
    /// </summary>
    [JsonProperty("allowedSelfTradePreventionModes")]
    public IEnumerable<BinanceSelfTradePreventionMode> AllowedSelfTradePreventionModes { get; set; } = [];

    /// <summary>
    /// Filters for order on this symbol
    /// </summary>
    public IEnumerable<BinanceSymbolFilter> Filters { get; set; } = [];

    /// <summary>
    /// Filter for the max accuracy of the price for this symbol
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolPriceFilter? PriceFilter => Filters.OfType<BinanceSymbolPriceFilter>().FirstOrDefault();

    /// <summary>
    /// Filter for the maximum deviation of the price
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolPercentPriceFilter? PricePercentFilter => Filters.OfType<BinanceSymbolPercentPriceFilter>().FirstOrDefault();

    /// <summary>
    /// Filter for the maximum deviation of the price per side
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolPercentPriceBySideFilter? PricePercentByPriceFilter => Filters.OfType<BinanceSymbolPercentPriceBySideFilter>().FirstOrDefault();

    /// <summary>
    /// Filter for max accuracy of the quantity for this symbol
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolLotSizeFilter? LotSizeFilter => Filters.OfType<BinanceSymbolLotSizeFilter>().FirstOrDefault();

    /// <summary>
    /// Filter for max accuracy of the quantity for this symbol, specifically for market orders
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolMarketLotSizeFilter? MarketLotSizeFilter => Filters.OfType<BinanceSymbolMarketLotSizeFilter>().FirstOrDefault();

    /// <summary>
    /// Filter for the minimal quote quantity of an order for this symbol
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolMinNotionalFilter? MinNotionalFilter => Filters.OfType<BinanceSymbolMinNotionalFilter>().FirstOrDefault();

    /// <summary>
    /// Filter for the minimal quote quantity of an order for this symbol
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolNotionalFilter? NotionalFilter => Filters.OfType<BinanceSymbolNotionalFilter>().FirstOrDefault();

    /// <summary>
    /// Filter for max number of orders for this symbol
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolMaxOrdersFilter? MaxOrdersFilter => Filters.OfType<BinanceSymbolMaxOrdersFilter>().FirstOrDefault();

    /// <summary>
    /// Filter for max algorithmic orders for this symbol
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolMaxAlgorithmicOrdersFilter? MaxAlgorithmicOrdersFilter => Filters.OfType<BinanceSymbolMaxAlgorithmicOrdersFilter>().FirstOrDefault();

    /// <summary>
    /// Filter for max amount of iceberg parts for this symbol
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolIcebergPartsFilter? IceBergPartsFilter => Filters.OfType<BinanceSymbolIcebergPartsFilter>().FirstOrDefault();

    /// <summary>
    /// Filter for the maximum position on a symbol
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolMaxPositionFilter? MaxPositionFilter => Filters.OfType<BinanceSymbolMaxPositionFilter>().FirstOrDefault();

    /// <summary>
    /// Filter for the trailing delta values
    /// </summary>
    [JsonIgnore]
    public BinanceSymbolTrailingDeltaFilter? TrailingDeltaFilter => Filters.OfType<BinanceSymbolTrailingDeltaFilter>().FirstOrDefault();

    /// <summary>
    /// Filter for the trailing delta values
    /// </summary>
    [JsonIgnore]
    public BinanceMaxNumberOfIcebergOrdersFilter? MaxNumberOfIcebergOrdersFilter => Filters.OfType<BinanceMaxNumberOfIcebergOrdersFilter>().FirstOrDefault();
}
