﻿using Binance.ApiClient.Enums;
using ApiSharp.Converters;
using System.Collections.Generic;

namespace Binance.ApiClient.Converters
{
    internal class WorkingTypeConverter : BaseConverter<WorkingType>
    {
        public WorkingTypeConverter() : this(true) { }
        public WorkingTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<WorkingType, string>> Mapping => new List<KeyValuePair<WorkingType, string>>
        {
            new KeyValuePair<WorkingType, string>(WorkingType.Mark, "MARK_PRICE"),
            new KeyValuePair<WorkingType, string>(WorkingType.Contract, "CONTRACT_PRICE"),
        };
    }
}