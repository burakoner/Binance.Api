using Binance.Api.Wallet;

namespace Binance.Api.Converters;

internal class WithdrawDepositTransferTypeConverter : BaseConverter<BinanceWithdrawDepositTransferType>
{
    public WithdrawDepositTransferTypeConverter() : this(true)
    {
    }

    public WithdrawDepositTransferTypeConverter(bool quotes) : base(quotes)
    {
    }

    protected override List<KeyValuePair<BinanceWithdrawDepositTransferType, string>> Mapping =>
        new List<KeyValuePair<BinanceWithdrawDepositTransferType, string>>
        {
            new KeyValuePair<BinanceWithdrawDepositTransferType, string>(
                BinanceWithdrawDepositTransferType.Internal, "1"),
            new KeyValuePair<BinanceWithdrawDepositTransferType, string>(
                BinanceWithdrawDepositTransferType.External, "0"),
        };
}