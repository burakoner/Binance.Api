namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientCoin
{
    internal const string ModifyOrderPath = "ws-dapi/v1";
    internal const string ModifyOrderMethod = "order.modify";
    internal const int ModifyOrderIpWeight = 1;

    public async Task<CallResult<BinanceFuturesOrder>> PlaceOrderAsync(
        string symbol,
        BinanceOrderSide side,
        BinanceFuturesOrderType type,
        decimal? quantity,
        decimal? price = null,
        decimal? stopPrice = null,
        string? newClientOrderId = null,
        BinancePositionSide? positionSide = null,
        BinanceTimeInForce? timeInForce = null,
        BinanceOrderResponseType? orderResponseType = null,
        BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
        BinanceFuturesPriceMatch? priceMatch = null,
        BinanceFuturesWorkingType? workingType = null,
        bool? reduceOnly = null,
        bool? closePosition = null,
        bool? priceProtect = null,
        decimal? activationPrice = null,
        decimal? callbackRate = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (closePosition == true && positionSide != null)
        {
            if (positionSide == BinancePositionSide.Short && side == BinanceOrderSide.Sell)
                throw new ArgumentException("Can't close short position with order side sell");
            if (positionSide == BinancePositionSide.Long && side == BinanceOrderSide.Buy)
                throw new ArgumentException("Can't close long position with order side buy");
        }

        if (orderResponseType == BinanceOrderResponseType.Full)
            throw new ArgumentException("OrderResponseType.Full is not supported in Futures");

        var rulesCheck = await ((BinanceFuturesRestClientCoin)__.RestApiClient.CoinFutures).CheckTradingRulesAsync(symbol, type, quantity, null, price, stopPrice, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceFuturesOrder>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;

        var clientOrderId = BinanceHelpers.ApplyBrokerId(newClientOrderId, BinanceConstants.ClientOrderIdFutures, 36, SocketOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddOptional("quantity", quantity?.ToString(BinanceConstants.CI));
        parameters.AddOptional("newClientOrderId", clientOrderId);
        parameters.AddOptional("price", price?.ToString(BinanceConstants.CI));
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptionalEnum("positionSide", positionSide);
        parameters.AddOptional("stopPrice", stopPrice?.ToString(BinanceConstants.CI));
        parameters.AddOptional("activationPrice", activationPrice?.ToString(BinanceConstants.CI));
        parameters.AddOptional("callbackRate", callbackRate?.ToString(BinanceConstants.CI));
        parameters.AddOptionalEnum("workingType", workingType);
        parameters.AddOptional("reduceOnly", reduceOnly?.ToString().ToLower());
        parameters.AddOptional("closePosition", closePosition?.ToString().ToLower());
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));
        parameters.AddOptionalEnum("priceMatch", priceMatch);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptional("priceProtect", priceProtect?.ToString().ToUpper());

        return await RequestAsync<BinanceFuturesOrder>("ws-dapi/v1", $"order.place", parameters, true, true, weight: 0, ct: ct).ConfigureAwait(false);
    }

    public Task<CallResult<BinanceFuturesOrder>> ModifyOrderAsync(
        string symbol,
        BinanceOrderSide side,
        decimal quantity,
        decimal price,
        long? orderId = null,
        string? origClientOrderId = null,
        BinanceFuturesPriceMatch? priceMatch = null,
        long? modifyId = null,
        long? receiveWindow = null,
        CancellationToken ct = default)
    {
        var parameters = CreateModifyOrderParameters(
            symbol,
            side,
            quantity,
            price,
            orderId,
            origClientOrderId,
            priceMatch,
            modifyId,
            __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesOrder>(ModifyOrderPath, ModifyOrderMethod, parameters, true, true, weight: ModifyOrderIpWeight, ct: ct);
    }

    internal static ParameterCollection CreateModifyOrderParameters(
        string symbol,
        BinanceOrderSide side,
        decimal quantity,
        decimal price,
        long? orderId,
        string? origClientOrderId,
        BinanceFuturesPriceMatch? priceMatch,
        long? modifyId,
        long? receiveWindow)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("Symbol is required", nameof(symbol));
        if (side != BinanceOrderSide.Buy && side != BinanceOrderSide.Sell)
            throw new ArgumentOutOfRangeException(nameof(side), side, "Side must be Buy or Sell");
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), quantity, "Quantity must be greater than zero");
        if (price <= 0)
            throw new ArgumentOutOfRangeException(nameof(price), price, "Price must be greater than zero");
        if (origClientOrderId != null && string.IsNullOrWhiteSpace(origClientOrderId))
            throw new ArgumentException("Original client order id cannot be empty", nameof(origClientOrderId));
        if (!orderId.HasValue && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");
        if (priceMatch.HasValue)
            throw new ArgumentException("Binance currently requires price for Modify Order and also prohibits sending priceMatch with price", nameof(priceMatch));
        if (receiveWindow > 60_000)
            throw new ArgumentOutOfRangeException(nameof(receiveWindow), receiveWindow, "receiveWindow cannot exceed 60000 milliseconds");

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "quantity", quantity },
            { "price", price }
        };
        parameters.AddEnum("side", side);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("modifyId", modifyId);
        parameters.AddOptional("recvWindow", receiveWindow);

        return parameters;
    }

    public Task<CallResult<BinanceFuturesOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (!orderId.HasValue && string.IsNullOrEmpty(origClientOrderId))
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("orderId", orderId?.ToString(BinanceConstants.CI));
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesOrder>("ws-dapi/v1", $"order.cancel", parameters, true, true, weight: 1, ct: ct);
    }

    public Task<CallResult<BinanceFuturesOrder>> GetOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("orderId", orderId?.ToString(BinanceConstants.CI));
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesOrder>("ws-dapi/v1", $"order.status", parameters, true, true, weight: 1, ct: ct);
    }

    public Task<CallResult<List<BinanceFuturesCoinPosition>>> GetPositionsAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceFuturesCoinPosition>>("ws-dapi/v1", $"account.position", parameters, true, true, weight: 5, ct: ct);
    }
}
