namespace Binance.Api.Futures;

internal partial class BinanceFuturesRestClientUsd
{
    public Task<RestCallResult<IEnumerable<BinanceFuturesConvertSymbol>>> GetConvertSymbolsAsync(string? fromAsset = null, string? toAsset = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("fromAsset", fromAsset);
        parameters.AddOptional("toAsset", toAsset);

        return RequestAsync<IEnumerable<BinanceFuturesConvertSymbol>>(GetUrl(fapi, v1, "convert/exchangeInfo"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<BinanceFuturesConvertQuote>> ConvertQuoteRequestAsync(string fromAsset, string toAsset, decimal? fromQuantity = null, decimal? toQuantity = null, BinanceValidTime? validTime = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.Add("fromAsset", fromAsset);
        parameters.Add("toAsset", toAsset);
        parameters.AddOptional("fromAmount", fromQuantity);
        parameters.AddOptional("toAmount", toQuantity);
        if (validTime != null)
        {
            var time = validTime == BinanceValidTime.TenSeconds ? "10s" : validTime == BinanceValidTime.ThirtySeconds ? "30s" : validTime == BinanceValidTime.OneMinute ? "1m" : "2m";
            parameters.Add("validTime", time);
        }

        return RequestAsync<BinanceFuturesConvertQuote>(GetUrl(fapi, v1, "convert/getQuote"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 50);
    }

    public Task<RestCallResult<BinanceFuturesConvertQuoteResult>> ConvertAcceptQuoteAsync(string quoteId, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.Add("quoteId", quoteId);

        return RequestAsync<BinanceFuturesConvertQuoteResult>(GetUrl(fapi, v1, "convert/acceptQuote"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 200);
    }

    public Task<RestCallResult<BinanceFuturesConvertStatus>> GetConvertOrderStatusAsync(string? quoteId = null, string? orderId = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("quoteId", quoteId);
        parameters.AddOptional("orderId", orderId);

        return RequestAsync<BinanceFuturesConvertStatus>(GetUrl(fapi, v1, "convert/orderStatus"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 50);
    }
}