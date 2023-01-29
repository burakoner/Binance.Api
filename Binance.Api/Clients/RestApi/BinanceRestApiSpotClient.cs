namespace Binance.Api.Clients.RestApi;

public class BinanceRestApiSpotClient : RestApiClient
{
    // Clients
    public BinanceRestApiSpotServerClient Server { get; }
    public BinanceRestApiSpotAccountClient Account { get; }
    public BinanceRestApiSpotTradingClient Trading { get; }
    public BinanceRestApiSpotMarketDataClient MarketData { get; }
    public BinanceRestApiSpotUserStreamClient UserStream { get; }

    // Internal
    internal Log Log { get => this.log; }
    internal BinanceExchangeInfo ExchangeInfo;
    internal DateTime? LastExchangeInfoUpdate;
    internal TimeSyncState TimeSyncState = new("Binance Spot RestApi");

    // Root Client
    internal BinanceRestApiClient RootClient { get; }

    // Options
    public new BinanceRestApiClientOptions Options { get { return (BinanceRestApiClientOptions)base.Options; } }

    internal BinanceRestApiSpotClient(BinanceRestApiClient root) : base("Binance Spot RestApi", root.Options)
    {
        RootClient = root;

        RequestBodyEmptyContent = "";
        RequestBodyFormat = RequestBodyFormat.FormData;
        ArraySerialization = ArraySerialization.MultipleValues;

        this.Server = new BinanceRestApiSpotServerClient(this);
        this.Account = new BinanceRestApiSpotAccountClient(this);
        this.Trading = new BinanceRestApiSpotTradingClient(this);
        this.MarketData = new BinanceRestApiSpotMarketDataClient(this);
        this.UserStream = new BinanceRestApiSpotUserStreamClient(this);
    }

    protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
        => new BinanceAuthenticationProvider(credentials);

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

    protected override Task<RestCallResult<DateTime>> GetServerTimestampAsync()
        => Server.GetServerTimeAsync();

    protected override TimeSyncInfo GetTimeSyncInfo()
        => new(log, Options.AutoTimestamp, Options.TimestampRecalculationInterval, TimeSyncState);

    public override TimeSpan GetTimeOffset()
        => TimeSyncState.TimeOffset;

    protected string GetSymbolName(string baseAsset, string quoteAsset) =>
        (baseAsset + quoteAsset).ToUpper(CultureInfo.InvariantCulture);

    internal Uri GetUrl(string endpoint, string api, string version = null)
    {
        var result = Options.BaseAddress.AppendPath(api);

        if (!string.IsNullOrEmpty(version))
            result = result.AppendPath($"v{version}");

        return new Uri(result.AppendPath(endpoint));
    }

    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri,
        HttpMethod method,
        CancellationToken cancellationToken,
        Dictionary<string, object> parameters = null,
        bool signed = false,
        HttpMethodParameterPosition? postPosition = null,
        ArraySerialization? arraySerialization = null,
        int weight = 1,
        bool ignoreRateLimit = false) where T : class
    {
        var result = await SendRequestAsync<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRatelimit: ignoreRateLimit).ConfigureAwait(false);
        if (!result && result.Error!.Code == -1021 && Options.AutoTimestamp)
        {
            log.Write(LogLevel.Debug, "Received Invalid Timestamp error, triggering new time sync");
            TimeSyncState.LastSyncTime = DateTime.MinValue;
        }

        return result;
    }

    internal async Task<RestCallResult<BinancePlacedOrder>> PlaceOrderInternal(Uri uri,
        string symbol,
        OrderSide side,
        SpotOrderType type,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        string newClientOrderId = null,
        decimal? price = null,
        TimeInForce? timeInForce = null,
        decimal? stopPrice = null,
        decimal? icebergQty = null,
        SideEffectType? sideEffectType = null,
        bool? isIsolated = null,
        OrderResponseType? orderResponseType = null,
        int? trailingDelta = null,
        int? receiveWindow = null,
        int weight = 1,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        if (quoteQuantity != null && type != SpotOrderType.Market)
            throw new ArgumentException("quoteQuantity is only valid for market orders");

        if (quantity == null && quoteQuantity == null || quantity != null && quoteQuantity != null)
            throw new ArgumentException("1 of either should be specified, quantity or quoteOrderQuantity");

        var rulesCheck = await CheckTradeRules(symbol, quantity, quoteQuantity, price, stopPrice, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            log.Write(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinancePlacedOrder>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;
        quoteQuantity = rulesCheck.QuoteQuantity;

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "side", JsonConvert.SerializeObject(side, new OrderSideConverter(false)) },
                { "type", JsonConvert.SerializeObject(type, new SpotOrderTypeConverter(false)) }
            };
        parameters.AddOptionalParameter("quantity", quantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("quoteOrderQty", quoteQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("newClientOrderId", newClientOrderId);
        parameters.AddOptionalParameter("price", price?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("timeInForce", timeInForce == null ? null : JsonConvert.SerializeObject(timeInForce, new TimeInForceConverter(false)));
        parameters.AddOptionalParameter("stopPrice", stopPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("icebergQty", icebergQty?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("sideEffectType", sideEffectType == null ? null : JsonConvert.SerializeObject(sideEffectType, new SideEffectTypeConverter(false)));
        parameters.AddOptionalParameter("isIsolated", isIsolated);
        parameters.AddOptionalParameter("newOrderRespType", orderResponseType == null ? null : JsonConvert.SerializeObject(orderResponseType, new OrderResponseTypeConverter(false)));
        parameters.AddOptionalParameter("trailingDelta", trailingDelta);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinancePlacedOrder>(uri, HttpMethod.Post, ct, parameters, true, weight: weight).ConfigureAwait(false);
    }

    internal async Task<BinanceTradeRuleResult> CheckTradeRules(string symbol, decimal? quantity, decimal? quoteQuantity, decimal? price, decimal? stopPrice, SpotOrderType? type, CancellationToken ct)
    {
        var outputQuantity = quantity;
        var outputQuoteQuantity = quoteQuantity;
        var outputPrice = price;
        var outputStopPrice = stopPrice;

        if (Options.SpotOptions.TradeRulesBehavior == TradeRulesBehavior.None)
            return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, outputPrice, outputStopPrice);

        if (ExchangeInfo == null || LastExchangeInfoUpdate == null || (DateTime.UtcNow - LastExchangeInfoUpdate.Value).TotalMinutes > Options.SpotOptions.TradeRulesUpdateInterval.TotalMinutes)
            await Server.GetExchangeInfoAsync(ct).ConfigureAwait(false);

        if (ExchangeInfo == null)
            return BinanceTradeRuleResult.CreateFailed("Unable to retrieve trading rules, validation failed");

        var symbolData = ExchangeInfo.Symbols.SingleOrDefault(s => string.Equals(s.Name, symbol, StringComparison.CurrentCultureIgnoreCase));
        if (symbolData == null)
            return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Symbol {symbol} not found");

        if (type != null)
        {
            if (!symbolData.OrderTypes.Contains(type.Value))
            {
                return BinanceTradeRuleResult.CreateFailed(
                    $"Trade rules check failed: {type} order type not allowed for {symbol}");
            }
        }

        if (symbolData.LotSizeFilter != null || symbolData.MarketLotSizeFilter != null && type == SpotOrderType.Market)
        {
            var minQty = symbolData.LotSizeFilter?.MinQuantity;
            var maxQty = symbolData.LotSizeFilter?.MaxQuantity;
            var stepSize = symbolData.LotSizeFilter?.StepSize;
            if (type == SpotOrderType.Market && symbolData.MarketLotSizeFilter != null)
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
                    if (Options.SpotOptions.TradeRulesBehavior == TradeRulesBehavior.ThrowError)
                    {
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: LotSize filter failed. Original quantity: {quantity}, Closest allowed: {outputQuantity}");
                    }

                    log.Write(LogLevel.Information, $"Quantity clamped from {quantity} to {outputQuantity} based on lot size filter");
                }
            }
        }

        if (symbolData.MinNotionalFilter != null && outputQuoteQuantity != null)
        {
            if (quoteQuantity < symbolData.MinNotionalFilter.MinNotional)
            {
                if (Options.SpotOptions.TradeRulesBehavior == TradeRulesBehavior.ThrowError)
                {
                    return BinanceTradeRuleResult.CreateFailed(
                        $"Trade rules check failed: MinNotional filter failed. Order value: {quoteQuantity}, minimal order value: {symbolData.MinNotionalFilter.MinNotional}");
                }

                outputQuoteQuantity = symbolData.MinNotionalFilter.MinNotional;
                log.Write(LogLevel.Information, $"QuoteQuantity adjusted from {quoteQuantity} to {outputQuoteQuantity} based on min notional filter");
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
                    if (Options.SpotOptions.TradeRulesBehavior == TradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter max/min failed. Original price: {price}, Closest allowed: {outputPrice}");

                    log.Write(LogLevel.Information, $"price clamped from {price} to {outputPrice} based on price filter");
                }

                if (stopPrice != null)
                {
                    outputStopPrice = BinanceHelpers.ClampPrice(symbolData.PriceFilter.MinPrice,
                        symbolData.PriceFilter.MaxPrice, stopPrice.Value);
                    if (outputStopPrice != stopPrice)
                    {
                        if (Options.SpotOptions.TradeRulesBehavior == TradeRulesBehavior.ThrowError)
                        {
                            return BinanceTradeRuleResult.CreateFailed(
                                $"Trade rules check failed: Stop price filter max/min failed. Original stop price: {stopPrice}, Closest allowed: {outputStopPrice}");
                        }

                        log.Write(LogLevel.Information,
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
                    if (Options.SpotOptions.TradeRulesBehavior == TradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter tick failed. Original price: {price}, Closest allowed: {outputPrice}");

                    log.Write(LogLevel.Information, $"price floored from {beforePrice} to {outputPrice} based on price filter");
                }

                if (stopPrice != null)
                {
                    var beforeStopPrice = outputStopPrice;
                    outputStopPrice = BinanceHelpers.FloorPrice(symbolData.PriceFilter.TickSize, stopPrice.Value);
                    if (outputStopPrice != beforeStopPrice)
                    {
                        if (Options.SpotOptions.TradeRulesBehavior == TradeRulesBehavior.ThrowError)
                        {
                            return BinanceTradeRuleResult.CreateFailed(
                                $"Trade rules check failed: Stop price filter tick failed. Original stop price: {stopPrice}, Closest allowed: {outputStopPrice}");
                        }

                        log.Write(LogLevel.Information,
                            $"Stop price floored from {beforeStopPrice} to {outputStopPrice} based on price filter");
                    }
                }
            }
        }

        if (symbolData.MinNotionalFilter == null || quantity == null || outputPrice == null)
            return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, outputPrice, outputStopPrice);

        var currentQuantity = outputQuantity ?? quantity.Value;
        var notional = currentQuantity * outputPrice.Value;
        if (notional < symbolData.MinNotionalFilter.MinNotional)
        {
            if (Options.SpotOptions.TradeRulesBehavior == TradeRulesBehavior.ThrowError)
            {
                return BinanceTradeRuleResult.CreateFailed(
                    $"Trade rules check failed: MinNotional filter failed. Order quantity: {notional}, minimal order quantity: {symbolData.MinNotionalFilter.MinNotional}");
            }

            if (symbolData.LotSizeFilter == null)
                return BinanceTradeRuleResult.CreateFailed("Trade rules check failed: MinNotional filter failed. Unable to auto comply because LotSizeFilter not present");

            var minQuantity = symbolData.MinNotionalFilter.MinNotional / outputPrice.Value;
            var stepSize = symbolData.LotSizeFilter!.StepSize;
            outputQuantity = BinanceHelpers.Floor(minQuantity + (stepSize - minQuantity % stepSize));
            log.Write(LogLevel.Information, $"Quantity clamped from {currentQuantity} to {outputQuantity} based on min notional filter");
        }

        return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, outputPrice, outputStopPrice);
    }

}