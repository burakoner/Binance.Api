namespace Binance.Api.Futures;

internal partial class BinanceRestApiClientFuturesUsd
{
    /*
    /// <inheritdoc />
    internal class BinanceRestClientUsdFuturesApiExchangeData : IBinanceRestClientUsdFuturesApiExchangeData
    {

        #region Get Convert Symbols

        /// <inheritdoc />
        public async Task<RestCallResult<IEnumerable<BinanceFuturesConvertSymbol>>> GetConvertSymbolsAsync(string? fromAsset = null, string? toAsset = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("fromAsset", fromAsset);
            parameters.AddOptional("toAsset", toAsset);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/fapi/v1/convert/exchangeInfo", BinanceExchange.RateLimiter.FuturesRest, 20, false);
            var result = await _baseClient.SendAsync<IEnumerable<BinanceFuturesConvertSymbol>>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }

    #region Convert Quote Request

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesConvertQuote>> ConvertQuoteRequestAsync(string fromAsset, string toAsset, decimal? fromQuantity = null, decimal? toQuantity = null, ValidTime? validTime = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.Add("fromAsset", fromAsset);
        parameters.Add("toAsset", toAsset);
        parameters.AddOptional("fromAmount", fromQuantity);
        parameters.AddOptional("toAmount", toQuantity);
        if (validTime != null)
        {
            var time = validTime == ValidTime.TenSeconds ? "10s" : validTime == ValidTime.ThirtySeconds ? "30s" : validTime == ValidTime.OneMinute ? "1m" : "2m";
            parameters.Add("validTime", time);
        }
        var request = _definitions.GetOrCreate(HttpMethod.Post, "/fapi/v1/convert/getQuote", BinanceExchange.RateLimiter.FuturesRest, 50, true);
        var result = await _baseClient.SendAsync<BinanceFuturesConvertQuote>(request, parameters, ct).ConfigureAwait(false);
        return result;
    }

    #endregion

    #region Convert Accept Quote

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesQuoteResult>> ConvertAcceptQuoteAsync(string quoteId, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.Add("quoteId", quoteId);
        var request = _definitions.GetOrCreate(HttpMethod.Post, "/fapi/v1/convert/acceptQuote", BinanceExchange.RateLimiter.FuturesRest, 200, true);
        var result = await _baseClient.SendAsync<BinanceFuturesQuoteResult>(request, parameters, ct).ConfigureAwait(false);
        return result;
    }

    #endregion

    #region Get Convert Order Status

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesConvertStatus>> GetConvertOrderStatusAsync(string? quoteId = null, string? orderId = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("quoteId", quoteId);
        parameters.AddOptional("orderId", orderId);
        var request = _definitions.GetOrCreate(HttpMethod.Get, "/fapi/v1/convert/orderStatus", BinanceExchange.RateLimiter.FuturesRest, 50, true);
        var result = await _baseClient.SendAsync<BinanceFuturesConvertStatus>(request, parameters, ct).ConfigureAwait(false);
        return result;
    }

    #endregion
    */
}