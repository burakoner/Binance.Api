using System.Collections.Generic;
using Binance.ApiClient.Enums;
using ApiSharp.Converters;

namespace Binance.ApiClient.Converters
{
    internal class IsolatedMarginTransferDirectionConverter : BaseConverter<IsolatedMarginTransferDirection>
    {
        public IsolatedMarginTransferDirectionConverter() : this(true) { }
        public IsolatedMarginTransferDirectionConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<IsolatedMarginTransferDirection, string>> Mapping => new List<KeyValuePair<IsolatedMarginTransferDirection, string>>
        {
            new KeyValuePair<IsolatedMarginTransferDirection, string>(IsolatedMarginTransferDirection.Spot, "SPOT"),
            new KeyValuePair<IsolatedMarginTransferDirection, string>(IsolatedMarginTransferDirection.IsolatedMargin, "ISOLATED_MARGIN"),
        };
    }
}
