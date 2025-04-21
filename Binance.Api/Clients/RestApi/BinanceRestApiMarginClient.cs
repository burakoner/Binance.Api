using Binance.Api.Margin.Enums;

namespace Binance.Api.Clients.RestApi;

public class BinanceRestApiMarginClient : RestApiClient
{
    // Api
    protected const string v1 = "1";
    protected const string v3 = "3";
    protected const string api = "api";
    protected const string sapi = "sapi";

    // Server
    private const string pingEndpoint = "ping";
    private const string checkTimeEndpoint = "time";
    private const string systemStatusEndpoint = "system/status";
    private const string exchangeInfoEndpoint = "exchangeInfo";

    // Trading
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
    private const string newMarginOCOOrderEndpoint = "margin/order/oco";
    private const string cancelMarginOCOOrderEndpoint = "margin/orderList";
    private const string getMarginOCOOrderEndpoint = "margin/orderList";
    private const string allMarginOCOOrderEndpoint = "margin/allOrderList";
    private const string openMarginOCOOrderEndpoint = "margin/openOrderList";
    private const string myMarginTradesEndpoint = "margin/myTrades";
    private const string maxBorrowableEndpoint = "margin/maxBorrowable";
    private const string maxTransferableEndpoint = "margin/maxTransferable";
    private const string marginLevelInformation = "margin/tradeCoeff";
    private const string transferIsolatedMarginAccountEndpoint = "margin/isolated/transfer";
    private const string isolatedMarginTransferHistoryEndpoint = "margin/isolated/transfer";
    private const string isolatedMarginAccountEndpoint = "margin/isolated/account";
    private const string disableIsolatedMarginAccountEndpoint = "margin/isolated/account";
    private const string enableIsolatedMarginAccountEndpoint = "margin/isolated/account";
    private const string isolatedMarginAccountLimitEndpoint = "margin/isolated/accountLimit";
    private const string isolatedMarginSymbolEndpoint = "margin/isolated/pair";
    private const string isolatedMarginAllSymbolEndpoint = "margin/isolated/allPairs";
    private const string toggleBnbBurnEndpoint = "bnbBurn";
    private const string getBnbBurnEndpoint = "bnbBurn";
    private const string interestRateHistoryEndpoint = "margin/interestRateHistory";
    private const string interestMarginDataEndpoint = "margin/crossMarginData";
    // TODO: Query Isolated Margin Fee Data (USER_DATA)
    private const string isolatedMargingTierEndpoint = "margin/isolatedMarginTier";
    private const string marginOrderRateLimitEndpoint = "margin/rateLimit/order";
    private const string marginDustLogEndpoint = "margin/dribblet";
    // TODO: Cross margin collateral ratio (MARKET_DATA)

    // User Data Stream - Cross Margin
    private const string crossMarginCreateListenKeyEndpoint = "userDataStream";
    private const string crossMarginUpdateListenKeyEndpoint = "userDataStream";
    private const string crossMarginDeleteListenKeyEndpoint = "userDataStream";

    // User Data Stream - Isolated Margin
    private const string isolatedMarginCreateListenKeyEndpoint = "userDataStream/isolated";
    private const string isolatedMarginUpdateListenKeyEndpoint = "userDataStream/isolated";
    private const string isolatedMarginDeleteListenKeyEndpoint = "userDataStream/isolated";

    // Portfolio Margin
    private const string portfolioMarginAccountEndpoint = "portfolio/account";
    private const string portfolioMarginCollateralRateEndpoint = "portfolio/collateralRate";
    private const string portfolioMarginLoanEndpoint = "portfolio/pmLoan";
    private const string portfolioMarginRepayEndpoint = "portfolio/repay";

    // Internal
    internal ILogger Logger { get => this._logger; }
    internal BinanceExchangeInfo ExchangeInfo;
    internal DateTime? LastExchangeInfoUpdate;
    internal TimeSyncState TimeSyncState = new("Binance Margin RestApi");

    // Root Client
    internal BinanceRestApiClient RootClient { get; }

    // Options
    public new BinanceRestApiClientOptions ClientOptions { get { return (BinanceRestApiClientOptions)base.ClientOptions; } }

    /// <summary>
    /// Event triggered when an order is placed via this client. Only available for Spot orders
    /// </summary>
    public event Action<long> OnOrderPlaced;

    /// <summary>
    /// Event triggered when an order is canceled via this client. Note that this does not trigger when using CancelAllOrdersAsync. Only available for Spot orders
    /// </summary>
    public event Action<long> OnOrderCanceled;

    internal BinanceRestApiMarginClient(BinanceRestApiClient root) : base(root.Logger, root.Options)
    {
        RootClient = root;

        RequestBodyFormat = RestRequestBodyFormat.FormData;
        ArraySerialization = ArraySerialization.MultipleValues;
    }

    #region Overrided Methods
    protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
        => new BinanceAuthenticationProvider(credentials);

    protected override Error ParseErrorResponse(JToken error)
    {
        if (!error.HasValues)
            return new ServerError(error.ToString());

        if (error["msg"] == null && error["code"] == null)
            return new ServerError(error.ToString());

        if (error["msg"] != null && error["code"] == null)
            return new ServerError((string)error["msg"]!);

        return new ServerError((int)error["code"]!, (string)error["msg"]!);
    }

    protected override Task<RestCallResult<DateTime>> GetServerTimestampAsync()
        => RootClient.Spot.GetTimeAsync();

    protected override TimeSyncInfo GetTimeSyncInfo()
        => new(Logger, ClientOptions.AutoTimestamp, ClientOptions.TimestampRecalculationInterval, TimeSyncState);

    protected override TimeSpan GetTimeOffset()
        => TimeSyncState.TimeOffset;
    #endregion

    #region Internal Methods
    internal void InvokeOrderPlaced(long id)
        => OnOrderPlaced?.Invoke(id);

    internal void InvokeOrderCanceled(long id)
        => OnOrderCanceled?.Invoke(id);

    internal string GetSymbolName(string baseAsset, string quoteAsset) =>
        (baseAsset + quoteAsset).ToUpper(CultureInfo.InvariantCulture);

    internal Uri GetUrl(string endpoint, string api, string version = null)
    {
        var result = ClientOptions.BaseAddress.AppendPath(api);

        if (!string.IsNullOrEmpty(version))
            result = result.AppendPath($"v{version}");

        return new Uri(result.AppendPath(endpoint));
    }

    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
    {
        var result = await SendRequestAsync<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight).ConfigureAwait(false);
        if (!result && result.Error!.Code == -1021 && ClientOptions.AutoTimestamp)
        {
            Logger.Log(LogLevel.Debug, "Received Invalid Timestamp error, triggering new time sync");
            TimeSyncState.LastSyncTime = DateTime.MinValue;
        }
        return result;
    }

    internal async Task<RestCallResult<BinancePlacedOrder>> PlaceOrderInternal(Uri uri,
        string symbol,
        BinanceSpotOrderSide side,
        BinanceSpotOrderType type,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        string newClientOrderId = null,
        decimal? price = null,
        BinanceSpotTimeInForce? timeInForce = null,
        decimal? stopPrice = null,
        decimal? icebergQty = null,
        BinanceSideEffectType? sideEffectType = null,
        bool? isIsolated = null,
        BinanceSpotOrderResponseType? orderResponseType = null,
        int? trailingDelta = null,
        int? receiveWindow = null,
        int weight = 1,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        if (quoteQuantity != null && type != BinanceSpotOrderType.Market)
            throw new ArgumentException("quoteQuantity is only valid for market orders");

        if (quantity == null && quoteQuantity == null || quantity != null && quoteQuantity != null)
            throw new ArgumentException("1 of either should be specified, quantity or quoteOrderQuantity");

        var rulesCheck = await CheckTradeRules(symbol, quantity, quoteQuantity, price, stopPrice, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinancePlacedOrder>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;
        quoteQuantity = rulesCheck.QuoteQuantity;

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "side", JsonConvert.SerializeObject(side, new OrderSideConverter(false)) },
                { "type", JsonConvert.SerializeObject(type, new SpotOrderTypeConverter(false)) }
            };
        parameters.AddOptionalParameter("quantity", quantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("quoteOrderQty", quoteQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("newClientOrderId", newClientOrderId);
        parameters.AddOptionalParameter("price", price?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("timeInForce", timeInForce == null ? null : JsonConvert.SerializeObject(timeInForce, new TimeInForceConverter(false)));
        parameters.AddOptionalParameter("stopPrice", stopPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("icebergQty", icebergQty?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("sideEffectType", sideEffectType == null ? null : JsonConvert.SerializeObject(sideEffectType, new SideEffectTypeConverter(false)));
        parameters.AddOptionalParameter("isIsolated", isIsolated);
        parameters.AddOptionalParameter("newOrderRespType", orderResponseType == null ? null : JsonConvert.SerializeObject(orderResponseType, new OrderResponseTypeConverter(false)));
        parameters.AddOptionalParameter("trailingDelta", trailingDelta);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinancePlacedOrder>(uri, HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: weight).ConfigureAwait(false);
    }

    internal async Task<BinanceTradeRuleResult> CheckTradeRules(string symbol, decimal? quantity, decimal? quoteQuantity, decimal? price, decimal? stopPrice, BinanceSpotOrderType? type, CancellationToken ct)
    {
        var outputQuantity = quantity;
        var outputQuoteQuantity = quoteQuantity;
        var outputPrice = price;
        var outputStopPrice = stopPrice;

        if (ClientOptions.SpotOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.None)
            return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, outputPrice, outputStopPrice);

        if (ExchangeInfo == null || LastExchangeInfoUpdate == null || (DateTime.UtcNow - LastExchangeInfoUpdate.Value).TotalMinutes > ClientOptions.SpotOptions.TradeRulesUpdateInterval.TotalMinutes)
            await RootClient.Spot.GetExchangeInfoAsync(ct).ConfigureAwait(false);

        if (ExchangeInfo == null)
            return BinanceTradeRuleResult.CreateFailed("Unable to retrieve trading rules, validation failed");

        var symbolData = ExchangeInfo.Symbols.SingleOrDefault(s => string.Equals(s.Symbol, symbol, StringComparison.CurrentCultureIgnoreCase));
        if (symbolData == null)
            return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Symbol {symbol} not found");

        if (type != null)
        {
            if (!symbolData.OrderTypes.Contains(type.Value))
            {
                return BinanceTradeRuleResult.CreateFailed(
                    $"Trade rules check failed: {type} order type not allowed for {symbol}");
            }
        }

        if (symbolData.LotSizeFilter != null || symbolData.MarketLotSizeFilter != null && type == BinanceSpotOrderType.Market)
        {
            var minQty = symbolData.LotSizeFilter?.MinQuantity;
            var maxQty = symbolData.LotSizeFilter?.MaxQuantity;
            var stepSize = symbolData.LotSizeFilter?.StepSize;
            if (type == BinanceSpotOrderType.Market && symbolData.MarketLotSizeFilter != null)
            {
                minQty = symbolData.MarketLotSizeFilter.MinQuantity;
                if (symbolData.MarketLotSizeFilter.MaxQuantity != 0)
                    maxQty = symbolData.MarketLotSizeFilter.MaxQuantity;

                if (symbolData.MarketLotSizeFilter.StepSize != 0)
                    stepSize = symbolData.MarketLotSizeFilter.StepSize;
            }

            if (minQty.HasValue && quantity.HasValue)
            {
                outputQuantity = BinanceHelpers.ClampQuantity(minQty.Value, maxQty!.Value, stepSize!.Value, quantity.Value);
                if (outputQuantity != quantity.Value)
                {
                    if (ClientOptions.SpotOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                    {
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: LotSize filter failed. Original quantity: {quantity}, Closest allowed: {outputQuantity}");
                    }

                    Logger.Log(LogLevel.Information, $"Quantity clamped from {quantity} to {outputQuantity} based on lot size filter");
                }
            }
        }

        if (symbolData.MinNotionalFilter != null && outputQuoteQuantity != null)
        {
            if (quoteQuantity < symbolData.MinNotionalFilter.MinNotional)
            {
                if (ClientOptions.SpotOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                {
                    return BinanceTradeRuleResult.CreateFailed(
                        $"Trade rules check failed: MinNotional filter failed. Order value: {quoteQuantity}, minimal order value: {symbolData.MinNotionalFilter.MinNotional}");
                }

                outputQuoteQuantity = symbolData.MinNotionalFilter.MinNotional;
                Logger.Log(LogLevel.Information, $"QuoteQuantity adjusted from {quoteQuantity} to {outputQuoteQuantity} based on min notional filter");
            }
        }

        if (price == null)
            return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, null, outputStopPrice);

        if (symbolData.PriceFilter != null)
        {
            if (symbolData.PriceFilter.MaxPrice != 0 && symbolData.PriceFilter.MinPrice != 0)
            {
                outputPrice = BinanceHelpers.ClampPrice(symbolData.PriceFilter.MinPrice, symbolData.PriceFilter.MaxPrice, price.Value);
                if (outputPrice != price)
                {
                    if (ClientOptions.SpotOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter max/min failed. Original price: {price}, Closest allowed: {outputPrice}");

                    Logger.Log(LogLevel.Information, $"price clamped from {price} to {outputPrice} based on price filter");
                }

                if (stopPrice != null)
                {
                    outputStopPrice = BinanceHelpers.ClampPrice(symbolData.PriceFilter.MinPrice,
                        symbolData.PriceFilter.MaxPrice, stopPrice.Value);
                    if (outputStopPrice != stopPrice)
                    {
                        if (ClientOptions.SpotOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                        {
                            return BinanceTradeRuleResult.CreateFailed(
                                $"Trade rules check failed: Stop price filter max/min failed. Original stop price: {stopPrice}, Closest allowed: {outputStopPrice}");
                        }

                        Logger.Log(LogLevel.Information,
                            $"Stop price clamped from {stopPrice} to {outputStopPrice} based on price filter");
                    }
                }
            }

            if (symbolData.PriceFilter.TickSize != 0)
            {
                var beforePrice = outputPrice;
                outputPrice = BinanceHelpers.FloorPrice(symbolData.PriceFilter.TickSize, price.Value);
                if (outputPrice != beforePrice)
                {
                    if (ClientOptions.SpotOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter tick failed. Original price: {price}, Closest allowed: {outputPrice}");

                    Logger.Log(LogLevel.Information, $"price floored from {beforePrice} to {outputPrice} based on price filter");
                }

                if (stopPrice != null)
                {
                    var beforeStopPrice = outputStopPrice;
                    outputStopPrice = BinanceHelpers.FloorPrice(symbolData.PriceFilter.TickSize, stopPrice.Value);
                    if (outputStopPrice != beforeStopPrice)
                    {
                        if (ClientOptions.SpotOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                        {
                            return BinanceTradeRuleResult.CreateFailed(
                                $"Trade rules check failed: Stop price filter tick failed. Original stop price: {stopPrice}, Closest allowed: {outputStopPrice}");
                        }

                        Logger.Log(LogLevel.Information,
                            $"Stop price floored from {beforeStopPrice} to {outputStopPrice} based on price filter");
                    }
                }
            }
        }

        if (symbolData.MinNotionalFilter == null || quantity == null || outputPrice == null)
            return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, outputPrice, outputStopPrice);

        var currentQuantity = outputQuantity ?? quantity.Value;
        var notional = currentQuantity * outputPrice.Value;
        if (notional < symbolData.MinNotionalFilter.MinNotional)
        {
            if (ClientOptions.SpotOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
            {
                return BinanceTradeRuleResult.CreateFailed(
                    $"Trade rules check failed: MinNotional filter failed. Order quantity: {notional}, minimal order quantity: {symbolData.MinNotionalFilter.MinNotional}");
            }

            if (symbolData.LotSizeFilter == null)
                return BinanceTradeRuleResult.CreateFailed("Trade rules check failed: MinNotional filter failed. Unable to auto comply because LotSizeFilter not present");

            var minQuantity = symbolData.MinNotionalFilter.MinNotional / outputPrice.Value;
            var stepSize = symbolData.LotSizeFilter!.StepSize;
            outputQuantity = BinanceHelpers.Floor(minQuantity + (stepSize - minQuantity % stepSize));
            Logger.Log(LogLevel.Information, $"Quantity clamped from {currentQuantity} to {outputQuantity} based on min notional filter");
        }

        return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, outputPrice, outputStopPrice);
    }
    #endregion

    #region Server Methods

    #region Ping
    public async Task<RestCallResult<long>> PingAsync(CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        var result = await SendRequestInternal<object>(GetUrl(pingEndpoint, api, v3), HttpMethod.Get, ct).ConfigureAwait(false);
        sw.Stop();
        return result ? result.As(sw.ElapsedMilliseconds) : result.As<long>(default!);
    }
    #endregion

    #region Server Time
    public async Task<RestCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
    {
        var result = await SendRequestInternal<BinanceServerTime>(GetUrl(checkTimeEndpoint, api, v3), HttpMethod.Get, ct, ignoreRatelimit: true).ConfigureAwait(false);
        return result.As(result.Data?.ServerTime ?? default);
    }
    #endregion

    #region System Status
    public async Task<RestCallResult<BinanceSystemStatus>> GetSystemStatusAsync(CancellationToken ct = default)
    {
        return await SendRequestInternal<BinanceSystemStatus>(GetUrl(systemStatusEndpoint, sapi, v1), HttpMethod.Get, ct).ConfigureAwait(false);
    }
    #endregion

    #region Exchange Information
    public Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
         => GetExchangeInfoAsync(Array.Empty<string>(), ct);

    public Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(string symbol, CancellationToken ct = default)
         => GetExchangeInfoAsync([symbol], ct);

    public Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(BinancePermissionType permission, CancellationToken ct = default)
         => GetExchangeInfoAsync([permission], ct);

    public async Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(BinancePermissionType[] permissions, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();

        if (permissions.Length > 1)
        {
            var list = new List<string>();
            foreach (var permission in permissions)
            {
                list.Add(permission.ToString().ToUpper());
            }

            parameters.Add("permissions", JsonConvert.SerializeObject(list));
        }
        else if (permissions.Any())
        {
            parameters.Add("permissions", permissions.First().ToString().ToUpper());
        }

        var exchangeInfoResult = await SendRequestInternal<BinanceExchangeInfo>(GetUrl(exchangeInfoEndpoint, api, v3), HttpMethod.Get, ct, queryParameters: parameters, serialization: ArraySerialization.Array, requestWeight: 10).ConfigureAwait(false);
        if (!exchangeInfoResult)
            return exchangeInfoResult;

        ExchangeInfo = exchangeInfoResult.Data;
        LastExchangeInfoUpdate = DateTime.UtcNow;
        Logger.Log(LogLevel.Information, "Trade rules updated");
        return exchangeInfoResult;
    }

    public async Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();

        if (symbols.Count() > 1)
        {
            parameters.Add("symbols", JsonConvert.SerializeObject(symbols));
        }
        else if (symbols.Any())
        {
            parameters.Add("symbol", symbols.First());
        }

        var exchangeInfoResult = await SendRequestInternal<BinanceExchangeInfo>(GetUrl(exchangeInfoEndpoint, api, v3), HttpMethod.Get, ct, queryParameters: parameters, serialization: ArraySerialization.Array, requestWeight: 10).ConfigureAwait(false);
        if (!exchangeInfoResult)
            return exchangeInfoResult;

        ExchangeInfo = exchangeInfoResult.Data;
        LastExchangeInfoUpdate = DateTime.UtcNow;
        Logger.Log(LogLevel.Information, "Trade rules updated");
        return exchangeInfoResult;
    }
    #endregion

    #region Get Products
    public async Task<RestCallResult<IEnumerable<BinanceProduct>>> GetProductsAsync(CancellationToken ct = default)
    {
        var url = ClientOptions.BaseAddress.Replace("api.", "www.").AppendPath("exchange-api/v2/public/asset-service/product/get-products");

        var data = await SendRequestInternal<BinanceExchangeApiWrapper<IEnumerable<BinanceProduct>>>(new Uri(url), HttpMethod.Get, ct).ConfigureAwait(false);
        if (!data)
            return data.As<IEnumerable<BinanceProduct>>(null);

        if (!data.Data.Success)
            return data.AsError<IEnumerable<BinanceProduct>>(new ServerError(data.Data.Code, data.Data.Message + " - " + data.Data.MessageDetail));

        return data.As(data.Data.Data);
    }
    #endregion

    #endregion

    #region Trading Methods

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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceTransaction>(GetUrl(marginTransferEndpoint, sapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 600).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceTransaction>(GetUrl(marginBorrowEndpoint, sapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 3000).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceTransaction>(GetUrl(marginRepayEndpoint, sapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 3000).ConfigureAwait(false);
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

        return await SendRequestInternal<BinanceMarginAsset>(GetUrl(marginAssetEndpoint, sapi, v1), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
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

        return await SendRequestInternal<BinanceMarginPair>(GetUrl(marginPairEndpoint, sapi, v1), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Get All Margin Assets
    public async Task<RestCallResult<IEnumerable<BinanceMarginAsset>>> GetMarginAssetsAsync(CancellationToken ct = default)
    {
        return await SendRequestInternal<IEnumerable<BinanceMarginAsset>>(GetUrl(marginAssetsEndpoint, sapi, v1), HttpMethod.Get, ct).ConfigureAwait(false);
    }
    #endregion

    #region Get All Cross Margin Pairs
    public async Task<RestCallResult<IEnumerable<BinanceMarginPair>>> GetMarginSymbolsAsync(CancellationToken ct = default)
    {
        return await SendRequestInternal<IEnumerable<BinanceMarginPair>>(GetUrl(marginPairsEndpoint, sapi, v1), HttpMethod.Get, ct).ConfigureAwait(false);
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

        return await SendRequestInternal<BinanceMarginPriceIndex>(GetUrl(marginPriceIndexEndpoint, sapi, v1), HttpMethod.Get, ct, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Margin Account New Order
    public async Task<RestCallResult<BinancePlacedOrder>> PlaceMarginOrderAsync(string symbol,
        BinanceSpotOrderSide side,
        BinanceSpotOrderType type,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        string newClientOrderId = null,
        decimal? price = null,
        BinanceSpotTimeInForce? timeInForce = null,
        decimal? stopPrice = null,
        decimal? icebergQuantity = null,
        BinanceSideEffectType? sideEffectType = null,
        bool? isIsolated = null,
        BinanceSpotOrderResponseType? orderResponseType = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        var result = await PlaceOrderInternal(GetUrl(newMarginOrderEndpoint, sapi, v1),
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

        if (result) InvokeOrderPlaced(result.Data.Id);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceOrderBase>(GetUrl(cancelMarginOrderEndpoint, sapi, v1), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 10).ConfigureAwait(false);
        if (result) InvokeOrderCanceled(result.Data.Id);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceOrderBase>>(GetUrl(cancelOpenMarginOrdersEndpoint, sapi, v1), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceTransferHistory>>(GetUrl(transferHistoryEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceLoan>>(GetUrl(getLoanEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceRepay>>(GetUrl(getRepayEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceInterestHistory>>(GetUrl(interestHistoryEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceForcedLiquidation>>(GetUrl(forceLiquidationHistoryEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Query Margin Account Details
    public async Task<RestCallResult<BinanceMarginAccount>> GetMarginAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceMarginAccount>(GetUrl(marginAccountInfoEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceOrder>(GetUrl(queryMarginOrderEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Query Margin Account's Open Order
    public async Task<RestCallResult<IEnumerable<BinanceOrder>>> GetOpenMarginOrdersAsync(string symbol = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol?.ValidateBinanceSymbol();
        if (isIsolated == true && symbol == null)
            throw new ArgumentException("Symbol must be provided for isolated margin");

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("isIsolated", isIsolated);

        return await SendRequestInternal<IEnumerable<BinanceOrder>>(GetUrl(openMarginOrdersEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceOrder>>(GetUrl(allMarginOrdersEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 200).ConfigureAwait(false);
    }
    #endregion

    #region Margin Account New OCO Order
    public async Task<RestCallResult<BinanceMarginOrderOcoList>> PlaceMarginOCOOrderAsync(string symbol,
        BinanceSpotOrderSide side,
        decimal price,
        decimal stopPrice,
        decimal quantity,
        decimal? stopLimitPrice = null,
        BinanceSpotTimeInForce? stopLimitTimeInForce = null,
        decimal? stopIcebergQuantity = null,
        decimal? limitIcebergQuantity = null,
        BinanceSideEffectType? sideEffectType = null,
        bool? isIsolated = null,
        string listClientOrderId = null,
        string limitClientOrderId = null,
        string stopClientOrderId = null,
        BinanceSpotOrderResponseType? orderResponseType = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var rulesCheck = await CheckTradeRules(symbol, quantity, null, price, stopPrice, null, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceMarginOrderOcoList>(GetUrl(newMarginOCOOrderEndpoint, sapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 6).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceMarginOrderOcoList>(GetUrl(cancelMarginOCOOrderEndpoint, sapi, v1), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceMarginOrderOcoList>(GetUrl(getMarginOCOOrderEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceMarginOrderOcoList>>(GetUrl(allMarginOCOOrderEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 200).ConfigureAwait(false);
    }
    #endregion

    #region Query Open OCO
    public async Task<RestCallResult<IEnumerable<BinanceMarginOrderOcoList>>> GetMarginOpenOcoOrdersAsync(string symbol = null, bool? isIsolated = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("isIsolated", isIsolated?.ToString());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceMarginOrderOcoList>>(GetUrl(openMarginOCOOrderEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceTrade>>(GetUrl(myMarginTradesEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceMarginAmount>(GetUrl(maxBorrowableEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 50).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceMarginAmount>(GetUrl(maxTransferableEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 50).ConfigureAwait(false);

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

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceMarginLevel>(GetUrl(marginLevelInformation, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceTransaction>(GetUrl(transferIsolatedMarginAccountEndpoint, sapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceIsolatedMarginTransfer>>(GetUrl(isolatedMarginTransferHistoryEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 600).ConfigureAwait(false);
    }
    #endregion

    #region Query isolated margin account
    public async Task<RestCallResult<BinanceIsolatedMarginAccount>> GetIsolatedMarginAccountAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceIsolatedMarginAccount>(GetUrl(isolatedMarginAccountEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Disable Isolated Margin Account
    public async Task<RestCallResult<BinanceCreateIsolatedMarginAccountResult>> DisableIsolatedMarginAccountAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            {"symbol", symbol}
        };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceCreateIsolatedMarginAccountResult>(GetUrl(disableIsolatedMarginAccountEndpoint, sapi, v1), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 300).ConfigureAwait(false);
    }
    #endregion

    #region Enable Isolated Margin Account
    public async Task<RestCallResult<BinanceCreateIsolatedMarginAccountResult>> EnableIsolatedMarginAccountAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                {"symbol", symbol}
            };

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceCreateIsolatedMarginAccountResult>(GetUrl(enableIsolatedMarginAccountEndpoint, sapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 300).ConfigureAwait(false);
    }
    #endregion

    #region Query Enabled Isolated Margin Account Limit
    public async Task<RestCallResult<BinanceIsolatedMarginAccountLimit>> GetEnabledIsolatedMarginAccountLimitAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceIsolatedMarginAccountLimit>(GetUrl(isolatedMarginAccountLimitEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
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

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceIsolatedMarginSymbol>(GetUrl(isolatedMarginSymbolEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Get All Isolated Margin Symbol
    public async Task<RestCallResult<IEnumerable<BinanceIsolatedMarginSymbol>>> GetIsolatedMarginSymbolsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceIsolatedMarginSymbol>>(GetUrl(isolatedMarginAllSymbolEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBnbBurnStatus>(GetUrl(toggleBnbBurnEndpoint, sapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get BNB Burn Status
    public async Task<RestCallResult<BinanceBnbBurnStatus>> GetBnbBurnStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBnbBurnStatus>(GetUrl(getBnbBurnEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceInterestRateHistory>>(GetUrl(interestRateHistoryEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get Cross Margin Interest Data
    public async Task<RestCallResult<IEnumerable<BinanceInterestMarginData>>> GetInterestMarginDataAsync(string asset = null, string vipLevel = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        asset?.ValidateNotNull(nameof(asset));

        var parameters = new Dictionary<string, object>();

        parameters.AddOptionalParameter("coin", asset);
        parameters.AddOptionalParameter("vipLevel", vipLevel?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceInterestMarginData>>(GetUrl(interestMarginDataEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceIsolatedMarginTierData>>(GetUrl(isolatedMargingTierEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Margin order rate limit
    public async Task<RestCallResult<IEnumerable<BinanceOrderRateLimit>>> GetMarginOrderRateLimitStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceOrderRateLimit>>(GetUrl(marginOrderRateLimitEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20).ConfigureAwait(false);
    }
    #endregion

    #region Margin DustLog
    public async Task<RestCallResult<BinanceDustLogList>> GetMarginDustLogAsync(DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        var result = await SendRequestInternal<BinanceDustLogList>(GetUrl(marginDustLogEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        return result;
    }
    #endregion

    #endregion

    #region User Stream Methods

    #region Create a ListenKey (Cross Margin)
    public async Task<RestCallResult<string>> CreateCrossMarginUserStreamListenKeyAsync(CancellationToken ct = default)
    {
        var result = await SendRequestInternal<BinanceListenKey>(GetUrl(crossMarginCreateListenKeyEndpoint, sapi, v1), HttpMethod.Post, ct).ConfigureAwait(false);
        return result.As(result.Data?.ListenKey!);
    }
    #endregion

    #region Ping/Keep-alive a ListenKey (Cross Margin)
    public async Task<RestCallResult<object>> KeepAliveCrossMarginUserStreamListenKeyAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));

        var parameters = new Dictionary<string, object>
            {
                { "listenKey", listenKey },
            };

        return await SendRequestInternal<object>(GetUrl(crossMarginUpdateListenKeyEndpoint, sapi, v1), HttpMethod.Put, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Close a ListenKey (Cross Margin)
    public async Task<RestCallResult<object>> StopCrossMarginUserStreamListenKeyAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new Dictionary<string, object>
            {
                { "listenKey", listenKey }
            };

        return await SendRequestInternal<object>(GetUrl(crossMarginDeleteListenKeyEndpoint, sapi, v1), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Create a ListenKey (Isolated Margin)
    public async Task<RestCallResult<string>> StartIsolatedMarginUserStreamAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new Dictionary<string, object>()
        {
            {"symbol", symbol}
        };

        var result = await SendRequestInternal<BinanceListenKey>(GetUrl(isolatedMarginCreateListenKeyEndpoint, sapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
        return result.As(result.Data?.ListenKey!);
    }
    #endregion

    #region Ping/Keep-alive a ListenKey (Isolated Margin)
    public async Task<RestCallResult<object>> KeepAliveIsolatedMarginUserStreamAsync(string symbol, string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new Dictionary<string, object>
        {
            { "listenKey", listenKey },
            {"symbol", symbol}
        };

        return await SendRequestInternal<object>(GetUrl(isolatedMarginUpdateListenKeyEndpoint, sapi, v1), HttpMethod.Put, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Close a ListenKey (Isolated Margin)
    public async Task<RestCallResult<object>> CloseIsolatedMarginUserStreamAsync(string symbol, string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new Dictionary<string, object>
        {
            { "listenKey", listenKey },
            {"symbol", symbol}
        };

        return await SendRequestInternal<object>(GetUrl(isolatedMarginDeleteListenKeyEndpoint, sapi, v1), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #endregion

    #region Portfolio Methods

    #region Get Portfolio Margin Account Info
    public async Task<RestCallResult<BinancePortfolioMarginInfo>> GetPortfolioMarginAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinancePortfolioMarginInfo>(GetUrl(portfolioMarginAccountEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Portfolio Margin Collateral Rate
    public async Task<RestCallResult<IEnumerable<BinancePortfolioMarginCollateralRate>>> GetPortfolioMarginCollateralRateAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<IEnumerable<BinancePortfolioMarginCollateralRate>>(GetUrl(portfolioMarginCollateralRateEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 50).ConfigureAwait(false);
    }
    #endregion

    #region Query Portfolio Margin Bankruptcy Loan Amount
    public async Task<RestCallResult<BinancePortfolioMarginLoan>> GetPortfolioMarginBankruptcyLoanAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinancePortfolioMarginLoan>(GetUrl(portfolioMarginLoanEndpoint, sapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 500).ConfigureAwait(false);
    }
    #endregion

    #region Portfolio Margin Bankruptcy Loan Repay
    public async Task<RestCallResult<BinanceTransaction>> PortfolioMarginBankruptcyLoanRepayAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceTransaction>(GetUrl(portfolioMarginRepayEndpoint, sapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 3000).ConfigureAwait(false);
    }
    #endregion

    #endregion

}
