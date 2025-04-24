namespace Binance.Api.Converters;

internal class PeriodIntervalConverter : BaseConverter<BinancePeriodInterval>
{
    public PeriodIntervalConverter() : this(true) { }
    public PeriodIntervalConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BinancePeriodInterval, string>> Mapping => new List<KeyValuePair<BinancePeriodInterval, string>>
    {
        new KeyValuePair<BinancePeriodInterval, string>(BinancePeriodInterval.FiveMinutes, "5m"),
        new KeyValuePair<BinancePeriodInterval, string>(BinancePeriodInterval.FifteenMinutes, "15m"),
        new KeyValuePair<BinancePeriodInterval, string>(BinancePeriodInterval.ThirtyMinutes, "30m"),
        new KeyValuePair<BinancePeriodInterval, string>(BinancePeriodInterval.OneHour, "1h"),
        new KeyValuePair<BinancePeriodInterval, string>(BinancePeriodInterval.TwoHour, "2h"),
        new KeyValuePair<BinancePeriodInterval, string>(BinancePeriodInterval.FourHour, "4h"),
        new KeyValuePair<BinancePeriodInterval, string>(BinancePeriodInterval.SixHour, "6h"),
        new KeyValuePair<BinancePeriodInterval, string>(BinancePeriodInterval.TwelveHour, "12h"),
        new KeyValuePair<BinancePeriodInterval, string>(BinancePeriodInterval.OneDay, "1d")
    };
}
