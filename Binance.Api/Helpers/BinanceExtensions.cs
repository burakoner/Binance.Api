namespace Binance.Api.Helpers;

public static class BinanceExtensions
{

    public static void ValidateBinanceSymbol(this string symbolString)
    {
        if (string.IsNullOrEmpty(symbolString))
            throw new ArgumentException("Symbol is not provided");

        if (!Regex.IsMatch(symbolString, "^([A-Z|a-z|0-9]{5,})$"))
            throw new ArgumentException($"{symbolString} is not a valid Binance symbol. Should be [BaseAsset][QuoteAsset], e.g. BTCUSDT");
    }
}