using Binance.Api.Models.RestApi.Account;
using Binance.Api.Models.RestApi.Margin;

namespace Binance.Api.Clients.RestApi.Margin;

public class BinanceRestApiMarginTradingClient
{
    // Api
    protected const string marginApi = "sapi";
    protected const string marginVersion = "1";

    // Margin
    private const string marginTransferEndpoint = "margin/transfer";
    private const string marginBorrowEndpoint = "margin/loan";
    private const string marginRepayEndpoint = "margin/repay";
    private const string marginAssetEndpoint = "margin/asset";
    private const string marginPairEndpoint = "margin/pair";
    private const string marginAssetsEndpoint = "margin/allAssets";
    private const string marginPairsEndpoint = "margin/allPairs";
    private const string marginPriceIndexEndpoint = "margin/priceIndex";
    private const string newMarginOrderEndpoint = "margin/order";
    private const string cancelMarginOrderEndpoint = "margin/order";
    private const string cancelOpenMarginOrdersEndpoint = "margin/openOrders";
    private const string transferHistoryEndpoint = "margin/transfer";
    private const string getLoanEndpoint = "margin/loan";
    private const string getRepayEndpoint = "margin/repay";
    private const string interestHistoryEndpoint = "margin/interestHistory";
    private const string forceLiquidationHistoryEndpoint = "margin/forceLiquidationRec";
    private const string marginAccountInfoEndpoint = "margin/account";
    private const string queryMarginOrderEndpoint = "margin/order";
    private const string openMarginOrdersEndpoint = "margin/openOrders";
    private const string allMarginOrdersEndpoint = "margin/allOrders";

    // Margin OCO
    private const string newMarginOCOOrderEndpoint = "margin/order/oco";
    private const string cancelMarginOCOOrderEndpoint = "margin/orderList";
    private const string getMarginOCOOrderEndpoint = "margin/orderList";
    private const string allMarginOCOOrderEndpoint = "margin/allOrderList";
    private const string openMarginOCOOrderEndpoint = "margin/openOrderList";

    // Margin
    private const string myMarginTradesEndpoint = "margin/myTrades";
    private const string maxBorrowableEndpoint = "margin/maxBorrowable";
    private const string maxTransferableEndpoint = "margin/maxTransferable";
    private const string marginLevelInformation = "margin/tradeCoeff";

    // Isolated Margin
    private const string transferIsolatedMarginAccountEndpoint = "margin/isolated/transfer";
    private const string isolatedMarginTransferHistoryEndpoint = "margin/isolated/transfer";
    private const string isolatedMarginAccountEndpoint = "margin/isolated/account";
    private const string disableIsolatedMarginAccountEndpoint = "margin/isolated/account";
    private const string enableIsolatedMarginAccountEndpoint = "margin/isolated/account";
    private const string isolatedMarginAccountLimitEndpoint = "margin/isolated/accountLimit";
    private const string isolatedMarginSymbolEndpoint = "margin/isolated/pair";
    private const string isolatedMarginAllSymbolEndpoint = "margin/isolated/allPairs";

    // BNB Burn
    private const string toggleBnbBurnEndpoint = "bnbBurn";
    private const string getBnbBurnEndpoint = "bnbBurn";

    // Margin
    private const string interestRateHistoryEndpoint = "margin/interestRateHistory";
    private const string interestMarginDataEndpoint = "margin/crossMarginData";
    // TODO: Query Isolated Margin Fee Data (USER_DATA)
    private const string isolatedMargingTierEndpoint = "margin/isolatedMarginTier";
    private const string marginOrderRateLimitEndpoint = "margin/rateLimit/order";
    private const string marginDustLogEndpoint = "margin/dribblet";
    // TODO: Cross margin collateral ratio (MARKET_DATA)

    /// <summary>
    /// Event triggered when an order is placed via this client. Only available for Spot orders
    /// </summary>
    public event Action<OrderId> OnOrderPlaced;

    /// <summary>
    /// Event triggered when an order is canceled via this client. Note that this does not trigger when using CancelAllOrdersAsync. Only available for Spot orders
    /// </summary>
    public event Action<OrderId> OnOrderCanceled;

    // Internal References
    internal BinanceRestApiClient RootClient { get; }
    internal BinanceRestApiMarginClient MarginClient { get; }
    internal BinanceRestApiClientOptions Options { get => RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MarginClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await MarginClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiMarginTradingClient(BinanceRestApiClient root, BinanceRestApiMarginClient margin)
    {
        RootClient = root;
        MarginClient = margin;
    }

    internal void InvokeOrderPlaced(OrderId id)
        => OnOrderPlaced?.Invoke(id);

    internal void InvokeOrderCanceled(OrderId id)
        => OnOrderCanceled?.Invoke(id);

    #region Cross Margin Account Transfer
    public async Task<RestCallResult<BinanceTransaction>> CrossMarginTransferAsync(string asset, decimal quantity, TransferDirectionType type, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));
        var parameters = new Dictionary<string, object>
            {
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
                { "type", JsonConvert.SerializeObject(type, new TransferDirectionTypeConverter(false)) }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceTransaction>(GetUrl(marginTransferEndpoint, marginApi, marginVersion), HttpMethod.Post, ct, parameters, true, weight: 600).ConfigureAwait(false);
    }
    #endregion

    #region Margin Account Borrow
    public async Task<RestCallResult<BinanceTransaction>> MarginBorrowAsync(string asset, decimal quantity, bool? isIsolated = null, string symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));
        if (isIsolated == true && symbol == null)
            throw new ArgumentException("Symbol should be specified when using isolated margin");

        var parameters = new Dictionary<string, object>
            {
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
        parameters.AddOptionalParameter("isIsolated", isIsolated?.ToString().ToLower());
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceTransaction>(GetUrl(marginBorrowEndpoint, marginApi, marginVersion), HttpMethod.Post, ct, parameters, true, weight: 3000).ConfigureAwait(false);
    }
    #endregion

    #region Margin Account Repay
    public async Task<RestCallResult<BinanceTransaction>> MarginRepayAsync(string asset, decimal quantity, bool? isIsolated = null, string symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));
        var parameters = new Dictionary<string, object>
            {
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
        parameters.AddOptionalParameter("isIsolated", isIsolated?.ToString().ToLower());
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceTransaction>(GetUrl(marginRepayEndpoint, marginApi, marginVersion), HttpMethod.Post, ct, parameters, true, weight: 3000).ConfigureAwait(false);
    }
    #endregion

    #region Query Margin Asset
    public async Task<RestCallResult<BinanceMarginAsset>> GetMarginAssetAsync(string asset, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));

        var parameters = new Dictionary<string, object>
            {
                {"asset", asset}
            };

        return await SendRequestInternal<BinanceMarginAsset>(GetUrl(marginAssetEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Query Margin Pair
    public async Task<RestCallResult<BinanceMarginPair>> GetMarginSymbolAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateNotNull(nameof(symbol));

        var parameters = new Dictionary<string, object>
            {
                {"symbol", symbol}
            };

        return await SendRequestInternal<BinanceMarginPair>(GetUrl(marginPairEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Get All Margin Assets
    public async Task<RestCallResult<IEnumerable<BinanceMarginAsset>>> GetMarginAssetsAsync(CancellationToken ct = default)
    {
        return await SendRequestInternal<IEnumerable<BinanceMarginAsset>>(GetUrl(marginAssetsEndpoint, marginApi, marginVersion), HttpMethod.Get, ct).ConfigureAwait(false);
    }
    #endregion

    #region Get All Cross Margin Pairs
    public async Task<RestCallResult<IEnumerable<BinanceMarginPair>>> GetMarginSymbolsAsync(CancellationToken ct = default)
    {
        return await SendRequestInternal<IEnumerable<BinanceMarginPair>>(GetUrl(marginPairsEndpoint, marginApi, marginVersion), HttpMethod.Get, ct).ConfigureAwait(false);
    }
    #endregion

    #region Query Margin PriceIndex
    public async Task<RestCallResult<BinanceMarginPriceIndex>> GetMarginPriceIndexAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateNotNull(nameof(symbol));

        var parameters = new Dictionary<string, object>
            {
                {"symbol", symbol}
            };

        return await SendRequestInternal<BinanceMarginPriceIndex>(GetUrl(marginPriceIndexEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Margin Account New Order
    public async Task<RestCallResult<BinancePlacedOrder>> PlaceMarginOrderAsync(string symbol,
        OrderSide side,
        SpotOrderType type,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        string newClientOrderId = null,
        decimal? price = null,
        TimeInForce? timeInForce = null,
        decimal? stopPrice = null,
        decimal? icebergQuantity = null,
        SideEffectType? sideEffectType = null,
        bool? isIsolated = null,
        OrderResponseType? orderResponseType = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        var result = await MarginClient.PlaceOrderInternal(GetUrl(newMarginOrderEndpoint, marginApi, marginVersion),
            symbol,
            side,
            type,
            quantity,
            quoteQuantity,
            newClientOrderId,
            price,
            timeInForce,
            stopPrice,
            icebergQuantity,
            sideEffectType,
            isIsolated,
            orderResponseType,
            null,
            receiveWindow,
            weight: 6,
            ct).ConfigureAwait(false);

        if (result)
            InvokeOrderPlaced(new OrderId { Id = result.Data.Id.ToString(CultureInfo.InvariantCulture) });
        return result;
    }
    #endregion

    #region Margin Account Cancel Order
    public async Task<RestCallResult<BinanceOrderBase>> CancelMarginOrderAsync(string symbol, long? orderId = null, string origClientOrderId = null, string newClientOrderId = null, bool? isIsolated = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        if (!orderId.HasValue && string.IsNullOrEmpty(origClientOrderId))
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptionalParameter("isIsolated", isIsolated);
        parameters.AddOptionalParameter("newClientOrderId", newClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceOrderBase>(GetUrl(cancelMarginOrderEndpoint, marginApi, marginVersion), HttpMethod.Delete, ct, parameters, true, weight: 10).ConfigureAwait(false);
        if (result)
            InvokeOrderCanceled(new OrderId { Id = result.Data.Id.ToString(CultureInfo.InvariantCulture) });
        return result;
    }
    #endregion

    #region Margin Account Cancel All Open Orders
    public async Task<RestCallResult<IEnumerable<BinanceOrderBase>>> CancelAllMarginOrdersAsync(string symbol, bool? isIsolated = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("isIsolated", isIsolated);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceOrderBase>>(GetUrl(cancelOpenMarginOrdersEndpoint, marginApi, marginVersion), HttpMethod.Delete, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Get Transfer History
    public async Task<RestCallResult<BinanceQueryRecords<BinanceTransferHistory>>> GetCrossMarginTransferHistoryAsync(TransferDirection direction, int? page = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 100);

        var parameters = new Dictionary<string, object>
            {
                { "direction", JsonConvert.SerializeObject(direction, new TransferDirectionConverter(false)) }
            };
        parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("current", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceTransferHistory>>(GetUrl(transferHistoryEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Query Loan Record
    public async Task<RestCallResult<BinanceQueryRecords<BinanceLoan>>> GetMarginLoansAsync(string asset, long? transactionId = null, DateTime? startTime = null, DateTime? endTime = null, int? current = 1, int? limit = 10, string isolatedSymbol = null, bool? archived = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));
        limit?.ValidateIntBetween(nameof(limit), 1, 100);
        var parameters = new Dictionary<string, object>
            {
                { "asset", asset }
            };
        parameters.AddOptionalParameter("txId", transactionId?.ToString(CultureInfo.InvariantCulture));

        // TxId or startTime must be sent. txId takes precedence.
        if (!transactionId.HasValue)
        {
            parameters.AddOptionalParameter("startTime", (startTime ?? DateTime.MinValue).ConvertToMilliseconds());
        }
        else
        {
            parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        }

        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("current", current?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("archived", archived);
        parameters.AddOptionalParameter("isolatedSymbol", isolatedSymbol);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceLoan>>(GetUrl(getLoanEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Query Repay Record
    public async Task<RestCallResult<BinanceQueryRecords<BinanceRepay>>> GetMarginRepaysAsync(string asset, long? transactionId = null, DateTime? startTime = null, DateTime? endTime = null, int? current = null, int? size = null, string isolatedSymbol = null, bool? archived = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));
        var parameters = new Dictionary<string, object>
            {
                { "asset", asset }
            };
        parameters.AddOptionalParameter("txId", transactionId?.ToString(CultureInfo.InvariantCulture));

        // TxId or startTime must be sent. txId takes precedence.
        if (!transactionId.HasValue)
        {
            parameters.AddOptionalParameter("startTime", (startTime ?? DateTime.MinValue).ConvertToMilliseconds());
        }
        else
        {
            parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        }

        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("current", current?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("size", size?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("isolatedSymbol", isolatedSymbol);
        parameters.AddOptionalParameter("archived", archived);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceRepay>>(GetUrl(getRepayEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Get Interest History
    public async Task<RestCallResult<BinanceQueryRecords<BinanceInterestHistory>>> GetMarginInterestHistoryAsync(string asset = null, int? page = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, string isolatedSymbol = null, bool? archived = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 100);
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("asset", asset);
        parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("current", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("isolatedSymbol", isolatedSymbol);
        parameters.AddOptionalParameter("archived", archived);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceInterestHistory>>(GetUrl(interestHistoryEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Get Force Liquidation Record
    public async Task<RestCallResult<BinanceQueryRecords<BinanceForcedLiquidation>>> GetMarginForcedLiquidationHistoryAsync(int? page = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, string isolatedSymbol = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 100);

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("isolatedSymbol", isolatedSymbol);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceForcedLiquidation>>(GetUrl(forceLiquidationHistoryEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Query Margin Account Details
    public async Task<RestCallResult<BinanceMarginAccount>> GetMarginAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceMarginAccount>(GetUrl(marginAccountInfoEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Query Margin Account's Order
    public async Task<RestCallResult<BinanceOrder>> GetMarginOrderAsync(string symbol, long? orderId = null, string origClientOrderId = null, bool? isIsolated = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId should be provided");

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("isIsolated", isIsolated);
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceOrder>(GetUrl(queryMarginOrderEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Query Margin Account's Open Order
    public async Task<RestCallResult<IEnumerable<BinanceOrder>>> GetOpenMarginOrdersAsync(string symbol = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol?.ValidateBinanceSymbol();
        if (isIsolated == true && symbol == null)
            throw new ArgumentException("Symbol must be provided for isolated margin");

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("isIsolated", isIsolated);

        return await SendRequestInternal<IEnumerable<BinanceOrder>>(GetUrl(openMarginOrdersEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Query Margin Account's All Order
    public async Task<RestCallResult<IEnumerable<BinanceOrder>>> GetMarginOrdersAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("isIsolated", isIsolated);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceOrder>>(GetUrl(allMarginOrdersEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 200).ConfigureAwait(false);
    }
    #endregion

    #region Margin Account New OCO Order
    public async Task<RestCallResult<BinanceMarginOrderOcoList>> PlaceMarginOCOOrderAsync(string symbol,
        OrderSide side,
        decimal price,
        decimal stopPrice,
        decimal quantity,
        decimal? stopLimitPrice = null,
        TimeInForce? stopLimitTimeInForce = null,
        decimal? stopIcebergQuantity = null,
        decimal? limitIcebergQuantity = null,
        SideEffectType? sideEffectType = null,
        bool? isIsolated = null,
        string listClientOrderId = null,
        string limitClientOrderId = null,
        string stopClientOrderId = null,
        OrderResponseType? orderResponseType = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var rulesCheck = await MarginClient.CheckTradeRules(symbol, quantity, null, price, stopPrice, null, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            MarginClient.Log.Write(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceMarginOrderOcoList>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity!.Value;
        price = rulesCheck.Price!.Value;
        stopPrice = rulesCheck.StopPrice!.Value;

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "side", JsonConvert.SerializeObject(side, new OrderSideConverter(false)) },
                { "quantity", quantity.ToString(CultureInfo.InvariantCulture) },
                { "price", price.ToString(CultureInfo.InvariantCulture) },
                { "stopPrice", stopPrice.ToString(CultureInfo.InvariantCulture) }
            };
        parameters.AddOptionalParameter("stopLimitPrice", stopLimitPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("isIsolated", isIsolated?.ToString());
        parameters.AddOptionalParameter("sideEffectType", sideEffectType == null ? null : JsonConvert.SerializeObject(sideEffectType, new SideEffectTypeConverter(false)));
        parameters.AddOptionalParameter("listClientOrderId", listClientOrderId);
        parameters.AddOptionalParameter("limitClientOrderId", limitClientOrderId);
        parameters.AddOptionalParameter("stopClientOrderId", stopClientOrderId);
        parameters.AddOptionalParameter("limitIcebergQty", limitIcebergQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("newOrderRespType", orderResponseType == null ? null : JsonConvert.SerializeObject(orderResponseType, new OrderResponseTypeConverter(false)));
        parameters.AddOptionalParameter("stopIcebergQty", stopIcebergQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("stopLimitTimeInForce", stopLimitTimeInForce == null ? null : JsonConvert.SerializeObject(stopLimitTimeInForce, new TimeInForceConverter(false)));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceMarginOrderOcoList>(GetUrl(newMarginOCOOrderEndpoint, marginApi, marginVersion), HttpMethod.Post, ct, parameters, true, weight: 6).ConfigureAwait(false);
    }
    #endregion

    #region Cancel OCO 
    public async Task<RestCallResult<BinanceMarginOrderOcoList>> CancelMarginOcoOrderAsync(string symbol, bool? isIsolated = null, long? orderListId = null, string listClientOrderId = null, string newClientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        if (!orderListId.HasValue && string.IsNullOrEmpty(listClientOrderId))
            throw new ArgumentException("Either orderListId or listClientOrderId must be sent");

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("isIsolated", isIsolated?.ToString());
        parameters.AddOptionalParameter("orderListId", orderListId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("listClientOrderId", listClientOrderId);
        parameters.AddOptionalParameter("newClientOrderId", newClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceMarginOrderOcoList>(GetUrl(cancelMarginOCOOrderEndpoint, marginApi, marginVersion), HttpMethod.Delete, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Query OCO
    public async Task<RestCallResult<BinanceMarginOrderOcoList>> GetMarginOcoOrderAsync(string symbol = null, bool? isIsolated = null, long? orderListId = null, string origClientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderListId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderListId or origClientOrderId must be sent");

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("isIsolated", isIsolated.ToString());
        parameters.AddOptionalParameter("orderListId", orderListId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceMarginOrderOcoList>(GetUrl(getMarginOCOOrderEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Query all OCO
    public async Task<RestCallResult<IEnumerable<BinanceMarginOrderOcoList>>> GetMarginOcoOrdersAsync(string symbol = null, bool? isIsolated = null, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (fromId != null && (startTime != null || endTime != null))
            throw new ArgumentException("Start/end time can only be provided without fromId parameter");

        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("isIsolated", isIsolated?.ToString());
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceMarginOrderOcoList>>(GetUrl(allMarginOCOOrderEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 200).ConfigureAwait(false);
    }
    #endregion

    #region Query Open OCO
    public async Task<RestCallResult<IEnumerable<BinanceMarginOrderOcoList>>> GetMarginOpenOcoOrdersAsync(string symbol = null, bool? isIsolated = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("isIsolated", isIsolated?.ToString());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceMarginOrderOcoList>>(GetUrl(openMarginOCOOrderEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Query Margin Account's Trade List
    public async Task<RestCallResult<IEnumerable<BinanceTrade>>> GetMarginUserTradesAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, bool? isIsolated = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("isIsolated", isIsolated);
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceTrade>>(GetUrl(myMarginTradesEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Query Max Borrow
    public async Task<RestCallResult<BinanceMarginAmount>> GetMarginMaxBorrowAmountAsync(string asset, string isolatedSymbol = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));

        var parameters = new Dictionary<string, object>
            {
                { "asset", asset }
            };

        parameters.AddOptionalParameter("isolatedSymbol", isolatedSymbol);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceMarginAmount>(GetUrl(maxBorrowableEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 50).ConfigureAwait(false);
    }
    #endregion

    #region Query Max Transfer-Out Amount
    public async Task<RestCallResult<decimal>> GetMarginMaxTransferAmountAsync(string asset, string isolatedSymbol = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));
        var parameters = new Dictionary<string, object>
            {
                { "asset", asset }
            };

        parameters.AddOptionalParameter("isolatedSymbol", isolatedSymbol);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceMarginAmount>(GetUrl(maxTransferableEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 50).ConfigureAwait(false);

        if (!result)
            return result.As<decimal>(default);

        return result.As(result.Data.Quantity);
    }
    #endregion

    #region Margin Level Information
    public async Task<RestCallResult<BinanceMarginLevel>> GetMarginLevelInformationAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));

        var parameters = new Dictionary<string, object>
            {
                { "email", email },
            };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceMarginLevel>(GetUrl(marginLevelInformation, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Isolated Margin Account Transfer
    public async Task<RestCallResult<BinanceTransaction>> IsolatedMarginAccountTransferAsync(string asset,
        string symbol, IsolatedMarginTransferDirection from, IsolatedMarginTransferDirection to, decimal quantity,
        int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));
        symbol.ValidateNotNull(nameof(symbol));

        var parameters = new Dictionary<string, object>
        {
            {"asset", asset},
            {"symbol", symbol},
            {"transFrom", JsonConvert.SerializeObject(from, new IsolatedMarginTransferDirectionConverter(false))},
            {"transTo", JsonConvert.SerializeObject(to, new IsolatedMarginTransferDirectionConverter(false))},
            {"amount", quantity.ToString(CultureInfo.InvariantCulture)}
        };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceTransaction>(GetUrl(transferIsolatedMarginAccountEndpoint, "sapi", "1"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Get Isolated Margin Transfer History
    public async Task<RestCallResult<BinanceQueryRecords<BinanceIsolatedMarginTransfer>>>
        GetIsolatedMarginAccountTransferHistoryAsync(string symbol, string asset = null,
            IsolatedMarginTransferDirection? from = null, IsolatedMarginTransferDirection? to = null,
            DateTime? startTime = null, DateTime? endTime = null, int? current = 1, int? limit = 10,
            int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var parameters = new Dictionary<string, object>
        {
            {"symbol", symbol}
        };

        parameters.AddOptionalParameter("asset", asset);
        parameters.AddOptionalParameter("from", !from.HasValue ? null : JsonConvert.SerializeObject(from, new IsolatedMarginTransferDirectionConverter(false)));
        parameters.AddOptionalParameter("to", !to.HasValue ? null : JsonConvert.SerializeObject(to, new IsolatedMarginTransferDirectionConverter(false)));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds()?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds()?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("current", current?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceIsolatedMarginTransfer>>(GetUrl(isolatedMarginTransferHistoryEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 600).ConfigureAwait(false);
    }
    #endregion

    #region Query isolated margin account
    public async Task<RestCallResult<BinanceIsolatedMarginAccount>> GetIsolatedMarginAccountAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceIsolatedMarginAccount>(GetUrl(isolatedMarginAccountEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Disable Isolated Margin Account
    public async Task<RestCallResult<BinanceCreateIsolatedMarginAccountResult>> DisableIsolatedMarginAccountAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            {"symbol", symbol}
        };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceCreateIsolatedMarginAccountResult>(GetUrl(disableIsolatedMarginAccountEndpoint, "sapi", "1"), HttpMethod.Delete, ct, parameters, true, weight: 300).ConfigureAwait(false);
    }
    #endregion

    #region Enable Isolated Margin Account
    public async Task<RestCallResult<BinanceCreateIsolatedMarginAccountResult>> EnableIsolatedMarginAccountAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                {"symbol", symbol}
            };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceCreateIsolatedMarginAccountResult>(GetUrl(enableIsolatedMarginAccountEndpoint, "sapi", "1"), HttpMethod.Post, ct, parameters, true, weight: 300).ConfigureAwait(false);
    }
    #endregion

    #region Query Enabled Isolated Margin Account Limit
    public async Task<RestCallResult<BinanceIsolatedMarginAccountLimit>> GetEnabledIsolatedMarginAccountLimitAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceIsolatedMarginAccountLimit>(GetUrl(isolatedMarginAccountLimitEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Query Isolated Margin Symbol 
    public async Task<RestCallResult<BinanceIsolatedMarginSymbol>> GetIsolatedMarginSymbolAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var parameters = new Dictionary<string, object>
            {
                {"symbol", symbol}
            };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceIsolatedMarginSymbol>(GetUrl(isolatedMarginSymbolEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Get All Isolated Margin Symbol
    public async Task<RestCallResult<IEnumerable<BinanceIsolatedMarginSymbol>>> GetIsolatedMarginSymbolsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceIsolatedMarginSymbol>>(GetUrl(isolatedMarginAllSymbolEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Toggle BNB Burn On Spot Trade And Margin Interest
    public async Task<RestCallResult<BinanceBnbBurnStatus>> SetBnbBurnStatusAsync(bool? spotTrading = null, bool? marginInterest = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (spotTrading == null && marginInterest == null)
            throw new ArgumentException("SpotTrading or MarginInterest should be provided");

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("spotBNBBurn", spotTrading);
        parameters.AddOptionalParameter("interestBNBBurn", marginInterest);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBnbBurnStatus>(GetUrl(toggleBnbBurnEndpoint, "sapi", "1"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Get BNB Burn Status
    public async Task<RestCallResult<BinanceBnbBurnStatus>> GetBnbBurnStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBnbBurnStatus>(GetUrl(getBnbBurnEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Get Interest Rate History
    public async Task<RestCallResult<IEnumerable<BinanceInterestRateHistory>>> GetMarginInterestRateHistoryAsync(string asset, string vipLevel = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        asset?.ValidateNotNull(nameof(asset));
        limit?.ValidateIntBetween(nameof(limit), 1, 100);

        var parameters = new Dictionary<string, object>
            {
                { "asset", asset! }
            };
        parameters.AddOptionalParameter("vipLevel", vipLevel?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceInterestRateHistory>>(GetUrl(interestRateHistoryEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Get Cross Margin Interest Data
    public async Task<RestCallResult<IEnumerable<BinanceInterestMarginData>>> GetInterestMarginDataAsync(string asset = null, string vipLevel = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        asset?.ValidateNotNull(nameof(asset));

        var parameters = new Dictionary<string, object>();

        parameters.AddOptionalParameter("coin", asset);
        parameters.AddOptionalParameter("vipLevel", vipLevel?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceInterestMarginData>>(GetUrl(interestMarginDataEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Get Isolated margin tier data
    public async Task<RestCallResult<IEnumerable<BinanceIsolatedMarginTierData>>> GetIsolatedMarginTierDataAsync(string symbol, int? tier = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("tier", tier);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceIsolatedMarginTierData>>(GetUrl(isolatedMargingTierEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Margin order rate limit
    public async Task<RestCallResult<IEnumerable<BinanceOrderRateLimit>>> GetMarginOrderRateLimitStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceOrderRateLimit>>(GetUrl(marginOrderRateLimitEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true, weight: 20).ConfigureAwait(false);
    }
    #endregion

    #region Margin DustLog
    public async Task<RestCallResult<BinanceDustLogList>> GetMarginDustLogAsync(DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        var result = await SendRequestInternal<BinanceDustLogList>(GetUrl(marginDustLogEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        return result;
    }
    #endregion

}