using Binance.Api.Wallet;

namespace Binance.Api.Margin;

internal partial class BinanceMarginRestClient
{
    public Task<RestCallResult<List<BinanceMarginInterestRate>>> GetFutureHourlyInterestRateAsync(
        IEnumerable<string> assets,
        bool isolated,
        CancellationToken ct = default)
    {
        var assetList = ValidateMarginAssets(assets);
        var parameters = new ParameterCollection
        {
            { "assets", string.Join(",", assetList) },
            { "isIsolated", isolated ? "TRUE" : "FALSE" }
        };

        return RequestAsync<List<BinanceMarginInterestRate>>(GetUrl(sapi, v1, "margin/next-hourly-interest-rate"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<BinanceMarginInterestHistoryResult>> GetMarginInterestHistoryAsync(
        string? asset = null,
        string? isolatedSymbol = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        long? current = null,
        long? size = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        ValidateOptionalMarginAsset(asset, nameof(asset));
        ValidateOptionalMarginSymbol(isolatedSymbol, nameof(isolatedSymbol));
        ValidateMarginDateRange(startTime, endTime, 30, "interest history");
        ValidateMarginPagination(current, size);

        var parameters = new ParameterCollection();
        parameters.AddOptional("asset", asset);
        parameters.AddOptional("isolatedSymbol", isolatedSymbol);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", current);
        parameters.AddOptional("size", size);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginInterestHistoryResult>(GetUrl(sapi, v1, "margin/interestHistory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceWalletTransaction>> BorrowAsync(
        string asset,
        decimal quantity,
        bool isIsolated = false,
        string? symbol = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
        => BorrowRepayAsync(asset, quantity, BinanceMarginBorrowRepayType.Borrow, isIsolated, symbol, receiveWindow, ct);

    public Task<RestCallResult<BinanceWalletTransaction>> RepayAsync(
        string asset,
        decimal quantity,
        bool isIsolated = false,
        string? symbol = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
        => BorrowRepayAsync(asset, quantity, BinanceMarginBorrowRepayType.Repay, isIsolated, symbol, receiveWindow, ct);

    public Task<RestCallResult<BinanceMarginBorrowRepayHistory>> GetMarginBorrowRepayHistoryAsync(
        BinanceMarginBorrowRepayType type,
        string? asset = null,
        string? isolatedSymbol = null,
        long? transactionId = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        long? current = null,
        long? size = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        ValidateOptionalMarginAsset(asset, nameof(asset));
        ValidateOptionalMarginSymbol(isolatedSymbol, nameof(isolatedSymbol));
        if (!transactionId.HasValue)
            ValidateMarginDateRange(startTime, endTime, asset == null ? 7 : 30, "borrow/repay history");
        ValidateMarginPagination(current, size);

        var parameters = new ParameterCollection();
        parameters.AddEnum("type", type);
        parameters.AddOptional("asset", asset);
        parameters.AddOptional("isolatedSymbol", isolatedSymbol);
        parameters.AddOptional("txId", transactionId);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", current);
        parameters.AddOptional("size", size);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginBorrowRepayHistory>(GetUrl(sapi, v1, "margin/borrow-repay"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<List<BinanceMarginInterestRateHistory>>> GetMarginInterestRateHistoryAsync(
        string asset,
        long? vipLevel = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        ValidateRequiredMarginAsset(asset, nameof(asset));
        ValidateMarginDateRange(startTime, endTime, 30, "interest-rate history");

        var parameters = new ParameterCollection
        {
            { "asset", asset }
        };
        parameters.AddOptional("vipLevel", vipLevel);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceMarginInterestRateHistory>>(GetUrl(sapi, v1, "margin/interestRateHistory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceMarginAmount>> GetMarginMaxBorrowAmountAsync(
        string asset,
        string? isolatedSymbol = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        ValidateRequiredMarginAsset(asset, nameof(asset));
        ValidateOptionalMarginSymbol(isolatedSymbol, nameof(isolatedSymbol));

        var parameters = new ParameterCollection
        {
            { "asset", asset }
        };
        parameters.AddOptional("isolatedSymbol", isolatedSymbol);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        // Effective 2026-04-16; the live endpoint schema and generated connector still show the retired 50 IP value.
        return RequestAsync<BinanceMarginAmount>(GetUrl(sapi, v1, "margin/maxBorrowable"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 750);
    }

    private Task<RestCallResult<BinanceWalletTransaction>> BorrowRepayAsync(
        string asset,
        decimal quantity,
        BinanceMarginBorrowRepayType type,
        bool isIsolated,
        string? symbol,
        int? receiveWindow,
        CancellationToken ct)
    {
        ValidateRequiredMarginAsset(asset, nameof(asset));
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "quantity must be greater than zero");
        if (isIsolated && string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("symbol is required for Isolated Margin borrowing and repayment", nameof(symbol));
        if (!isIsolated && symbol != null)
            throw new ArgumentException("symbol is supported only for Isolated Margin borrowing and repayment", nameof(symbol));
        ValidateOptionalMarginSymbol(symbol, nameof(symbol));

        var parameters = new ParameterCollection
        {
            { "asset", asset },
            { "isIsolated", isIsolated ? "TRUE" : "FALSE" },
            { "amount", quantity.ToString(BinanceConstants.CI) }
        };
        parameters.AddEnum("type", type);
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceWalletTransaction>(GetUrl(sapi, v1, "margin/borrow-repay"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1_500);
    }

    private static string[] ValidateMarginAssets(IEnumerable<string> assets)
    {
        if (assets == null)
            throw new ArgumentNullException(nameof(assets));
        var result = assets.ToArray();
        if (result.Length is < 1 or > 20)
            throw new ArgumentOutOfRangeException(nameof(assets), "assets must contain between 1 and 20 entries");
        if (result.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("assets cannot contain an empty entry", nameof(assets));
        if (result.Any(asset => asset.Contains(',')))
            throw new ArgumentException("an individual asset cannot contain a comma", nameof(assets));
        return result;
    }

    private static void ValidateRequiredMarginAsset(string asset, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(asset))
            throw new ArgumentException("asset is required", parameterName);
    }

    private static void ValidateOptionalMarginAsset(string? asset, string parameterName)
    {
        if (asset != null)
            ValidateRequiredMarginAsset(asset, parameterName);
    }

    private static void ValidateOptionalMarginSymbol(string? symbol, string parameterName)
    {
        if (symbol == null)
            return;
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("symbol cannot be empty when provided", parameterName);
        symbol.ValidateBinanceSymbol();
    }

    private static void ValidateMarginDateRange(DateTime? startTime, DateTime? endTime, int maximumDays, string rangeName)
    {
        if (startTime > endTime)
            throw new ArgumentException("startTime cannot be later than endTime", nameof(startTime));
        if (startTime.HasValue && endTime.HasValue && endTime.Value - startTime.Value > TimeSpan.FromDays(maximumDays))
            throw new ArgumentException($"The explicit {rangeName} range cannot exceed {maximumDays} days", nameof(endTime));
    }

    private static void ValidateMarginPagination(long? current, long? size)
    {
        if (current is <= 0)
            throw new ArgumentOutOfRangeException(nameof(current), "current must be greater than zero when provided");
        if (size is <= 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(size), "size must be between 1 and 100 when provided");
    }
}
