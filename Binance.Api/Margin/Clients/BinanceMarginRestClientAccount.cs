using Binance.Api.Wallet;

namespace Binance.Api.Margin;

internal partial class BinanceMarginRestClient
{
    public Task<RestCallResult<BinanceCrossMarginLeverageResult>> AdjustMaximumLeverageAsync(long maxLeverage, CancellationToken ct = default)
    {
        if (maxLeverage is not (3 or 5 or 10))
            throw new ArgumentOutOfRangeException(nameof(maxLeverage), "maxLeverage must be 3, 5, or 10");

        var parameters = new ParameterCollection
        {
            { "maxLeverage", maxLeverage },
        };

        return RequestAsync<BinanceCrossMarginLeverageResult>(GetUrl(sapi, v1, "margin/max-leverage"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 3000);
    }

    public Task<RestCallResult<BinanceIsolatedMarginCreateAccountResult>> DisableIsolatedMarginAccountAsync(string symbol,
        int? receiveWindow = null, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("symbol is required", nameof(symbol));
        symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            {"symbol", symbol}
        };
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceIsolatedMarginCreateAccountResult>(GetUrl(sapi, v1, "margin/isolated/account"), HttpMethod.Delete, ct, true, queryParameters: parameters, requestWeight: 300);
    }

    public Task<RestCallResult<BinanceIsolatedMarginCreateAccountResult>> EnableIsolatedMarginAccountAsync(string symbol,
        int? receiveWindow = null, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("symbol is required", nameof(symbol));
        symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            {"symbol", symbol}
        };
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceIsolatedMarginCreateAccountResult>(GetUrl(sapi, v1, "margin/isolated/account"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 300);
    }

    public Task<RestCallResult<BinanceWalletBnbBurnStatus>> GetBnbBurnStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceWalletBnbBurnStatus>(GetUrl(sapi, v1, "bnbBurn"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceMarginLevel>> GetMarginLevelInformationAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginLevel>(GetUrl(sapi, v1, "margin/tradeCoeff"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<List<BinanceMarginCapitalFlow>>> GetMarginCapitalFlowAsync(
        string? asset = null,
        string? symbol = null,
        BinanceMarginCapitalFlowType? type = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        long? fromId = null,
        long? limit = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        ValidateOptionalMarginAsset(asset, nameof(asset));
        ValidateOptionalMarginSymbol(symbol, nameof(symbol));
        ValidateMarginDateRange(startTime, endTime, 7, "capital-flow");
        var oldestSupportedTime = DateTime.UtcNow.AddDays(-90);
        if (startTime?.ToUniversalTime() < oldestSupportedTime)
            throw new ArgumentOutOfRangeException(nameof(startTime), "capital-flow data is available only for the last 90 days");
        if (endTime?.ToUniversalTime() < oldestSupportedTime)
            throw new ArgumentOutOfRangeException(nameof(endTime), "capital-flow data is available only for the last 90 days");
        if (limit > 1_000)
            throw new ArgumentOutOfRangeException(nameof(limit), "limit cannot exceed 1000");

        var parameters = new ParameterCollection();
        parameters.AddOptional("asset", asset);
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptionalEnum("type", type);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceMarginCapitalFlow>>(GetUrl(sapi, v1, "margin/capital-flow"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<BinanceMarginAccount>> GetMarginAccountInfoAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginAccount>(GetUrl(sapi, v1, "margin/account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<List<BinanceCrossMarginFee>>> GetCrossMarginFeeDataAsync(string? asset = null, long? vipLevel = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        ValidateOptionalMarginAsset(asset, nameof(asset));

        var parameters = new ParameterCollection();

        parameters.AddOptional("coin", asset);
        parameters.AddOptional("vipLevel", vipLevel);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        var weight = asset == null ? 5 : 1;
        return RequestAsync<List<BinanceCrossMarginFee>>(GetUrl(sapi, v1, "margin/crossMarginData"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<BinanceIsolatedMarginAccountLimit>> GetEnabledIsolatedMarginAccountLimitAsync(
        int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceIsolatedMarginAccountLimit>(GetUrl(sapi, v1, "margin/isolated/accountLimit"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceIsolatedMarginAccount>> GetIsolatedMarginAccountAsync(
        IEnumerable<string>? symbols = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbols", ValidateOptionalMarginSymbols(symbols));
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceIsolatedMarginAccount>(GetUrl(sapi, v1, "margin/isolated/account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<List<BinanceIsolatedMarginFee>>> GetIsolatedMarginFeeDataAsync(string? symbol = null, long? vipLevel = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        ValidateOptionalMarginSymbol(symbol, nameof(symbol));

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("vipLevel", vipLevel);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        var weight = symbol == null ? 10 : 1;
        return RequestAsync<List<BinanceIsolatedMarginFee>>(GetUrl(sapi, v1, "margin/isolatedMarginData"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: weight);
    }

    private static string? ValidateOptionalMarginSymbols(IEnumerable<string>? symbols)
    {
        if (symbols == null)
            return null;

        var values = symbols.ToArray();
        if (values.Length is < 1 or > 5)
            throw new ArgumentOutOfRangeException(nameof(symbols), "symbols must contain between 1 and 5 entries");
        if (values.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("symbols cannot contain an empty entry", nameof(symbols));
        if (values.Any(symbol => symbol.Contains(',')))
            throw new ArgumentException("an individual symbol cannot contain a comma", nameof(symbols));
        foreach (var symbol in values)
            symbol.ValidateBinanceSymbol();

        return string.Join(",", values);
    }

}
