using Binance.Net.Enums;
using Binance.Net.Objects.Models.Spot;

namespace Binance.Net.Objects.Models.Futures
{
    /// <summary>
    /// Information about a futures symbol
    /// </summary>
    public record BinanceFuturesSymbol
    {
        /// <summary>
        /// Filters for order on this symbol
        /// </summary>
        [JsonProperty("filters")]
        public IEnumerable<BinanceSymbolFilter> Filters { get; set; } = Array.Empty<BinanceSymbolFilter>();
        /// <summary>
        /// Contract type
        /// </summary>
        [JsonProperty("contractType")]
        public ContractType? ContractType { get; set; }
        /// <summary>
        /// The maintenance margin percent
        /// </summary>
        [JsonProperty("maintMarginPercent")]
        public decimal MaintMarginPercent { get; set; }
        /// <summary>
        /// The price Precision
        /// </summary>
        [JsonProperty("pricePrecision")]
        public int PricePrecision { get; set; }
        /// <summary>
        /// The quantity precision
        /// </summary>
        [JsonProperty("quantityPrecision")]
        public int QuantityPrecision { get; set; }
        /// <summary>
        /// The required margin percentage
        /// </summary>
        [JsonProperty("requiredMarginPercent")]
        public decimal RequiredMarginPercent { get; set; }
        /// <summary>
        /// The base asset
        /// </summary>
        [JsonProperty("baseAsset")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// Margin asset
        /// </summary>
        [JsonProperty("marginAsset")]
        public string MarginAsset { get; set; } = string.Empty;
        /// <summary>
        /// The quote asset
        /// </summary>
        [JsonProperty("quoteAsset")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// The precision of the base asset
        /// </summary>
        [JsonProperty("baseAssetPrecision")]
        public int BaseAssetPrecision { get; set; }
        /// <summary>
        /// The precision of the quote asset
        /// </summary>
        [JsonProperty("quotePrecision")]
        public int QuoteAssetPrecision { get; set; }
        /// <summary>
        /// Allowed order types
        /// </summary>
        [JsonProperty("orderTypes")]
        public IEnumerable<FuturesOrderType> OrderTypes { get; set; } = Array.Empty<FuturesOrderType>();
        /// <summary>
        /// The symbol
        /// </summary>
        [JsonProperty("symbol")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Pair
        /// </summary>
        [JsonProperty("pair")]
        public string Pair { get; set; } = string.Empty;
        /// <summary>
        /// Delivery Date
        /// </summary>
        [JsonProperty("deliveryDate"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime DeliveryDate { get; set; }
        /// <summary>
        /// Delivery Date
        /// </summary>
        [JsonProperty("onboardDate"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime ListingDate { get; set; }
        /// <summary>
        /// Trigger protect
        /// </summary>
        [JsonProperty("triggerProtect")]
        public decimal TriggerProtect { get; set; }
        /// <summary>
        /// Currently Empty
        /// </summary>
        [JsonProperty("underlyingType")]
        public UnderlyingType UnderlyingType { get; set; }
        /// <summary>
        /// Sub types
        /// </summary>
        [JsonProperty("underlyingSubType")]
        public IEnumerable<string> UnderlyingSubType { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Liquidation fee
        /// </summary>
        [JsonProperty("liquidationFee")]
        public decimal LiquidationFee { get; set; }
        /// <summary>
        /// The max price difference rate (from mark price) a market order can make
        /// </summary>
        [JsonProperty("marketTakeBound")]
        public decimal MarketTakeBound { get; set; }

        /// <summary>
        /// Allowed order time in force
        /// </summary>
        [JsonProperty("timeInForce")]
        public IEnumerable<TimeInForce> TimeInForce { get; set; } = Array.Empty<TimeInForce>();
        /// <summary>
        /// Filter for the max accuracy of the price for this symbol
        /// </summary>
        [JsonIgnore]
        public BinanceSymbolPriceFilter? PriceFilter => Filters.OfType<BinanceSymbolPriceFilter>().FirstOrDefault();
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
        /// Filter for max number of orders for this symbol
        /// </summary>
        [JsonIgnore]
        public BinanceSymbolMaxOrdersFilter? MaxOrdersFilter => Filters.OfType<BinanceSymbolMaxOrdersFilter>().FirstOrDefault();

        /// <summary>
        /// Filter for max number of orders for this symbol
        /// </summary>
        [JsonIgnore]
        public BinanceSymbolMaxAlgorithmicOrdersFilter? MaxAlgoOrdersFilter => Filters.OfType<BinanceSymbolMaxAlgorithmicOrdersFilter>().FirstOrDefault();

        /// <summary>
        /// Filter for the maximum deviation of the price
        /// </summary>
        [JsonIgnore]
        public BinanceSymbolPercentPriceFilter? PricePercentFilter => Filters.OfType<BinanceSymbolPercentPriceFilter>().FirstOrDefault();

        /// <summary>
        /// Filter for the maximum deviation of the price
        /// </summary>
        [JsonIgnore]
        public BinanceSymbolMinNotionalFilter? MinNotionalFilter => Filters.OfType<BinanceSymbolMinNotionalFilter>().FirstOrDefault();
    }

    /// <summary>
    /// Information about a futures symbol
    /// </summary>
    public record BinanceFuturesUsdtSymbol: BinanceFuturesSymbol
    {
        /// <summary>
        /// The status of the symbol
        /// </summary>
        [JsonProperty("status")]
        public SymbolStatus Status { get; set; }

        /// <summary>
        /// The status of the symbol
        /// </summary>
        [JsonProperty("settlePlan")]
        public decimal SettlePlan { get; set; }
    }

    /// <summary>
    /// Information about a futures symbol
    /// </summary>
    public record BinanceFuturesCoinSymbol: BinanceFuturesSymbol
    {

        /// <summary>
        /// The status of the symbol
        /// </summary>
        [JsonProperty("contractStatus")]
        public SymbolStatus Status { get; set; }

        /// <summary>
        /// Contract size
        /// </summary>
        [JsonProperty("contractSize")]
        public int ContractSize { get; set; }

        /// <summary>
        /// Equal quantity precision
        /// </summary>
        [JsonProperty("equalQtyPrecision")]
        public int EqualQuantityPrecision { get; set; }
       
    }
}
