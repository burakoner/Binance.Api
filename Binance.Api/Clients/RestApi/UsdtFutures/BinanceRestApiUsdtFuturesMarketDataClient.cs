using Binance.Api.Models.RestApi.Futures;

namespace Binance.Api.Clients.RestApi.UsdtFutures;

public class BinanceRestApiUsdtFuturesMarketDataClient
{
    // Api
    private const string api = "fapi";
    private const string publicVersion = "1";
    private const string tradingDataapi = "futures/data";

    // Order Book
    private const string orderBookEndpoint = "depth";

    // Trades
    private const string recentTradesEndpoint = "trades";
    private const string historicalTradesEndpoint = "historicalTrades";
    private const string aggregatedTradesEndpoint = "aggTrades";

    // Klines
    private const string klinesEndpoint = "klines";
    private const string continuousContractKlineEndpoint = "continuousKlines";
    private const string indexPriceKlinesKlineEndpoint = "indexPriceKlines";
    private const string markPriceKlinesEndpoint = "markPriceKlines";

    // Index & Rates
    private const string markPriceEndpoint = "premiumIndex";
    private const string fundingRateHistoryEndpoint = "fundingRate";

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
    private const string takerBuySellVolumeRatioEndpoint = "takerlongshortRatio";
    // TODO: Historical BLVT NAV Kline/Candlestick
    private const string compositeIndexapi = "indexInfo";
    private const string assetIndexEndpoint = "assetIndex";

    // Internal References
    internal BinanceRestApiClient RootClient { get; }
    internal BinanceRestApiUsdtFuturesClient MainClient { get; }
    internal BinanceRestApiClientOptions Options { get => MainClient.RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiUsdtFuturesMarketDataClient(BinanceRestApiUsdtFuturesClient main)
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
        var result = await SendRequestInternal<BinanceFuturesOrderBook>(GetUrl(orderBookEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: requestWeight).ConfigureAwait(false);
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
        var result = await SendRequestInternal<IEnumerable<BinanceRecentTradeQuote>>(GetUrl(recentTradesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: 5).ConfigureAwait(false);
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

        var result = await SendRequestInternal<IEnumerable<BinanceRecentTradeQuote>>(GetUrl(historicalTradesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: 20).ConfigureAwait(false);
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

        return await SendRequestInternal<IEnumerable<BinanceAggregatedTrade>>(GetUrl(aggregatedTradesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: 20).ConfigureAwait(false);
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
        var result = await SendRequestInternal<IEnumerable<BinanceFuturesUsdtKline>>(GetUrl(klinesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: requestWeight).ConfigureAwait(false);
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
        var result = await SendRequestInternal<IEnumerable<BinanceFuturesUsdtKline>>(GetUrl(continuousContractKlineEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: requestWeight).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceKline>>(result.Data);
    }
    #endregion

    #region Index Price Kline/Candlestick Data
    public async Task<RestCallResult<IEnumerable<IBinanceKline>>> GetIndexPriceKlinesAsync(string pair, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
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
        var result = await SendRequestInternal<IEnumerable<BinanceFuturesUsdtKline>>(GetUrl(indexPriceKlinesKlineEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: requestWeight).ConfigureAwait(false);
        return result.As<IEnumerable<IBinanceKline>>(result.Data);
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
        return await SendRequestInternal<IEnumerable<BinanceFuturesMarkIndexKline>>(GetUrl(markPriceKlinesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: requestWeight).ConfigureAwait(false);
    }
    #endregion

    #region Mark Price
    public async Task<RestCallResult<BinanceFuturesMarkPrice>> GetMarkPriceAsync(string symbol,
        CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);

        return await SendRequestInternal<BinanceFuturesMarkPrice>(GetUrl(markPriceEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesMarkPrice>>> GetMarkPricesAsync(CancellationToken ct = default)
    {
        return await SendRequestInternal<IEnumerable<BinanceFuturesMarkPrice>>(GetUrl(markPriceEndpoint, api, publicVersion), HttpMethod.Get, ct).ConfigureAwait(false);
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

        return await SendRequestInternal<IEnumerable<BinanceFuturesFundingRateHistory>>(GetUrl(fundingRateHistoryEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
    }
    #endregion

    #region 24hr Ticker Price Change Statistics
    public async Task<RestCallResult<IBinance24HPrice>> GetTickerAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);

        var result = await SendRequestInternal<Binance24HPrice>(GetUrl(price24HEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        return result.As<IBinance24HPrice>(result.Data);
    }

    public async Task<RestCallResult<IEnumerable<IBinance24HPrice>>> GetTickersAsync(CancellationToken ct = default)
    {
        var result = await SendRequestInternal<IEnumerable<Binance24HPrice>>(GetUrl(price24HEndpoint, api, publicVersion), HttpMethod.Get, ct, weight: 40).ConfigureAwait(false);
        return result.As<IEnumerable<IBinance24HPrice>>(result.Data);
    }
    #endregion

    #region Symbol Price Ticker
    public async Task<RestCallResult<BinancePrice>> GetPriceAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "symbol", symbol }
        };

        return await SendRequestInternal<BinancePrice>(GetUrl(allPricesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinancePrice>>> GetPricesAsync(CancellationToken ct = default)
    {
        return await SendRequestInternal<IEnumerable<BinancePrice>>(GetUrl(allPricesEndpoint, api, publicVersion), HttpMethod.Get, ct, weight: 2).ConfigureAwait(false);
    }
    #endregion

    #region Symbol Order Book Ticker
    public async Task<RestCallResult<BinanceBookPrice>> GetBookPriceAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);

        return await SendRequestInternal<BinanceBookPrice>(GetUrl(bookPricesEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
    }

    public async Task<RestCallResult<IEnumerable<BinanceBookPrice>>> GetBookPricesAsync(CancellationToken ct = default)
    {
        return await SendRequestInternal<IEnumerable<BinanceBookPrice>>(GetUrl(bookPricesEndpoint, api, publicVersion), HttpMethod.Get, ct, weight: 2).ConfigureAwait(false);
    }
    #endregion

    #region Open Interest
    public async Task<RestCallResult<BinanceFuturesOpenInterest>> GetOpenInterestAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
        {
            { "symbol", symbol }
        };

        return await SendRequestInternal<BinanceFuturesOpenInterest>(GetUrl(openInterestEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
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

        return await SendRequestInternal<IEnumerable<BinanceFuturesOpenInterestHistory>>(GetUrl(openInterestHistoryEndpoint, tradingDataapi), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
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

        return await SendRequestInternal<IEnumerable<BinanceFuturesLongShortRatio>>(url, HttpMethod.Get, ct, parameters).ConfigureAwait(false);
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

        return await SendRequestInternal<IEnumerable<BinanceFuturesLongShortRatio>>(url, HttpMethod.Get, ct, parameters).ConfigureAwait(false);
    }
    #endregion

    #region Long/Short Ratio (Accounts)
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

        return await SendRequestInternal<IEnumerable<BinanceFuturesLongShortRatio>>(url, HttpMethod.Get, ct, parameters).ConfigureAwait(false);
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

        return await SendRequestInternal<IEnumerable<BinanceFuturesBuySellVolumeRatio>>(GetUrl(takerBuySellVolumeRatioEndpoint, tradingDataapi), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
    }
    #endregion

    #region Composite Index Symbol Information
    public async Task<RestCallResult<IEnumerable<BinanceFuturesCompositeIndexInfo>>> GetCompositeIndexInfoAsync(string symbol = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        return await SendRequestInternal<IEnumerable<BinanceFuturesCompositeIndexInfo>>(GetUrl(compositeIndexapi, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
    }
    #endregion

    #region Multi-Assets Mode Asset Index
    public async Task<RestCallResult<IEnumerable<BinanceFuturesAssetIndex>>> GetAssetIndexesAsync(CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        return await SendRequestInternal<IEnumerable<BinanceFuturesAssetIndex>>(GetUrl(assetIndexEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters, weight: 10).ConfigureAwait(false);
    }

    public async Task<RestCallResult<BinanceFuturesAssetIndex>> GetAssetIndexAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
            {
                { "symbol", symbol }
            };
        return await SendRequestInternal<BinanceFuturesAssetIndex>(GetUrl(assetIndexEndpoint, api, publicVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
    }
    #endregion

}