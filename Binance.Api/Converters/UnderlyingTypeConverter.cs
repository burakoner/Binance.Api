using Binance.ApiClient.Enums;
using ApiSharp.Converters;
using System.Collections.Generic;

namespace Binance.ApiClient.Converters
{
    internal class UnderlyingTypeConverter : BaseConverter<UnderlyingType>
    {
        public UnderlyingTypeConverter() : this(true) { }
        public UnderlyingTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<UnderlyingType, string>> Mapping => new List<KeyValuePair<UnderlyingType, string>>
        {
            new KeyValuePair<UnderlyingType, string>(UnderlyingType.Coin, "COIN"),
            new KeyValuePair<UnderlyingType, string>(UnderlyingType.Index, "INDEX")
        };
    }
}
