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

    public Task<CallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(BinanceSpotSymbolStatusFilter status, CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [], status: status, ct: ct);

    public Task<CallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(BinancePermissionType permission, CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [], status: null, permissions: [permission], ct: ct);

    public async Task<CallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(
        IEnumerable<string> symbols,
        BinanceSpotSymbolStatusFilter? status = null,
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

        var result = await RequestAsync<BinanceSpotExchangeInfo>("ws-api/v3", $"exchangeInfo", parameters, weight: 20, ct: ct).ConfigureAwait(false);
        if (!result) return result;

        ExchangeInfo = result.Data;
        LastExchangeInfoUpdate = DateTime.UtcNow;
        _logger.Log(LogLevel.Information, "Trade rules updated");
        return result;
    }

    public Task<CallResult<BinanceSpotExecutionRules>> GetExecutionRulesAsync(CancellationToken ct = default)
        => GetExecutionRulesAsync([], null, ct);

    public Task<CallResult<BinanceSpotExecutionRules>> GetExecutionRulesAsync(string symbol, CancellationToken ct = default)
        => GetExecutionRulesAsync([symbol], null, ct);

    public Task<CallResult<BinanceSpotExecutionRules>> GetExecutionRulesAsync(IEnumerable<string> symbols, CancellationToken ct = default)
        => GetExecutionRulesAsync(symbols, null, ct);

    public Task<CallResult<BinanceSpotExecutionRules>> GetExecutionRulesAsync(BinanceSpotSymbolStatusFilter status, CancellationToken ct = default)
        => GetExecutionRulesAsync([], status, ct);

    private Task<CallResult<BinanceSpotExecutionRules>> GetExecutionRulesAsync(
        IEnumerable<string> symbols,
        BinanceSpotSymbolStatusFilter? status,
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
        return RequestAsync<BinanceSpotExecutionRules>("ws-api/v3", "executionRules", parameters, weight: weight, ct: ct);
    }
}
