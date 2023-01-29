using System.Collections.Generic;
using ApiSharp.Converters;
using Binance.ApiClient.Enums;

namespace Binance.ApiClient.Converters
{
    internal class SideEffectTypeConverter: BaseConverter<SideEffectType>
    {
        public SideEffectTypeConverter(): this(true) { }
        public SideEffectTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<SideEffectType, string>> Mapping => new List<KeyValuePair<SideEffectType, string>>
        {
            new KeyValuePair<SideEffectType, string>(SideEffectType.NoSideEffect, "NO_SIDE_EFFECT"),
            new KeyValuePair<SideEffectType, string>(SideEffectType.MarginBuy, "MARGIN_BUY"),
            new KeyValuePair<SideEffectType, string>(SideEffectType.AutoRepay, "AUTO_REPAY"),
        };
    }
}
