using Binance.Api.Spot;

namespace Binance.Api.Converters;

internal class SymbolStatusConverter : BaseConverter<BinanceSymbolStatus>
{
    public SymbolStatusConverter() : this(true) { }
    public SymbolStatusConverter(bool quotes) : base(quotes) { }

    protected override List<KeyValuePair<BinanceSymbolStatus, string>> Mapping => new List<KeyValuePair<BinanceSymbolStatus, string>>
    {
        new KeyValuePair<BinanceSymbolStatus, string>(BinanceSymbolStatus.AuctionMatch, "AUCTION_MATCH"),
        new KeyValuePair<BinanceSymbolStatus, string>(BinanceSymbolStatus.Break, "BREAK"),
        new KeyValuePair<BinanceSymbolStatus, string>(BinanceSymbolStatus.EndOfDay, "END_OF_DAY"),
        new KeyValuePair<BinanceSymbolStatus, string>(BinanceSymbolStatus.Halt, "HALT"),
        new KeyValuePair<BinanceSymbolStatus, string>(BinanceSymbolStatus.PostTrading, "POST_TRADING"),
        new KeyValuePair<BinanceSymbolStatus, string>(BinanceSymbolStatus.PreTrading, "PRE_TRADING"),
        new KeyValuePair<BinanceSymbolStatus, string>(BinanceSymbolStatus.PendingTrading, "PENDING_TRADING"),
        new KeyValuePair<BinanceSymbolStatus, string>(BinanceSymbolStatus.Trading, "TRADING"),
        new KeyValuePair<BinanceSymbolStatus, string>(BinanceSymbolStatus.Close, "CLOSE"),
        new KeyValuePair<BinanceSymbolStatus, string>(BinanceSymbolStatus.PreDelivering, "PRE_DELIVERING"),
        new KeyValuePair<BinanceSymbolStatus, string>(BinanceSymbolStatus.Delivering, "DELIVERING"),
        new KeyValuePair<BinanceSymbolStatus, string>(BinanceSymbolStatus.PreSettle, "PRE_SETTLE"),
        new KeyValuePair<BinanceSymbolStatus, string>(BinanceSymbolStatus.Settling, "SETTLING"),
    };
}
