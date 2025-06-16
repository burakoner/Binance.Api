namespace Binance.Api.Options;

internal partial class BinanceOptionsRestClientMarketMaker
{
    public Task<RestCallResult<BinanceOptionsMarketMakerBlockOrder>> PlaceBlockOrderAsync(BinanceOptionsLiquidity liquidity, IEnumerable<BinanceOptionsMarketMakerBlockOrderLeg> legs, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddEnum("liquidity", liquidity);
        parameters.AddParameter("orders", legs);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsMarketMakerBlockOrder>(GetUrl(eapi, v1, "block/order/create"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5);
    }

    public async Task<RestCallResult<bool>> CancelBlockOrderAsync(string blockOrderMatchingKey, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("blockOrderMatchingKey", blockOrderMatchingKey);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceOptionsMarketMakerBlockOrder>(GetUrl(eapi, v1, "block/order/create"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success) return result.AsError<bool>(result.Error!);

        return result.As(result.Success);
    }

    public Task<RestCallResult<BinanceOptionsMarketMakerBlockOrder>> ExtendBlockOrderAsync(string blockOrderMatchingKey, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("blockOrderMatchingKey", blockOrderMatchingKey);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsMarketMakerBlockOrder>(GetUrl(eapi, v1, "block/order/create"), HttpMethod.Put, ct, true, bodyParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<List<BinanceOptionsMarketMakerBlockOrder>>> GetBlockOrderAsync(
        string? blockOrderMatchingKey = null,
        string? underlying = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("blockOrderMatchingKey", blockOrderMatchingKey);
        parameters.AddOptional("underlying", underlying);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceOptionsMarketMakerBlockOrder>>(GetUrl(eapi, v1, "block/order/orders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<BinanceOptionsMarketMakerBlockOrder>> AcceptBlockOrderAsync(string blockOrderMatchingKey, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("blockOrderMatchingKey", blockOrderMatchingKey);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsMarketMakerBlockOrder>(GetUrl(eapi, v1, "block/order/execute"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<BinanceOptionsMarketMakerBlockOrder>> GetBlockOrderAsync(string blockOrderMatchingKey, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("blockOrderMatchingKey", blockOrderMatchingKey);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsMarketMakerBlockOrder>(GetUrl(eapi, v1, "block/order/execute"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<List<BinanceOptionsMarketMakerBlockTrade>>> GetBlockTradesAsync(
        string? underlying = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("underlying", underlying);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceOptionsMarketMakerBlockTrade>>(GetUrl(eapi, v1, "block/order/orders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }
}