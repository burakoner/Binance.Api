namespace Binance.Api.Spot;

internal partial class BinanceSpotSocketClient
{
    public Task<CallResult<BinanceSpotOrderList>> CancelOrderListAsync(
        string symbol,
        long? orderListId = null,
        string? listClientOrderId = null,
        string? newClientOrderId = null,
        decimal? receiveWindow = null,
        CancellationToken ct = default)
    {
        var normalizedReceiveWindow = _.ReceiveWindow(receiveWindow);
        var parameters = BinanceSpotOrderListRequestBuilder.Cancel(symbol, orderListId, listClientOrderId, newClientOrderId, normalizedReceiveWindow, false);
        return RequestAsync<BinanceSpotOrderList>("ws-api/v3", "orderList.cancel", parameters, true, true, weight: 1, ct: ct);
    }

    public Task<CallResult<BinanceSpotOrderList>> PlaceOcoOrderListAsync(BinanceSpotOcoOrderListRequest request, CancellationToken ct = default)
        => PlaceOrderListAsync(request, "orderList.place.oco", BinanceSpotOrderListRequestBuilder.Oco, ct);

    public Task<CallResult<BinanceSpotOrderList>> PlaceOpoOrderListAsync(BinanceSpotOpoOrderListRequest request, CancellationToken ct = default)
        => PlaceOrderListAsync(request, "orderList.place.opo", BinanceSpotOrderListRequestBuilder.Opo, ct);

    public Task<CallResult<BinanceSpotOrderList>> PlaceOpocoOrderListAsync(BinanceSpotOpocoOrderListRequest request, CancellationToken ct = default)
        => PlaceOrderListAsync(request, "orderList.place.opoco", BinanceSpotOrderListRequestBuilder.Opoco, ct);

    public Task<CallResult<BinanceSpotOrderList>> PlaceOtoOrderListAsync(BinanceSpotOtoOrderListRequest request, CancellationToken ct = default)
        => PlaceOrderListAsync(request, "orderList.place.oto", BinanceSpotOrderListRequestBuilder.Oto, ct);

    public Task<CallResult<BinanceSpotOrderList>> PlaceOtocoOrderListAsync(BinanceSpotOtocoOrderListRequest request, CancellationToken ct = default)
        => PlaceOrderListAsync(request, "orderList.place.otoco", BinanceSpotOrderListRequestBuilder.Otoco, ct);

    private Task<CallResult<BinanceSpotOrderList>> PlaceOrderListAsync<TRequest>(
        TRequest request,
        string method,
        Func<TRequest, Func<string?, string?>, decimal?, bool, ParameterCollection> createParameters,
        CancellationToken ct)
        where TRequest : BinanceSpotOrderListRequest
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var normalizedReceiveWindow = _.ReceiveWindow(request.ReceiveWindow);
        string? ApplyClientOrderId(string? clientOrderId)
            => BinanceHelpers.ApplyBrokerId(clientOrderId, BinanceConstants.ClientOrderIdSpot, 36, SocketOptions.AllowAppendingClientOrderId);

        var parameters = createParameters(request, ApplyClientOrderId, normalizedReceiveWindow, false);
        return RequestAsync<BinanceSpotOrderList>("ws-api/v3", method, parameters, true, true, weight: 1, ct: ct);
    }
}
