using Binance.Api.Shared.Responses;
using Binance.Api.Spot.Internal;

namespace Binance.Api.Clients.RestApi;

public class BinanceRestApiUsdtFuturesClient : RestApiClient
{
    // Clients
    public BinanceRestApiFuturesAlgoClient Algo { get => RootClient.FuturesAlgo; }
    public BinanceRestApiFuturesLoanClient Loan { get => RootClient.FuturesLoan; }
    public BinanceRestApiFuturesTransferClient Transfer { get => RootClient.FuturesTransfer; }

    // Api
    private const string v1 = "1";
    private const string fapi = "fapi";
    private const string tradingDataApi = "futures/data";

    // Server
    private const string pingEndpoint = "ping";
    private const string checkTimeEndpoint = "time";
    private const string exchangeInfoEndpoint = "exchangeInfo";

    // Market Data
    private const string orderBookEndpoint = "depth";
    private const string recentTradesEndpoint = "trades";
    private const string historicalTradesEndpoint = "historicalTrades";
    private const string aggregatedTradesEndpoint = "aggTrades";
    private const string klinesEndpoint = "klines";
    private const string continuousContractKlineEndpoint = "continuousKlines";
    private const string indexPriceKlinesKlineEndpoint = "indexPriceKlines";
    private const string markPriceKlinesEndpoint = "markPriceKlines";
    private const string markPriceEndpoint = "premiumIndex";
    private const string fundingRateHistoryEndpoint = "fundingRate";
    private const string price24HEndpoint = "ticker/24hr";
    private const string allPricesEndpoint = "ticker/price";
    private const string bookPricesEndpoint = "ticker/bookTicker";
    private const string openInterestEndpoint = "openInterest";
    private const string openInterestHistoryEndpoint = "openInterestHist";
    private const string topLongShortAccountRatioEndpoint = "topLongShortAccountRatio";
    private const string topLongShortPositionRatioEndpoint = "topLongShortPositionRatio";
    private const string globalLongShortAccountRatioEndpoint = "globalLongShortAccountRatio";
    private const string takerBuySellVolumeRatioEndpoint = "takerlongshortRatio";
    // TODO: Historical BLVT NAV Kline/Candlestick
    private const string compositeIndexapi = "indexInfo";
    private const string assetIndexEndpoint = "assetIndex";

    // Account
    private const string positionModeSideSetEndpoint = "positionSide/dual";
    private const string positionModeSideGetEndpoint = "positionSide/dual";
    private const string futuresAccountMultiAssetsModeSetEndpoint = "multiAssetsMargin";
    private const string futuresAccountMultiAssetsModeGetEndpoint = "multiAssetsMargin";
    private const string futuresAccountBalanceEndpoint = "balance";
    private const string accountInfoEndpoint = "account";
    private const string changeInitialLeverageEndpoint = "leverage";
    private const string changeMarginTypeEndpoint = "marginType";
    private const string positionMarginEndpoint = "positionMargin";
    private const string positionMarginChangeHistoryEndpoint = "positionMargin/history";
    private const string positionInformationEndpoint = "positionRisk";
    private const string incomeHistoryEndpoint = "income";
    private const string leverageBracketEndpoint = "leverageBracket";
    private const string adlQuantileEndpoint = "adlQuantile";
    private const string tradingStatusEndpoint = "apiTradingStatus";
    private const string futuresAccountUserCommissionRateEndpoint = "commissionRate";
    private const string downloadIdEndpoint = "income/asyn";
    private const string downloadLinkEndpoint = "income/asyn/id";

    // Trade
    private const string newOrderEndpoint = "order";
    private const string multipleNewOrdersEndpoint = "batchOrders";
    private const string queryOrderEndpoint = "order";
    private const string cancelOrderEndpoint = "order";
    private const string cancelAllOrdersEndpoint = "allOpenOrders";
    private const string cancelMultipleOrdersEndpoint = "batchOrders";
    private const string countDownCancelAllEndpoint = "countdownCancelAll";
    private const string openOrderEndpoint = "openOrder";
    private const string openOrdersEndpoint = "openOrders";
    private const string allOrdersEndpoint = "allOrders";
    private const string myFuturesTradesEndpoint = "userTrades";
    private const string forceOrdersEndpoint = "forceOrders";

    // User Stream
    private const string getFuturesListenKeyEndpoint = "listenKey";
    private const string keepFuturesListenKeyAliveEndpoint = "listenKey";
    private const string closeFuturesListenKeyEndpoint = "listenKey";

    // Portfolio
    // TODO: Portfolio Margin Exchange Information
    // TODO: Portfolio Margin Account Information

    // Internal
    internal ILogger Logger { get => this._logger; }
    internal BinanceFuturesUsdtExchangeInfo ExchangeInfo;
    internal DateTime? LastExchangeInfoUpdate;
    internal TimeSyncState TimeSyncState = new("Binance UsdtFutures RestApi");

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

    internal BinanceRestApiUsdtFuturesClient(BinanceRestApiClient root) : base(root.Logger, root.Options)
    {
        RootClient = root;

        RequestBodyFormat = RestRequestBodyFormat.FormData;
        ArraySerialization = ArraySerialization.MultipleValues;
    }

    #region Overrided Methods
    protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
        => new BinanceAuthenticationProvider(credentials);

    protected override Task<RestCallResult<DateTime>> GetServerTimestampAsync()
        => GetServerTimeAsync();

    protected override TimeSyncInfo GetTimeSyncInfo()
        => new(Logger, ClientOptions.AutoTimestamp, ClientOptions.TimestampRecalculationInterval, TimeSyncState);

    protected override TimeSpan GetTimeOffset()
        => TimeSyncState.TimeOffset;

    public string GetSymbolName(string baseAsset, string quoteAsset) =>
        (baseAsset + quoteAsset).ToUpper(CultureInfo.InvariantCulture);

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
    #endregion

    #region Internal Methods

    internal void InvokeOrderPlaced(long id)
    {
        OnOrderPlaced?.Invoke(id);
    }

    internal void InvokeOrderCanceled(long id)
    {
        OnOrderCanceled?.Invoke(id);
    }

    internal Uri GetUrl(string endpoint, string api, string version = null)
    {
        var result = ClientOptions.UsdtFuturesOptions.BaseAddress.AppendPath(api);

        if (!string.IsNullOrEmpty(version))
            result = result.AppendPath($"v{version}");

        return new Uri(result.AppendPath(endpoint));
    }

    internal async Task<BinanceTradeRuleResult> CheckTradeRules(string symbol, decimal? quantity, decimal? quoteQuantity, decimal? price, decimal? stopPrice, FuturesOrderType type, CancellationToken ct)
    {
        var outputQuantity = quantity;
        var outputQuoteQuantity = quoteQuantity;
        var outputPrice = price;
        var outputStopPrice = stopPrice;

        if (ClientOptions.UsdtFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.None)
            return BinanceTradeRuleResult.CreatePassed(outputQuantity, quoteQuantity, outputPrice, outputStopPrice);

        if (ExchangeInfo == null || LastExchangeInfoUpdate == null || (DateTime.UtcNow - LastExchangeInfoUpdate.Value).TotalMinutes > ClientOptions.UsdtFuturesOptions.TradeRulesUpdateInterval.TotalMinutes)
            await GetExchangeInfoAsync(ct).ConfigureAwait(false);

        if (ExchangeInfo == null)
            return BinanceTradeRuleResult.CreateFailed("Unable to retrieve trading rules, validation failed");

        var symbolData = ExchangeInfo.Symbols.SingleOrDefault(s => string.Equals(s.Name, symbol, StringComparison.CurrentCultureIgnoreCase));
        if (symbolData == null)
            return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Symbol {symbol} not found");

        if (!symbolData.OrderTypes.Contains(type))
            return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: {type} order type not allowed for {symbol}");

        if (symbolData.LotSizeFilter != null || symbolData.MarketLotSizeFilter != null && type == FuturesOrderType.Market)
        {
            var minQty = symbolData.LotSizeFilter?.MinQuantity;
            var maxQty = symbolData.LotSizeFilter?.MaxQuantity;
            var stepSize = symbolData.LotSizeFilter?.StepSize;
            if (type == FuturesOrderType.Market && symbolData.MarketLotSizeFilter != null)
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
                    if (ClientOptions.UsdtFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                    {
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: LotSize filter failed. Original quantity: {quantity}, Closest allowed: {outputQuantity}");
                    }

                    Logger.Log(LogLevel.Information, $"Quantity clamped from {quantity} to {outputQuantity}");
                }
            }
        }

        if (symbolData.MinNotionalFilter != null && outputQuoteQuantity != null)
        {
            if (quoteQuantity < symbolData.MinNotionalFilter.MinNotional)
            {
                if (ClientOptions.UsdtFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                    return BinanceTradeRuleResult.CreateFailed(
                        $"Trade rules check failed: MinNotional filter failed. Order value: {quoteQuantity}, minimal order value: {symbolData.MinNotionalFilter.MinNotional}");

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
                    if (ClientOptions.UsdtFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter max/min failed. Original price: {price}, Closest allowed: {outputPrice}");

                    Logger.Log(LogLevel.Information, $"price clamped from {price} to {outputPrice}");
                }

                if (stopPrice != null)
                {
                    outputStopPrice = BinanceHelpers.ClampPrice(symbolData.PriceFilter.MinPrice,
                        symbolData.PriceFilter.MaxPrice, stopPrice.Value);
                    if (outputStopPrice != stopPrice)
                    {
                        if (ClientOptions.UsdtFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                            return BinanceTradeRuleResult.CreateFailed(
                                $"Trade rules check failed: Stop price filter max/min failed. Original stop price: {stopPrice}, Closest allowed: {outputStopPrice}");

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
                    if (ClientOptions.UsdtFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter tick failed. Original price: {price}, Closest allowed: {outputPrice}");

                    Logger.Log(LogLevel.Information, $"price rounded from {beforePrice} to {outputPrice}");
                }

                if (stopPrice != null)
                {
                    var beforeStopPrice = outputStopPrice;
                    outputStopPrice = BinanceHelpers.FloorPrice(symbolData.PriceFilter.TickSize, stopPrice.Value);
                    if (outputStopPrice != beforeStopPrice)
                    {
                        if (ClientOptions.UsdtFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                            return BinanceTradeRuleResult.CreateFailed(
                                $"Trade rules check failed: Stop price filter tick failed. Original stop price: {stopPrice}, Closest allowed: {outputStopPrice}");

                        Logger.Log(LogLevel.Information,
                            $"Stop price floored from {beforeStopPrice} to {outputStopPrice} based on price filter");
                    }
                }
            }
        }

        return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, outputPrice, outputStopPrice);
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
    #endregion

    #region Server Methods

    #region Test Connectivity
    public async Task<RestCallResult<long>> PingAsync(CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        var result = await SendRequestInternal<object>(GetUrl(pingEndpoint, fapi, v1), HttpMethod.Get, ct).ConfigureAwait(false);
        sw.Stop();
        return result ? result.As(sw.ElapsedMilliseconds) : result.As<long>(default!);
    }
    #endregion

    #region Check Server Time
    public async Task<RestCallResult<DateTime>> GetServerTimeAsync(bool resetAutoTimestamp = false, CancellationToken ct = default)
    {
        var url = GetUrl(checkTimeEndpoint, fapi, v1);
        var result = await SendRequestInternal<BinanceServerTime>(url, HttpMethod.Get, ct, ignoreRatelimit: true).ConfigureAwait(false);
        return result.As(result.Data?.ServerTime ?? default);
    }
    #endregion

    #region Exchange Information
    public async Task<RestCallResult<BinanceFuturesUsdtExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
    {
        var exchangeInfoResult = await SendRequestInternal<BinanceFuturesUsdtExchangeInfo>(GetUrl(exchangeInfoEndpoint, fapi, v1), HttpMethod.Get, ct).ConfigureAwait(false);
        if (!exchangeInfoResult)
            return exchangeInfoResult;

        ExchangeInfo = exchangeInfoResult.Data;
        LastExchangeInfoUpdate = DateTime.UtcNow;
        Logger.Log(LogLevel.Information, "Trade rules updated");
        return exchangeInfoResult;
    }
    #endregion

    #endregion

    #region Market Data Methods

    #region Order Book
    public async Task<RestCallResult<BinanceFuturesOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntValues(nameof(limit), 5, 10, 20, 50, 100, 500, 1000);
        var parameters = new Dictionary<string, object> { { "symbol", symbol } };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var requestWeight = limit == null ? 10 : limit <= 50 ? 2 : limit == 100 ? 5 : limit == 500 ? 10 : 20;
        var result = await SendRequestInternal<BinanceFuturesOrderBook>(GetUrl(orderBookEndpoint, fapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
        if (result && string.IsNullOrEmpty(result.Data.Symbol))
            result.Data.Symbol = symbol;
        return result.As(result.Data);
    }
    #endregion

    #region Recent Trades List
    public async Task<RestCallResult<IEnumerable<IBinanceRecentTrade>>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object> { { "symbol", symbol } };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        var result = await SendRequestInternal<IEnumerable<BinanceRecentTradeQuote>>(GetUrl(recentTradesEndpoint, fapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceRecentTrade>>(result.Data);
    }
    #endregion

    #region Old Trades Lookup
    public async Task<RestCallResult<IEnumerable<IBinanceRecentTrade>>> GetTradeHistoryAsync(string symbol, int? limit = null, long? fromId = null,
        CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        var parameters = new Dictionary<string, object> { { "symbol", symbol } };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<IEnumerable<BinanceRecentTradeQuote>>(GetUrl(historicalTradesEndpoint, fapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 20).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceRecentTrade>>(result.Data);
    }
    #endregion

    #region Compressed/Aggregate Trades List
    public async Task<RestCallResult<IEnumerable<BinanceAggregatedTrade>>> GetAggregatedTradeHistoryAsync(string symbol, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object> { { "symbol", symbol } };
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceAggregatedTrade>>(GetUrl(aggregatedTradesEndpoint, fapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 20).ConfigureAwait(false);
    }
    #endregion

    #region Kline/Candlestick Data
    public async Task<RestCallResult<IEnumerable<IBinanceKline>>> GetKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new ParameterCollection {
                { "symbol", symbol },
            };
        parameters.AddEnum("interval", interval);

        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        var result = await SendRequestInternal<IEnumerable<BinanceFuturesUsdtKline>>(GetUrl(klinesEndpoint, fapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceKline>>(result.Data);
    }
    #endregion

    #region Continuous Contract Kline/Candlestick Data
    public async Task<RestCallResult<IEnumerable<IBinanceKline>>> GetContinuousContractKlinesAsync(string pair, ContractType contractType, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new ParameterCollection {
                { "pair", pair },
                { "contractType", JsonConvert.SerializeObject(contractType, new ContractTypeConverter(false)) }
            };
        parameters.AddEnum("interval", interval);

        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        var result = await SendRequestInternal<IEnumerable<BinanceFuturesUsdtKline>>(GetUrl(continuousContractKlineEndpoint, fapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceKline>>(result.Data);
    }
    #endregion

    #region Index Price Kline/Candlestick Data
    public async Task<RestCallResult<IEnumerable<IBinanceKline>>> GetIndexPriceKlinesAsync(string pair, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new ParameterCollection {
                { "pair", pair },
            };
        parameters.AddEnum("interval", interval);

        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        var result = await SendRequestInternal<IEnumerable<BinanceFuturesUsdtKline>>(GetUrl(indexPriceKlinesKlineEndpoint, fapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceKline>>(result.Data);
    }
    #endregion

    #region Mark Price Kline/Candlestick Data
    public async Task<RestCallResult<IEnumerable<BinanceFuturesMarkIndexKline>>> GetMarkPriceKlinesAsync(string symbol, BinanceKlineInterval interval, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);

        var parameters = new ParameterCollection {
                { "symbol", symbol },
            };
        parameters.AddEnum("interval", interval);

        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        return await SendRequestInternal<IEnumerable<BinanceFuturesMarkIndexKline>>(GetUrl(markPriceKlinesEndpoint, fapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
    }
    #endregion

    #region Mark Price
    public async Task<RestCallResult<BinanceFuturesMarkPrice>> GetMarkPriceAsync(string symbol,
        CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);

        return await SendRequestInternal<BinanceFuturesMarkPrice>(GetUrl(markPriceEndpoint, fapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesMarkPrice>>> GetMarkPricesAsync(CancellationToken ct = default)
    {
        return await SendRequestInternal<IEnumerable<BinanceFuturesMarkPrice>>(GetUrl(markPriceEndpoint, fapi, v1), HttpMethod.Get, ct).ConfigureAwait(false);
    }
    #endregion

    #region Get Funding Rate History
    public async Task<RestCallResult<IEnumerable<BinanceFuturesFundingRateHistory>>> GetFundingRatesAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        var parameters = new Dictionary<string, object> {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFuturesFundingRateHistory>>(GetUrl(fundingRateHistoryEndpoint, fapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region 24hr Ticker Price Change Statistics
    public  Task<RestCallResult<BinanceFullTicker>> GetTickerAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);

        return SendRequestInternal<BinanceFullTicker>(GetUrl(price24HEndpoint, fapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters);
    }

    public  Task<RestCallResult<IEnumerable<BinanceFullTicker>>> GetTickersAsync(CancellationToken ct = default)
    {
        return SendRequestInternal<IEnumerable<BinanceFullTicker>>(GetUrl(price24HEndpoint, fapi, v1), HttpMethod.Get, ct, requestWeight: 40);
    }
    #endregion

    #region Symbol Price Ticker
    public async Task<RestCallResult<BinancePriceTicker>> GetPriceAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "symbol", symbol }
        };

        return await SendRequestInternal<BinancePriceTicker>(GetUrl(allPricesEndpoint, fapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinancePriceTicker>>> GetPricesAsync(CancellationToken ct = default)
    {
        return await SendRequestInternal<IEnumerable<BinancePriceTicker>>(GetUrl(allPricesEndpoint, fapi, v1), HttpMethod.Get, ct, requestWeight: 2).ConfigureAwait(false);
    }
    #endregion

    #region Symbol Order Book Ticker
    public async Task<RestCallResult<BinanceBookTicker>> GetBookPriceAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);

        return await SendRequestInternal<BinanceBookTicker>(GetUrl(bookPricesEndpoint, fapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceBookTicker>>> GetBookPricesAsync(CancellationToken ct = default)
    {
        return await SendRequestInternal<IEnumerable<BinanceBookTicker>>(GetUrl(bookPricesEndpoint, fapi, v1), HttpMethod.Get, ct, requestWeight: 2).ConfigureAwait(false);
    }
    #endregion

    #region Open Interest
    public async Task<RestCallResult<BinanceFuturesOpenInterest>> GetOpenInterestAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
        {
            { "symbol", symbol }
        };

        return await SendRequestInternal<BinanceFuturesOpenInterest>(GetUrl(openInterestEndpoint, fapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Open Interest History
    public async Task<RestCallResult<IEnumerable<BinanceFuturesOpenInterestHistory>>> GetOpenInterestHistoryAsync(string symbol, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new Dictionary<string, object> {
            { "symbol", symbol },
            { "period", JsonConvert.SerializeObject(period, new PeriodIntervalConverter(false)) }
        };

        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());

        return await SendRequestInternal<IEnumerable<BinanceFuturesOpenInterestHistory>>(GetUrl(openInterestHistoryEndpoint, tradingDataApi), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Top Trader Long/Short Ratio (Accounts)
    public async Task<RestCallResult<IEnumerable<BinanceFuturesLongShortRatio>>> GetTopLongShortAccountRatioAsync(string symbolPair, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var url = GetUrl(topLongShortAccountRatioEndpoint, tradingDataApi);
        var parameters = new Dictionary<string, object> {
                { url.ToString().Contains("dapi") ? "pair": "symbol", symbolPair },
                { "period", JsonConvert.SerializeObject(period, new PeriodIntervalConverter(false)) }
            };

        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());

        return await SendRequestInternal<IEnumerable<BinanceFuturesLongShortRatio>>(url, HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Top Trader Long/Short Ratio (Positions)
    public async Task<RestCallResult<IEnumerable<BinanceFuturesLongShortRatio>>> GetTopLongShortPositionRatioAsync(string symbolPair, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var url = GetUrl(topLongShortPositionRatioEndpoint, tradingDataApi);
        var parameters = new Dictionary<string, object> {
            { url.ToString().Contains("dapi") ? "pair": "symbol", symbolPair },
            { "period", JsonConvert.SerializeObject(period, new PeriodIntervalConverter(false)) }
        };

        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());

        return await SendRequestInternal<IEnumerable<BinanceFuturesLongShortRatio>>(url, HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Long/Short Ratio (Accounts)
    public async Task<RestCallResult<IEnumerable<BinanceFuturesLongShortRatio>>> GetGlobalLongShortAccountRatioAsync(string symbolPair, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var url = GetUrl(globalLongShortAccountRatioEndpoint, tradingDataApi);
        var parameters = new Dictionary<string, object> {
            { url.ToString().Contains("dapi") ? "pair": "symbol", symbolPair },
            { "period", JsonConvert.SerializeObject(period, new PeriodIntervalConverter(false)) }
        };

        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());

        return await SendRequestInternal<IEnumerable<BinanceFuturesLongShortRatio>>(url, HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Taker Buy/Sell Volume Ratio
    public async Task<RestCallResult<IEnumerable<BinanceFuturesBuySellVolumeRatio>>> GetTakerBuySellVolumeRatioAsync(string symbol, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new Dictionary<string, object> {
            { "symbol", symbol },
            { "period", JsonConvert.SerializeObject(period, new PeriodIntervalConverter(false)) }
        };

        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());

        return await SendRequestInternal<IEnumerable<BinanceFuturesBuySellVolumeRatio>>(GetUrl(takerBuySellVolumeRatioEndpoint, tradingDataApi), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Composite Index Symbol Information
    public async Task<RestCallResult<IEnumerable<BinanceFuturesCompositeIndexInfo>>> GetCompositeIndexInfoAsync(string symbol = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        return await SendRequestInternal<IEnumerable<BinanceFuturesCompositeIndexInfo>>(GetUrl(compositeIndexapi, fapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Multi-Assets Mode Asset Index
    public async Task<RestCallResult<IEnumerable<BinanceFuturesAssetIndex>>> GetAssetIndexesAsync(CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        return await SendRequestInternal<IEnumerable<BinanceFuturesAssetIndex>>(GetUrl(assetIndexEndpoint, fapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }

    public async Task<RestCallResult<BinanceFuturesAssetIndex>> GetAssetIndexAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
            {
                { "symbol", symbol }
            };
        return await SendRequestInternal<BinanceFuturesAssetIndex>(GetUrl(assetIndexEndpoint, fapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #endregion

    #region Account Methods

    #region Change Position Mode
    public async Task<RestCallResult<BinanceResult>> ModifyPositionModeAsync(bool dualPositionSide, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "dualSidePosition", dualPositionSide.ToString().ToLower() }
        };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceResult>(GetUrl(positionModeSideSetEndpoint, fapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get Current Position Mode
    public async Task<RestCallResult<BinanceFuturesPositionMode>> GetPositionModeAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesPositionMode>(GetUrl(positionModeSideGetEndpoint, fapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 30).ConfigureAwait(false);
    }
    #endregion

    #region Change Multi-Assets Mode
    public async Task<RestCallResult<BinanceResult>> SetMultiAssetsModeAsync(bool enabled, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "multiAssetsMargin", enabled.ToString() }
        };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceResult>(GetUrl(futuresAccountMultiAssetsModeSetEndpoint, fapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get Current Multi-Assets Mode
    public async Task<RestCallResult<BinanceFuturesMultiAssetMode>> GetMultiAssetsModeAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesMultiAssetMode>(GetUrl(futuresAccountMultiAssetsModeGetEndpoint, fapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 30).ConfigureAwait(false);
    }
    #endregion

    #region Future Account Balance
    public async Task<RestCallResult<IEnumerable<BinanceFuturesAccountBalance>>> GetBalancesAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFuturesAccountBalance>>(GetUrl(futuresAccountBalanceEndpoint, fapi, "2"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
    }
    #endregion

    #region Account Information
    public async Task<RestCallResult<BinanceFuturesAccountInfo>> GetAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesAccountInfo>(GetUrl(accountInfoEndpoint, fapi, "2"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
    }
    #endregion

    #region Change Initial Leverage
    public async Task<RestCallResult<BinanceFuturesInitialLeverageChangeResult>> ChangeInitialLeverageAsync(string symbol, int leverage, long? receiveWindow = null, CancellationToken ct = default)
    {
        leverage.ValidateIntBetween(nameof(leverage), 1, 125);
        var parameters = new Dictionary<string, object>
        {
            { "symbol", symbol },
            { "leverage", leverage }
        };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceFuturesInitialLeverageChangeResult>(GetUrl(changeInitialLeverageEndpoint, fapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Change Margin Type
    public async Task<RestCallResult<BinanceFuturesChangeMarginTypeResult>> ChangeMarginTypeAsync(string symbol, FuturesMarginType marginType, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "symbol", symbol },
            { "marginType", JsonConvert.SerializeObject(marginType, new FuturesMarginTypeConverter(false)) }
        };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceFuturesChangeMarginTypeResult>(GetUrl(changeMarginTypeEndpoint, fapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Modify Isolated Position Margin
    public async Task<RestCallResult<BinanceFuturesPositionMarginResult>> ModifyPositionMarginAsync(string symbol, decimal quantity, FuturesMarginChangeDirectionType type, PositionSide? positionSide = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "symbol", symbol },
            { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
            { "type", JsonConvert.SerializeObject(type, new FuturesMarginChangeDirectionTypeConverter(false)) }
        };
        parameters.AddOptionalParameter("positionSide", positionSide == null ? null : JsonConvert.SerializeObject(positionSide, new PositionSideConverter(false)));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesPositionMarginResult>(GetUrl(positionMarginEndpoint, fapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get Postion Margin Change History
    public async Task<RestCallResult<IEnumerable<BinanceFuturesMarginChangeHistoryResult>>> GetMarginChangeHistoryAsync(string symbol, FuturesMarginChangeDirectionType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "symbol", symbol }
        };
        parameters.AddOptionalParameter("type", type.HasValue ? JsonConvert.SerializeObject(type, new FuturesMarginChangeDirectionTypeConverter(false)) : null);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFuturesMarginChangeHistoryResult>>(GetUrl(positionMarginChangeHistoryEndpoint, fapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Position Information
    public async Task<RestCallResult<IEnumerable<BinancePositionDetailsUsdt>>> GetPositionInformationAsync(string symbol = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinancePositionDetailsUsdt>>(GetUrl(positionInformationEndpoint, fapi, "2"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
    }
    #endregion

    #region Get Income History
    public async Task<RestCallResult<IEnumerable<BinanceFuturesIncomeHistory>>> GetIncomeHistoryAsync(string symbol = null, string incomeType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("incomeType", incomeType);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFuturesIncomeHistory>>(GetUrl(incomeHistoryEndpoint, fapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 30).ConfigureAwait(false);
    }
    #endregion

    #region Notional and Leverage Brackets
    public async Task<RestCallResult<IEnumerable<BinanceFuturesSymbolBracket>>> GetBracketsAsync(string symbolOrPair = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var url = GetUrl(leverageBracketEndpoint, fapi, v1);
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter(url.ToString().Contains("dapi") ? "pair" : "symbol", symbolOrPair);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFuturesSymbolBracket>>(url, HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Position ADL Quantile Estimations
    public async Task<RestCallResult<IEnumerable<BinanceFuturesQuantileEstimation>>> GetPositionAdlQuantileEstimationAsync(string symbol = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        if (symbol == null)
            return await SendRequestInternal<IEnumerable<BinanceFuturesQuantileEstimation>>(GetUrl(adlQuantileEndpoint, fapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);

        var result = await SendRequestInternal<BinanceFuturesQuantileEstimation>(GetUrl(adlQuantileEndpoint, fapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result)
            return result.As<IEnumerable<BinanceFuturesQuantileEstimation>>(null);

        return result.As<IEnumerable<BinanceFuturesQuantileEstimation>>([result.Data]);
    }
    #endregion

    #region Futures Trading Quantitative Rules Indicators
    public async Task<RestCallResult<BinanceFuturesTradingStatus>> GetTradingStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesTradingStatus>(GetUrl(tradingStatusEndpoint, fapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);

    }
    #endregion

    #region User Commission Rate
    public async Task<RestCallResult<BinanceFuturesAccountUserCommissionRate>> GetUserCommissionRateAsync(string symbol, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "symbol", symbol}
        };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceFuturesAccountUserCommissionRate>(GetUrl(futuresAccountUserCommissionRateEndpoint, fapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20).ConfigureAwait(false);
    }
    #endregion

    #region Get Download Id For Futures Transaction History
    public async Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForTransactionHistoryAsync(DateTime startTime, DateTime endTime, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "startTime", startTime.ConvertToMilliseconds() },
            { "endTime", endTime.ConvertToMilliseconds() },
        };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceFuturesDownloadIdInfo>(GetUrl(downloadIdEndpoint, fapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
    }
    #endregion

    #region Get Futures Transaction History Download Link by Id
    public async Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForTransactionHistoryAsync(string downloadId, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
    {
        { "downloadId", downloadId }
    };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceFuturesDownloadLink>(GetUrl(downloadLinkEndpoint, fapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
    }
    #endregion

    #endregion

    #region Trading Methods

    #region New Order
    public async Task<RestCallResult<BinanceFuturesPlacedOrder>> PlaceOrderAsync(
        string symbol,
        BinanceSpotOrderSide side,
        FuturesOrderType type,
        decimal? quantity,
        decimal? price = null,
        PositionSide? positionSide = null,
        BinanceSpotTimeInForce? timeInForce = null,
        bool? reduceOnly = null,
        string newClientOrderId = null,
        decimal? stopPrice = null,
        decimal? activationPrice = null,
        decimal? callbackRate = null,
        WorkingType? workingType = null,
        bool? closePosition = null,
        BinanceSpotOrderResponseType? orderResponseType = null,
        bool? priceProtect = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (closePosition == true && positionSide != null)
        {
            if (positionSide == PositionSide.Short && side == BinanceSpotOrderSide.Sell)
                throw new ArgumentException("Can't close short position with order side sell");
            if (positionSide == PositionSide.Long && side == BinanceSpotOrderSide.Buy)
                throw new ArgumentException("Can't close long position with order side buy");
        }

        if (orderResponseType == BinanceSpotOrderResponseType.Full)
            throw new ArgumentException("OrderResponseType.Full is not supported in Futures");

        var rulesCheck = await CheckTradeRules(symbol, quantity, null, price, stopPrice, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceFuturesPlacedOrder>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;

        var parameters = new Dictionary<string, object>
        {
            { "symbol", symbol },
            { "side", JsonConvert.SerializeObject(side, new OrderSideConverter(false)) },
            { "type", JsonConvert.SerializeObject(type, new FuturesOrderTypeConverter(false)) }
        };
        parameters.AddOptionalParameter("quantity", quantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("newClientOrderId", newClientOrderId);
        parameters.AddOptionalParameter("price", price?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("timeInForce", timeInForce == null ? null : JsonConvert.SerializeObject(timeInForce, new TimeInForceConverter(false)));
        parameters.AddOptionalParameter("positionSide", positionSide == null ? null : JsonConvert.SerializeObject(positionSide, new PositionSideConverter(false)));
        parameters.AddOptionalParameter("stopPrice", stopPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("activationPrice", activationPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("callbackRate", callbackRate?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("workingType", workingType == null ? null : JsonConvert.SerializeObject(workingType, new WorkingTypeConverter(false)));
        parameters.AddOptionalParameter("reduceOnly", reduceOnly?.ToString().ToLower());
        parameters.AddOptionalParameter("closePosition", closePosition?.ToString().ToLower());
        parameters.AddOptionalParameter("newOrderRespType", orderResponseType == null ? null : JsonConvert.SerializeObject(orderResponseType, new OrderResponseTypeConverter(false)));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("priceProtect", priceProtect?.ToString().ToUpper());

        var result = await SendRequestInternal<BinanceFuturesPlacedOrder>(GetUrl(newOrderEndpoint, fapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
        if (result) InvokeOrderPlaced(result.Data.Id);
        return result;
    }
    #endregion

    #region Place Multiple Orders
    public async Task<RestCallResult<IEnumerable<CallResult<BinanceFuturesPlacedOrder>>>> PlaceMultipleOrdersAsync(BinanceFuturesBatchOrder[] orders, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (orders.Length <= 0 || orders.Length > 5)
            throw new ArgumentException("Order list should be at least 1 and max 5 orders");

        if (ClientOptions.UsdtFuturesOptions.TradeRulesBehavior != BinanceTradeRulesBehavior.None)
        {
            foreach (var order in orders)
            {
                var rulesCheck = await CheckTradeRules(order.Symbol, order.Quantity, null, order.Price, order.StopPrice, order.Type, ct).ConfigureAwait(false);
                if (!rulesCheck.Passed)
                {
                    Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
                    return new RestCallResult<IEnumerable<CallResult<BinanceFuturesPlacedOrder>>>(new ArgumentError(rulesCheck.ErrorMessage!));
                }

                order.Quantity = rulesCheck.Quantity;
                order.Price = rulesCheck.Price;
                order.StopPrice = rulesCheck.StopPrice;
            }
        }

        var parameters = new Dictionary<string, object>();
        var parameterOrders = new Dictionary<string, object>[orders.Length];
        int i = 0;
        foreach (var order in orders)
        {
            var orderParameters = new Dictionary<string, object>()
            {
                { "symbol", order.Symbol },
                { "side", JsonConvert.SerializeObject(order.Side, new OrderSideConverter(false)) },
                { "type", JsonConvert.SerializeObject(order.Type, new FuturesOrderTypeConverter(false)) },
                { "newOrderRespType", "RESULT" }
            };

            orderParameters.AddOptionalParameter("quantity", order.Quantity?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalParameter("newClientOrderId", order.NewClientOrderId);
            orderParameters.AddOptionalParameter("price", order.Price?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalParameter("timeInForce", order.TimeInForce == null ? null : JsonConvert.SerializeObject(order.TimeInForce, new TimeInForceConverter(false)));
            orderParameters.AddOptionalParameter("positionSide", order.PositionSide == null ? null : JsonConvert.SerializeObject(order.PositionSide, new PositionSideConverter(false)));
            orderParameters.AddOptionalParameter("stopPrice", order.StopPrice?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalParameter("activationPrice", order.ActivationPrice?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalParameter("callbackRate", order.CallbackRate?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalParameter("workingType", order.WorkingType == null ? null : JsonConvert.SerializeObject(order.WorkingType, new WorkingTypeConverter(false)));
            orderParameters.AddOptionalParameter("reduceOnly", order.ReduceOnly?.ToString().ToLower());
            orderParameters.AddOptionalParameter("priceProtect", order.PriceProtect?.ToString().ToUpper());
            parameterOrders[i] = orderParameters;
            i++;
        }

        parameters.Add("batchOrders", JsonConvert.SerializeObject(parameterOrders));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var response = await SendRequestInternal<IEnumerable<BinanceFuturesMultipleOrderPlaceResult>>(GetUrl(multipleNewOrdersEndpoint, fapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!response.Success)
            return response.As<IEnumerable<CallResult<BinanceFuturesPlacedOrder>>>(default);

        var result = new List<CallResult<BinanceFuturesPlacedOrder>>();
        foreach (var item in response.Data)
        {
            result.Add(item.Code != 0
                ? new CallResult<BinanceFuturesPlacedOrder>(new ServerError(item.Code, item.Message))
                : new CallResult<BinanceFuturesPlacedOrder>(item));
        }

        return response.As<IEnumerable<CallResult<BinanceFuturesPlacedOrder>>>(result);
    }
    #endregion

    #region Query Order
    public async Task<RestCallResult<BinanceFuturesOrder>> GetOrderAsync(string symbol, long? orderId = null, string origClientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new Dictionary<string, object>
        {
            { "symbol", symbol }
        };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesOrder>(GetUrl(queryOrderEndpoint, fapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Cancel Order
    public async Task<RestCallResult<BinanceFuturesCancelOrder>> CancelOrderAsync(string symbol, long? orderId = null, string origClientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (!orderId.HasValue && string.IsNullOrEmpty(origClientOrderId))
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceFuturesCancelOrder>(GetUrl(cancelOrderEndpoint, fapi, v1), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);

        if (result) InvokeOrderCanceled(result.Data.Id);
        return result;
    }
    #endregion

    #region Cancel All Open Orders
    public async Task<RestCallResult<BinanceFuturesCancelAllOrders>> CancelAllOrdersAsync(string symbol, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesCancelAllOrders>(GetUrl(cancelAllOrdersEndpoint, fapi, v1), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Cancel Multiple Orders
    public async Task<RestCallResult<IEnumerable<CallResult<BinanceFuturesCancelOrder>>>> CancelMultipleOrdersAsync(string symbol, List<long> orderIdList = null, List<string> origClientOrderIdList = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderIdList == null && origClientOrderIdList == null)
            throw new ArgumentException("Either orderIdList or origClientOrderIdList must be sent");

        if (orderIdList?.Count > 10)
            throw new ArgumentException("orderIdList cannot contain more than 10 items");

        if (origClientOrderIdList?.Count > 10)
            throw new ArgumentException("origClientOrderIdList cannot contain more than 10 items");

        var parameters = new Dictionary<string, object>
        {
            { "symbol", symbol }
        };

        if (orderIdList != null)
            parameters.AddOptionalParameter("orderIdList", $"[{string.Join(",", orderIdList)}]");

        if (origClientOrderIdList != null)
            parameters.AddOptionalParameter("origClientOrderIdList", $"[{string.Join(",", origClientOrderIdList.Select(id => $"\"{id}\""))}]");

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var response = await SendRequestInternal<IEnumerable<BinanceFuturesMultipleOrderCancelResult>>(GetUrl(cancelMultipleOrdersEndpoint, fapi, v1), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);

        if (!response.Success)
            return response.As<IEnumerable<CallResult<BinanceFuturesCancelOrder>>>(default);

        var result = new List<CallResult<BinanceFuturesCancelOrder>>();
        foreach (var item in response.Data)
        {
            result.Add(item.Code != 0
                ? new CallResult<BinanceFuturesCancelOrder>(new ServerError(item.Code, item.Message))
                : new CallResult<BinanceFuturesCancelOrder>(item));
        }

        return response.As<IEnumerable<CallResult<BinanceFuturesCancelOrder>>>(result);
    }
    #endregion

    #region Auto-Cancel All Open Orders
    public async Task<RestCallResult<BinanceFuturesCountDownResult>> CancelAllOrdersAfterTimeoutAsync(string symbol, TimeSpan countDownTime, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "countdownTime", (int)countDownTime.TotalMilliseconds }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesCountDownResult>(GetUrl(countDownCancelAllEndpoint, fapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Query Current Open Order
    public async Task<RestCallResult<BinanceFuturesOrder>> GetOpenOrderAsync(string symbol, long? orderId = null, string origClientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesOrder>(GetUrl(openOrderEndpoint, fapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Current All Open Orders
    public async Task<RestCallResult<IEnumerable<BinanceFuturesOrder>>> GetOpenOrdersAsync(string symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("symbol", symbol);

        return await SendRequestInternal<IEnumerable<BinanceFuturesOrder>>(GetUrl(openOrdersEndpoint, fapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: symbol == null ? 40 : 1).ConfigureAwait(false);
    }
    #endregion

    #region All Orders
    public async Task<RestCallResult<IEnumerable<BinanceFuturesOrder>>> GetOrdersAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFuturesOrder>>(GetUrl(allOrdersEndpoint, fapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
    }
    #endregion

    #region Account Trade List
    public async Task<RestCallResult<IEnumerable<BinanceRecentTradeQuote>>> GetUserTradesAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceRecentTradeQuote>>(GetUrl(myFuturesTradesEndpoint, fapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
    }
    #endregion

    #region User's Force Orders
    public async Task<RestCallResult<IEnumerable<BinanceFuturesOrder>>> GetForcedOrdersAsync(string symbol = null, AutoCloseType? closeType = null, DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("autoCloseType", closeType.HasValue ? JsonConvert.SerializeObject(closeType, new AutoCloseTypeConverter(false)) : null);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());

        return await SendRequestInternal<IEnumerable<BinanceFuturesOrder>>(GetUrl(forceOrdersEndpoint, fapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: symbol == null ? 50 : 20).ConfigureAwait(false);
    }
    #endregion

    #endregion

    #region User Stream Methods

    #region Start User Data Stream
    public async Task<RestCallResult<string>> StartUserStreamAsync(CancellationToken ct = default)
    {
        var result = await SendRequestInternal<BinanceListenKey>(GetUrl(getFuturesListenKeyEndpoint, fapi, v1), HttpMethod.Post, ct, true).ConfigureAwait(false);
        return result.As(result.Data?.ListenKey!);
    }
    #endregion

    #region Keepalive User Data Stream
    public async Task<RestCallResult<object>> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new Dictionary<string, object>
        {
            { "listenKey", listenKey }
        };

        return await SendRequestInternal<object>(GetUrl(keepFuturesListenKeyAliveEndpoint, fapi, v1), HttpMethod.Put, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Close User Data Stream
    public async Task<RestCallResult<object>> StopUserStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new Dictionary<string, object>
        {
            { "listenKey", listenKey }
        };

        return await SendRequestInternal<object>(GetUrl(closeFuturesListenKeyEndpoint, fapi, v1), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #endregion

    #region Portfolio Methods
    #endregion

}
