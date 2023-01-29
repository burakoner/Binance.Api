using System.Collections.Generic;
using Binance.ApiClient.Enums;
using ApiSharp.Converters;

namespace Binance.ApiClient.Converters
{
    internal class SystemStatusConverter : BaseConverter<SystemStatus>
    {
        public SystemStatusConverter() : this(true)
        {
        }

        public SystemStatusConverter(bool quotes) : base(quotes)
        {
        }

        protected override List<KeyValuePair<SystemStatus, string>> Mapping => new List<KeyValuePair<SystemStatus, string>>
        {
            new KeyValuePair<SystemStatus, string>(SystemStatus.Normal, "0"),
            new KeyValuePair<SystemStatus, string>(SystemStatus.Maintenance, "1")
        };
    }
}
