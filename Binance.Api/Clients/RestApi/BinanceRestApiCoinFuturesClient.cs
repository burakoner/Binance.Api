using Binance.Api.Shared.Responses;
using Binance.Api.Spot.Internal;

namespace Binance.Api.Clients.RestApi;

public class BinanceRestApiCoinFuturesClient : RestApiClient
{
    // Clients
    public BinanceRestApiFuturesAlgoClient Algo { get => RootClient.FuturesAlgo; }
    public BinanceRestApiFuturesLoanClient Loan { get => RootClient.FuturesLoan; }
    public BinanceRestApiFuturesTransferClient Transfer { get => RootClient.FuturesTransfer; }

    // Api
    private const string v1 = "1";
    private const string dapi = "dapi";
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
    private const string markPriceEndpoint = "premiumIndex";
    private const string fundingRateHistoryEndpoint = "fundingRate";
    private const string klinesEndpoint = "klines";
    private const string continuousContractKlineEndpoint = "continuousKlines";
    private const string indexPriceKlineEndpoint = "indexPriceKlines";
    private const string markPriceKlinesEndpoint = "markPriceKlines";
    private const string price24HEndpoint = "ticker/24hr";
    private const string allPricesEndpoint = "ticker/price";
    private const string bookPricesEndpoint = "ticker/bookTicker";
    private const string openInterestEndpoint = "openInterest";
    private const string openInterestHistoryEndpoint = "openInterestHist";
    private const string topLongShortAccountRatioEndpoint = "topLongShortAccountRatio";
    private const string topLongShortPositionRatioEndpoint = "topLongShortPositionRatio";
    private const string globalLongShortAccountRatioEndpoint = "globalLongShortAccountRatio";
    private const string takerBuySellVolumeRatioEndpoint = "takerBuySellVol";
    private const string basisEndpoint = "basis";

    // Account
    private const string positionModeSideSetEndpoint = "positionSide/dual";
    private const string positionModeSideGetEndpoint = "positionSide/dual";
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
    private const string futuresAccountUserCommissionRateEndpoint = "commissionRate";

    // Trading
    private const string newOrderEndpoint = "order";
    // TODO: Modify Order
    private const string multipleNewOrdersEndpoint = "batchOrders";
    // TODO: Modify Multiple Orders
    // TODO: Get Order Modify History
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
    internal BinanceFuturesCoinExchangeInfo ExchangeInfo;
    internal DateTime? LastExchangeInfoUpdate;
    internal TimeSyncState TimeSyncState = new("Binance CoinFutures RestApi");

    // Root Client
    internal BinanceRestApiClient RootClient { get; }

    // Options
    public new BinanceRestApiClientOptions ClientOptions { get { return (BinanceRestApiClientOptions)base.ClientOptions; } }

    internal BinanceRestApiCoinFuturesClient(BinanceRestApiClient root) : base(root.Logger, root.Options)
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
        => new TimeSyncInfo(Logger, ClientOptions.AutoTimestamp, ClientOptions.TimestampRecalculationInterval, TimeSyncState);

    protected override TimeSpan GetTimeOffset()
        => TimeSyncState.TimeOffset;

    protected string GetSymbolName(string baseAsset, string quoteAsset) =>
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

    /// <summary>
    /// Event triggered when an order is placed via this client. Only available for Spot orders
    /// </summary>
    public event Action<long> OnOrderPlaced;

    /// <summary>
    /// Event triggered when an order is canceled via this client. Note that this does not trigger when using CancelAllOrdersAsync. Only available for Spot orders
    /// </summary>
    public event Action<long> OnOrderCanceled;

    internal Uri GetUrl(string endpoint, string api, string version = null)
    {
        var result = ClientOptions.CoinFuturesOptions.BaseAddress.AppendPath(api);

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

        if (ClientOptions.CoinFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.None)
            return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, outputPrice, outputStopPrice);

        if (ExchangeInfo == null || LastExchangeInfoUpdate == null || (DateTime.UtcNow - LastExchangeInfoUpdate.Value).TotalMinutes > ClientOptions.CoinFuturesOptions.TradeRulesUpdateInterval.TotalMinutes)
            await GetExchangeInfoAsync(ct).ConfigureAwait(false);

        if (ExchangeInfo == null)
            return BinanceTradeRuleResult.CreateFailed("Unable to retrieve trading rules, validation failed");

        var symbolData = ExchangeInfo.Symbols.SingleOrDefault(s => string.Equals(s.Name, symbol, StringComparison.CurrentCultureIgnoreCase));
        if (symbolData == null)
            return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Symbol {symbol} not found");

        if (!symbolData.OrderTypes.Contains(type))
            return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: {type} order type not allowed for {symbol}");

        if (symbolData.LotSizeFilter != null || symbolData.MarketLotSizeFilter != null && type == Enums.FuturesOrderType.Market)
        {
            var minQty = symbolData.LotSizeFilter?.MinQuantity;
            var maxQty = symbolData.LotSizeFilter?.MaxQuantity;
            var stepSize = symbolData.LotSizeFilter?.StepSize;
            if (type == Enums.FuturesOrderType.Market && symbolData.MarketLotSizeFilter != null)
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
                    if (ClientOptions.CoinFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
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
                if (ClientOptions.CoinFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
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
                    if (ClientOptions.CoinFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter max/min failed. Original price: {price}, Closest allowed: {outputPrice}");

                    Logger.Log(LogLevel.Information, $"price clamped from {price} to {outputPrice}");
                }

                if (stopPrice != null)
                {
                    outputStopPrice = BinanceHelpers.ClampPrice(symbolData.PriceFilter.MinPrice,
                        symbolData.PriceFilter.MaxPrice, stopPrice.Value);
                    if (outputStopPrice != stopPrice)
                    {
                        if (ClientOptions.CoinFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
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
                    if (ClientOptions.CoinFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter tick failed. Original price: {price}, Closest allowed: {outputPrice}");

                    Logger.Log(LogLevel.Information, $"price rounded from {beforePrice} to {outputPrice}");
                }

                if (stopPrice != null)
                {
                    var beforeStopPrice = outputStopPrice;
                    outputStopPrice = BinanceHelpers.FloorPrice(symbolData.PriceFilter.TickSize, stopPrice.Value);
                    if (outputStopPrice != beforeStopPrice)
                    {
                        if (ClientOptions.CoinFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
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
        var result = await SendRequestInternal<object>(GetUrl(pingEndpoint, dapi, v1), HttpMethod.Get, ct).ConfigureAwait(false);
        sw.Stop();
        return result ? result.As(sw.ElapsedMilliseconds) : result.As<long>(default!);
    }
    #endregion

    #region Check Server time
    public async Task<RestCallResult<DateTime>> GetServerTimeAsync(bool resetAutoTimestamp = false, CancellationToken ct = default)
    {
        var url = GetUrl(checkTimeEndpoint, dapi, v1);
        var result = await SendRequestInternal<BinanceServerTime>(url, HttpMethod.Get, ct, ignoreRatelimit: true).ConfigureAwait(false);
        return result.As(result.Data?.ServerTime ?? default);
    }
    #endregion

    #region Exchange Information
    public async Task<RestCallResult<BinanceFuturesCoinExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
    {
        var exchangeInfoResult = await SendRequestInternal<BinanceFuturesCoinExchangeInfo>(GetUrl(exchangeInfoEndpoint, dapi, v1), HttpMethod.Get, ct).ConfigureAwait(false);
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
        var result = await SendRequestInternal<BinanceFuturesOrderBook>(GetUrl(orderBookEndpoint, dapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
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
        var result = await SendRequestInternal<IEnumerable<BinanceRecentTradeBase>>(GetUrl(recentTradesEndpoint, dapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
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

        var result = await SendRequestInternal<IEnumerable<BinanceRecentTradeBase>>(GetUrl(historicalTradesEndpoint, dapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 20).ConfigureAwait(false);
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

        return await SendRequestInternal<IEnumerable<BinanceAggregatedTrade>>(GetUrl(aggregatedTradesEndpoint, dapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 20).ConfigureAwait(false);
    }
    #endregion

    #region Index Price and Mark Price
    public async Task<RestCallResult<IEnumerable<BinanceFuturesCoinMarkPrice>>> GetMarkPricesAsync(string symbol = null, string pair = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("pair", pair);

        return await SendRequestInternal<IEnumerable<BinanceFuturesCoinMarkPrice>>(GetUrl(markPriceEndpoint, dapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);

    }
    #endregion

    #region Get Funding Rate History of Perpetual Futures
    public async Task<RestCallResult<IEnumerable<BinanceFuturesFundingRateHistory>>> GetFundingRatesAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        var parameters = new Dictionary<string, object> {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFuturesFundingRateHistory>>(GetUrl(fundingRateHistoryEndpoint, dapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
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
        var result = await SendRequestInternal<IEnumerable<BinanceFuturesCoinKline>>(GetUrl(klinesEndpoint, dapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
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
        var result = await SendRequestInternal<IEnumerable<BinanceFuturesCoinKline>>(GetUrl(continuousContractKlineEndpoint, dapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceKline>>(result.Data);
    }
    #endregion

    #region Index Price Kline/Candlestick Data
    public async Task<RestCallResult<IEnumerable<BinanceFuturesMarkIndexKline>>> GetIndexPriceKlinesAsync(string pair, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
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
        return await SendRequestInternal<IEnumerable<BinanceFuturesMarkIndexKline>>(GetUrl(indexPriceKlineEndpoint, dapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
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
        return await SendRequestInternal<IEnumerable<BinanceFuturesMarkIndexKline>>(GetUrl(markPriceKlinesEndpoint, dapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
    }
    #endregion

    #region 24hr Ticker Price Change Statistics
    public async Task<RestCallResult<IEnumerable<IBinance24HPrice>>> GetTickersAsync(string symbol = null, string pair = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("pair", pair);

        var result = await SendRequestInternal<IEnumerable<BinanceFuturesCoin24HPrice>>(GetUrl(price24HEndpoint, dapi, v1),
                HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: symbol == null ? 40 : 1).ConfigureAwait(false);
        return result.As<IEnumerable<IBinance24HPrice>>(result.Success ? result.Data : null);
    }
    #endregion

    #region Symbol Price Ticker
    public async Task<RestCallResult<IEnumerable<BinanceFuturesCoinPrice>>> GetPricesAsync(string symbol = null, string pair = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("pair", pair);

        return await SendRequestInternal<IEnumerable<BinanceFuturesCoinPrice>>(GetUrl(allPricesEndpoint, dapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: symbol == null ? 2 : 1).ConfigureAwait(false);
    }
    #endregion

    #region Symbol Order Book Ticker
    public async Task<RestCallResult<IEnumerable<BinanceFuturesBookPrice>>> GetBookPricesAsync(string symbol = null, string pair = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("pair", pair);

        return await SendRequestInternal<IEnumerable<BinanceFuturesBookPrice>>(
                GetUrl(bookPricesEndpoint, dapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: symbol == null ? 2 : 1)
            .ConfigureAwait(false);
    }
    #endregion

    #region Open Interest
    public async Task<RestCallResult<BinanceFuturesCoinOpenInterest>> GetOpenInterestAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
            {
                { "symbol", symbol }
            };

        return await SendRequestInternal<BinanceFuturesCoinOpenInterest>(GetUrl(openInterestEndpoint, dapi, v1), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Open Interest Statistics
    public async Task<RestCallResult<IEnumerable<BinanceFuturesCoinOpenInterestHistory>>> GetOpenInterestHistoryAsync(string pair, ContractType contractType, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new Dictionary<string, object> {
                { "pair", pair },
                { "period", JsonConvert.SerializeObject(period, new PeriodIntervalConverter(false)) },
                { "contractType", JsonConvert.SerializeObject(contractType, new ContractTypeConverter(false)) }
            };

        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());

        return await SendRequestInternal<IEnumerable<BinanceFuturesCoinOpenInterestHistory>>(GetUrl(openInterestHistoryEndpoint, tradingDataApi), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Top Trader Long/Short Ratio (Accounts)
    public async Task<RestCallResult<IEnumerable<BinanceFuturesLongShortRatio>>> GetTopLongShortAccountRatioAsync(string symbolPair, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var url = GetUrl(topLongShortAccountRatioEndpoint, tradingDataApi);
        var parameters = new Dictionary<string, object> {
                { url.ToString().Contains(dapi) ? "pair": "symbol", symbolPair },
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
                { url.ToString().Contains(dapi) ? "pair": "symbol", symbolPair },
                { "period", JsonConvert.SerializeObject(period, new PeriodIntervalConverter(false)) }
            };

        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());

        return await SendRequestInternal<IEnumerable<BinanceFuturesLongShortRatio>>(url, HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Long/Short Ratio
    public async Task<RestCallResult<IEnumerable<BinanceFuturesLongShortRatio>>> GetGlobalLongShortAccountRatioAsync(string symbolPair, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var url = GetUrl(globalLongShortAccountRatioEndpoint, tradingDataApi);
        var parameters = new Dictionary<string, object> {
                { url.ToString().Contains(dapi) ? "pair": "symbol", symbolPair },
                { "period", JsonConvert.SerializeObject(period, new PeriodIntervalConverter(false)) }
            };

        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());

        return await SendRequestInternal<IEnumerable<BinanceFuturesLongShortRatio>>(url, HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Taker Buy/Sell Volume
    public async Task<RestCallResult<IEnumerable<BinanceFuturesCoinBuySellVolumeRatio>>> GetTakerBuySellVolumeRatioAsync(string pair, ContractType contractType, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new Dictionary<string, object> {
                { "pair", pair },
                { "period", JsonConvert.SerializeObject(period, new PeriodIntervalConverter(false)) },
                { "contractType", JsonConvert.SerializeObject(contractType, new ContractTypeConverter(false)) }
            };

        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());

        return await SendRequestInternal<IEnumerable<BinanceFuturesCoinBuySellVolumeRatio>>(GetUrl(takerBuySellVolumeRatioEndpoint, tradingDataApi), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Basis
    public async Task<RestCallResult<IEnumerable<BinanceFuturesBasis>>> GetBasisAsync(string pair, ContractType contractType, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new Dictionary<string, object> {
                { "pair", pair },
                { "period", JsonConvert.SerializeObject(period, new PeriodIntervalConverter(false)) },
                { "contractType", JsonConvert.SerializeObject(contractType, new ContractTypeConverter(false)) }
            };

        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());

        return await SendRequestInternal<IEnumerable<BinanceFuturesBasis>>(GetUrl(basisEndpoint, tradingDataApi), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #endregion

    #region Account Methods

    #region Change Position Mode
    public async Task<RestCallResult<BinanceResult>> ModifyPositionModeAsync(bool dualPositionSide, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "dualSidePosition", dualPositionSide.ToString().ToLower() }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceResult>(GetUrl(positionModeSideSetEndpoint, dapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get Current Position Mode
    public async Task<RestCallResult<BinanceFuturesPositionMode>> GetPositionModeAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesPositionMode>(GetUrl(positionModeSideGetEndpoint, dapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 30).ConfigureAwait(false);
    }
    #endregion

    #region Future Account Balance
    public async Task<RestCallResult<IEnumerable<BinanceFuturesAccountBalance>>> GetBalancesAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFuturesAccountBalance>>(GetUrl(futuresAccountBalanceEndpoint, dapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Account Information
    public async Task<RestCallResult<BinanceFuturesCoinAccountInfo>> GetAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesCoinAccountInfo>(GetUrl(accountInfoEndpoint, dapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
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

        return await SendRequestInternal<BinanceFuturesInitialLeverageChangeResult>(GetUrl(changeInitialLeverageEndpoint, dapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
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

        return await SendRequestInternal<BinanceFuturesChangeMarginTypeResult>(GetUrl(changeMarginTypeEndpoint, dapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Modify Isolated Position Margin
    public async Task<RestCallResult<BinanceFuturesPositionMarginResult>> ModifyPositionMarginAsync(string symbol, decimal quantity, FuturesMarginChangeDirectionType type, PositionSide? positionSide = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
                { "type", JsonConvert.SerializeObject(type, new FuturesMarginChangeDirectionTypeConverter(false)) },
            };
        parameters.AddOptionalParameter("positionSide", positionSide == null ? null : JsonConvert.SerializeObject(positionSide, new PositionSideConverter(false)));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesPositionMarginResult>(GetUrl(positionMarginEndpoint, dapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
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

        return await SendRequestInternal<IEnumerable<BinanceFuturesMarginChangeHistoryResult>>(GetUrl(positionMarginChangeHistoryEndpoint, dapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Position Information
    public async Task<RestCallResult<IEnumerable<BinancePositionDetailsCoin>>> GetPositionInformationAsync(string marginAsset = null, string pair = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();

        parameters.AddOptionalParameter("marginAsset", marginAsset);
        parameters.AddOptionalParameter("pair", pair);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinancePositionDetailsCoin>>(GetUrl(positionInformationEndpoint, dapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
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

        return await SendRequestInternal<IEnumerable<BinanceFuturesIncomeHistory>>(GetUrl(incomeHistoryEndpoint, dapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20).ConfigureAwait(false);
    }
    #endregion

    #region Notional Bracket for Pair
    public async Task<RestCallResult<IEnumerable<BinanceFuturesSymbolBracket>>> GetPairBracketsAsync(string pair = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var url = GetUrl(leverageBracketEndpoint, dapi, v1);
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("pair", pair);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFuturesSymbolBracket>>(url, HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Notional Bracket for Symbol
    public async Task<RestCallResult<IEnumerable<BinanceFuturesSymbolBracket>>> GetSymbolBracketsAsync(string symbol = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var url = GetUrl(leverageBracketEndpoint, dapi, "2");
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFuturesSymbolBracket>>(url, HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Position ADL Quantile Estimation
    public async Task<RestCallResult<IEnumerable<BinanceFuturesQuantileEstimation>>> GetPositionAdlQuantileEstimationAsync(string symbol = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFuturesQuantileEstimation>>(GetUrl(adlQuantileEndpoint, dapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
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
        return await SendRequestInternal<BinanceFuturesAccountUserCommissionRate>(GetUrl(futuresAccountUserCommissionRateEndpoint, dapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20).ConfigureAwait(false);
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

        var result = await SendRequestInternal<BinanceFuturesPlacedOrder>(GetUrl(newOrderEndpoint, dapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
        if (result) InvokeOrderPlaced(result.Data.Id);
        return result;
    }
    #endregion

    #region Multiple New Orders
    public async Task<RestCallResult<IEnumerable<CallResult<BinanceFuturesPlacedOrder>>>> PlaceMultipleOrdersAsync(
        BinanceFuturesBatchOrder[] orders,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (orders.Length <= 0 || orders.Length > 5)
            throw new ArgumentException("Order list should be at least 1 and max 5 orders");

        if (ClientOptions.CoinFuturesOptions.TradeRulesBehavior != BinanceTradeRulesBehavior.None)
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

        var response = await SendRequestInternal<IEnumerable<BinanceFuturesMultipleOrderPlaceResult>>(GetUrl(multipleNewOrdersEndpoint, dapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5).ConfigureAwait(false);
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

        return await SendRequestInternal<BinanceFuturesOrder>(GetUrl(queryOrderEndpoint, dapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
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

        var result = await SendRequestInternal<BinanceFuturesCancelOrder>(GetUrl(cancelOrderEndpoint, dapi, v1), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
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

        return await SendRequestInternal<BinanceFuturesCancelAllOrders>(GetUrl(cancelAllOrdersEndpoint, dapi, v1), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
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

        var response = await SendRequestInternal<IEnumerable<BinanceFuturesMultipleOrderCancelResult>>(GetUrl(cancelMultipleOrdersEndpoint, dapi, v1), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);

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

        return await SendRequestInternal<BinanceFuturesCountDownResult>(GetUrl(countDownCancelAllEndpoint, dapi, v1), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 10).ConfigureAwait(false);
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

        return await SendRequestInternal<BinanceFuturesOrder>(GetUrl(openOrderEndpoint, dapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Current All Open Orders
    public async Task<RestCallResult<IEnumerable<BinanceFuturesOrder>>> GetOpenOrdersAsync(string symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("symbol", symbol);

        return await SendRequestInternal<IEnumerable<BinanceFuturesOrder>>(GetUrl(openOrdersEndpoint, dapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: symbol == null ? 5 : 1).ConfigureAwait(false);
    }
    #endregion

    #region All Orders
    public async Task<RestCallResult<IEnumerable<BinanceFuturesOrder>>> GetOrdersAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFuturesOrder>>(GetUrl(allOrdersEndpoint, dapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: symbol == null ? 20 : 40).ConfigureAwait(false);
    }
    #endregion

    #region Account Trade List
    public async Task<RestCallResult<IEnumerable<BinanceRecentTradeQuote>>> GetUserTradesAsync(string symbol = null, string pair = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("pair", pair);
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceRecentTradeQuote>>(GetUrl(myFuturesTradesEndpoint, dapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: symbol == null ? 40 : 20).ConfigureAwait(false);
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

        return await SendRequestInternal<IEnumerable<BinanceFuturesOrder>>(GetUrl(forceOrdersEndpoint, dapi, v1), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: symbol == null ? 50 : 20).ConfigureAwait(false);
    }
    #endregion

    #endregion

    #region User Stream Methods

    #region Start User Data Stream
    public async Task<RestCallResult<string>> StartUserStreamAsync(CancellationToken ct = default)
    {
        var result = await SendRequestInternal<BinanceListenKey>(GetUrl(getFuturesListenKeyEndpoint, dapi, v1), HttpMethod.Post, ct, true).ConfigureAwait(false);
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

        return await SendRequestInternal<object>(GetUrl(keepFuturesListenKeyAliveEndpoint, dapi, v1), HttpMethod.Put, ct, true, bodyParameters: parameters).ConfigureAwait(false);
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

        return await SendRequestInternal<object>(GetUrl(closeFuturesListenKeyEndpoint, dapi, v1), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #endregion

    #region Portfolio Methods
    #endregion

}