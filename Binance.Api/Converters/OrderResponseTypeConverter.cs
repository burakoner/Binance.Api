using System.Collections.Generic;
using ApiSharp.Converters;
using Binance.ApiClient.Enums;

namespace Binance.ApiClient.Converters
{
    internal class OrderResponseTypeConverter: BaseConverter<OrderResponseType>
    {
        public OrderResponseTypeConverter(): this(true) { }
        public OrderResponseTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OrderResponseType, string>> Mapping => new List<KeyValuePair<OrderResponseType, string>>
        {
            new KeyValuePair<OrderResponseType, string>(OrderResponseType.Acknowledge, "ACK"),
            new KeyValuePair<OrderResponseType, string>(OrderResponseType.Result, "RESULT"),
            new KeyValuePair<OrderResponseType, string>( OrderResponseType.Full, "FULL")
        };
    }
}
