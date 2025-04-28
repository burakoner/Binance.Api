namespace Binance.Api.Shared;

internal class BinanceSymbolFilterTypeConverter : BaseConverter<BinanceSymbolFilterType>
{
    public BinanceSymbolFilterTypeConverter() : this(true) { }
    public BinanceSymbolFilterTypeConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BinanceSymbolFilterType, string>> Mapping => new List<KeyValuePair<BinanceSymbolFilterType, string>>
    {
        new KeyValuePair<BinanceSymbolFilterType, string>(BinanceSymbolFilterType.LotSize, "LOT_SIZE"),
        new KeyValuePair<BinanceSymbolFilterType, string>(BinanceSymbolFilterType.MarketLotSize, "MARKET_LOT_SIZE"),
        new KeyValuePair<BinanceSymbolFilterType, string>(BinanceSymbolFilterType.MinNotional, "MIN_NOTIONAL"),
        new KeyValuePair<BinanceSymbolFilterType, string>(BinanceSymbolFilterType.Price, "PRICE_FILTER"),
        new KeyValuePair<BinanceSymbolFilterType, string>(BinanceSymbolFilterType.PricePercent, "PERCENT_PRICE"),
        new KeyValuePair<BinanceSymbolFilterType, string>(BinanceSymbolFilterType.IcebergParts, "ICEBERG_PARTS"),
        new KeyValuePair<BinanceSymbolFilterType, string>(BinanceSymbolFilterType.MaxNumberOrders, "MAX_NUM_ORDERS"),
        new KeyValuePair<BinanceSymbolFilterType, string>(BinanceSymbolFilterType.MaxNumberAlgorithmicOrders, "MAX_NUM_ALGO_ORDERS"),
        new KeyValuePair<BinanceSymbolFilterType, string>(BinanceSymbolFilterType.MaxPosition, "MAX_POSITION"),
        new KeyValuePair<BinanceSymbolFilterType, string>(BinanceSymbolFilterType.PercentagePriceBySide, "PERCENT_PRICE_BY_SIDE"),
        new KeyValuePair<BinanceSymbolFilterType, string>(BinanceSymbolFilterType.TrailingDelta, "TRAILING_DELTA"),
        new KeyValuePair<BinanceSymbolFilterType, string>(BinanceSymbolFilterType.Notional, "NOTIONAL"),
    };
}
