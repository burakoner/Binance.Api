using System.Collections.Generic;
using Binance.ApiClient.Enums;
using ApiSharp.Converters;

namespace Binance.ApiClient.Converters
{
    internal class IndicatorTypeConverter : BaseConverter<IndicatorType>
    {
        public IndicatorTypeConverter() : this(true) { }
        public IndicatorTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<IndicatorType, string>> Mapping => new List<KeyValuePair<IndicatorType, string>>
        {
            new KeyValuePair<IndicatorType, string>(IndicatorType.CancelationRatio, "GCR"),
            new KeyValuePair<IndicatorType, string>(IndicatorType.UnfilledRatio, "UFR"),
            new KeyValuePair<IndicatorType, string>(IndicatorType.ExpirationRatio, "IFER")
        };
    }
}
