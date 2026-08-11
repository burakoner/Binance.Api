namespace Binance.Api.Options;

internal partial class BinanceOptionsRestClientMarketMaker
{
    public Task<RestCallResult<BinanceOptionsMarketMakerBlockOrder>> PlaceBlockOrderAsync(
        BinanceOptionsLiquidity liquidity,
        IEnumerable<BinanceOptionsMarketMakerBlockOrderRequestLeg> legs,
        long? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (liquidity != BinanceOptionsLiquidity.Maker && liquidity != BinanceOptionsLiquidity.Taker)
            throw new ArgumentOutOfRangeException(nameof(liquidity), liquidity, "liquidity must be MAKER or TAKER");
        if (legs == null)
            throw new ArgumentNullException(nameof(legs));

        var materializedLegs = legs.ToList();
        if (materializedLegs.Count != 1)
            throw new ArgumentException("Exactly one leg must be provided", nameof(legs));

        var leg = materializedLegs[0] ?? throw new ArgumentException("The block order leg cannot be null", nameof(legs));
        ValidateRequiredValue(leg.Symbol, nameof(legs));
        if (leg.Side != BinanceOrderSide.Buy && leg.Side != BinanceOrderSide.Sell)
            throw new ArgumentOutOfRangeException(nameof(legs), leg.Side, "leg side must be BUY or SELL");
        if (leg.Type != BinanceOptionsOrderType.Limit)
            throw new ArgumentOutOfRangeException(nameof(legs), leg.Type, "leg type must be LIMIT");

        var legParameters = new ParameterCollection
        {
            { "symbol", leg.Symbol }
        };
        legParameters.AddEnum("side", leg.Side);
        legParameters.AddEnum("type", leg.Type);
        legParameters.AddParameter("quantity", leg.Quantity.ToString(BinanceConstants.CI));
        legParameters.AddOptional("price", leg.Price?.ToString(BinanceConstants.CI));

        var parameters = new ParameterCollection();
        parameters.AddEnum("liquidity", liquidity);
        parameters.AddParameter("legs", JsonConvert.SerializeObject(new[] { legParameters }));
        parameters.AddOptional("recvWindow", _.ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsMarketMakerBlockOrder>(GetUrl(eapi, v1, "block/order/create"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5);
    }

    public async Task<RestCallResult<bool>> CancelBlockOrderAsync(string blockOrderMatchingKey, long? receiveWindow = null, CancellationToken ct = default)
    {
        ValidateRequiredValue(blockOrderMatchingKey, nameof(blockOrderMatchingKey));
        var parameters = new ParameterCollection();
        parameters.AddParameter("blockOrderMatchingKey", blockOrderMatchingKey);
        parameters.AddOptional("recvWindow", _.ValidateReceiveWindow(receiveWindow));

        var result = await RequestAsync<object>(GetUrl(eapi, v1, "block/order/create"), HttpMethod.Delete, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result.Success) return result.AsError<bool>(result.Error!);

        return result.As(result.Success);
    }

    public Task<RestCallResult<BinanceOptionsMarketMakerBlockOrder>> ExtendBlockOrderAsync(string blockOrderMatchingKey, long? receiveWindow = null, CancellationToken ct = default)
    {
        ValidateRequiredValue(blockOrderMatchingKey, nameof(blockOrderMatchingKey));
        var parameters = new ParameterCollection();
        parameters.AddParameter("blockOrderMatchingKey", blockOrderMatchingKey);
        parameters.AddOptional("recvWindow", _.ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsMarketMakerBlockOrder>(GetUrl(eapi, v1, "block/order/create"), HttpMethod.Put, ct, true, bodyParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<List<BinanceOptionsMarketMakerBlockOrder>>> GetBlockOrdersAsync(
        string? blockOrderMatchingKey = null,
        string? underlying = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        long? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (blockOrderMatchingKey != null)
            ValidateRequiredValue(blockOrderMatchingKey, nameof(blockOrderMatchingKey));
        if (underlying != null)
            ValidateRequiredValue(underlying, nameof(underlying));

        var parameters = new ParameterCollection();
        parameters.AddOptional("blockOrderMatchingKey", blockOrderMatchingKey);
        parameters.AddOptional("underlying", underlying);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _.ValidateReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceOptionsMarketMakerBlockOrder>>(GetUrl(eapi, v1, "block/order/orders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<BinanceOptionsMarketMakerBlockOrder>> AcceptBlockOrderAsync(string blockOrderMatchingKey, long? receiveWindow = null, CancellationToken ct = default)
    {
        ValidateRequiredValue(blockOrderMatchingKey, nameof(blockOrderMatchingKey));
        var parameters = new ParameterCollection();
        parameters.AddParameter("blockOrderMatchingKey", blockOrderMatchingKey);
        parameters.AddOptional("recvWindow", _.ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsMarketMakerBlockOrder>(GetUrl(eapi, v1, "block/order/execute"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<BinanceOptionsMarketMakerBlockOrder>> GetBlockTradeDetailsAsync(string blockOrderMatchingKey, long? receiveWindow = null, CancellationToken ct = default)
    {
        ValidateRequiredValue(blockOrderMatchingKey, nameof(blockOrderMatchingKey));
        var parameters = new ParameterCollection();
        parameters.AddParameter("blockOrderMatchingKey", blockOrderMatchingKey);
        parameters.AddOptional("recvWindow", _.ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsMarketMakerBlockOrder>(GetUrl(eapi, v1, "block/order/execute"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<List<BinanceOptionsMarketMakerBlockTrade>>> GetBlockTradesAsync(
        string? underlying = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        long? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (underlying != null)
            ValidateRequiredValue(underlying, nameof(underlying));

        var parameters = new ParameterCollection();
        parameters.AddOptional("underlying", underlying);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _.ValidateReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceOptionsMarketMakerBlockTrade>>(GetUrl(eapi, v1, "block/user-trades"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }
}
