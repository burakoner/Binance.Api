using Binance.Api.Models.RestApi.Server;

namespace Binance.Api.Clients.RestApi.Spot;

public class BinanceRestApiSpotServerClient
{
    // Api
    protected const string api = "api";
    protected const string publicVersion = "3";

    // Server
    private const string pingEndpoint = "ping";
    private const string checkTimeEndpoint = "time";
    private const string systemStatusEndpoint = "system/status";
    private const string exchangeInfoEndpoint = "exchangeInfo";

    // Internal References
    internal BinanceRestApiSpotClient MainClient { get; }
    internal BinanceRestApiClientOptions ClientOptions { get => MainClient.RootClient.ClientOptions; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal BinanceRestApiSpotServerClient(BinanceRestApiSpotClient main)
    {
        MainClient = main;
    }

    #region Ping
    public async Task<RestCallResult<long>> PingAsync(CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        var result = await SendRequestInternal<object>(GetUrl(pingEndpoint, api, publicVersion), HttpMethod.Get, ct).ConfigureAwait(false);
        sw.Stop();
        return result ? result.As(sw.ElapsedMilliseconds) : result.As<long>(default!);
    }
    #endregion

    #region Server Time
    public async Task<RestCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
    {
        var result = await SendRequestInternal<BinanceServerTime>(GetUrl(checkTimeEndpoint, api, publicVersion), HttpMethod.Get, ct, ignoreRatelimit: true).ConfigureAwait(false);
        return result.As(result.Data?.ServerTime ?? default);
    }
    #endregion

    #region System Status
    public async Task<RestCallResult<BinanceSystemStatus>> GetSystemStatusAsync(CancellationToken ct = default)
    {
        return await SendRequestInternal<BinanceSystemStatus>(GetUrl(systemStatusEndpoint, "sapi", "1"), HttpMethod.Get, ct).ConfigureAwait(false);
    }
    #endregion

    #region Exchange Information
    public Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
         => GetExchangeInfoAsync(Array.Empty<string>(), ct);

    public Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(string symbol, CancellationToken ct = default)
         => GetExchangeInfoAsync(new string[] { symbol }, ct);

    public Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(AccountType permission, CancellationToken ct = default)
         => GetExchangeInfoAsync(new AccountType[] { permission }, ct);

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

        var exchangeInfoResult = await SendRequestInternal<BinanceExchangeInfo>(GetUrl(exchangeInfoEndpoint, api, publicVersion), HttpMethod.Get, ct, queryParameters: parameters, serialization: ArraySerialization.Array, requestWeight: 10).ConfigureAwait(false);
        if (!exchangeInfoResult)
            return exchangeInfoResult;

        MainClient.ExchangeInfo = exchangeInfoResult.Data;
        MainClient.LastExchangeInfoUpdate = DateTime.UtcNow;
        MainClient.Log.Write(LogLevel.Information, "Trade rules updated");
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

        var exchangeInfoResult = await SendRequestInternal<BinanceExchangeInfo>(GetUrl(exchangeInfoEndpoint, api, publicVersion), HttpMethod.Get, ct, queryParameters: parameters, serialization: ArraySerialization.Array, requestWeight: 10).ConfigureAwait(false);
        if (!exchangeInfoResult)
            return exchangeInfoResult;

        MainClient.ExchangeInfo = exchangeInfoResult.Data;
        MainClient.LastExchangeInfoUpdate = DateTime.UtcNow;
        MainClient.Log.Write(LogLevel.Information, "Trade rules updated");
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

}