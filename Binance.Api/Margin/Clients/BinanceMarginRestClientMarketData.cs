namespace Binance.Api.Margin;

internal partial class BinanceMarginRestClient
{
    public Task<RestCallResult<List<BinanceCrossMarginCollateralRatio>>> GetCrossMarginCollateralRatioAsync(CancellationToken ct = default)
        => RequestAsync<List<BinanceCrossMarginCollateralRatio>>(GetUrl(sapi, v1, "margin/crossMarginCollateralRatio"), HttpMethod.Get, ct, false, requestWeight: 100);

    public Task<RestCallResult<List<BinanceMarginSymbol>>> GetMarginSymbolsAsync(string? symbol = null, CancellationToken ct = default)
    {
        ValidateOptionalMarginSymbol(symbol, nameof(symbol));

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);

        return RequestAsync<List<BinanceMarginSymbol>>(GetUrl(sapi, v1, "margin/allPairs"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceIsolatedMarginSymbol>>> GetIsolatedMarginSymbolsAsync(string? symbol = null, int? receiveWindow =
        null, CancellationToken ct = default)
    {
        ValidateOptionalMarginSymbol(symbol, nameof(symbol));

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceIsolatedMarginSymbol>>(GetUrl(sapi, v1, "margin/isolated/allPairs"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<List<BinanceMarginAsset>>> GetMarginAssetsAsync(string? asset = null, CancellationToken ct = default)
    {
        ValidateOptionalMarginAsset(asset, nameof(asset));

        var parameters = new ParameterCollection();
        parameters.AddOptional("asset", asset);

        return RequestAsync<List<BinanceMarginAsset>>(GetUrl(sapi, v1, "margin/allAssets"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceMarginDelistSchedule>>> GetMarginDelistScheduleAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceMarginDelistSchedule>>(GetUrl(sapi, v1, "margin/delist-schedule"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<BinanceMarginLimitPricePairs>> GetMarginLimitPricePairsAsync(CancellationToken ct = default)
        => RequestAsync<BinanceMarginLimitPricePairs>(GetUrl(sapi, v1, "margin/limit-price-pairs"), HttpMethod.Get, ct, false, requestWeight: 1);

    public Task<RestCallResult<List<BinanceMarginListSchedule>>> GetMarginListScheduleAsync(
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceMarginListSchedule>>(GetUrl(sapi, v1, "margin/list-schedule"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<List<BinanceMarginRiskBasedLiquidationRatio>>> GetMarginRiskBasedLiquidationRatiosAsync(CancellationToken ct = default)
        => RequestAsync<List<BinanceMarginRiskBasedLiquidationRatio>>(GetUrl(sapi, v1, "margin/risk-based-liquidation-ratio"), HttpMethod.Get, ct, false, requestWeight: 1);

    public Task<RestCallResult<BinanceMarginRestrictedAssets>> GetMarginRestrictedAssetsAsync(CancellationToken ct = default)
        => RequestAsync<BinanceMarginRestrictedAssets>(GetUrl(sapi, v1, "margin/restricted-asset"), HttpMethod.Get, ct, false, requestWeight: 1);

    public Task<RestCallResult<List<BinanceIsolatedMarginTier>>> GetIsolatedMarginTierDataAsync(string symbol, long? tier = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("symbol is required", nameof(symbol));
        symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection();
        parameters.Add("symbol", symbol);
        parameters.AddOptional("tier", tier);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceIsolatedMarginTier>>(GetUrl(sapi, v1, "margin/isolatedMarginTier"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceMarginPriceIndex>> GetMarginPriceIndexAsync(string symbol, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("symbol is required", nameof(symbol));
        symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection();
        parameters.Add("symbol", symbol);

        return RequestAsync<BinanceMarginPriceIndex>(GetUrl(sapi, v1, "margin/priceIndex"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<BinanceMarginAvailableInventory>> GetMarginAvailableInventoryAsync(BinanceMarginInventoryType type, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddEnum("type", type);

        return RequestAsync<BinanceMarginAvailableInventory>(GetUrl(sapi, v1, "margin/available-inventory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 50);
    }

}
