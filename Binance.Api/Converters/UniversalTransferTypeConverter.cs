using Binance.Api.Wallet;

namespace Binance.Api.Converters;

internal class UniversalTransferTypeConverter : BaseConverter<BinanceUniversalTransferType>
{
    public UniversalTransferTypeConverter() : this(true) { }
    public UniversalTransferTypeConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BinanceUniversalTransferType, string>> Mapping => new List<KeyValuePair<BinanceUniversalTransferType, string>>
    {
        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.MainToFunding, "MAIN_FUNDING"),
        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.MainToUsdFutures, "MAIN_UMFUTURE"),
        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.MainToCoinFutures, "MAIN_CMFUTURE"),
        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.MainToMargin, "MAIN_MARGIN"),
        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.MainToMining, "MAIN_MINING"),

        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.FundingToMain, "FUNDING_MAIN"),
        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.FundingToUsdFutures, "FUNDING_UMFUTURE"),
        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.FundingToMargin, "FUNDING_MARGIN"),

        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.UsdFuturesToMain, "UMFUTURE_MAIN"),
        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.UsdFuturesToFunding, "UMFUTURE_FUNDING"),
        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.UsdFuturesToMargin, "UMFUTURE_MARGIN"),

        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.CoinFuturesToMain, "CMFUTURE_MAIN"),
        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.CoinFuturesToMargin, "CMFUTURE_MARGIN"),

        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.MarginToIsolatedMargin, "MARGIN_ISOLATEDMARGIN"),
        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.IsolatedMarginToMargin, "ISOLATEDMARGIN_MARGIN"),

        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.MarginToMain, "MARGIN_MAIN"),
        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.MarginToUsdFutures, "MARGIN_UMFUTURE"),
        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.MarginToCoinFutures, "MARGIN_CMFUTURE"),
        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.MarginToMining, "MARGIN_MINING"),
        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.MarginToFunding, "MARGIN_FUNDING"),

        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.MiningToMain, "MINING_MAIN"),
        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.MiningToUsdFutures, "MINING_UMFUTURE"),
        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.MiningToMargin, "MINING_MARGIN"),

        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.FundingToCoinFutures, "FUNDING_CMFUTURE"),
        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.CoinFuturesToFunding, "CMFUTURE_FUNDING"),
        new KeyValuePair<BinanceUniversalTransferType, string>(BinanceUniversalTransferType.IsolatedMarginToIsolatedMargin, "ISOLATEDMARGIN_ISOLATEDMARGIN"),
    };
}