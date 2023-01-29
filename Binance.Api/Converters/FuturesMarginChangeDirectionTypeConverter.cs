﻿using System.Collections.Generic;
using ApiSharp.Converters;
using Binance.ApiClient.Enums;

namespace Binance.ApiClient.Converters
{
    internal class FuturesMarginChangeDirectionTypeConverter : BaseConverter<FuturesMarginChangeDirectionType>
    {
        public FuturesMarginChangeDirectionTypeConverter(): this(false) { }
        public FuturesMarginChangeDirectionTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<FuturesMarginChangeDirectionType, string>> Mapping => new List<KeyValuePair<FuturesMarginChangeDirectionType, string>>
        {
            new KeyValuePair<FuturesMarginChangeDirectionType, string>(FuturesMarginChangeDirectionType.Add, "1"),
            new KeyValuePair<FuturesMarginChangeDirectionType, string>(FuturesMarginChangeDirectionType.Reduce, "2")
        };
    }
}