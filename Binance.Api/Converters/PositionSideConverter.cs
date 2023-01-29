﻿using Binance.ApiClient.Enums;
using ApiSharp.Converters;
using System.Collections.Generic;

namespace Binance.ApiClient.Converters
{
    internal class PositionSideConverter : BaseConverter<PositionSide>
    {
        public PositionSideConverter() : this(true) { }
        public PositionSideConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<PositionSide, string>> Mapping => new List<KeyValuePair<PositionSide, string>>
        {
            new KeyValuePair<PositionSide, string>(PositionSide.Short, "SHORT"),
            new KeyValuePair<PositionSide, string>(PositionSide.Long, "LONG"),
            new KeyValuePair<PositionSide, string>(PositionSide.Both, "BOTH"),
        };
    }
}