using Binance.ApiClient.Models.RestApi.Futures;
using Binance.ApiClient.Models.RestApi.Server;

namespace Binance.ApiClient.Clients.RestApi.UsdtFutures;

public class BinanceRestApiUsdtFuturesServerClient
{
    // Api
    private const string api = "fapi";
    private const string publicVersion = "1";
    private const string tradingDataApi = "futures/data";

    // Server
    private const string pingEndpoint = "ping";
    private const string checkTimeEndpoint = "time";
    private const string exchangeInfoEndpoint = "exchangeInfo";

    // Internal References
    internal BinanceRestApiClient RootClient { get; }
    internal BinanceRestApiUsdtFuturesClient UsdtFuturesClient { get; }
    internal BinanceRestApiClientOptions Options { get => RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => UsdtFuturesClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await UsdtFuturesClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiUsdtFuturesServerClient(BinanceRestApiClient root, BinanceRestApiUsdtFuturesClient usdt)
    {
        RootClient = root;
        UsdtFuturesClient = usdt;
    }

    #region Test Connectivity
    public async Task<RestCallResult<long>> PingAsync(CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        var result = await SendRequestInternal<object>(GetUrl(pingEndpoint, api, "1"), HttpMethod.Get, ct).ConfigureAwait(false);
        sw.Stop();
        return result ? result.As(sw.ElapsedMilliseconds) : result.As<long>(default!);
    }
    #endregion

    #region Check Server Time
    public async Task<RestCallResult<DateTime>> GetServerTimeAsync(bool resetAutoTimestamp = false, CancellationToken ct = default)
    {
        var url = GetUrl(checkTimeEndpoint, api, "1");
        var result = await SendRequestInternal<BinanceServerTime>(url, HttpMethod.Get, ct, ignoreRateLimit: true).ConfigureAwait(false);
        return result.As(result.Data?.ServerTime ?? default);
    }
    #endregion

    #region Exchange Information
    public async Task<RestCallResult<BinanceFuturesUsdtExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
    {
        var exchangeInfoResult = await SendRequestInternal<BinanceFuturesUsdtExchangeInfo>(GetUrl(exchangeInfoEndpoint, api, "1"), HttpMethod.Get, ct).ConfigureAwait(false);
        if (!exchangeInfoResult)
            return exchangeInfoResult;

        UsdtFuturesClient.ExchangeInfo = exchangeInfoResult.Data;
        UsdtFuturesClient.LastExchangeInfoUpdate = DateTime.UtcNow;
        UsdtFuturesClient.Log.Write(LogLevel.Information, "Trade rules updated");
        return exchangeInfoResult;
    }
    #endregion
}