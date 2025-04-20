namespace Binance.Api.Spot;

public class BinanceSpotRestApiGeneralClient(BinanceSpotRestApiClient parent)
{
    // Api
    private const string api = "api";
    private const string v1 = "1";
    private const string v3 = "3";

    // Parent Clients
    internal BinanceRestApiClient _ { get; } = parent._;
    internal BinanceSpotRestApiClient __ { get; } = parent;
    internal ILogger? Logger { get; } = parent.Logger;
    internal BinanceRestApiClientOptions ClientOptions { get; } = parent.ClientOptions;

    // Internal
    internal BinanceExchangeInfo? ExchangeInfo { get; private set; }
    internal DateTime? LastExchangeInfoUpdate { get; private set; }

    public async Task<RestCallResult<TimeSpan>> PingAsync(CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        var result = await __.SendRequestInternal<object>(__.GetUrl(api, v3, "ping"), HttpMethod.Get, ct).ConfigureAwait(false);
        sw.Stop();

        return result.Success
            ? result.As(sw.Elapsed)
            : result.AsError<TimeSpan>(result.Error!);
    }

    public async Task<RestCallResult<DateTime>> GetTimeAsync(CancellationToken ct = default)
    {
        var result = await __.SendRequestInternal<BinanceServerTime>(__.GetUrl(api, v3, "time"), HttpMethod.Get, ct, ignoreRatelimit: true).ConfigureAwait(false);

        return result.Success
            ? result.As(result.Data?.ServerTime ?? default)
            : result.AsError<DateTime>(result.Error!);
    }

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

        var exchangeInfoResult = await __.SendRequestInternal<BinanceExchangeInfo>(__.GetUrl(api, v3, "exchangeInfo"), HttpMethod.Get, ct, queryParameters: parameters, serialization: ArraySerialization.Array, requestWeight: 10).ConfigureAwait(false);
        if (!exchangeInfoResult)
            return exchangeInfoResult;

        ExchangeInfo = exchangeInfoResult.Data;
        LastExchangeInfoUpdate = DateTime.UtcNow;
        Logger?.Log(LogLevel.Information, "Trade rules updated");
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

        var exchangeInfoResult = await __.SendRequestInternal<BinanceExchangeInfo>(__.GetUrl(api, v3, "exchangeInfo"), HttpMethod.Get, ct, queryParameters: parameters, serialization: ArraySerialization.Array, requestWeight: 10).ConfigureAwait(false);
        if (!exchangeInfoResult)
            return exchangeInfoResult;

        ExchangeInfo = exchangeInfoResult.Data;
        LastExchangeInfoUpdate = DateTime.UtcNow;
        Logger?.Log(LogLevel.Information, "Trade rules updated");
        return exchangeInfoResult;
    }
}