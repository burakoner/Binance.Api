using Binance.Api.Models.RestApi.Futures;

namespace Binance.Api.Clients.RestApi.CoinFutures;

public class BinanceRestApiCoinFuturesMarketDataClient
{
    // Api
    private const string api = "dapi";
    private const string publicVersion = "1";
    private const string tradingDataapi = "futures/data";

    // Order Book
    private const string orderBookEndpoint = "depth";

    // Trades
    private const string recentTradesEndpoint = "trades";
    private const string historicalTradesEndpoint = "historicalTrades";
    private const string aggregatedTradesEndpoint = "aggTrades";

    // Index & Rates
    private const string markPriceEndpoint = "premiumIndex";
    private const string fundingRateHistoryEndpoint = "fundingRate";

    // Klines
    private const string klinesEndpoint = "klines";
    private const string continuousContractKlineEndpoint = "continuousKlines";
    private const string indexPriceKlineEndpoint = "indexPriceKlines";
    private const string markPriceKlinesEndpoint = "markPriceKlines";

    // Tickers
    private const string price24HEndpoint = "ticker/24hr";
    private const string allPricesEndpoint = "ticker/price";
    private const string bookPricesEndpoint = "ticker/bookTicker";

    // Interests
    private const string openInterestEndpoint = "openInterest";
    private const string openInterestHistoryEndpoint = "openInterestHist";

    // Ratios
    private const string topLongShortAccountRatioEndpoint = "topLongShortAccountRatio";
    private const string topLongShortPositionRatioEndpoint = "topLongShortPositionRatio";
    private const string globalLongShortAccountRatioEndpoint = "globalLongShortAccountRatio";

    // Other
    private const string takerBuySellVolumeRatioEndpoint = "takerBuySellVol";
    private const string basisEndpoint = "basis";


    // Internal References
    internal BinanceRestApiCoinFuturesClient MainClient { get; }
    internal BinanceRestApiClientOptions Options { get => MainClient.RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal BinanceRestApiCoinFuturesMarketDataClient(BinanceRestApiCoinFuturesClient main)
    {
        MainClient = main;
    }

    #region Order Book
    public async Task<RestCallResult<BinanceFuturesOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntValues(nameof(limit), 5, 10, 20, 50, 100, 500, 1000);
        var parameters = new Dictionary<string, object> { { "symbol", symbol } };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var requestWeight = limit == null ? 10 : limit <= 50 ? 2 : limit == 100 ? 5 : limit == 500 ? 10 : 20;
        var result = await SendRequestInternal<BinanceFuturesOrderBook>(GetUrl(orderBookEndpoint, api, publicVersion), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
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
        var result = await SendRequestInternal<IEnumerable<BinanceRecentTradeBase>>(GetUrl(recentTradesEndpoint, api, publicVersion), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
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

        var result = await SendRequestInternal<IEnumerable<BinanceRecentTradeBase>>(GetUrl(historicalTradesEndpoint, api, publicVersion), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 20).ConfigureAwait(false);
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

        return await SendRequestInternal<IEnumerable<BinanceAggregatedTrade>>(GetUrl(aggregatedTradesEndpoint, api, publicVersion), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 20).ConfigureAwait(false);
    }
    #endregion

    #region Index Price and Mark Price
    public async Task<RestCallResult<IEnumerable<BinanceFuturesCoinMarkPrice>>> GetMarkPricesAsync(string symbol = null, string pair = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("pair", pair);

        return await SendRequestInternal<IEnumerable<BinanceFuturesCoinMarkPrice>>(GetUrl(markPriceEndpoint, api, publicVersion), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);

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

        return await SendRequestInternal<IEnumerable<BinanceFuturesFundingRateHistory>>(GetUrl(fundingRateHistoryEndpoint, api, publicVersion), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Kline/Candlestick Data
    public async Task<RestCallResult<IEnumerable<IBinanceKline>>> GetKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new Dictionary<string, object> {
                { "symbol", symbol },
                { "interval", JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false)) }
            };
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        var result = await SendRequestInternal<IEnumerable<BinanceFuturesCoinKline>>(GetUrl(klinesEndpoint, api, publicVersion), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceKline>>(result.Data);
    }
    #endregion

    #region Continuous Contract Kline/Candlestick Data
    public async Task<RestCallResult<IEnumerable<IBinanceKline>>> GetContinuousContractKlinesAsync(string pair, ContractType contractType, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new Dictionary<string, object> {
                { "pair", pair },
                { "interval", JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false)) },
                { "contractType", JsonConvert.SerializeObject(contractType, new ContractTypeConverter(false)) }
            };
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        var result = await SendRequestInternal<IEnumerable<BinanceFuturesCoinKline>>(GetUrl(continuousContractKlineEndpoint, api, publicVersion), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceKline>>(result.Data);
    }
    #endregion

    #region Index Price Kline/Candlestick Data
    public async Task<RestCallResult<IEnumerable<BinanceFuturesMarkIndexKline>>> GetIndexPriceKlinesAsync(string pair, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);
        var parameters = new Dictionary<string, object> {
                { "pair", pair },
                { "interval", JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false)) }
            };
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        return await SendRequestInternal<IEnumerable<BinanceFuturesMarkIndexKline>>(GetUrl(indexPriceKlineEndpoint, api, publicVersion), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
    }
    #endregion

    #region Mark Price Kline/Candlestick Data
    public async Task<RestCallResult<IEnumerable<BinanceFuturesMarkIndexKline>>> GetMarkPriceKlinesAsync(string symbol, KlineInterval interval, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1500);

        var parameters = new Dictionary<string, object> {
                { "symbol", symbol },
                { "interval", JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false)) }
            };

        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());

        var requestWeight = limit == null ? 5 : limit <= 100 ? 1 : limit <= 500 ? 2 : limit <= 1000 ? 5 : 10;
        return await SendRequestInternal<IEnumerable<BinanceFuturesMarkIndexKline>>(GetUrl(markPriceKlinesEndpoint, api, publicVersion), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: requestWeight).ConfigureAwait(false);
    }
    #endregion

    #region 24hr Ticker Price Change Statistics
    public async Task<RestCallResult<IEnumerable<IBinance24HPrice>>> GetTickersAsync(string symbol = null, string pair = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("pair", pair);

        var result = await SendRequestInternal<IEnumerable<BinanceFuturesCoin24HPrice>>(GetUrl(price24HEndpoint, api, publicVersion),
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

        return await SendRequestInternal<IEnumerable<BinanceFuturesCoinPrice>>(GetUrl(allPricesEndpoint, api, publicVersion), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: symbol == null ? 2 : 1).ConfigureAwait(false);
    }
    #endregion

    #region Symbol Order Book Ticker
    public async Task<RestCallResult<IEnumerable<BinanceFuturesBookPrice>>> GetBookPricesAsync(string symbol = null, string pair = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("pair", pair);

        return await SendRequestInternal<IEnumerable<BinanceFuturesBookPrice>>(
                GetUrl(bookPricesEndpoint, api, publicVersion), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: symbol == null ? 2 : 1)
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

        return await SendRequestInternal<BinanceFuturesCoinOpenInterest>(GetUrl(openInterestEndpoint, api, publicVersion), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
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

        return await SendRequestInternal<IEnumerable<BinanceFuturesCoinOpenInterestHistory>>(GetUrl(openInterestHistoryEndpoint, tradingDataapi), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Top Trader Long/Short Ratio (Accounts)
    public async Task<RestCallResult<IEnumerable<BinanceFuturesLongShortRatio>>> GetTopLongShortAccountRatioAsync(string symbolPair, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var url = GetUrl(topLongShortAccountRatioEndpoint, tradingDataapi);
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

        var url = GetUrl(topLongShortPositionRatioEndpoint, tradingDataapi);
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

    #region Long/Short Ratio
    public async Task<RestCallResult<IEnumerable<BinanceFuturesLongShortRatio>>> GetGlobalLongShortAccountRatioAsync(string symbolPair, PeriodInterval period, int? limit = null, DateTime? startTime = null, DateTime? endTime = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var url = GetUrl(globalLongShortAccountRatioEndpoint, tradingDataapi);
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

        return await SendRequestInternal<IEnumerable<BinanceFuturesCoinBuySellVolumeRatio>>(GetUrl(takerBuySellVolumeRatioEndpoint, tradingDataapi), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
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

        return await SendRequestInternal<IEnumerable<BinanceFuturesBasis>>(GetUrl(basisEndpoint, tradingDataapi), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

}