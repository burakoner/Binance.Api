namespace Binance.Api.Futures;

internal partial class BinanceFuturesRestClientCoin(BinanceFuturesRestClient parent) : IBinanceFuturesRestClientCoin
{
    // Api
    private const string v1 = "1";
    private const string v2 = "2";
    private const string v3 = "3";
    private const string dapi = "dapi";

    // Parent
    private BinanceRestApiClient __ { get; } = parent._;
    private BinanceFuturesRestClient _ { get; } = parent;

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceRestApiClientOptions RestOptions => _.RestOptions;
    internal DateTime? LastExchangeInfoUpdate { get; private set; }
    internal BinanceFuturesCoinExchangeInfo? ExchangeInfo { get; private set; }

    // Request
    private Task<RestCallResult<T>> RequestAsync<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object>? queryParameters = null,
        Dictionary<string, object>? bodyParameters = null,
        Dictionary<string, string>? headerParameters = null,
        ArraySerialization? serialization = null,
        JsonSerializer? deserializer = null,
        bool ignoreRatelimit = false,
        int requestWeight = 1) where T : class
        => _._.RequestAsync<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    // GetUrl
    private Uri GetUrl(string api, string version, string endpoint)
    {
        var url = BinanceAddress.Default.CoinFuturesRestApiAddress.AppendPath(api);
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }

    internal async Task<BinanceTradeRuleResult> CheckTradingRulesAsync(string symbol, BinanceFuturesOrderType type, decimal? quantity, decimal? quoteQuantity, decimal? price, decimal? stopPrice, CancellationToken ct)
    {
        if (RestOptions.CoinFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.None)
            return BinanceTradeRuleResult.CreatePassed(quantity, quoteQuantity, price, stopPrice);

        if (ExchangeInfo == null || LastExchangeInfoUpdate == null || (DateTime.UtcNow - LastExchangeInfoUpdate.Value).TotalMinutes > RestOptions.CoinFuturesOptions.TradeRulesUpdateInterval.TotalMinutes)
            await GetExchangeInfoAsync(ct).ConfigureAwait(false);

        if (ExchangeInfo == null)
            return BinanceTradeRuleResult.CreateFailed("Unable to retrieve trading rules, validation failed");

        var symbolInfo = ExchangeInfo?.Symbols.SingleOrDefault(s => string.Equals(s.Symbol, symbol, StringComparison.CurrentCultureIgnoreCase));
        if (symbolInfo == null) return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Symbol {symbol} not found");

        if (!symbolInfo.OrderTypes.Contains(type))
            return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: {type} order type not allowed for {symbol}");

        return BinanceHelpers.ValidateFuturesTradingRules(Logger, RestOptions.SpotOptions.TradeRulesBehavior, symbolInfo, type, quantity, quoteQuantity, price, stopPrice);
    }

}