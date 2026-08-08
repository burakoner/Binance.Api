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
        var result = await RequestAsync<BinanceServerTime>(GetUrl(api, v3, "time"), HttpMethod.Get, ct, requestWeight: 1).ConfigureAwait(false);

        return result.Success
            ? result.As(result.Data?.ServerTime ?? default)
            : result.AsError<DateTime>(result.Error!);
    }

    public Task<RestCallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [], ct: ct);

    public Task<RestCallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(string symbol, CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [symbol], ct: ct);

    public Task<RestCallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(BinanceSpotSymbolStatus status, CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [], status: status, ct: ct);

    public Task<RestCallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(BinancePermissionType permission, CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [], status: null, permissions: [permission], ct: ct);

    public async Task<RestCallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(
        IEnumerable<string> symbols,
        BinanceSpotSymbolStatus? status = null,
        IEnumerable<BinancePermissionType>? permissions = null,
        bool? showPermissionSets = null,
        CancellationToken ct = default)
    {
        if (symbols == null)
            throw new ArgumentNullException(nameof(symbols));
        var symbolList = symbols.ToArray();
        foreach (var symbol in symbolList)
            symbol.ValidateBinanceSymbol();
        var permissionList = permissions?.ToArray();
        if (symbolList.Length > 0 && status.HasValue)
            throw new ArgumentException("symbolStatus cannot be combined with symbol or symbols.", nameof(status));
        if (symbolList.Length > 0 && permissionList?.Length > 0)
            throw new ArgumentException("permissions cannot be combined with symbol or symbols.", nameof(permissions));

        var parameters = new ParameterCollection();

        // Symbol(s)
        if (symbolList.Length > 1)
        {
            parameters.Add("symbols", JsonConvert.SerializeObject(symbolList));
        }
        else if (symbolList.Length == 1)
        {
            parameters.Add("symbol", symbolList[0]);
        }

        // Permissions
        if (permissionList?.Length > 1)
        {
            parameters.Add("permissions", JsonConvert.SerializeObject(permissionList.Select(MapConverter.GetString)));
        }
        else if (permissionList?.Length == 1)
        {
            parameters.Add("permissions", MapConverter.GetString(permissionList[0])!);
        }

        // Permission Sets
        parameters.AddOptional("showPermissionSets", showPermissionSets?.ToString().ToLowerInvariant());
        parameters.AddOptionalEnum("symbolStatus", status);

        var result = await RequestAsync<BinanceSpotExchangeInfo>(GetUrl(api, v3, "exchangeInfo"), HttpMethod.Get, ct, queryParameters: parameters, serialization: ArraySerialization.Array, requestWeight: 20).ConfigureAwait(false);
        if (!result)
            return result;

        ExchangeInfo = result.Data;
        LastExchangeInfoUpdate = DateTime.UtcNow;
        Logger.Log(LogLevel.Information, "Trade rules updated");
        return result;
    }

    public Task<RestCallResult<BinanceSpotExecutionRules>> GetExecutionRulesAsync(CancellationToken ct = default)
        => GetExecutionRulesAsync([], null, ct);

    public Task<RestCallResult<BinanceSpotExecutionRules>> GetExecutionRulesAsync(string symbol, CancellationToken ct = default)
        => GetExecutionRulesAsync([symbol], null, ct);

    public Task<RestCallResult<BinanceSpotExecutionRules>> GetExecutionRulesAsync(IEnumerable<string> symbols, CancellationToken ct = default)
        => GetExecutionRulesAsync(symbols, null, ct);

    public Task<RestCallResult<BinanceSpotExecutionRules>> GetExecutionRulesAsync(BinanceSpotSymbolStatus status, CancellationToken ct = default)
        => GetExecutionRulesAsync([], status, ct);

    private Task<RestCallResult<BinanceSpotExecutionRules>> GetExecutionRulesAsync(
        IEnumerable<string> symbols,
        BinanceSpotSymbolStatus? status,
        CancellationToken ct)
    {
        if (symbols == null)
            throw new ArgumentNullException(nameof(symbols));
        var symbolList = symbols.ToArray();
        foreach (var symbol in symbolList)
            symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection();
        if (symbolList.Length > 1)
            parameters.Add("symbols", JsonConvert.SerializeObject(symbolList));
        else if (symbolList.Length == 1)
            parameters.Add("symbol", symbolList[0]);
        parameters.AddOptionalEnum("symbolStatus", status);

        var weight = symbolList.Length == 0 ? 40 : Math.Min(symbolList.Length * 2, 40);
        return RequestAsync<BinanceSpotExecutionRules>(
            GetUrl(api, v3, "executionRules"),
            HttpMethod.Get,
            ct,
            queryParameters: parameters,
            requestWeight: weight);
    }
}
