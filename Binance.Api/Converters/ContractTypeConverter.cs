using Binance.Api.Futures.Enums;

namespace Binance.Api.Converters;

internal class ContractTypeConverter : BaseConverter<BinanceContractType>
{
    public ContractTypeConverter() : this(true) { }
    public ContractTypeConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BinanceContractType, string>> Mapping => new List<KeyValuePair<BinanceContractType, string>>
    {
        new KeyValuePair<BinanceContractType, string>(BinanceContractType.Perpetual, "PERPETUAL"),
        new KeyValuePair<BinanceContractType, string>(BinanceContractType.CurrentMonth, "CURRENT_MONTH"),
        new KeyValuePair<BinanceContractType, string>(BinanceContractType.CurrentQuarter, "CURRENT_QUARTER"),
        new KeyValuePair<BinanceContractType, string>(BinanceContractType.CurrentQuarterDelivering, "CURRENT_QUARTER DELIVERING"),
        new KeyValuePair<BinanceContractType, string>(BinanceContractType.NextQuarter, "NEXT_QUARTER"),
        new KeyValuePair<BinanceContractType, string>(BinanceContractType.NextQuarterDelivering, "NEXT_QUARTER DELIVERING"),
        new KeyValuePair<BinanceContractType, string>(BinanceContractType.NextMonth, "NEXT_MONTH"),
        new KeyValuePair<BinanceContractType, string>(BinanceContractType.Unknown, ""),
    };
}
