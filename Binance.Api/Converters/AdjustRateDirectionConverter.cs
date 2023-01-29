﻿using System.Collections.Generic;
using Binance.ApiClient.Enums;
using ApiSharp.Converters;

namespace Binance.ApiClient.Converters
{
    internal class AdjustRateDirectionConverter : BaseConverter<AdjustRateDirection>
    {
        public AdjustRateDirectionConverter() : this(true) { }
        public AdjustRateDirectionConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<AdjustRateDirection, string>> Mapping => new List<KeyValuePair<AdjustRateDirection, string>>
        {
            new KeyValuePair<AdjustRateDirection, string>(AdjustRateDirection.Reduced, "REDUCED"),
            new KeyValuePair<AdjustRateDirection, string>(AdjustRateDirection.Additional, "ADDITIONAL")
        };
    }
}
