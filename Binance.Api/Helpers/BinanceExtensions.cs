namespace Binance.Api.Helpers;

internal static class BinanceExtensions
{
    public static void ValidateBinanceSymbol(this string symbol)
    {
        if (string.IsNullOrEmpty(symbol))
            throw new ArgumentException("Symbol is not provided");

        if (!Regex.IsMatch(symbol, "^([A-Z|a-z|0-9]{5,})$"))
            throw new ArgumentException($"{symbol} is not a valid Binance symbol. Should be [BaseAsset][QuoteAsset], e.g. BTCUSDT");
    }
}