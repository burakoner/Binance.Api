namespace Binance.Api.Spot;

internal static class BinanceSpotMarketDataValidation
{
    public static int DepthWeight(int? limit)
        => limit is null or <= 100 ? 5 : limit <= 500 ? 25 : limit <= 1000 ? 50 : 250;

    public static int Ticker24HourWeight(int? symbolCount)
        => symbolCount is null or > 100 ? 80 : symbolCount <= 20 ? 2 : 40;

    public static int RollingTickerWeight(int symbolCount)
        => Math.Min(symbolCount * 4, 200);

    public static string[] Symbols(IEnumerable<string> symbols, int? maximum = null)
    {
        if (symbols == null)
            throw new ArgumentNullException(nameof(symbols));

        var result = symbols.ToArray();
        if (result.Length == 0)
            throw new ArgumentException("At least one symbol is required.", nameof(symbols));
        if (maximum.HasValue && result.Length > maximum.Value)
            throw new ArgumentException($"The maximum number of symbols is {maximum.Value}.", nameof(symbols));

        foreach (var symbol in result)
            symbol.ValidateBinanceSymbol();
        return result;
    }

    public static void TimeRange(DateTime? startTime, DateTime? endTime)
    {
        if (startTime > endTime)
            throw new ArgumentException("startTime cannot be later than endTime.", nameof(startTime));
    }

    public static string? TimeZone(string? timeZone)
    {
        if (timeZone == null)
            return null;
        if (string.IsNullOrWhiteSpace(timeZone))
            throw new ArgumentException("timeZone cannot be empty.", nameof(timeZone));

        var parts = timeZone.Split(':');
        if (parts.Length is < 1 or > 2 ||
            !int.TryParse(parts[0], NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var hours) ||
            parts.Length == 2 && !int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out _))
            throw new ArgumentException("timeZone must contain hours or hours and minutes.", nameof(timeZone));

        var minutes = parts.Length == 2
            ? int.Parse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture)
            : 0;
        if (minutes is < 0 or > 59)
            throw new ArgumentOutOfRangeException(nameof(timeZone), "timeZone minutes must be between 00 and 59.");

        var negative = parts[0].StartsWith("-", StringComparison.Ordinal);
        if (hours is < -14 or > 14)
            throw new ArgumentOutOfRangeException(nameof(timeZone), "timeZone must be between -12:00 and +14:00.");

        var totalMinutes = Math.Abs(hours) * 60 + minutes;
        if (negative)
            totalMinutes = -totalMinutes;
        if (totalMinutes is < -720 or > 840)
            throw new ArgumentOutOfRangeException(nameof(timeZone), "timeZone must be between -12:00 and +14:00.");

        return timeZone;
    }

    public static string? WindowSize(TimeSpan? windowSize)
    {
        if (!windowSize.HasValue)
            return null;

        var value = windowSize.Value;
        if (value <= TimeSpan.Zero || value > TimeSpan.FromDays(7) || value.Ticks % TimeSpan.TicksPerMinute != 0)
            throw new ArgumentOutOfRangeException(nameof(windowSize), "windowSize must be a whole number of minutes from 1 minute through 7 days.");

        if (value.TotalDays >= 1)
        {
            if (value.Ticks % TimeSpan.TicksPerDay != 0)
                throw new ArgumentException("windowSize units cannot be combined.", nameof(windowSize));
            return ((int)value.TotalDays).ToString(CultureInfo.InvariantCulture) + "d";
        }

        if (value.TotalHours >= 1)
        {
            if (value.Ticks % TimeSpan.TicksPerHour != 0)
                throw new ArgumentException("windowSize units cannot be combined.", nameof(windowSize));
            return ((int)value.TotalHours).ToString(CultureInfo.InvariantCulture) + "h";
        }

        return ((int)value.TotalMinutes).ToString(CultureInfo.InvariantCulture) + "m";
    }
}
