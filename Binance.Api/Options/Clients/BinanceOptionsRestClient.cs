namespace Binance.Api.Options;

internal partial class BinanceOptionsRestClient : IBinanceOptionsRestClient
{
    // Api
    private const string v1 = "1";
    private const string eapi = "eapi";

    // Parent
    internal BinanceRestApiClient _ { get; }

    // Internal
    private ILogger Logger => _.Logger;
    private BinanceRestApiClientOptions RestOptions => _.ApiOptions;
    private DateTime? LastExchangeInfoUpdate { get;  set; }
    private BinanceOptionsExchangeInfo? ExchangeInfo { get;  set; }

    // Interface Properties
    public IBinanceOptionsRestClientMarketMaker MarketMaker { get; }

    public BinanceOptionsRestClient(BinanceRestApiClient root)
    {
        _ = root;
        MarketMaker = new BinanceOptionsRestClientMarketMaker(this);
    }

    private Task<RestCallResult<T>> RequestAsync<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object>? queryParameters = null,
        Dictionary<string, object>? bodyParameters = null,
        Dictionary<string, string>? headerParameters = null,
        ArraySerialization? serialization = null,
        JsonSerializer? deserializer = null,
        bool ignoreRatelimit = false,
        int requestWeight = 1) where T : class
        => _.RequestAsync<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    private Uri GetUrl(string api, string version, string endpoint)
    {
        var url = BinanceAddress.Default.OptionsRestApiAddress;
        if (!string.IsNullOrEmpty(api)) url = url.AppendPath($"{api}");
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }

    internal async Task<BinanceTradeRuleResult> CheckTradingRulesAsync(string symbol, BinanceOptionsOrderType? type, decimal? quantity, decimal? quoteQuantity, decimal? price, decimal? stopPrice, CancellationToken ct)
    {
        if (RestOptions.SpotOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.None)
            return BinanceTradeRuleResult.CreatePassed(quantity, quoteQuantity, price, stopPrice);

        if (ExchangeInfo == null || LastExchangeInfoUpdate == null || (DateTime.UtcNow - LastExchangeInfoUpdate.Value).TotalMinutes > RestOptions.SpotOptions.TradeRulesUpdateInterval.TotalMinutes)
            await GetExchangeInfoAsync(ct).ConfigureAwait(false);

        if (ExchangeInfo == null)
            return BinanceTradeRuleResult.CreateFailed("Unable to retrieve trading rules, validation failed");

        var symbolInfo = ExchangeInfo.Symbols.SingleOrDefault(s => string.Equals(s.Symbol, symbol, StringComparison.CurrentCultureIgnoreCase));
        if (symbolInfo == null) return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Symbol {symbol} not found");

        return BinanceHelpers.ValidateOptionsTradingRules(Logger, RestOptions.SpotOptions.TradeRulesBehavior, symbolInfo, type, quantity, quoteQuantity, price, stopPrice);
    }
}