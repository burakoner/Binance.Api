namespace Binance.Api.Clients.RestApi;

public class BinanceRestApiSpotClient : RestApiClient
{
    // Api
    private const string v1 = "1";
    private const string v3 = "3";
    private const string api = "api";
    private const string sapi = "sapi";

    // Server
    private const string systemStatusEndpoint = "system/status";

    // Market Data
    private const string orderBookEndpoint = "depth";
    private const string recentTradesEndpoint = "trades";
    private const string historicalTradesEndpoint = "historicalTrades";
    private const string aggregatedTradesEndpoint = "aggTrades";
    private const string klinesEndpoint = "klines";
    private const string uiKlinesEndpoint = "uiKlines";
    private const string averagePriceEndpoint = "avgPrice";
    private const string price24HEndpoint = "ticker/24hr";
    private const string allPricesEndpoint = "ticker/price";
    private const string bookPricesEndpoint = "ticker/bookTicker";
    private const string rollingWindowPriceEndpoint = "ticker";

    // Wallet
    private const string userCoinsEndpoint = "capital/config/getall";
    private const string accountSnapshotEndpoint = "accountSnapshot";
    private const string disableFastWithdrawSwitchEndpoint = "account/disableFastWithdrawSwitch";
    private const string enableFastWithdrawSwitchEndpoint = "account/enableFastWithdrawSwitch";
    private const string withdrawEndpoint = "capital/withdraw/apply";
    private const string depositHistoryEndpoint = "capital/deposit/hisrec";
    private const string withdrawHistoryEndpoint = "capital/withdraw/history";
    private const string depositAddressEndpoint = "capital/deposit/address";
    private const string accountStatusEndpoint = "account/status";
    private const string tradingStatusEndpoint = "account/apiTradingStatus";
    private const string dustLogEndpoint = "asset/dribblet";
    private const string dustElligableEndpoint = "asset/dust-btc";
    private const string dustTransferEndpoint = "asset/dust";
    private const string dividendRecordsEndpoint = "asset/assetDividend";
    private const string assetDetailsEndpoint = "asset/assetDetail";
    private const string tradeFeeEndpoint = "asset/tradeFee";
    private const string universalTransferEndpoint = "asset/transfer";
    private const string fundingWalletEndpoint = "asset/get-funding-asset";
    private const string balancesEndpoint = "asset/getUserAsset";

    // Account
    private const string convertTransferEndpoint = "asset/convert-transfer";
    private const string convertTransferHistoryEndpoint = "asset/convert-transfer/queryByPage";
    private const string apiRestrictionsEndpoint = "account/apiRestrictions";
    // TODO: Get Cloud-Mining payment and refund history (USER_DATA)
    // TODO: Query auto-converting stable coins (USER_DATA)
    // TODO: Switch on/off BUSD and stable coins conversion (USER_DATA)

    // Trading
    private const string newTestOrderEndpoint = "order/test";
    private const string newOrderEndpoint = "order";
    private const string cancelOrderEndpoint = "order";
    private const string cancelAllOpenOrderEndpoint = "openOrders";
    private const string queryOrderEndpoint = "order";
    private const string cancelReplaceOrderEndpoint = "order/cancelReplace";
    private const string openOrdersEndpoint = "openOrders";
    private const string allOrdersEndpoint = "allOrders";
    private const string newOcoOrderEndpoint = "order/oco";
    private const string cancelOcoOrderEndpoint = "orderList";
    private const string getOcoOrderEndpoint = "orderList";
    private const string getAllOcoOrderEndpoint = "allOrderList";
    private const string getOpenOcoOrderEndpoint = "openOrderList";
    private const string accountInfoEndpoint = "account";
    private const string myTradesEndpoint = "myTrades";
    private const string orderRateLimitEndpoint = "rateLimit/order";
    // TODO: Query Prevented Matches (USER_DATA)

    // User Data Stream
    private const string spotCreateListenKeyEndpoint = "userDataStream";
    private const string spotUpdateListenKeyEndpoint = "userDataStream";
    private const string spotDeleteListenKeyEndpoint = "userDataStream";

    /// <summary>
    /// Event triggered when an order is placed via this client. Only available for Spot orders
    /// </summary>
    public event Action<long> OnOrderPlaced;

    /// <summary>
    /// Event triggered when an order is canceled via this client. 
    /// Note that this does not trigger when using CancelAllOrdersAsync. Only available for Spot orders
    /// </summary>
    public event Action<long> OnOrderCanceled;

    // Internal
    internal ILogger Logger { get => this._logger; }
    internal BinanceExchangeInfo ExchangeInfo;
    internal DateTime? LastExchangeInfoUpdate;
    internal TimeSyncState TimeSyncState = new("Binance Spot RestApi");

    // Root Client
    internal BinanceRestApiClient _ { get; }

    // Options
    public new BinanceRestApiClientOptions ClientOptions { get { return (BinanceRestApiClientOptions)base.ClientOptions; } }

    internal BinanceRestApiSpotClient(BinanceRestApiClient root) : base(root.Logger, root.ClientOptions)
    {
        _ = root;

        RequestBodyFormat = RestRequestBodyFormat.FormData;
        ArraySerialization = ArraySerialization.MultipleValues;
    }

    #region Overrided Methods
    /// <inheritdoc/>
    protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
        => new BinanceAuthenticationProvider(credentials);

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    protected override Task<RestCallResult<DateTime>> GetServerTimestampAsync()
        => GetServerTimeAsync();

    /// <inheritdoc/>
    protected override TimeSyncInfo GetTimeSyncInfo()
        => new(Logger, ClientOptions.AutoTimestamp, ClientOptions.TimestampRecalculationInterval, TimeSyncState);

    /// <inheritdoc/>
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

    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object>? queryParameters = null, Dictionary<string, object>? bodyParameters = null, Dictionary<string, string>? headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer? deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
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
        OrderSide side,
        SpotOrderType type,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        string? newClientOrderId = null,
        decimal? price = null,
        TimeInForce? timeInForce = null,
        decimal? stopPrice = null,
        decimal? icebergQty = null,
        SideEffectType? sideEffectType = null,
        bool? isIsolated = null,
        OrderResponseType? orderResponseType = null,
        int? trailingDelta = null,
        int? receiveWindow = null,
        int weight = 1,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        if (quoteQuantity != null && type != SpotOrderType.Market)
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinancePlacedOrder>(uri, HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: weight).ConfigureAwait(false);
    }

    internal async Task<BinanceTradeRuleResult> CheckTradeRules(string symbol, decimal? quantity, decimal? quoteQuantity, decimal? price, decimal? stopPrice, SpotOrderType? type, CancellationToken ct)
    {
        var outputQuantity = quantity;
        var outputQuoteQuantity = quoteQuantity;
        var outputPrice = price;
        var outputStopPrice = stopPrice;

        if (ClientOptions.SpotOptions.TradeRulesBehavior == TradeRulesBehavior.None)
            return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, outputPrice, outputStopPrice);

        if (ExchangeInfo == null || LastExchangeInfoUpdate == null || (DateTime.UtcNow - LastExchangeInfoUpdate.Value).TotalMinutes > ClientOptions.SpotOptions.TradeRulesUpdateInterval.TotalMinutes)
            await GetExchangeInfoAsync(ct).ConfigureAwait(false);

        if (ExchangeInfo == null)
            return BinanceTradeRuleResult.CreateFailed("Unable to retrieve trading rules, validation failed");

        var symbolData = ExchangeInfo.Symbols.SingleOrDefault(s => string.Equals(s.Name, symbol, StringComparison.CurrentCultureIgnoreCase));
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

        if (symbolData.LotSizeFilter != null || symbolData.MarketLotSizeFilter != null && type == SpotOrderType.Market)
        {
            var minQty = symbolData.LotSizeFilter?.MinQuantity;
            var maxQty = symbolData.LotSizeFilter?.MaxQuantity;
            var stepSize = symbolData.LotSizeFilter?.StepSize;
            if (type == SpotOrderType.Market && symbolData.MarketLotSizeFilter != null)
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
                    if (ClientOptions.SpotOptions.TradeRulesBehavior == TradeRulesBehavior.ThrowError)
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
                if (ClientOptions.SpotOptions.TradeRulesBehavior == TradeRulesBehavior.ThrowError)
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
                    if (ClientOptions.SpotOptions.TradeRulesBehavior == TradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter max/min failed. Original price: {price}, Closest allowed: {outputPrice}");

                    Logger.Log(LogLevel.Information, $"price clamped from {price} to {outputPrice} based on price filter");
                }

                if (stopPrice != null)
                {
                    outputStopPrice = BinanceHelpers.ClampPrice(symbolData.PriceFilter.MinPrice,
                        symbolData.PriceFilter.MaxPrice, stopPrice.Value);
                    if (outputStopPrice != stopPrice)
                    {
                        if (ClientOptions.SpotOptions.TradeRulesBehavior == TradeRulesBehavior.ThrowError)
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
                    if (ClientOptions.SpotOptions.TradeRulesBehavior == TradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter tick failed. Original price: {price}, Closest allowed: {outputPrice}");

                    Logger.Log(LogLevel.Information, $"price floored from {beforePrice} to {outputPrice} based on price filter");
                }

                if (stopPrice != null)
                {
                    var beforeStopPrice = outputStopPrice;
                    outputStopPrice = BinanceHelpers.FloorPrice(symbolData.PriceFilter.TickSize, stopPrice.Value);
                    if (outputStopPrice != beforeStopPrice)
                    {
                        if (ClientOptions.SpotOptions.TradeRulesBehavior == TradeRulesBehavior.ThrowError)
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
            if (ClientOptions.SpotOptions.TradeRulesBehavior == TradeRulesBehavior.ThrowError)
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

    #region General Methods

    public async Task<RestCallResult<long>> PingAsync(CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        var result = await SendRequestInternal<object>(_.GetUrl(api, v3, "ping"), HttpMethod.Get, ct).ConfigureAwait(false);
        sw.Stop();
        return result ? result.As(sw.ElapsedMilliseconds) : result.As<long>(default!);
    }

    public async Task<RestCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
    {
        var result = await SendRequestInternal<BinanceServerTime>(_.GetUrl(api, v3, "time"), HttpMethod.Get, ct, ignoreRatelimit: true).ConfigureAwait(false);
        return result.As(result.Data?.ServerTime ?? default);
    }

    public Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
         => GetExchangeInfoAsync(Array.Empty<string>(), ct);

    public Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(string symbol, CancellationToken ct = default)
         => GetExchangeInfoAsync([symbol], ct);

    public Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(AccountType permission, CancellationToken ct = default)
         => GetExchangeInfoAsync([permission], ct);

    public async Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(AccountType[] permissions, CancellationToken ct = default)
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

        var exchangeInfoResult = await SendRequestInternal<BinanceExchangeInfo>(_.GetUrl(api, v3, "exchangeInfo"), HttpMethod.Get, ct, queryParameters: parameters, serialization: ArraySerialization.Array, requestWeight: 10).ConfigureAwait(false);
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

        var exchangeInfoResult = await SendRequestInternal<BinanceExchangeInfo>(_.GetUrl(api, v3, "exchangeInfo"), HttpMethod.Get, ct, queryParameters: parameters, serialization: ArraySerialization.Array, requestWeight: 10).ConfigureAwait(false);
        if (!exchangeInfoResult)
            return exchangeInfoResult;

        ExchangeInfo = exchangeInfoResult.Data;
        LastExchangeInfoUpdate = DateTime.UtcNow;
        Logger.Log(LogLevel.Information, "Trade rules updated");
        return exchangeInfoResult;
    }
    #endregion


    #region System Status
    public async Task<RestCallResult<BinanceSystemStatus>> GetSystemStatusAsync(CancellationToken ct = default)
    {
        return await SendRequestInternal<BinanceSystemStatus>(_.GetUrl(sapi, v1, systemStatusEndpoint), HttpMethod.Get, ct).ConfigureAwait(false);
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

    #region Market Data Methods

    #region Order Book
    public async Task<RestCallResult<BinanceOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 5000);
        var parameters = new Dictionary<string, object> { { "symbol", symbol } };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        var requestWeight = limit == null ? 1 : limit <= 100 ? 1 : limit <= 500 ? 5 : limit <= 1000 ? 10 : 50;
        var result = await SendRequestInternal<BinanceOrderBook>(_.GetUrl(api, v3, orderBookEndpoint), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
        if (result)
            result.Data.Symbol = symbol;
        return result;
    }
    #endregion

    #region Recent Trades List
    public async Task<RestCallResult<IEnumerable<IBinanceRecentTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object> { { "symbol", symbol } };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        var result = await SendRequestInternal<IEnumerable<BinanceRecentTradeQuote>>(_.GetUrl(api, v3, recentTradesEndpoint), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceRecentTrade>>(result.Data);
    }
    #endregion

    #region Old Trade Lookup
    public async Task<RestCallResult<IEnumerable<IBinanceRecentTrade>>> GetTradeHistoryAsync(string symbol, int? limit = null, long? fromId = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        var parameters = new Dictionary<string, object> { { "symbol", symbol } };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<IEnumerable<BinanceRecentTradeQuote>>(_.GetUrl(api, v3, historicalTradesEndpoint), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceRecentTrade>>(result.Data);
    }
    #endregion

    #region Compressed/Aggregate Trades List
    public async Task<RestCallResult<IEnumerable<BinanceAggregatedTrade>>> GetAggregatedTradeHistoryAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object> { { "symbol", symbol } };
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceAggregatedTrade>>(_.GetUrl(api, v3, aggregatedTradesEndpoint), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Kline/Candlestick Data
    public async Task<RestCallResult<IEnumerable<IBinanceKline>>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new Dictionary<string, object> {
                { "symbol", symbol },
                { "interval", JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false)) }
            };
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<IEnumerable<BinanceSpotKline>>(_.GetUrl(api, v3, klinesEndpoint), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceKline>>(result.Data);
    }
    #endregion

    #region UI Kline Data
    public async Task<RestCallResult<IEnumerable<IBinanceKline>>> GetUiKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new Dictionary<string, object> {
                { "symbol", symbol },
                { "interval", JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false)) }
            };
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<IEnumerable<BinanceSpotKline>>(_.GetUrl(api, v3, uiKlinesEndpoint), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceKline>>(result.Data);
    }
    #endregion

    #region Current Average Price
    public async Task<RestCallResult<BinanceAveragePrice>> GetCurrentAvgPriceAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new Dictionary<string, object> { { "symbol", symbol } };

        return await SendRequestInternal<BinanceAveragePrice>(_.GetUrl(api, v3, averagePriceEndpoint), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region 24hr Ticker Price Change Statistics
    public async Task<RestCallResult<IBinanceTick>> GetTickerAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new Dictionary<string, object> { { "symbol", symbol } };

        var result = await SendRequestInternal<Binance24HPrice>(_.GetUrl(api, v3, price24HEndpoint), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        return result.As<IBinanceTick>(result.Data);
    }

    public async Task<RestCallResult<IEnumerable<IBinanceTick>>> GetTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var parameters = new Dictionary<string, object> { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };
        var symbolCount = symbols.Count();
        var weight = symbolCount <= 20 ? 1 : symbolCount <= 100 ? 20 : 40;
        var result = await SendRequestInternal<IEnumerable<Binance24HPrice>>(_.GetUrl(api, v3, price24HEndpoint), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceTick>>(result.Data);
    }

    public async Task<RestCallResult<IEnumerable<IBinanceTick>>> GetTickersAsync(CancellationToken ct = default)
    {
        var result = await SendRequestInternal<IEnumerable<Binance24HPrice>>(_.GetUrl(api, v3, price24HEndpoint), HttpMethod.Get, ct, requestWeight: 40).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceTick>>(result.Data);
    }
    #endregion

    #region Symbol Price Ticker
    public async Task<RestCallResult<BinancePrice>> GetPriceAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };

        return await SendRequestInternal<BinancePrice>(_.GetUrl(api, v3, allPricesEndpoint), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinancePrice>>> GetPricesAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var parameters = new Dictionary<string, object> { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };
        return await SendRequestInternal<IEnumerable<BinancePrice>>(_.GetUrl(api, v3, allPricesEndpoint), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinancePrice>>> GetPricesAsync(CancellationToken ct = default)
    {
        return await SendRequestInternal<IEnumerable<BinancePrice>>(_.GetUrl(api, v3, allPricesEndpoint), HttpMethod.Get, ct, requestWeight: 2).ConfigureAwait(false);
    }
    #endregion

    #region Symbol Order Book Ticker
    public async Task<RestCallResult<BinanceBookPrice>> GetBookPriceAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new Dictionary<string, object> { { "symbol", symbol } };

        return await SendRequestInternal<BinanceBookPrice>(_.GetUrl(api, v3, bookPricesEndpoint), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceBookPrice>>> GetBookPricesAsync(IEnumerable<string> symbols, CancellationToken ct = default)
    {
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();
        var parameters = new Dictionary<string, object> { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };

        return await SendRequestInternal<IEnumerable<BinanceBookPrice>>(_.GetUrl(api, v3, bookPricesEndpoint), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceBookPrice>>> GetBookPricesAsync(CancellationToken ct = default)
    {
        return await SendRequestInternal<IEnumerable<BinanceBookPrice>>(_.GetUrl(api, v3, bookPricesEndpoint), HttpMethod.Get, ct, requestWeight: 2).ConfigureAwait(false);
    }
    #endregion

    #region Rolling Window Price Change Ticker
    public async Task<RestCallResult<IBinance24HPrice>> GetRollingWindowTickerAsync(string symbol, TimeSpan? windowSize = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new Dictionary<string, object> { { "symbol", symbol } };
        parameters.AddOptionalParameter("windowSize", windowSize == null ? null : GetWindowSize(windowSize.Value));

        var result = await SendRequestInternal<Binance24HPrice>(_.GetUrl(api, v3, rollingWindowPriceEndpoint), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 2).ConfigureAwait(false);
        return result.As<IBinance24HPrice>(result.Data);
    }

    public async Task<RestCallResult<IEnumerable<IBinance24HPrice>>> GetRollingWindowTickersAsync(IEnumerable<string> symbols, TimeSpan? windowSize = null, CancellationToken ct = default)
    {
        foreach (var symbol in symbols)
            symbol.ValidateBinanceSymbol();

        var parameters = new Dictionary<string, object> { { "symbols", $"[{string.Join(",", symbols.Select(s => $"\"{s}\""))}]" } };
        parameters.AddOptionalParameter("windowSize", windowSize == null ? null : GetWindowSize(windowSize.Value));
        var symbolCount = symbols.Count();
        var weight = Math.Min(symbolCount * 2, 100);
        var result = await SendRequestInternal<IEnumerable<Binance24HPrice>>(_.GetUrl(api, v3, rollingWindowPriceEndpoint), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: weight).ConfigureAwait(false);
        return result.As<IEnumerable<IBinance24HPrice>>(result.Data);
    }

    private string GetWindowSize(TimeSpan timeSpan)
    {
        if (timeSpan.TotalHours < 1)
            return timeSpan.TotalMinutes + "m";
        else if (timeSpan.TotalHours < 24)
            return timeSpan.TotalHours + "h";
        return timeSpan.TotalDays + "d";
    }
    #endregion

    #endregion

    #region Account Methods

    #region All Coins' Information
    public async Task<RestCallResult<IEnumerable<BinanceUserAsset>>> GetUserAssetsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));


        return await SendRequestInternal<IEnumerable<BinanceUserAsset>>(_.GetUrl(sapi, v1, userCoinsEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Daily Account Snapshots
    public async Task<RestCallResult<IEnumerable<BinanceSpotAccountSnapshot>>> GetDailySpotAccountSnapshotAsync(
        DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null,
        CancellationToken ct = default) =>
        await GetDailyAccountSnapshot<IEnumerable<BinanceSpotAccountSnapshot>>(AccountType.Spot, startTime, endTime, limit, receiveWindow, ct).ConfigureAwait(false);

    public async Task<RestCallResult<IEnumerable<BinanceMarginAccountSnapshot>>> GetDailyMarginAccountSnapshotAsync(
        DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null,
        CancellationToken ct = default) =>
        await GetDailyAccountSnapshot<IEnumerable<BinanceMarginAccountSnapshot>>(AccountType.Margin, startTime, endTime, limit, receiveWindow, ct).ConfigureAwait(false);

    public async Task<RestCallResult<IEnumerable<BinanceFuturesAccountSnapshot>>> GetDailyFutureAccountSnapshotAsync(
        DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null,
        CancellationToken ct = default) =>
        await GetDailyAccountSnapshot<IEnumerable<BinanceFuturesAccountSnapshot>>(AccountType.Futures, startTime, endTime, limit, receiveWindow, ct).ConfigureAwait(false);


    private async Task<RestCallResult<T>> GetDailyAccountSnapshot<T>(AccountType accountType, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null,
        CancellationToken ct = default) where T : class
    {
        limit?.ValidateIntBetween(nameof(limit), 5, 30);

        var parameters = new Dictionary<string, object>
            {
                { "type", MapConverter.GetString(accountType) }
            };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceSnapshotWrapper<T>>(_.GetUrl(sapi, v1, accountSnapshotEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 2400).ConfigureAwait(false);
        if (!result.Success)
            return result.As<T>(default);

        if (result.Data.Code != 200)
            return result.AsError<T>(new ServerError(result.Data.Code, result.Data.Message));

        return result.As(result.Data.SnapshotData);
    }
    #endregion

    #region Disable Fast Withdraw Switch
    public async Task<RestCallResult<object>> DisableFastWithdrawSwitchAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<object>(_.GetUrl(sapi, v1, disableFastWithdrawSwitchEndpoint), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Enable Fast Withdraw Switch
    public async Task<RestCallResult<object>> EnableFastWithdrawSwitchAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<object>(_.GetUrl(sapi, v1, enableFastWithdrawSwitchEndpoint), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Withdraw
    public async Task<RestCallResult<BinanceWithdrawalPlaced>> WithdrawAsync(string asset, string address, decimal quantity, string withdrawOrderId = null, string network = null, string addressTag = null, string name = null, bool? transactionFeeFlag = null, WalletType? walletType = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));
        address.ValidateNotNull(nameof(address));

        var parameters = new Dictionary<string, object>
            {
                { "coin", asset },
                { "address", address },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
        parameters.AddOptionalParameter("name", name);
        parameters.AddOptionalParameter("withdrawOrderId", withdrawOrderId);
        parameters.AddOptionalParameter("network", network);
        parameters.AddOptionalParameter("transactionFeeFlag", transactionFeeFlag);
        parameters.AddOptionalParameter("addressTag", addressTag);
        parameters.AddOptionalParameter("walletType", walletType != null ? JsonConvert.SerializeObject(walletType, new WalletTypeConverter(false)) : null);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceWithdrawalPlaced>(_.GetUrl(sapi, v1, withdrawEndpoint), HttpMethod.Post, ct, true, queryParameters: parameters).ConfigureAwait(false);
        return result;
    }
    #endregion

    #region Withdraw History (Supporting Network)
    public async Task<RestCallResult<IEnumerable<BinanceWithdrawal>>> GetWithdrawalHistoryAsync(string asset = null, string withdrawOrderId = null, WithdrawalStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, int? limit = null, int? offset = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("coin", asset);
        parameters.AddOptionalParameter("withdrawOrderId", withdrawOrderId);
        parameters.AddOptionalParameter("status", status != null ? JsonConvert.SerializeObject(status, new WithdrawalStatusConverter(false)) : null);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit);
        parameters.AddOptionalParameter("offset", offset);

        var result = await SendRequestInternal<IEnumerable<BinanceWithdrawal>>(_.GetUrl(sapi, v1, withdrawHistoryEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        return result;
    }
    #endregion

    #region Deposit History (Supporting Network)
    public async Task<RestCallResult<IEnumerable<BinanceDeposit>>> GetDepositHistoryAsync(string asset = null, DepositStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? offset = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("coin", asset);
        parameters.AddOptionalParameter("offset", offset?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("status", status != null ? JsonConvert.SerializeObject(status, new DepositStatusConverter(false)) : null);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceDeposit>>(
                _.GetUrl(sapi, v1, depositHistoryEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters)
            .ConfigureAwait(false);
    }
    #endregion

    #region Deposit Address (Supporting Network)
    public async Task<RestCallResult<BinanceDepositAddress>> GetDepositAddressAsync(string asset, string network = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));

        var parameters = new Dictionary<string, object>
            {
                { "coin", asset }
            };
        parameters.AddOptionalParameter("network", network);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceDepositAddress>(_.GetUrl(sapi, v1, depositAddressEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Account Status
    public async Task<RestCallResult<BinanceAccountStatus>> GetAccountStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceAccountStatus>(_.GetUrl(sapi, v1, accountStatusEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        return result;
    }
    #endregion

    #region Account API Trading Status
    public async Task<RestCallResult<BinanceTradingStatus>> GetTradingStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceResult<BinanceTradingStatus>>(_.GetUrl(sapi, v1, tradingStatusEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        if (!result)
            return result.As<BinanceTradingStatus>(default);

        return !string.IsNullOrEmpty(result.Data.Message) ? result.AsError<BinanceTradingStatus>(new ServerError(result.Data.Message!)) : result.As(result.Data.Data);
    }
    #endregion

    #region DustLog
    public async Task<RestCallResult<BinanceDustLogList>> GetDustLogAsync(DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        var result = await SendRequestInternal<BinanceDustLogList>(_.GetUrl(sapi, v1, dustLogEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        return result;
    }
    #endregion

    #region Get Assets That Can Be Converted Into BNB
    public async Task<RestCallResult<BinanceElligableDusts>> GetAssetsForDustTransferAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceElligableDusts>(_.GetUrl(sapi, v1, dustElligableEndpoint), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Dust Transfer
    public async Task<RestCallResult<BinanceDustTransferResult>> DustTransferAsync(IEnumerable<string> assets, int? receiveWindow = null, CancellationToken ct = default)
    {
        var assetsArray = assets.ToArray();

        assetsArray.ValidateNotNull(nameof(assets));
        foreach (var asset in assetsArray)
            asset.ValidateNotNull(nameof(asset));

        var parameters = new Dictionary<string, object>
            {
                { "asset", assetsArray }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceDustTransferResult>(_.GetUrl(sapi, v1, dustTransferEndpoint), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Asset Dividend Record
    public async Task<RestCallResult<BinanceQueryRecords<BinanceDividendRecord>>> GetAssetDividendRecordsAsync(string asset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("asset", asset);
        parameters.AddOptionalParameter("limit", limit);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceDividendRecord>>(_.GetUrl(sapi, v1, dividendRecordsEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Asset Detail
    public async Task<RestCallResult<Dictionary<string, BinanceAssetDetails>>> GetAssetDetailsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<Dictionary<string, BinanceAssetDetails>>(_.GetUrl(sapi, v1, assetDetailsEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        return result;
    }
    #endregion

    #region Trade Fee
    public async Task<RestCallResult<IEnumerable<BinanceTradeFee>>> GetTradeFeeAsync(string symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol?.ValidateBinanceSymbol();
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<IEnumerable<BinanceTradeFee>>(_.GetUrl(sapi, v1, tradeFeeEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        return result;
    }
    #endregion

    #region User Universal Transfer
    public async Task<RestCallResult<BinanceTransaction>> TransferAsync(UniversalTransferType type, string asset, decimal quantity, string fromSymbol = null, string toSymbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "type", JsonConvert.SerializeObject(type, new UniversalTransferTypeConverter(false)) },
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };

        parameters.AddOptionalParameter("fromSymbol", fromSymbol);
        parameters.AddOptionalParameter("toSymbol", toSymbol);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceTransaction>(_.GetUrl(sapi, v1, universalTransferEndpoint), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Query User Universal Transfer History
    public async Task<RestCallResult<BinanceQueryRecords<BinanceTransfer>>> GetTransfersAsync(UniversalTransferType type, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "type", JsonConvert.SerializeObject(type, new UniversalTransferTypeConverter(false)) }
            };
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("current", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("size", pageSize?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceTransfer>>(_.GetUrl(sapi, v1, universalTransferEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Funding Wallet
    public async Task<RestCallResult<IEnumerable<BinanceFundingAsset>>> GetFundingWalletAsync(string asset = null, bool? needBtcValuation = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("asset", asset);
        parameters.AddOptionalParameter("needBtcValuation", needBtcValuation?.ToString());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFundingAsset>>(_.GetUrl(sapi, v1, fundingWalletEndpoint), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region User Asset
    public async Task<RestCallResult<IEnumerable<BinanceUserBalance>>> GetBalancesAsync(string asset = null, bool? needBtcValuation = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("asset", asset);
        parameters.AddOptionalParameter("needBtcValuation", needBtcValuation);
        return await SendRequestInternal<IEnumerable<BinanceUserBalance>>(_.GetUrl(sapi, v3, balancesEndpoint), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5).ConfigureAwait(false);
    }
    #endregion

    #region BUSD Convert
    public async Task<RestCallResult<BinanceConvertTransferResult>> ConvertTransferAsync(string clientTransferId, string asset, decimal quantity, string targetAsset, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
            {
                { "clientTranId", clientTransferId },
                { "asset", asset },
                { "amount", quantity },
                { "targetAsset", targetAsset }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceConvertTransferResult>(_.GetUrl(sapi, v1, convertTransferEndpoint), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5).ConfigureAwait(false);
    }
    #endregion

    #region BUSD Convert History
    public async Task<RestCallResult<BinanceQueryRecords<BinanceConvertTransferRecord>>> GetConvertTransferHistoryAsync(DateTime startTime, DateTime endTime, long? transferId = null, string asset = null, int? page = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
            {
                { "startTime", startTime.ConvertToMilliseconds() },
                { "endTime", endTime.ConvertToMilliseconds() },
            };
        parameters.AddOptionalParameter("tranId", transferId);
        parameters.AddOptionalParameter("asset", asset);
        parameters.AddOptionalParameter("current", page);
        parameters.AddOptionalParameter("size", limit);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceConvertTransferRecord>>(_.GetUrl(sapi, v1, convertTransferHistoryEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
    }
    #endregion

    #region Get API Key Permission
    public async Task<RestCallResult<BinanceAPIKeyPermissions>> GetAPIKeyPermissionsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceAPIKeyPermissions>(_.GetUrl(sapi, v1, apiRestrictionsEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #endregion

    #region Trading Methods

    #region Test New Order 
    public async Task<RestCallResult<BinancePlacedOrder>> PlaceTestOrderAsync(string symbol,
        OrderSide side,
        SpotOrderType type,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        string? newClientOrderId = null,
        decimal? price = null,
        TimeInForce? timeInForce = null,
        decimal? stopPrice = null,
        decimal? icebergQty = null,
        OrderResponseType? orderResponseType = null,
        int? trailingDelta = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        return await PlaceOrderInternal(_.GetUrl(api, v3, newTestOrderEndpoint),
            symbol,
            side,
            type,
            quantity,
            quoteQuantity,
            newClientOrderId,
            price,
            timeInForce,
            stopPrice,
            icebergQty,
            null,
            null,
            orderResponseType,
            trailingDelta,
            receiveWindow,
            1,
            ct).ConfigureAwait(false);
    }
    #endregion

    #region New Order
    public async Task<RestCallResult<BinancePlacedOrder>> PlaceOrderAsync(string symbol,
        OrderSide side,
        SpotOrderType type,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        string? newClientOrderId = null,
        decimal? price = null,
        TimeInForce? timeInForce = null,
        decimal? stopPrice = null,
        decimal? icebergQty = null,
        OrderResponseType? orderResponseType = null,
        int? trailingDelta = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        var result = await PlaceOrderInternal(_.GetUrl(api, v3, newOrderEndpoint),
            symbol,
            side,
            type,
            quantity,
            quoteQuantity,
            newClientOrderId,
            price,
            timeInForce,
            stopPrice,
            icebergQty,
            null,
            null,
            orderResponseType,
            trailingDelta,
            receiveWindow,
            1,
            ct).ConfigureAwait(false);
        if (result)
            InvokeOrderPlaced(result.Data.Id);
        return result;
    }
    #endregion

    #region Cancel Order
    public async Task<RestCallResult<BinanceOrderBase>> CancelOrderAsync(string symbol, long? orderId = null, string origClientOrderId = null, string newClientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
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
        parameters.AddOptionalParameter("newClientOrderId", newClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceOrderBase>(_.GetUrl(api, v3, cancelOrderEndpoint), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
        if (result)
            InvokeOrderCanceled(result.Data.Id);
        return result;
    }
    #endregion

    #region Cancel All Open Orders on a Symbol
    public async Task<RestCallResult<IEnumerable<BinanceOrderBase>>> CancelAllOrdersAsync(string symbol, long? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceOrderBase>>(_.GetUrl(api, v3, cancelAllOpenOrderEndpoint), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Cancel an Existing Order and Send a New Order
    public async Task<RestCallResult<BinanceReplaceOrderResult>> ReplaceOrderAsync(string symbol,
        OrderSide side,
        SpotOrderType type,
        CancelReplaceMode cancelReplaceMode,
        long? cancelOrderId = null,
        string? cancelClientOrderId = null,
        string? newCancelClientOrderId = null,
        string? newClientOrderId = null,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        decimal? price = null,
        TimeInForce? timeInForce = null,
        decimal? stopPrice = null,
        decimal? icebergQty = null,
        OrderResponseType? orderResponseType = null,
        int? trailingDelta = null,
        int? strategyId = null,
        int? strategyType = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        if (cancelOrderId == null && cancelClientOrderId == null || cancelOrderId != null && cancelClientOrderId != null)
            throw new ArgumentException("1 of either should be specified, cancelOrderId or cancelClientOrderId");

        if (quoteQuantity != null && type != SpotOrderType.Market)
            throw new ArgumentException("quoteQuantity is only valid for market orders");

        if (quantity == null && quoteQuantity == null || quantity != null && quoteQuantity != null)
            throw new ArgumentException("1 of either should be specified, quantity or quoteOrderQuantity");

        var rulesCheck = await CheckTradeRules(symbol, quantity, quoteQuantity, price, stopPrice, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceReplaceOrderResult>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;
        quoteQuantity = rulesCheck.QuoteQuantity;

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "side", JsonConvert.SerializeObject(side, new OrderSideConverter(false)) },
                { "type", JsonConvert.SerializeObject(type, new SpotOrderTypeConverter(false)) },
                { "cancelReplaceMode", MapConverter.GetString(cancelReplaceMode) }
            };
        parameters.AddOptionalParameter("cancelNewClientOrderId", newCancelClientOrderId);
        parameters.AddOptionalParameter("newClientOrderId", newClientOrderId);
        parameters.AddOptionalParameter("cancelOrderId", cancelOrderId);
        parameters.AddOptionalParameter("strategyId", strategyId);
        parameters.AddOptionalParameter("strategyType", strategyType);
        parameters.AddOptionalParameter("cancelOrigClientOrderId", cancelClientOrderId);
        parameters.AddOptionalParameter("quantity", quantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("quoteOrderQty", quoteQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("price", price?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("timeInForce", timeInForce == null ? null : JsonConvert.SerializeObject(timeInForce, new TimeInForceConverter(false)));
        parameters.AddOptionalParameter("stopPrice", stopPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("icebergQty", icebergQty?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("newOrderRespType", orderResponseType == null ? null : JsonConvert.SerializeObject(orderResponseType, new OrderResponseTypeConverter(false)));
        parameters.AddOptionalParameter("trailingDelta", trailingDelta);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceReplaceOrderResult>(_.GetUrl(api, v3, cancelReplaceOrderEndpoint), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        if (!result && result.Raw != null)
        {
            // Attempt to parse the error
            var jsonData = result.Raw.ToJToken(Logger);
            if (jsonData != null)
            {
                var dataNode = jsonData["data"];
                if (dataNode == null)
                    return result;

                var error = dataNode?["cancelResult"]?.ToString() == "FAILURE" ? dataNode!["cancelResponse"] : jsonData["data"]!["newOrderResponse"];
                if (error != null && error.HasValues)
                    return result.AsError<BinanceReplaceOrderResult>(new ServerError(error!.Value<int>("code"), error.Value<string>("msg")!));
            }
        }

        if (result && result.Data.NewOrderResult == OrderOperationResult.Success)
            InvokeOrderPlaced(result.Data.NewOrderResponse!.Id);
        return result;
    }
    #endregion

    #region Current Open Orders
    public async Task<RestCallResult<IEnumerable<BinanceOrder>>> GetOpenOrdersAsync(string symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol?.ValidateBinanceSymbol();

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("symbol", symbol);

        return await SendRequestInternal<IEnumerable<BinanceOrder>>(_.GetUrl(api, v3, openOrdersEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: symbol == null ? 40 : 3).ConfigureAwait(false);
    }
    #endregion

    #region Query Order
    public async Task<RestCallResult<BinanceOrder>> GetOrderAsync(string symbol, long? orderId = null, string origClientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceOrder>(_.GetUrl(api, v3, queryOrderEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 2).ConfigureAwait(false);
    }
    #endregion

    #region All Orders 
    public async Task<RestCallResult<IEnumerable<BinanceOrder>>> GetOrdersAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceOrder>>(_.GetUrl(api, v3, allOrdersEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region New OCO
    public async Task<RestCallResult<BinanceOrderOcoList>> PlaceOcoOrderAsync(string symbol,
        OrderSide side,
        decimal quantity,
        decimal price,
        decimal stopPrice,
        decimal? stopLimitPrice = null,
        string listClientOrderId = null,
        string limitClientOrderId = null,
        string stopClientOrderId = null,
        decimal? limitIcebergQuantity = null,
        decimal? stopIcebergQuantity = null,
        TimeInForce? stopLimitTimeInForce = null,
        int? trailingDelta = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var rulesCheck = await CheckTradeRules(symbol, quantity, null, price, stopPrice, null, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceOrderOcoList>(new ArgumentError(rulesCheck.ErrorMessage!));
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
        parameters.AddOptionalParameter("listClientOrderId", listClientOrderId);
        parameters.AddOptionalParameter("limitClientOrderId", limitClientOrderId);
        parameters.AddOptionalParameter("stopClientOrderId", stopClientOrderId);
        parameters.AddOptionalParameter("limitIcebergQty", limitIcebergQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("stopIcebergQty", stopIcebergQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("stopLimitTimeInForce", stopLimitTimeInForce == null ? null : JsonConvert.SerializeObject(stopLimitTimeInForce, new TimeInForceConverter(false)));
        parameters.AddOptionalParameter("trailingDelta", trailingDelta);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceOrderOcoList>(_.GetUrl(api, v3, newOcoOrderEndpoint), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Cancel OCO 
    public async Task<RestCallResult<BinanceOrderOcoList>> CancelOcoOrderAsync(string symbol, long? orderListId = null, string listClientOrderId = null, string newClientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        if (!orderListId.HasValue && string.IsNullOrEmpty(listClientOrderId))
            throw new ArgumentException("Either orderListId or listClientOrderId must be sent");

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("orderListId", orderListId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("listClientOrderId", listClientOrderId);
        parameters.AddOptionalParameter("newClientOrderId", newClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceOrderOcoList>(_.GetUrl(api, v3, cancelOcoOrderEndpoint), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Query OCO
    public async Task<RestCallResult<BinanceOrderOcoList>> GetOcoOrderAsync(long? orderListId = null, string origClientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderListId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderListId or origClientOrderId must be sent");

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("orderListId", orderListId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceOrderOcoList>(_.GetUrl(api, v3, getOcoOrderEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 2).ConfigureAwait(false);
    }
    #endregion

    #region Query All OCO
    public async Task<RestCallResult<IEnumerable<BinanceOrderOcoList>>> GetOcoOrdersAsync(long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (fromId != null && (startTime != null || endTime != null))
            throw new ArgumentException("Start/end time can only be provided without fromId parameter");

        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceOrderOcoList>>(_.GetUrl(api, v3, getAllOcoOrderEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Query Open OCO
    public async Task<RestCallResult<IEnumerable<BinanceOrderOcoList>>> GetOpenOcoOrdersAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceOrderOcoList>>(_.GetUrl(api, v3, getOpenOcoOrderEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 3).ConfigureAwait(false);
    }
    #endregion

    #region Account Information
    public async Task<RestCallResult<BinanceAccountInfo>> GetAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceAccountInfo>(_.GetUrl(api, v3, accountInfoEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Account Trade List
    public async Task<RestCallResult<IEnumerable<BinanceTrade>>> GetUserTradesAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceTrade>>(_.GetUrl(api, v3, myTradesEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Query Current Order Count Usage
    public async Task<RestCallResult<IEnumerable<BinanceOrderRateLimit>>> GetOrderRateLimitStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceOrderRateLimit>>(_.GetUrl(api, v3, orderRateLimitEndpoint), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20).ConfigureAwait(false);
    }
    #endregion

    #endregion

    #region User Stream Methods

    #region Create a ListenKey (Spot)
    public async Task<RestCallResult<string>> CreateSpotUserStreamListenKeyAsync(CancellationToken ct = default)
    {
        var result = await SendRequestInternal<BinanceListenKey>(_.GetUrl(api, v3, spotCreateListenKeyEndpoint), HttpMethod.Post, ct, true).ConfigureAwait(false);
        return result.As(result.Data?.ListenKey!);
    }
    #endregion

    #region Ping/Keep-alive a ListenKey (Spot)
    public async Task<RestCallResult<object>> KeepAliveSpotUserStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));

        var parameters = new Dictionary<string, object>
            {
                { "listenKey", listenKey }
            };

        return await SendRequestInternal<object>(_.GetUrl(api, v3, spotUpdateListenKeyEndpoint), HttpMethod.Put, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Close a ListenKey (Spot)
    public async Task<RestCallResult<object>> StopSpotUserStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));

        var parameters = new Dictionary<string, object>
            {
                { "listenKey", listenKey }
            };

        return await SendRequestInternal<object>(_.GetUrl(api, v3, spotDeleteListenKeyEndpoint), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #endregion

}