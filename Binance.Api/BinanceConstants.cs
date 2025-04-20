namespace Binance.Api;

/// <summary>
/// Binance Constants
/// </summary>
internal static class BinanceConstants
{
    /// <summary>
    /// Invariant Culture Info
    /// </summary>
    public static CultureInfo CI { get; set; } = CultureInfo.InvariantCulture;

    internal const string ClientOrderIdSpot = "x-QQCDRXG2";
    internal const string ClientOrderIdFutures = "x-hF7HV9eK";
    internal const string ClientOrderIdPrefixSpot = ClientOrderIdSpot + BinanceHelpers.ClientOrderIdSeparator;
    internal const string ClientOrderIdPrefixFutures = ClientOrderIdFutures + BinanceHelpers.ClientOrderIdSeparator;
}
