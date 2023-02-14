using Binance.Api.Models.RestApi.Futures;
using Binance.Api.Models.RestApi.Server;

namespace Binance.Api.Clients.RestApi.UsdtFutures;

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
    internal BinanceRestApiUsdtFuturesClient MainClient { get; }
    internal BinanceRestApiClientOptions Options { get => MainClient.RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal BinanceRestApiUsdtFuturesServerClient(BinanceRestApiUsdtFuturesClient main)
    {
        MainClient = main;
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
        var result = await SendRequestInternal<BinanceServerTime>(url, HttpMethod.Get, ct, ignoreRatelimit: true).ConfigureAwait(false);
        return result.As(result.Data?.ServerTime ?? default);
    }
    #endregion

    #region Exchange Information
    public async Task<RestCallResult<BinanceFuturesUsdtExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
    {
        var exchangeInfoResult = await SendRequestInternal<BinanceFuturesUsdtExchangeInfo>(GetUrl(exchangeInfoEndpoint, api, "1"), HttpMethod.Get, ct).ConfigureAwait(false);
        if (!exchangeInfoResult)
            return exchangeInfoResult;

        MainClient.ExchangeInfo = exchangeInfoResult.Data;
        MainClient.LastExchangeInfoUpdate = DateTime.UtcNow;
        MainClient.Log.Write(LogLevel.Information, "Trade rules updated");
        return exchangeInfoResult;
    }
    #endregion
}