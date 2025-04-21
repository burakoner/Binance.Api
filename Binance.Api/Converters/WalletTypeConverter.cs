using Binance.Api.Wallet;

namespace Binance.Api.Converters;

internal class WalletTypeConverter : BaseConverter<BinanceWalletType>
{
    public WalletTypeConverter() : this(true) { }
    public WalletTypeConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BinanceWalletType, string>> Mapping => new List<KeyValuePair<BinanceWalletType, string>>
    {
        new KeyValuePair<BinanceWalletType, string>(BinanceWalletType.Spot, "0"),
        new KeyValuePair<BinanceWalletType, string>(BinanceWalletType.Funding, "1"),
    };
}
