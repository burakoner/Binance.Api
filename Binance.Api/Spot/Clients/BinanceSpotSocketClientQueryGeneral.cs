namespace Binance.Api.Spot;

internal partial class BinanceSpotSocketClient
{
    public async Task<CallResult<TimeSpan>> PingAsync(CancellationToken ct = default)
    {

        var sw = Stopwatch.StartNew();
        var result = await RequestAsync<object>("ws-api/v3", $"ping", [], ct: ct).ConfigureAwait(false);
        sw.Stop();

        return result.Success
            ? result.As(sw.Elapsed)
            : result.AsError<TimeSpan>(result.Error!);
    }

    public async Task<CallResult<DateTime>> GetTimeAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceServerTime>("ws-api/v3", $"time", [], false, ct: ct).ConfigureAwait(false);
        if (!result) return result.AsError<DateTime>(result.Error!);

        return result.As(result.Data.ServerTime);
    }

    public Task<CallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [], ct: ct);

    public Task<CallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(string symbol, CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [symbol], ct: ct);

    public Task<CallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(BinanceSymbolStatus status, CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [], status: status, ct: ct);

    public Task<CallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(BinancePermissionType permission, CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [], status: null, permissions: [permission], ct: ct);

    public async Task<CallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(
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

        var result = await RequestAsync<BinanceSpotExchangeInfo>("ws-api/v3", $"exchangeInfo", parameters, weight: 20, ct: ct).ConfigureAwait(false);
        if (!result) return result;

        ExchangeInfo = result.Data;
        LastExchangeInfoUpdate = DateTime.UtcNow;
        _logger.Log(LogLevel.Information, "Trade rules updated");
        return result;
    }
}