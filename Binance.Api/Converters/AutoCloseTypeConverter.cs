﻿namespace Binance.Api.Converters;

internal class AutoCloseTypeConverter : BaseConverter<AutoCloseType>
{
    public AutoCloseTypeConverter() : this(true) { }
    public AutoCloseTypeConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<AutoCloseType, string>> Mapping => new List<KeyValuePair<AutoCloseType, string>>
    {
        new KeyValuePair<AutoCloseType, string>(AutoCloseType.ADL, "ADL"),
        new KeyValuePair<AutoCloseType, string>(AutoCloseType.Liquidation, "LIQUIDATION")
    };
}
