using Binance.Api.Futures;

namespace Binance.Api.Converters;

internal class ContractTypeConverter : BaseConverter<BinanceFuturesContractType>
{
    public ContractTypeConverter() : this(true) { }
    public ContractTypeConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BinanceFuturesContractType, string>> Mapping => new List<KeyValuePair<BinanceFuturesContractType, string>>
    {
        new KeyValuePair<BinanceFuturesContractType, string>(BinanceFuturesContractType.Perpetual, "PERPETUAL"),
        new KeyValuePair<BinanceFuturesContractType, string>(BinanceFuturesContractType.CurrentMonth, "CURRENT_MONTH"),
        new KeyValuePair<BinanceFuturesContractType, string>(BinanceFuturesContractType.CurrentQuarter, "CURRENT_QUARTER"),
        new KeyValuePair<BinanceFuturesContractType, string>(BinanceFuturesContractType.CurrentQuarterDelivering, "CURRENT_QUARTER DELIVERING"),
        new KeyValuePair<BinanceFuturesContractType, string>(BinanceFuturesContractType.NextQuarter, "NEXT_QUARTER"),
        new KeyValuePair<BinanceFuturesContractType, string>(BinanceFuturesContractType.NextQuarterDelivering, "NEXT_QUARTER DELIVERING"),
        new KeyValuePair<BinanceFuturesContractType, string>(BinanceFuturesContractType.NextMonth, "NEXT_MONTH"),
        new KeyValuePair<BinanceFuturesContractType, string>(BinanceFuturesContractType.Unknown, ""),
    };
}
