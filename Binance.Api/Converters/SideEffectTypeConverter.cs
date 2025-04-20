using Binance.Api.Margin.Enums;

namespace Binance.Api.Converters;

internal class SideEffectTypeConverter : BaseConverter<BinanceSideEffectType>
{
    public SideEffectTypeConverter() : this(true) { }
    public SideEffectTypeConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BinanceSideEffectType, string>> Mapping => new List<KeyValuePair<BinanceSideEffectType, string>>
    {
        new KeyValuePair<BinanceSideEffectType, string>(BinanceSideEffectType.NoSideEffect, "NO_SIDE_EFFECT"),
        new KeyValuePair<BinanceSideEffectType, string>(BinanceSideEffectType.MarginBuy, "MARGIN_BUY"),
        new KeyValuePair<BinanceSideEffectType, string>(BinanceSideEffectType.AutoRepay, "AUTO_REPAY"),
    };
}
