namespace Binance.Api.Spot;

internal partial class BinanceSpotRestClient
{
    public async Task<RestCallResult<TimeSpan>> PingAsync(CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        var result = await RequestAsync<object>(GetUrl(api, v3, "ping"), HttpMethod.Get, ct).ConfigureAwait(false);
        sw.Stop();

        return result.Success
            ? result.As(sw.Elapsed)
            : result.AsError<TimeSpan>(result.Error!);
    }

    public async Task<RestCallResult<DateTime>> GetTimeAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceServerTime>(GetUrl(api, v3, "time"), HttpMethod.Get, ct, ignoreRatelimit: true).ConfigureAwait(false);

        return result.Success
            ? result.As(result.Data?.ServerTime ?? default)
            : result.AsError<DateTime>(result.Error!);
    }

    public Task<RestCallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [], ct: ct);

    public Task<RestCallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(string symbol, CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [symbol], ct: ct);

    public Task<RestCallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(BinanceSymbolStatus status, CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [], status: status, ct: ct);

    public Task<RestCallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(BinancePermissionType permission, CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [], status: null, permissions: [permission], ct: ct);

    public async Task<RestCallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(
        IEnumerable<string> symbols,
        BinanceSymbolStatus? status = null,
        IEnumerable<BinancePermissionType>? permissions = null,
        bool? showPermissionSets = null,
        CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();

        // Symbol(s)
        if (symbols.Count() > 1)
        {
            parameters.Add("symbols", JsonConvert.SerializeObject(symbols));
        }
        else if (symbols.Any())
        {
            parameters.Add("symbol", symbols.First());
        }

        // Permissions
        if (permissions != null && permissions?.Count() > 1)
        {
            var list = new List<string>();
            foreach (var permission in permissions)
            {
                list.Add(permission.ToString().ToUpper());
            }

            parameters.Add("permissions", JsonConvert.SerializeObject(list));
        }
        else if (permissions != null && permissions.Any())
        {
            parameters.Add("permissions", permissions.First().ToString().ToUpper());
        }

        // Permission Sets
        parameters.AddOptional("showPermissionSets", showPermissionSets?.ToString().ToLowerInvariant());
        parameters.AddOptionalEnum("symbolStatus", status);

        var exchangeInfoResult = await RequestAsync<BinanceSpotExchangeInfo>(GetUrl(api, v3, "exchangeInfo"), HttpMethod.Get, ct, queryParameters: parameters, serialization: ArraySerialization.Array, requestWeight: 20).ConfigureAwait(false);
        if (!exchangeInfoResult)
            return exchangeInfoResult;

        ExchangeInfo = exchangeInfoResult.Data;
        LastExchangeInfoUpdate = DateTime.UtcNow;
        Logger.Log(LogLevel.Information, "Trade rules updated");
        return exchangeInfoResult;
    }
}