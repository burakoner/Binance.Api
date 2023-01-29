using Binance.Api.Models.RestApi.Futures;

namespace Binance.Api.Clients.RestApi;

public class BinanceRestApiCoinFuturesClient : RestApiClient
{
    // Clients
    public BinanceRestApiCoinFuturesServerClient Server { get; }
    public BinanceRestApiCoinFuturesAccountClient Account { get; }
    public BinanceRestApiCoinFuturesTradingClient Trading { get; }
    public BinanceRestApiCoinFuturesMarketDataClient MarketData { get; }
    public BinanceRestApiCoinFuturesUserStreamClient UserStream { get; }
    public BinanceRestApiCoinFuturesPortfolioClient Portfolio { get; }
    public BinanceRestApiFuturesAlgoClient Algo { get => RootClient.FuturesAlgo; }
    public BinanceRestApiFuturesLoanClient Loan { get => RootClient.FuturesLoan; }
    public BinanceRestApiFuturesTransferClient Transfer { get => RootClient.FuturesTransfer; }

    // Internal
    internal Log Log { get => this.log; }
    internal BinanceFuturesCoinExchangeInfo ExchangeInfo;
    internal DateTime? LastExchangeInfoUpdate;
    internal TimeSyncState TimeSyncState = new("Binance CoinFutures RestApi");

    // Root Client
    internal BinanceRestApiClient RootClient { get; }

    // Options
    public new BinanceRestApiClientOptions Options { get { return (BinanceRestApiClientOptions)base.Options; } }

    internal BinanceRestApiCoinFuturesClient(BinanceRestApiClient root) : this(root, new BinanceRestApiClientOptions())
    {
    }

    internal BinanceRestApiCoinFuturesClient(BinanceRestApiClient root, BinanceRestApiClientOptions options) : base("Binance CoinFutures RestApi", options)
    {
        RootClient = root;

        RequestBodyEmptyContent = "";
        RequestBodyFormat = RequestBodyFormat.FormData;
        ArraySerialization = ArraySerialization.MultipleValues;

        this.Server = new BinanceRestApiCoinFuturesServerClient(root, this);
        this.Account = new BinanceRestApiCoinFuturesAccountClient(root, this);
        this.Trading = new BinanceRestApiCoinFuturesTradingClient(root, this);
        this.MarketData = new BinanceRestApiCoinFuturesMarketDataClient(root, this);
        this.UserStream = new BinanceRestApiCoinFuturesUserStreamClient(root, this);
        this.Portfolio = new BinanceRestApiCoinFuturesPortfolioClient(root, this);
    }

    internal void InvokeOrderPlaced(OrderId id)
    {
        OnOrderPlaced?.Invoke(id);
    }

    internal void InvokeOrderCanceled(OrderId id)
    {
        OnOrderCanceled?.Invoke(id);
    }

    /// <summary>
    /// Event triggered when an order is placed via this client. Only available for Spot orders
    /// </summary>
    public event Action<OrderId> OnOrderPlaced;

    /// <summary>
    /// Event triggered when an order is canceled via this client. Note that this does not trigger when using CancelAllOrdersAsync. Only available for Spot orders
    /// </summary>
    public event Action<OrderId> OnOrderCanceled;

    protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
        => new BinanceAuthenticationProvider(credentials);

    internal Uri GetUrl(string endpoint, string api, string version = null)
    {
        var result = Options.CoinFuturesOptions.BaseAddress.AppendPath(api);

        if (!string.IsNullOrEmpty(version))
            result = result.AppendPath($"v{version}");

        return new Uri(result.AppendPath(endpoint));
    }

    internal async Task<BinanceTradeRuleResult> CheckTradeRules(string symbol, decimal? quantity, decimal? quoteQuantity, decimal? price, decimal? stopPrice, FuturesOrderType type, CancellationToken ct)
    {
        var outputQuantity = quantity;
        var outputQuoteQuantity = quoteQuantity;
        var outputPrice = price;
        var outputStopPrice = stopPrice;

        if (Options.CoinFuturesOptions.TradeRulesBehavior == TradeRulesBehavior.None)
            return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, outputPrice, outputStopPrice);

        if (ExchangeInfo == null || LastExchangeInfoUpdate == null || (DateTime.UtcNow - LastExchangeInfoUpdate.Value).TotalMinutes > Options.CoinFuturesOptions.TradeRulesUpdateInterval.TotalMinutes)
            await Server.GetExchangeInfoAsync(ct).ConfigureAwait(false);

        if (ExchangeInfo == null)
            return BinanceTradeRuleResult.CreateFailed("Unable to retrieve trading rules, validation failed");

        var symbolData = ExchangeInfo.Symbols.SingleOrDefault(s => string.Equals(s.Name, symbol, StringComparison.CurrentCultureIgnoreCase));
        if (symbolData == null)
            return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Symbol {symbol} not found");

        if (!symbolData.OrderTypes.Contains(type))
            return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: {type} order type not allowed for {symbol}");

        if (symbolData.LotSizeFilter != null || symbolData.MarketLotSizeFilter != null && type == Enums.FuturesOrderType.Market)
        {
            var minQty = symbolData.LotSizeFilter?.MinQuantity;
            var maxQty = symbolData.LotSizeFilter?.MaxQuantity;
            var stepSize = symbolData.LotSizeFilter?.StepSize;
            if (type == Enums.FuturesOrderType.Market && symbolData.MarketLotSizeFilter != null)
            {
                minQty = symbolData.MarketLotSizeFilter.MinQuantity;
                if (symbolData.MarketLotSizeFilter.MaxQuantity != 0)
                    maxQty = symbolData.MarketLotSizeFilter.MaxQuantity;

                if (symbolData.MarketLotSizeFilter.StepSize != 0)
                    stepSize = symbolData.MarketLotSizeFilter.StepSize;
            }

            if (minQty.HasValue && quantity.HasValue)
            {
                outputQuantity = BinanceHelpers.ClampQuantity(minQty.Value, maxQty!.Value, stepSize!.Value, quantity.Value);
                if (outputQuantity != quantity.Value)
                {
                    if (Options.CoinFuturesOptions.TradeRulesBehavior == TradeRulesBehavior.ThrowError)
                    {
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: LotSize filter failed. Original quantity: {quantity}, Closest allowed: {outputQuantity}");
                    }

                    Log.Write(LogLevel.Information, $"Quantity clamped from {quantity} to {outputQuantity}");
                }
            }
        }

        if (symbolData.MinNotionalFilter != null && outputQuoteQuantity != null)
        {
            if (quoteQuantity < symbolData.MinNotionalFilter.MinNotional)
            {
                if (Options.CoinFuturesOptions.TradeRulesBehavior == TradeRulesBehavior.ThrowError)
                    return BinanceTradeRuleResult.CreateFailed(
                        $"Trade rules check failed: MinNotional filter failed. Order value: {quoteQuantity}, minimal order value: {symbolData.MinNotionalFilter.MinNotional}");

                outputQuoteQuantity = symbolData.MinNotionalFilter.MinNotional;
                Log.Write(LogLevel.Information, $"QuoteQuantity adjusted from {quoteQuantity} to {outputQuoteQuantity} based on min notional filter");
            }
        }

        if (price == null)
            return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, null, outputStopPrice);

        if (symbolData.PriceFilter != null)
        {
            if (symbolData.PriceFilter.MaxPrice != 0 && symbolData.PriceFilter.MinPrice != 0)
            {
                outputPrice = BinanceHelpers.ClampPrice(symbolData.PriceFilter.MinPrice, symbolData.PriceFilter.MaxPrice, price.Value);
                if (outputPrice != price)
                {
                    if (Options.CoinFuturesOptions.TradeRulesBehavior == TradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter max/min failed. Original price: {price}, Closest allowed: {outputPrice}");

                    Log.Write(LogLevel.Information, $"price clamped from {price} to {outputPrice}");
                }

                if (stopPrice != null)
                {
                    outputStopPrice = BinanceHelpers.ClampPrice(symbolData.PriceFilter.MinPrice,
                        symbolData.PriceFilter.MaxPrice, stopPrice.Value);
                    if (outputStopPrice != stopPrice)
                    {
                        if (Options.CoinFuturesOptions.TradeRulesBehavior == TradeRulesBehavior.ThrowError)
                            return BinanceTradeRuleResult.CreateFailed(
                                $"Trade rules check failed: Stop price filter max/min failed. Original stop price: {stopPrice}, Closest allowed: {outputStopPrice}");

                        Log.Write(LogLevel.Information,
                            $"Stop price clamped from {stopPrice} to {outputStopPrice} based on price filter");
                    }
                }
            }

            if (symbolData.PriceFilter.TickSize != 0)
            {
                var beforePrice = outputPrice;
                outputPrice = BinanceHelpers.FloorPrice(symbolData.PriceFilter.TickSize, price.Value);
                if (outputPrice != beforePrice)
                {
                    if (Options.CoinFuturesOptions.TradeRulesBehavior == TradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter tick failed. Original price: {price}, Closest allowed: {outputPrice}");

                    Log.Write(LogLevel.Information, $"price rounded from {beforePrice} to {outputPrice}");
                }

                if (stopPrice != null)
                {
                    var beforeStopPrice = outputStopPrice;
                    outputStopPrice = BinanceHelpers.FloorPrice(symbolData.PriceFilter.TickSize, stopPrice.Value);
                    if (outputStopPrice != beforeStopPrice)
                    {
                        if (Options.CoinFuturesOptions.TradeRulesBehavior == TradeRulesBehavior.ThrowError)
                            return BinanceTradeRuleResult.CreateFailed(
                                $"Trade rules check failed: Stop price filter tick failed. Original stop price: {stopPrice}, Closest allowed: {outputStopPrice}");

                        Log.Write(LogLevel.Information,
                            $"Stop price floored from {beforeStopPrice} to {outputStopPrice} based on price filter");
                    }
                }
            }
        }

        return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, outputPrice, outputStopPrice);
    }

    internal async Task<RestCallResult<T>> SendRequestInternal<T>(Uri uri, HttpMethod method, CancellationToken cancellationToken,
        Dictionary<string, object> parameters = null, bool signed = false, HttpMethodParameterPosition? postPosition = null,
        ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
    {
        var result = await SendRequestAsync<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRatelimit: ignoreRateLimit).ConfigureAwait(false);
        if (!result && result.Error!.Code == -1021 && Options.AutoTimestamp)
        {
            log.Write(LogLevel.Debug, "Received Invalid Timestamp error, triggering new time sync");
            TimeSyncState.LastSyncTime = DateTime.MinValue;
        }
        return result;
    }

    protected override Task<RestCallResult<DateTime>> GetServerTimestampAsync()
        => Server.GetServerTimeAsync();

    protected override TimeSyncInfo GetTimeSyncInfo()
        => new TimeSyncInfo(log, Options.AutoTimestamp, Options.TimestampRecalculationInterval, TimeSyncState);

    public override TimeSpan GetTimeOffset()
        => TimeSyncState.TimeOffset;

    public string GetSymbolName(string baseAsset, string quoteAsset) =>
        (baseAsset + quoteAsset).ToUpper(CultureInfo.InvariantCulture);

    protected override CallError ParseErrorResponse(JToken error)
    {
        if (!error.HasValues)
            return new ServerError(error.ToString());

        if (error["msg"] == null && error["code"] == null)
            return new ServerError(error.ToString());

        if (error["msg"] != null && error["code"] == null)
            return new ServerError((string)error["msg"]!);

        return new ServerError((int)error["code"]!, (string)error["msg"]!);
    }
}