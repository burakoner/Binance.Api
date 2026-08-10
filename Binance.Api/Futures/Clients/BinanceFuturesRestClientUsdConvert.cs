namespace Binance.Api.Futures;

internal partial class BinanceFuturesRestClientUsd
{
    public Task<RestCallResult<List<BinanceFuturesConvertSymbol>>> GetConvertSymbolsAsync(string? fromAsset = null, string? toAsset = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("fromAsset", fromAsset);
        parameters.AddOptional("toAsset", toAsset);

        return RequestAsync<List<BinanceFuturesConvertSymbol>>(GetUrl(fapi, v1, "convert/exchangeInfo"), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<BinanceFuturesConvertQuote>> ConvertQuoteRequestAsync(string fromAsset, string toAsset, decimal? fromAmount = null, decimal? toAmount = null, string? validTime = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (fromAsset is null)
            throw new ArgumentNullException(nameof(fromAsset));
        if (toAsset is null)
            throw new ArgumentNullException(nameof(toAsset));

        var parameters = new ParameterCollection();
        parameters.Add("fromAsset", fromAsset);
        parameters.Add("toAsset", toAsset);
        parameters.AddOptional("fromAmount", fromAmount);
        parameters.AddOptional("toAmount", toAmount);
        parameters.AddOptional("validTime", validTime);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesConvertQuote>(GetUrl(fapi, v1, "convert/getQuote"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 50);
    }

    public Task<RestCallResult<BinanceFuturesConvertQuoteResult>> ConvertAcceptQuoteAsync(string quoteId, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (quoteId is null)
            throw new ArgumentNullException(nameof(quoteId));

        var parameters = new ParameterCollection();
        parameters.Add("quoteId", quoteId);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

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
