using System.Collections.Generic;
using Binance.ApiClient.Enums;
using ApiSharp.Converters;

namespace Binance.ApiClient.Converters
{
    internal class ProjectTypeConverter : BaseConverter<ProjectType>
    {
        public ProjectTypeConverter() : this(true) { }
        public ProjectTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<ProjectType, string>> Mapping => new List<KeyValuePair<ProjectType, string>>
        {
            new KeyValuePair<ProjectType, string>(ProjectType.CustomizedFixed, "CUSTOMIZED_FIXED"),
            new KeyValuePair<ProjectType, string>(ProjectType.Activity, "ACTIVITY")
        };
    }
}
