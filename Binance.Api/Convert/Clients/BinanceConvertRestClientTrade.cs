namespace Binance.Api.Convert;

internal partial class BinanceConvertRestClient
{
    public Task<RestCallResult<BinanceConvertQuote>> QuoteRequestAsync(string fromAsset, string toAsset, decimal? fromAmount = null, decimal? toAmount = null, BinanceConvertWalletType? walletType = null, BinanceConvertValidTime? validTime = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(fromAsset))
            throw new ArgumentException("fromAsset cannot be empty.", nameof(fromAsset));
        if (string.IsNullOrWhiteSpace(toAsset))
            throw new ArgumentException("toAsset cannot be empty.", nameof(toAsset));
        if ((fromAmount is null) == (toAmount is null))
            throw new ArgumentException("Either fromAmount or toAmount must be sent, but not both.");

        var parameters = new ParameterCollection();
        parameters.AddParameter("fromAsset", fromAsset);
        parameters.AddParameter("toAsset", toAsset);
        parameters.AddOptional("fromAmount", fromAmount?.ToString(BinanceConstants.CI));
        parameters.AddOptional("toAmount", toAmount?.ToString(BinanceConstants.CI));
        parameters.AddOptionalEnum("walletType", walletType);
        parameters.AddOptionalEnum("validTime", validTime);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceConvertQuote>(GetUrl(sapi, v1, "convert/getQuote"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 200);
    }

    public Task<RestCallResult<BinanceConvertResult>> AcceptQuoteAsync(string quoteId, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(quoteId))
            throw new ArgumentException("quoteId cannot be empty.", nameof(quoteId));

        var parameters = new ParameterCollection();
        parameters.AddParameter("quoteId", quoteId);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceConvertResult>(GetUrl(sapi, v1, "convert/acceptQuote"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 500);
    }

    public Task<RestCallResult<BinanceListRangeResponse<BinanceConvertTrade>>> GetHistoryAsync(DateTime startTime, DateTime endTime, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (startTime > endTime)
            throw new ArgumentException("startTime cannot be later than endTime", nameof(startTime));
        if (endTime - startTime > TimeSpan.FromDays(30))
            throw new ArgumentException("The time range cannot exceed 30 days", nameof(endTime));
        if (limit > 1000)
            throw new ArgumentOutOfRangeException(nameof(limit), "limit cannot exceed 1000");

        var parameters = new ParameterCollection();
        parameters.AddMilliseconds("startTime", startTime);
        parameters.AddMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceListRangeResponse<BinanceConvertTrade>>(GetUrl(sapi, v1, "convert/tradeFlow"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 3000);
    }

    public Task<RestCallResult<BinanceConvertStatus>> GetStatusAsync(string? orderId = null, string? quoteId = null, CancellationToken ct = default)
    {
        var hasOrderId = !string.IsNullOrWhiteSpace(orderId);
        var hasQuoteId = !string.IsNullOrWhiteSpace(quoteId);
        if (hasOrderId == hasQuoteId)
            throw new ArgumentException("Either orderId or quoteId must be provided, but not both.");
        if (orderId != null && string.IsNullOrWhiteSpace(orderId))
            throw new ArgumentException("orderId cannot be empty.", nameof(orderId));
        if (quoteId != null && string.IsNullOrWhiteSpace(quoteId))
            throw new ArgumentException("quoteId cannot be empty.", nameof(quoteId));

        var parameters = new ParameterCollection();
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("quoteId", quoteId);

        return RequestAsync<BinanceConvertStatus>(GetUrl(sapi, v1, "convert/orderStatus"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<BinanceConvertLimitOrder>> PlaceLimitOrderAsync(string baseAsset, string quoteAsset,
        decimal limitPrice,
        BinanceOrderSide side,
        BinanceConvertExpiredTime expiredType,
        decimal? baseAmount = null,
        decimal? quoteAmount = null,
        BinanceConvertWalletType? walletType = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (baseAmount == null && quoteAmount == null || baseAmount != null && quoteAmount != null)
            throw new ArgumentException("Either baseAsset or quoteAsset must be sent, but not both");

        var parameters = new ParameterCollection();
        parameters.AddParameter("baseAsset", baseAsset);
        parameters.AddParameter("quoteAsset", quoteAsset);
        parameters.AddParameter("limitPrice", limitPrice);
        parameters.AddEnum("side", side);
        parameters.AddOptional("baseAmount", baseAmount?.ToString(BinanceConstants.CI));
        parameters.AddOptional("quoteAmount", quoteAmount?.ToString(BinanceConstants.CI));
        parameters.AddOptionalEnum("expiredType", expiredType);
        parameters.AddOptionalEnum("walletType", walletType);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceConvertLimitOrder>(GetUrl(sapi, v1, "convert/limit/placeOrder"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 500);
    }

    public Task<RestCallResult<BinanceConvertLimitOrderStatus>> CancelLimitOrderAsync(string orderId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("orderId", orderId);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceConvertLimitOrderStatus>(GetUrl(sapi, v1, "convert/limit/cancelOrder"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 500);
    }

    public async Task<RestCallResult<List<BinanceConvertOpenOrder>>> GetOpenLimitOrdersAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceListResponse<BinanceConvertOpenOrder>>(GetUrl(sapi, v1, "convert/limit/queryOpenOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 3000).ConfigureAwait(false);
        return result.Success ? result.As(result.Data.List) : result.As<List<BinanceConvertOpenOrder>>([]);
    }
}
