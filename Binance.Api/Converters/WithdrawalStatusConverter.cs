using Binance.Api.Wallet;

namespace Binance.Api.Converters;

internal class WithdrawalStatusConverter : BaseConverter<BinanceWithdrawalStatus>
{
    public WithdrawalStatusConverter() : this(true)
    {
    }

    public WithdrawalStatusConverter(bool quotes) : base(quotes)
    {
    }

    protected override List<KeyValuePair<BinanceWithdrawalStatus, string>> Mapping => new List<KeyValuePair<BinanceWithdrawalStatus, string>>
    {
        new KeyValuePair<BinanceWithdrawalStatus, string>(BinanceWithdrawalStatus.EmailSend, "0"),
        new KeyValuePair<BinanceWithdrawalStatus, string>(BinanceWithdrawalStatus.Canceled, "1"),
        new KeyValuePair<BinanceWithdrawalStatus, string>(BinanceWithdrawalStatus.AwaitingApproval, "2"),
        new KeyValuePair<BinanceWithdrawalStatus, string>(BinanceWithdrawalStatus.Rejected, "3"),
        new KeyValuePair<BinanceWithdrawalStatus, string>(BinanceWithdrawalStatus.Processing, "4"),
        new KeyValuePair<BinanceWithdrawalStatus, string>(BinanceWithdrawalStatus.Failure, "5"),
        new KeyValuePair<BinanceWithdrawalStatus, string>(BinanceWithdrawalStatus.Completed, "6")
    };
}
