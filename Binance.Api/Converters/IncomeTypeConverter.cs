using System.Collections.Generic;
using ApiSharp.Converters;
using Binance.ApiClient.Enums;

namespace Binance.ApiClient.Converters
{
    internal class IncomeTypeConverter: BaseConverter<IncomeType>
    {
        public IncomeTypeConverter(): this(true) { }
        public IncomeTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<IncomeType, string>> Mapping => new List<KeyValuePair<IncomeType, string>>
        {
            new KeyValuePair<IncomeType, string>(IncomeType.Transfer, "TRANSFER"),
            new KeyValuePair<IncomeType, string>(IncomeType.WelcomeBonus, "WELCOME_BONUS"),
            new KeyValuePair<IncomeType, string>(IncomeType.RealizedPnl, "REALIZED_PNL"),
            new KeyValuePair<IncomeType, string>(IncomeType.FundingFee, "FUNDING_FEE"),
            new KeyValuePair<IncomeType, string>(IncomeType.Commission, "COMMISSION"),
            new KeyValuePair<IncomeType, string>(IncomeType.InsuranceClear, "INSURANCE_CLEAR"),
        };
    }
}
