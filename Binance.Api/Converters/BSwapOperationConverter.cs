using Binance.ApiClient.Enums;
using ApiSharp.Converters;
using System.Collections.Generic;

namespace Binance.ApiClient.Converters
{
    internal class BSwapOperationConverter : BaseConverter<BSwapOperation>
    {
        public BSwapOperationConverter() : this(true) { }
        public BSwapOperationConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BSwapOperation, string>> Mapping => new List<KeyValuePair<BSwapOperation, string>>
        {
            new KeyValuePair<BSwapOperation, string>(BSwapOperation.Add, "ADD"),
            new KeyValuePair<BSwapOperation, string>(BSwapOperation.Remove, "REMOVE")
        };
    }
}
