namespace Binance.Api.Spot;

internal partial class BinanceSpotRestClient
{
    public async Task<RestCallResult<BinanceSpotOrderList>> CancelOrderListAsync(
        string symbol,
        long? orderListId = null,
        string? listClientOrderId = null,
        string? newClientOrderId = null,
        decimal? receiveWindow = null,
        CancellationToken ct = default)
    {
        var normalizedReceiveWindow = _.ReceiveWindow(receiveWindow);
        var parameters = BinanceSpotOrderListRequestBuilder.Cancel(symbol, orderListId, listClientOrderId, newClientOrderId, normalizedReceiveWindow, true);
        var result = await RequestAsync<BinanceSpotOrderList>(GetUrl(api, v3, "orderList"), HttpMethod.Delete, ct, true, queryParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        if (result)
        {
            foreach (var order in result.Data.Orders)
                InvokeOrderCanceled(order.OrderId);
        }

        return result;
    }

    public Task<RestCallResult<BinanceSpotOrderList>> PlaceOcoOrderListAsync(BinanceSpotOcoOrderListRequest request, CancellationToken ct = default)
        => PlaceOrderListAsync(request, GetUrl(api, v3, "orderList/oco"), BinanceSpotOrderListRequestBuilder.Oco, ct);

    public Task<RestCallResult<BinanceSpotOrderList>> PlaceOpoOrderListAsync(BinanceSpotOpoOrderListRequest request, CancellationToken ct = default)
        => PlaceOrderListAsync(request, GetUrl(api, v3, "orderList/opo"), BinanceSpotOrderListRequestBuilder.Opo, ct);

    public Task<RestCallResult<BinanceSpotOrderList>> PlaceOpocoOrderListAsync(BinanceSpotOpocoOrderListRequest request, CancellationToken ct = default)
        => PlaceOrderListAsync(request, GetUrl(api, v3, "orderList/opoco"), BinanceSpotOrderListRequestBuilder.Opoco, ct);

    public Task<RestCallResult<BinanceSpotOrderList>> PlaceOtoOrderListAsync(BinanceSpotOtoOrderListRequest request, CancellationToken ct = default)
        => PlaceOrderListAsync(request, GetUrl(api, v3, "orderList/oto"), BinanceSpotOrderListRequestBuilder.Oto, ct);

    public Task<RestCallResult<BinanceSpotOrderList>> PlaceOtocoOrderListAsync(BinanceSpotOtocoOrderListRequest request, CancellationToken ct = default)
        => PlaceOrderListAsync(request, GetUrl(api, v3, "orderList/otoco"), BinanceSpotOrderListRequestBuilder.Otoco, ct);

    private async Task<RestCallResult<BinanceSpotOrderList>> PlaceOrderListAsync<TRequest>(
        TRequest request,
        Uri endpoint,
        Func<TRequest, Func<string?, string?>, decimal?, bool, ParameterCollection> createParameters,
        CancellationToken ct)
        where TRequest : BinanceSpotOrderListRequest
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var normalizedReceiveWindow = _.ReceiveWindow(request.ReceiveWindow);
        string? ApplyClientOrderId(string? clientOrderId)
            => BinanceHelpers.ApplyBrokerId(clientOrderId, BinanceConstants.ClientOrderIdSpot, 36, RestOptions.AllowAppendingClientOrderId);

        var parameters = createParameters(request, ApplyClientOrderId, normalizedReceiveWindow, true);
        var result = await RequestAsync<BinanceSpotOrderList>(endpoint, HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        if (result)
        {
            foreach (var order in result.Data.Orders)
                InvokeOrderPlaced(order.OrderId);
        }

        return result;
    }
}
