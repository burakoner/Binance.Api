namespace Binance.Api.Spot;

/// <summary>
/// Binance Spot Rest API General Client
/// </summary>
/// <param name="parent">Parent Client</param>
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

    // Internal
    internal BinanceExchangeInfo? ExchangeInfo { get; private set; }
    internal DateTime? LastExchangeInfoUpdate { get; private set; }

    /// <summary>
    /// Pings the Binance API
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#test-connectivity" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if successful ping, false if no response</returns>
    public async Task<RestCallResult<TimeSpan>> PingAsync(CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        var result = await __.SendRequestInternal<object>(__.GetUrl(api, v3, "ping"), HttpMethod.Get, ct).ConfigureAwait(false);
        sw.Stop();

        return result.Success
            ? result.As(sw.Elapsed)
            : result.AsError<TimeSpan>(result.Error!);
    }

    /// <summary>
    /// Requests the server for the local time
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#check-server-time" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Server time</returns>
    public async Task<RestCallResult<DateTime>> GetTimeAsync(CancellationToken ct = default)
    {
        var result = await __.SendRequestInternal<BinanceServerTime>(__.GetUrl(api, v3, "time"), HttpMethod.Get, ct, ignoreRatelimit: true).ConfigureAwait(false);

        return result.Success
            ? result.As(result.Data?.ServerTime ?? default)
            : result.AsError<DateTime>(result.Error!);
    }

    /// <summary>
    /// Gets information about the exchange including rate limits and symbol list
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#exchange-information" /></para>
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Exchange Info</returns>
    public Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [], ct: ct);

    /// <summary>
    /// Gets information about the exchange including rate limits and symbol list
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#exchange-information" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to get data for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Exchange Info</returns>
    public Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(string symbol, CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [symbol], ct: ct);

    /// <summary>
    /// Gets information about the exchange including rate limits and symbol list
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#exchange-information" /></para>
    /// </summary>
    /// <param name="status">Filter by symbol status, Trading, Halt or Break</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Exchange Info</returns>
    public Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(BinanceSymbolStatus status, CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [], status: status, ct: ct);

    /// <summary>
    /// Gets information about the exchange including rate limits and symbol list
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#exchange-information" /></para>
    /// </summary>
    /// <param name="permission">Permission Type</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Exchange Info</returns>
    public Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(BinancePermissionType permission, CancellationToken ct = default)
         => GetExchangeInfoAsync(symbols: [], status: null, permissions: [permission], ct: ct);

    /// <summary>
    /// Gets information about the exchange including rate limits and symbol list
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#exchange-information" /></para>
    /// </summary>
    /// <param name="symbols">Symbols to get data for, for example `ETHUSDT`</param>
    /// <param name="status">Filter by symbol status, Trading, Halt or Break</param>
    /// <param name="permissions">Permission Types</param>
    /// <param name="showPermissionSets">Whether or not permission sets should be returned</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Exchange Info</returns>
    public async Task<RestCallResult<BinanceExchangeInfo>> GetExchangeInfoAsync(
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

        var exchangeInfoResult = await __.SendRequestInternal<BinanceExchangeInfo>(__.GetUrl(api, v3, "exchangeInfo"), HttpMethod.Get, ct, queryParameters: parameters, serialization: ArraySerialization.Array, requestWeight: 20).ConfigureAwait(false);
        if (!exchangeInfoResult)
            return exchangeInfoResult;

        ExchangeInfo = exchangeInfoResult.Data;
        LastExchangeInfoUpdate = DateTime.UtcNow;
        Logger?.Log(LogLevel.Information, "Trade rules updated");
        return exchangeInfoResult;
    }
}