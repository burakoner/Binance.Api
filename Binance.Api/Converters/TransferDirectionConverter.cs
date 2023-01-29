using System.Collections.Generic;
using ApiSharp.Converters;
using Binance.ApiClient.Enums;

namespace Binance.ApiClient.Converters
{
    internal class TransferDirectionConverter: BaseConverter<TransferDirection>
    {
        public TransferDirectionConverter(): this(true) { }
        public TransferDirectionConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<TransferDirection, string>> Mapping => new List<KeyValuePair<TransferDirection, string>>
        {
            new KeyValuePair<TransferDirection, string>(TransferDirection.RollIn, "ROLL_IN"),
            new KeyValuePair<TransferDirection, string>(TransferDirection.RollOut, "ROLL_OUT"),
        };
    }
}
