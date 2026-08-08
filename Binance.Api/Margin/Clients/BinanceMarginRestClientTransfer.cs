namespace Binance.Api.Margin;

internal partial class BinanceMarginRestClient
{
    public Task<RestCallResult<BinanceMarginTransferHistoryResult>> GetMarginTransfersAsync(
        BinanceMarginTransferDirection? direction = null,
        string? asset = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        long? current = null,
        long? size = null,
        string? isolatedSymbol = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        ValidateOptionalMarginAsset(asset, nameof(asset));
        ValidateOptionalMarginSymbol(isolatedSymbol, nameof(isolatedSymbol));
        ValidateMarginDateRange(startTime, endTime, 30, "transfer history");
        ValidateMarginPagination(current, size);

        var parameters = new ParameterCollection();
        parameters.AddOptional("asset", asset);
        parameters.AddOptionalEnum("type", direction);
        parameters.AddOptional("isolatedSymbol", isolatedSymbol);
        parameters.AddOptional("current", current);
        parameters.AddOptional("size", size);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginTransferHistoryResult>(GetUrl(sapi, v1, "margin/transfer"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public async Task<RestCallResult<decimal>> GetMarginMaxTransferAmountAsync(string asset, string? isolatedSymbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(asset))
            throw new ArgumentException("asset is required", nameof(asset));
        ValidateOptionalMarginSymbol(isolatedSymbol, nameof(isolatedSymbol));

        var parameters = new ParameterCollection
        {
            { "asset", asset }
        };
        parameters.AddOptional("isolatedSymbol", isolatedSymbol);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceMarginAmount>(GetUrl(sapi, v1, "margin/maxTransferable"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 50);
        if (!result) return result.As<decimal>(default);

        return result.As(result.Data.Quantity);
    }

}
