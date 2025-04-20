namespace Binance.Api.Converters;

internal class BinanceEarningTypeConverter : BaseConverter<EarningType>
{
    public BinanceEarningTypeConverter() : this(true) { }
    public BinanceEarningTypeConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<EarningType, string>> Mapping => new List<KeyValuePair<EarningType, string>>
    {
        new KeyValuePair<EarningType, string>(EarningType.MiningWallet, "0"),
        new KeyValuePair<EarningType, string>(EarningType.MergedMining, "1"),
        new KeyValuePair<EarningType, string>(EarningType.ActivityBonus, "2"),
        new KeyValuePair<EarningType, string>(EarningType.Rebate, "3"),
        new KeyValuePair<EarningType, string>(EarningType.SmartPool, "4"),
        new KeyValuePair<EarningType, string>(EarningType.MiningAddress, "5"),
        new KeyValuePair<EarningType, string>(EarningType.IncomeTransfer, "6"),
        new KeyValuePair<EarningType, string>(EarningType.PoolSavings, "7"),
        new KeyValuePair<EarningType, string>(EarningType.Transfered, "8"),
        new KeyValuePair<EarningType, string>(EarningType.IncomeTransfer, "31"),
        new KeyValuePair<EarningType, string>(EarningType.HashrateResaleMiningWallet, "32"),
        new KeyValuePair<EarningType, string>(EarningType.HashrateResalePoolSavings, "33")
    };
}
